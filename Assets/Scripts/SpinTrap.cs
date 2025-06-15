/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Controls a spinning trap hazard that rotates continuously until stopped.
*/

using UnityEngine;

/// <summary>
/// A spinning trap that rotates around its Z-axis at a specified speed.
/// Can be stopped via the StopSpinning method.
/// </summary>
public class SpinTrap : MonoBehaviour
{
    /// <summary>
    /// Rotation speed in degrees per second.
    /// </summary>
    public float spinSpeed = 180f;

    /// <summary>
    /// Whether the trap is currently spinning.
    /// </summary>
    bool isSpinning = true;

    /// <summary>
    /// Rotates the trap every frame if spinning is enabled.
    /// </summary>
    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Stops the spinning motion of the trap.
    /// </summary>
    public void StopSpinning()
    {
        isSpinning = false;
        Debug.Log("Spin trap stopped!");
    }
}
