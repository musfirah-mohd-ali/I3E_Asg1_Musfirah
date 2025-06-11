using UnityEngine;

public class DecoyHazardBehaviour : MonoBehaviour
{
    [SerializeField] GameObject spinKeyPrefab; // Assign SpinKey prefab in Inspector
    [SerializeField] int keysToSpawn = 1;

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

            Destroy(collision.gameObject); // Destroy projectile
            Destroy(gameObject); // Destroy decoy hazard
        }
    }
}
