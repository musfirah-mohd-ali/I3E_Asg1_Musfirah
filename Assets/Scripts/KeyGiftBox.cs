/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Spawns locked keys and plays a sound when hit by a projectile, then disables and destroys itself.
*/

using UnityEngine;

/// <summary>
/// When hit by a projectile, this gift box spawns locked keys, plays a sound, and destroys itself.
/// </summary>
public class KeyGiftBox : MonoBehaviour
{
    [SerializeField]
    GameObject LockedKeyPrefab; // Prefab for the locked key to spawn

    [SerializeField]
    AudioClip explosionSound;   // Sound to play on collision

    [SerializeField]
    int keyboxToSpawn = 1;      // Number of locked keys to spawn

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Hide visuals and disable collider
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // Spawn locked key(s)
            for (int i = 0; i < keyboxToSpawn; i++)
            {
                Vector3 lockedkeyPosition = transform.position + new Vector3(i * 0.5f, 0, 0);
                Instantiate(LockedKeyPrefab, lockedkeyPosition, LockedKeyPrefab.transform.rotation);
            }

            // Play explosion sound
            if (explosionSound != null)
            {
                GameObject soundPlayerObj = new GameObject("ExplosionSoundPlayer");
                AudioSource source = soundPlayerObj.AddComponent<AudioSource>();
                source.PlayOneShot(explosionSound);
                Destroy(soundPlayerObj, explosionSound.length);
            }

            // Destroy the projectile shortly after
            Destroy(collision.gameObject, 0.1f);

            // Destroy this gift box shortly after
            Destroy(gameObject, 0.5f);

            Debug.Log("Key has spawned.");
        }
    }
}
