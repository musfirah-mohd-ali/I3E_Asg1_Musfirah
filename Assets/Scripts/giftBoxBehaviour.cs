using UnityEngine;
public class giftBoxBehaviour : MonoBehaviour
{
    [SerializeField] GameObject collectiblePrefab;
    [SerializeField] AudioClip explosionSound; // Drag your clip in Inspector
    [SerializeField] int giftBoxToSpawn = 1;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Play explosion sound
            if (explosionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(explosionSound);
                GetComponent<MeshRenderer>().enabled = false; // hide visuals
                GetComponent<Collider>().enabled = false;     // disable collision

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
            Destroy(gameObject,0.5f); // Destroy gift box
            
        }
    }
}
