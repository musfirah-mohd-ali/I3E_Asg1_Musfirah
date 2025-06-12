using UnityEngine;
using TMPro;

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
    [SerializeField]
    AudioClip hazardSound; // drag the hazard sound here in Inspector
    [SerializeField]
    AudioClip respawnSound; // drag the collectible sound here in Inspector
    [SerializeField]
    AudioClip collectibleSound; // drag the collectible sound here in Inspector
    [SerializeField]
    AudioClip finalDoorSound;
    public bool HasLockedKey = false;
    [SerializeField]
    TextMeshProUGUI scoreText; // Reference to the UI Text component for displaying score
    [SerializeField]
    TextMeshProUGUI healthText; // Reference to the UI Text component for displaying health

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoreText.text = "Score: " + score.ToString(); // Initialize the score text
        UpdateHealthText(); // Initialize the health text
    }
    void UpdateHealthText()
    {
        healthText.text = "Health: " + health.ToString(); // Update the health text
    }
    public void ModifyScore(int amount)
    {
        // This method will be called to modify the player's score
        score += amount;
        scoreText.text = "Score: " + score.ToString(); // Update the score text
    }
    void OnInteract()
    {
        // This method will be called when the player interacts with an object
        Debug.Log("Player interacted " + gameObject.name);
        if (canInteract)
        {
            if (doorBehaviour != null)
            {
                Debug.Log("Interacting with door: " + doorBehaviour.gameObject.name);

                if (doorBehaviour.isFinalDoor && finalDoorSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(finalDoorSound);
                    Debug.Log("🎉 Final door sound played!");
                    doorBehaviour.Interact();
                }
                else if (doorBehaviour.CompareTag("LockedDoor"))
                {
                    if (HasLockedKey)
                    {
                        Debug.Log("Interacting with LockedDoor: " + doorBehaviour.gameObject.name);
                        // Check if the door requires a LockedKey
                        if (doorBehaviour != null)
                        {
                            doorBehaviour.Interact(); // Allow interaction only if the door requires a LockedKey
                        }
                    }
                }
                else
                {
                    doorBehaviour.Interact();
                }
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
            ModifyScore(10); ;
            Debug.Log("Collected: " + collision.gameObject.name);
            // Debug.Log("Score: " + score);
            // Debug.Log("Health: " + health);
            if (collectibleSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectibleSound);
            }
            if (health > 100)
            {
                health = 100; // Cap health at 100
               UpdateHealthText(); // Update the health text
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("hazardItems"))
        {
            ModifyScore(-5); // Decrease score by 5 for hitting a hazard item
            Debug.Log("Hit a hazard item: " + collision.gameObject.name);
            health -= 10;
            UpdateHealthText(); // Update the health text
            Debug.Log("Health: " + health);
            Debug.Log("Score: " + score);
            Destroy(collision.gameObject); // Destroy the hazard item
            if (hazardSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hazardSound);
            }
            if (health <= 0)
            {
                health = 0; // Ensure score doesn't go negative
                UpdateHealthText(); // Update the health text
                HandleDeathAndRespawn(); // Handle player death (e.g., respawn, game over)
            }
        }
        else if (collision.gameObject.CompareTag("Hazard"))
        {
            score -= 5;
            Debug.Log("Hit a hazard: " + gameObject.name);
            health -= 10;
            UpdateHealthText(); // Update the health text
            Debug.Log("Health: " + health);
            if (hazardSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hazardSound);
            }
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
            UpdateHealthText(); // Update the health text
            Debug.Log("Health: " + health);
            Debug.Log("Score: " + score);
            if (respawnSound != null && audioSource != null)
                // {
                //     audioSource.PlayOneShot(respawnSound);
                // }
                HandleDeathAndRespawn(); // For simplicity, destroy the player object
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
        else if (other.gameObject.CompareTag("LockedKey"))
        {
            HasLockedKey = true;
            Debug.Log("Collected LockedKey! HasLockedKey: " + HasLockedKey);
            Destroy(other.gameObject);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SpinKeySocket"))
        {
            //Debug.Log("trying to use SpinKeySocket");
            usingSpinKeySocket = other.GetComponent<SpinKeySocket>();
        }
        else if (other.gameObject.CompareTag("healingArea"))
        {
            health += 3;
            UpdateHealthText(); // Update the health text
            Debug.Log("Player entered healing area. Health: " + health);
            if (health >= 100)
            {
                health = 100; // Cap health at 100
                UpdateHealthText(); // Update the health text
                // Debug.Log("Player healed. Health: " + health);
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
        if (respawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(respawnSound);
        }
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
            else if (hitInfo.collider.CompareTag("FinalDoor"))
            {
                doorBehaviour = hitInfo.collider.GetComponent<DoorBehaviour>();
                canInteract = true;
                doorBehaviour.isFinalDoor = true; // Set the door as the final door
            }
            else if (hitInfo.collider.CompareTag("LockedDoor"))
            {
                doorBehaviour = hitInfo.collider.GetComponent<DoorBehaviour>();
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

        else
        {
            collectibleBehaviour = null; // Clear the collectibleBehaviour if no collectible is hit
            doorBehaviour = null; // Clear the doorBehaviour if no door is hit
            canInteract = false; // Set canInteract to false if nothing is hit
        }
    }

    public void SetSpinKeySocket(SpinKeySocket socket)
    {
        usingSpinKeySocket = socket; // Set the current SpinKeySocket
        //Debug.Log("SpinKeySocket set.");
    }
}