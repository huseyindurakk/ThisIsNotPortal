using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        Instance = this;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeCamera()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }
}
