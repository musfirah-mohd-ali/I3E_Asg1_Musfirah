using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    int score = 0;
    int health = 100;
    DoorBehaviour doorBehaviour;
    CollectibleBehaviour collectibleBehaviour;
    bool canInteract = false;
    public bool HasSpinKey = false;
    SpinKeySocket usingSpinKeySocket;

    [SerializeField]
    GameObject projectile; // The projectile prefab to be instantiated
    [SerializeField]
    float fireStrength = 1000f; // The strength of the fireball
    [SerializeField]
    Transform spawnPosition; // The position where the player will spawn
    [SerializeField]
    Transform spawnPoint; // The projectile prefab to be instantiated
    [SerializeField]
    float interactionDistance = 5f; // the distance at which the player can interact with objects
    public AudioClip fireSound; // drag the fire sound here in Inspector
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void ModifyScore(int amount)
    {
        // This method will be called to modify the player's score
        score += amount;
        Debug.Log("Score: " + score);
    }
    void OnInteract()
    {
        // This method will be called when the player interacts with an object
        Debug.Log("Player interacted " + gameObject.name);
        if (canInteract)
        {
            if (doorBehaviour != null)
            {
                Debug.Log("Interacting with door: " + gameObject.name);
                // Call the Interact method on the doorBehaviour component
                doorBehaviour.Interact();
            }
            else if (collectibleBehaviour != null)
            {
                collectibleBehaviour.Collect(this);
                if (collectibleBehaviour.gameObject.name == "SpinKey")
                {
                    HasSpinKey = true; // Set the flag to true if the collectible is a SpinKey
                    Debug.Log("SpinKey collected! HasSpinKey: " + HasSpinKey);
                }
            }
            else if (usingSpinKeySocket != null)
            {
                Debug.Log("try use key");
                usingSpinKeySocket.TryUseKey(); // Call the method to use the SpinKey
                return; // Exit the method after using the SpinKey
            }

        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // This method will be called when the player collides with another object
        // Debug.Log("Player collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Collectible"))
        {
            score += 10;
            Debug.Log("Collected: " + collision.gameObject.name);
            Debug.Log("Score: " + score);
            Debug.Log("Health: " + health);
            if (health > 100)
            {
                health = 100; // Cap health at 100
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("hazardItems"))
        {
            score -= 5;
            Debug.Log("Hit a hazard item: " + collision.gameObject.name);
            health -= 10;
            Debug.Log("Health: " + health);
            Debug.Log("Score: " + score);
            Destroy(collision.gameObject); // Destroy the hazard item
            if (score <= 0)
            {
                score = 0; // Ensure score doesn't go negative
                HandleDeathAndRespawn(); // Handle player death (e.g., respawn, game over)
            }
        }
        else if (collision.gameObject.CompareTag("Hazard"))
        {
            score -= 5;
            Debug.Log("Hit a hazard: " + gameObject.name);
            health -= 10;
            Debug.Log("Health: " + health);
            if (score <= 0)
            {
                score = 0; // Ensure score doesn't go negative
            }
            if (health <= 0)
            {
                Debug.Log("Player is dead.");
                HandleDeathAndRespawn();
                // Handle player death (e.g., respawn, game over)
                // For simplicity, destroy the player object
            }
        }
        else if (collision.gameObject.CompareTag("giftBox"))
        {
            Debug.Log("Touched a gift box.  damage taken.");
        }


    }
    void OnTriggerEnter(Collider other)
    {
        // This method will be called when the player enters a trigger collider
        // Debug.Log(other.gameObject.name + " entered trigger");
        if (other.gameObject.CompareTag("Collectible"))
        {
            collectibleBehaviour = other.gameObject.GetComponent<CollectibleBehaviour>();
            canInteract = true;
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            doorBehaviour = other.gameObject.GetComponent<DoorBehaviour>();
            canInteract = true;
        }
        else if (other.gameObject.CompareTag("Killer"))
        {
            Debug.Log("Player hit a killer object: " + gameObject.name);
            health = 0; // Set health to 0 to simulate death
            Debug.Log("Health: " + health);
            Debug.Log("Score: " + score);
            HandleDeathAndRespawn(); // For simplicity, destroy the player object
        }
        else if (other.gameObject.CompareTag("healingArea"))
        {
            health += 3;
            Debug.Log("Player entered healing area. Health: " + health);
            if (health >= 100)
            {
                health = 100; // Cap health at 100
                Debug.Log("Player healed. Health: " + health);
            }
        }
        else if (other.CompareTag("SpinKeySocket"))
        {
            SpinKeySocket socket = other.GetComponent<SpinKeySocket>();
            if (socket != null)
            {
                SetSpinKeySocket(socket); // Set the current SpinKeySocket
            }
        }
        else if (other.gameObject.CompareTag("SpinKey"))
        {
            HasSpinKey = true;
            Debug.Log("Collected SpinKey! HasSpinKey: " + HasSpinKey);
            Destroy(other.gameObject);
        }

    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("SpinKeySocket"))
        {
            Debug.Log("trying to use SpinKeySocket");
            SpinKeySocket socket = other.GetComponent<SpinKeySocket>();
            if (socket != null)
            {
                ClearSpinKeySocket(socket); // Clear the current SpinKeySocket
            }
        }
    }
    void ClearSpinKeySocket(SpinKeySocket socket)
    {
        if (usingSpinKeySocket == socket)
        {
            usingSpinKeySocket = null; // Clear the current SpinKeySocket
            Debug.Log("SpinKeySocket cleared.");
        }
    }
    void HandleDeathAndRespawn()
    {
        health = 100;
        score = 0;
        transform.position = spawnPosition.position; // Reset player position    
        Physics.SyncTransforms(); // Ensure the physics engine updates the player's position and rotation
        Debug.Log("Respawned at: " + spawnPosition.position);
    }
    void OnFire()
    {
        // Spawn the projectile
        GameObject fireball = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Vector3 fireDirection = spawnPoint.forward * fireStrength;
        fireball.GetComponent<Rigidbody>().AddForce(fireDirection);
        // Play the fire sound
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or fireSound!");
        }
    }

    void Update()
    {
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.yellow);
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            if (hitInfo.collider.CompareTag("Collectible"))
            {
                collectibleBehaviour = hitInfo.collider.GetComponent<CollectibleBehaviour>();
                canInteract = true;
            }
            else if (hitInfo.collider.CompareTag("Door"))
            {
                doorBehaviour = hitInfo.collider.GetComponent<DoorBehaviour>();
                canInteract = true;
            }
            else if (hitInfo.collider.CompareTag("SpinKeySocket"))
            {
                SpinKeySocket socket = hitInfo.collider.GetComponent<SpinKeySocket>();
                if (socket != null)
                {
                    SetSpinKeySocket(socket); // Set the current SpinKeySocket
                    canInteract = true;
                }
            }
        }
    }
    public void SetSpinKeySocket(SpinKeySocket socket)
    {
        usingSpinKeySocket = socket;
        Debug.Log("SpinKeySocket set.");
    }

}