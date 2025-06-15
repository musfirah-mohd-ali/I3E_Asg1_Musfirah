/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Handles the behavior of a decoy hazard that spawns SpinKeys when hit by a projectile.
*/

using UnityEngine;

/// <summary>
/// When hit by a projectile, this decoy hazard spawns SpinKey pickups and destroys itself.
/// </summary>
public class DecoyHazardBehaviour : MonoBehaviour
{
    [SerializeField] 
    GameObject spinKeyPrefab; // SpinKey prefab to spawn. Assign in Inspector.

    [SerializeField] 
    int keysToSpawn = 1; // Number of SpinKeys to spawn when hit.

    /// <summary>
    /// Called when a collision happens.
    /// If the collider is tagged "Projectile", spawn SpinKeys and destroy the projectile and this object.
    /// </summary>
    /// <param name="collision">Collision info.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Fake decoy hazard hit by projectile!");

            for (int i = 0; i < keysToSpawn; i++)
            {
                Vector3 spawnPos = transform.position + new Vector3(i * 0.5f, 0, 0);
                Instantiate(spinKeyPrefab, spawnPos, spinKeyPrefab.transform.rotation);
            }

            Destroy(collision.gameObject); // Destroy the projectile
            Destroy(gameObject);           // Destroy the decoy hazard itself
        }
    }
}
