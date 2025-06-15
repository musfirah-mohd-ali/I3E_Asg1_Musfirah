/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Teleports the player to a target location only after a "Pushable" object has entered the zone.
*/

using UnityEngine;

/// <summary>
/// Controls a teleportation zone that only activates for the player
/// after a pushable object enters the zone.
/// </summary>
public class TeleportZone : MonoBehaviour
{
    /// <summary>
    /// The target location to teleport the player to.
    /// </summary>
    [SerializeField]
    Transform teleportTarget;

    /// <summary>
    /// Indicates whether the pushable object is in the teleport zone.
    /// </summary>
    bool isPressed = false;

    /// <summary>
    /// Called when another collider enters the trigger zone.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
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

    /// <summary>
    /// Called when another collider exits the trigger zone.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            isPressed = false;
            Debug.Log("Pushable object exited teleport zone!");
        }
    }
}
