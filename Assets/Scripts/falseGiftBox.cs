using UnityEngine;

public class falseGiftBox : MonoBehaviour
{
    [SerializeField]
    GameObject flaseCollectiblePrefab;
    [SerializeField]
    int giftBoxToSpawn = 1;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            for (int i = 0; i < giftBoxToSpawn; i++)
            {
                Vector3 falsecollectiblePosition = transform.position + new Vector3(i * 0.5f, 0, 0);
                Instantiate(flaseCollectiblePrefab, falsecollectiblePosition, flaseCollectiblePrefab.transform.rotation);
            }
        }
        Destroy(collision.gameObject);
        Destroy(gameObject);
        Debug.Log("Gift box destroyed and collectibles spawned.");
    }
}
