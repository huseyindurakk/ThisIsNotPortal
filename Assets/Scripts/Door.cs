using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator door;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.SetBool("isOpen", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        door.SetBool("isOpen", false);
    }
}
