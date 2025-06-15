/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Spawns false collectibles and plays an explosion sound when hit by a projectile,
*              then disables its visuals and collider before destroying itself.
*/

using UnityEngine;

/// <summary>
/// Represents a false gift box that spawns fake collectibles when hit by a projectile.
/// </summary>
public class falseGiftBox : MonoBehaviour
{
    /// <summary>
    /// The prefab for the false collectible to spawn.
    /// </summary>
    [SerializeField]
    GameObject flaseCollectiblePrefab;

    /// <summary>
    /// The sound clip to play upon explosion.
    /// </summary>
    [SerializeField]
    AudioClip explosionSound;

    /// <summary>
    /// The number of false collectibles to spawn.
    /// </summary>
    [SerializeField]
    int giftBoxToSpawn = 1;

    /// <summary>
    /// Called when this object collides with another collider.
    /// Checks if the collider is a projectile to trigger the false gift box behavior.
    /// </summary>
    /// <param name="collision">Collision data associated with the collision event.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            for (int i = 0; i < giftBoxToSpawn; i++)
            {
                Vector3 falsecollectiblePosition = transform.position + new Vector3(i * 0.5f, 0, 0);
                Instantiate(flaseCollectiblePrefab, falsecollectiblePosition, flaseCollectiblePrefab.transform.rotation);
            }

            if (explosionSound != null)
            {
                GameObject soundPlayerObj = new GameObject("ExplosionSoundPlayer");
                AudioSource source = soundPlayerObj.AddComponent<AudioSource>();
                source.PlayOneShot(explosionSound);
                Destroy(soundPlayerObj, explosionSound.length);
            }

            Destroy(collision.gameObject, 0.1f);
            Destroy(gameObject, 0.5f);

            Debug.Log("False gift box destroyed and collectibles spawned.");
        }
    }
}
