using UnityEngine;

public class KeyGiftBox : MonoBehaviour
{
    [SerializeField]
    GameObject LockedKeyPrefab;
    [SerializeField]
    AudioClip explosionSound;  // Add this so you can assign the sound in Inspector
    [SerializeField]
    int keyboxToSpawn = 1;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            for (int i = 0; i < keyboxToSpawn; i++)
            {
                Vector3 lockedkeyPosition = transform.position + new Vector3(i * 0.5f, 0, 0);
                Instantiate(LockedKeyPrefab, lockedkeyPosition, LockedKeyPrefab.transform.rotation);
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
            Debug.Log("key has spawned.");
        }
    }
}
