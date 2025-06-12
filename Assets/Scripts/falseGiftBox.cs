using UnityEngine;

public class falseGiftBox : MonoBehaviour
{
    [SerializeField]
    GameObject flaseCollectiblePrefab;
    [SerializeField]
    AudioClip explosionSound;  // Add this so you can assign the sound in Inspector
    [SerializeField]
    int giftBoxToSpawn = 1;
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
