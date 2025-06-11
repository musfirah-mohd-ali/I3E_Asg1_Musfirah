using UnityEngine;

public class giftBoxBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject collectiblePrefab;
    [SerializeField]
    int giftBoxToSpawn = 1;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
            Debug.Log("Projectile hit the gift box: " + gameObject.name);
        {
            for (int i = 0; i < giftBoxToSpawn; i++)
            {
                Debug.Log("Spawning collectible " + (i + 1) + " at position: " + transform.position);
                Vector3 collectiblePosition = transform.position + new Vector3(i * 0.5f, i *0.05f, 0);
                Instantiate(collectiblePrefab, collectiblePosition, collectiblePrefab.transform.rotation);
            }
        }
        Destroy(collision.gameObject);
        Destroy(gameObject);
        Debug.Log("Gift box destroyed and collectibles spawned.");
    }
}
