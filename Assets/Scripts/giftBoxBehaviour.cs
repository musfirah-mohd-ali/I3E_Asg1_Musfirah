/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Handles behavior when a gift box is hit by a projectile, including playing an explosion sound,
*              hiding the object, and spawning collectible items.
*/

using UnityEngine;

/// <summary>
/// Manages gift box interactions. When hit by a projectile, it plays a sound, spawns collectibles, and destroys itself.
/// </summary>
public class giftBoxBehaviour : MonoBehaviour
{
    /// <summary>
    /// Prefab of the collectible item to spawn when the gift box is hit.
    /// </summary>
    [SerializeField] GameObject collectiblePrefab;

    /// <summary>
    /// Sound effect played when the gift box is destroyed.
    /// </summary>
    [SerializeField] AudioClip explosionSound;

    /// <summary>
    /// Number of collectible items to spawn.
    /// </summary>
    [SerializeField] int giftBoxToSpawn = 1;

    /// <summary>
    /// Reference to the AudioSource component on the gift box.
    /// </summary>
    AudioSource audioSource;

    /// <summary>
    /// Initializes references.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Handles collision with a projectile. Plays sound, spawns collectibles, and destroys the gift box.
    /// </summary>
    /// <param name="collision">Collision data associated with the projectile hit.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Play explosion sound
            if (explosionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(explosionSound);
                GetComponent<MeshRenderer>().enabled = false; // Hide visuals
                GetComponent<Collider>().enabled = false;     // Disable collision
            }
            else
            {
                Debug.LogWarning("AudioSource missing on GiftBox.");
            }

            // Spawn collectibles
            for (int i = 0; i < giftBoxToSpawn; i++)
            {
                Vector3 spawnPos = transform.position + new Vector3(i * 0.5f, i * 0.1f, 0);
                Instantiate(collectiblePrefab, spawnPos, collectiblePrefab.transform.rotation);
            }

            Destroy(collision.gameObject, 0.1f); // Destroy fireball
            Destroy(gameObject, 0.5f); // Destroy gift box
        }
    }
}
