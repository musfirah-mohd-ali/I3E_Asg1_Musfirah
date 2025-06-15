/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Handles the logic for collectible items (e.g., coins) that increase the player's score when collected.
*/

using UnityEngine;

/// <summary>
/// Controls the behavior of collectible objects that can increase the player's score.
/// </summary>
public class CollectibleBehaviour : MonoBehaviour
{
    /// <summary>
    /// The score value of this collectible item.
    /// </summary>
    public int collectibleValue = 10;

    /// <summary>
    /// Adds this collectible's value to the player's score.
    /// Only called if the object has the "Collectible" tag and the player interacts with it.
    /// </summary>
    /// <param name="player">Reference to the player interacting with the collectible.</param>
    public void Collect(PlayerBehaviour player)
    {
        player.ModifyScore(collectibleValue);
    }
}
