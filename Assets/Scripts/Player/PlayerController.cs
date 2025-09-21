using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public Animator animator;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask obstacleLayer = ~0;
    [SerializeField] private LayerMask spikeLayer = 0;

    [Header("States")]
    [SerializeField] private bool isFrozen = false;
    [SerializeField] private bool isReversed = false;

    private bool isMoving = false;
    private bool isCurrentlyTeleporting = false;

    private const float PortalReentryBlockSeconds = 0.2f;
    private float portalReentryTimer = 0f;

    private Transform _transform;
    private Collider _collider;

    private Coroutine moveCoroutine;
    private Coroutine slideCoroutine;
    private Coroutine teleportCoroutine;

    private Vector3 lastDirection = Vector3.zero;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        _transform = transform;
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (portalReentryTimer > 0f)
            portalReentryTimer -= Time.deltaTime;
    }

    public void ReceiveMovementCommand(Vector3 inputDirection)
    {
        if (isCurrentlyTeleporting) return;

        if (isFrozen)
        {
            Debug.Log($"{name} is frozen and cannot move this turn.");
            isFrozen = false;
            return;
        }

        Vector3 effectiveDir = EffectiveDirection(inputDirection);

        if (isMoving) return;

        if (!CanMove(effectiveDir))
        {
            Debug.Log($"{name} cannot move, an obstacle is in the way.");
            animator.SetBool("Obstacle", true);
            return;
        }

        TryStopCoroutine(ref slideCoroutine);

        moveCoroutine = StartCoroutine(MovePlayer(effectiveDir));
        animator.SetBool("isRun", true);
    }

    public void FreezePlayer() => isFrozen = true;
    public void ReversePlayer() => isReversed = true;

    public bool GetIsMoving() => isMoving || isCurrentlyTeleporting;

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        lastDirection = direction;

        Vector3 targetPos = _transform.position + direction * gridSize;

        while (Vector3.Distance(_transform.position, targetPos) > 0.001f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        SnapToGrid();
        isMoving = false;

        if (IsOnIce())
        {
            Vector3 slideDir = Cardinalize(direction);

            if (CanMove(slideDir))
                slideCoroutine = StartCoroutine(MoveOnIce(slideDir));
        }
    }

    private IEnumerator MoveOnIce(Vector3 direction)
    {
        isMoving = true;
        Vector3 dir = Cardinalize(direction);

        while (true)
        {
            if (!CanMove(dir)) break;

            Vector3 nextTarget = _transform.position + dir * gridSize;

            while (Vector3.Distance(_transform.position, nextTarget) > 0.001f)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, nextTarget, moveSpeed * Time.deltaTime);
                yield return null;
            }

            SnapToGrid();

            if (!IsOnIce()) break;
        }

        isMoving = false;
        slideCoroutine = null;
    }

    public IEnumerator TeleportThroughPortal(Vector3 startPortalPos, Vector3 endExitPos, float duration, Portal currentPortal, Portal destinationPortal)
    {
        TryStopCoroutine(ref moveCoroutine);
        TryStopCoroutine(ref slideCoroutine);

        isCurrentlyTeleporting = true;
        isMoving = true;

        Vector3 startPos = _transform.position;
        float half = Mathf.Max(0.01f, duration * 0.5f);
        float t = 0f;

        while (t < half)
        {
            float k = t / half;
            _transform.position = Vector3.Lerp(startPos, new Vector3(startPortalPos.x, _transform.position.y, startPortalPos.z), k);
            t += Time.deltaTime;
            yield return null;
        }

        _transform.position = new Vector3(destinationPortal.transform.position.x, _transform.position.y, destinationPortal.transform.position.z);

        startPos = _transform.position;
        t = 0f;
        while (t < half)
        {
            float k = t / half;
            Vector3 finalExit = new Vector3(endExitPos.x, _transform.position.y, endExitPos.z);
            _transform.position = Vector3.Lerp(startPos, finalExit, k);
            t += Time.deltaTime;
            yield return null;
        }

        _transform.position = new Vector3(endExitPos.x, _transform.position.y, endExitPos.z);
        SnapToGrid();

        currentPortal?.OnTeleportFinished();
        destinationPortal?.OnTeleportFinished();

        if (IsOnIce())
        {
            Vector3 rawDir = (endExitPos - destinationPortal.transform.position).normalized;
            Vector3 slideDir = Cardinalize(rawDir);
            if (CanMove(slideDir))
                slideCoroutine = StartCoroutine(MoveOnIce(slideDir));
        }

        portalReentryTimer = PortalReentryBlockSeconds;

        isMoving = false;
        isCurrentlyTeleporting = false;
        teleportCoroutine = null;
    }

    private void SnapToGrid()
    {
        Vector3 p = _transform.position;
        _transform.position = new Vector3(
            Mathf.Round(p.x / gridSize) * gridSize,
            p.y,
            Mathf.Round(p.z / gridSize) * gridSize
        );
    }

    private Vector3 Cardinalize(Vector3 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
            return new Vector3(Mathf.Sign(dir.x), 0f, 0f);
        else
            return new Vector3(0f, 0f, Mathf.Sign(dir.z));
    }

    private Vector3 EffectiveDirection(Vector3 input)
    {
        Vector3 cardinal = Cardinalize(input);
        if (cardinal == Vector3.zero) return Vector3.zero;

        if (isReversed)
        {
            isReversed = false;
            cardinal *= -1f;
        }
        return cardinal;
    }

    private bool IsOnIce()
    {
        if (Physics.Raycast(_transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 1.5f))
            return hit.collider != null && hit.collider.CompareTag("Ice");
        return false;
    }

    private bool CanMove(Vector3 direction)
    {
        if (direction == Vector3.zero) return false;

        Vector3 start = _transform.position;
        float distance = gridSize;

        if (_collider is CapsuleCollider cap)
        {
            float radius = Mathf.Max(0.01f, cap.radius * 0.95f);
            float height = Mathf.Max(cap.height * 0.5f - radius, 0.01f);

            Vector3 centerWS = _transform.TransformPoint(cap.center);
            Vector3 p1 = centerWS + Vector3.up * height;
            Vector3 p2 = centerWS - Vector3.up * height;

            if (Physics.CapsuleCast(p1, p2, radius, direction.normalized, out RaycastHit spikeHit, distance, spikeLayer, QueryTriggerInteraction.Ignore))
            {
                GameManager.Instance.LevelFailed();
                return false;
            }

            bool hit = Physics.CapsuleCast(p1, p2, radius, direction.normalized, out _, distance, obstacleLayer, QueryTriggerInteraction.Ignore);
            return !hit;
        }
        else if (_collider != null)
        {
            float skin = 0.05f;
            Vector3 origin = start + direction.normalized * skin;

            if (Physics.Raycast(origin, direction.normalized, out RaycastHit spikeHit, distance - skin, spikeLayer, QueryTriggerInteraction.Ignore))
            {
                GameManager.Instance.LevelFailed();
                return false;
            }

            bool hit = Physics.Raycast(origin, direction.normalized, distance - skin, obstacleLayer, QueryTriggerInteraction.Ignore);
            return !hit;
        }
        else
        {
            if (Physics.Raycast(start, direction.normalized, out RaycastHit spikeHit, distance, spikeLayer, QueryTriggerInteraction.Ignore))
            {
                GameManager.Instance.LevelFailed();
                return false;
            }

            bool hit = Physics.Raycast(start, direction.normalized, distance, obstacleLayer, QueryTriggerInteraction.Ignore);
            return !hit;
        }
    }

    private void TryStopCoroutine(ref Coroutine coRef)
    {
        if (coRef != null)
        {
            StopCoroutine(coRef);
            coRef = null;
        }
    }
}
