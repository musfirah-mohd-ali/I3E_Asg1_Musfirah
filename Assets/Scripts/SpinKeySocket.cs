/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Detects player presence and allows using the SpinKey to stop a SpinTrap.
*/

using UnityEngine;

/// <summary>
/// Represents a socket where the player can use a SpinKey to stop a spinning trap.
/// </summary>
public class SpinKeySocket : MonoBehaviour
{
    /// <summary>
    /// Reference to the SpinTrap to stop.
    /// Assign this in the Inspector.
    /// </summary>
    public SpinTrap spinTrap;

    /// <summary>
    /// Reference to the player currently inside the trigger.
    /// </summary>
    PlayerBehaviour player;

    /// <summary>
    /// Detects when the player enters the trigger zone and caches PlayerBehaviour.
    /// </summary>
    /// <param name="other">Collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerBehaviour>();
        }
    }

    /// <summary>
    /// Clears the player reference when they leave the trigger zone.
    /// </summary>
    /// <param name="other">Collider that exited the trigger.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

    /// <summary>
    /// Attempts to use the SpinKey to stop the SpinTrap.
    /// Called when player presses interact (E).
    /// </summary>
    public void TryUseKey()
    {
        if (player != null && player.HasSpinKey)
        {
            spinTrap.StopSpinning();
            player.HasSpinKey = false;
            Debug.Log("SpinKey used to stop the trap!");
        }
        else
        {
            Debug.Log("No SpinKey to use!");
        }
    }
}
