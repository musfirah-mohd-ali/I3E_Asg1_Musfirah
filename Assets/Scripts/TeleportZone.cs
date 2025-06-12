using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    [SerializeField]
    Transform teleportTarget;
    bool isPressed = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            isPressed = true;
            Debug.Log("Pushable object entered teleport zone!");
        }
        if (other.CompareTag("Player") && isPressed)
        {
            Debug.Log("Player entered teleport zone!");
            other.transform.position = teleportTarget.position;
            Physics.SyncTransforms(); // Ensure physics updates immediately
            Debug.Log("Player teleported!");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            isPressed = false;
            Debug.Log("Pushable object exited teleport zone!");
        }
    }
}
