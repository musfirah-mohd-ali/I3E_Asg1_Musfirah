using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    [SerializeField]
    Transform teleportTarget;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered teleport zone!");
            other.transform.position = teleportTarget.position;
            Physics.SyncTransforms(); // Ensure physics updates immediately
            Debug.Log("Player teleported!");
        }
    }
}
