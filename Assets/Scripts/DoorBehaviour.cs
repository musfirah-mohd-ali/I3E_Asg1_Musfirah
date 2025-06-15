/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Handles basic door interaction logic, including toggling door rotation when interacted with.
*/

using UnityEngine;

/// <summary>
/// Controls the open/close behavior of a door when interacted with.
/// </summary>
public class DoorBehaviour : MonoBehaviour
{
    /// <summary>
    /// Indicates whether the door is currently open.
    /// </summary>
    private bool isOpen = false;

    /// <summary>
    /// Indicates whether this is the final door in the game.
    /// Used to trigger game completion logic.
    /// </summary>
    public bool isFinalDoor = false;

    /// <summary>
    /// Toggles the door's rotation between open and closed states.
    /// </summary>
    public void Interact()
    {
        Vector3 doorRotation = transform.eulerAngles;

        if (!isOpen)
        {
            doorRotation.y += 90f;
            isOpen = true;
        }
        else
        {
            doorRotation.y -= 90f;
            isOpen = false;
        }

        transform.eulerAngles = doorRotation;
    }
}
