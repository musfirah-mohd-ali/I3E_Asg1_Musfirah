/*
* Author: Musfirah
* Date: 15/06/2025
* Description: Handles player interactions such as collecting items, using keys, interacting with doors and hazards, firing projectiles, and managing UI and audio feedback in a Unity environment.
*/

using UnityEngine;
using TMPro;

/// <summary>
/// Manages player behavior, including interactions, scoring, health, audio, and UI updates.
/// </summary>
public class PlayerBehaviour : MonoBehaviour
{
    // Score and health tracking
    int score = 0;
    int health = 100;

    // References to interactable components
    DoorBehaviour doorBehaviour;
    CollectibleBehaviour collectibleBehaviour;
    bool canInteract = false;

    /// <summary>Indicates whether the player has collected the SpinKey.</summary>
    public bool HasSpinKey = false;

    SpinKeySocket usingSpinKeySocket;

    // Projectile and interaction settings
    [SerializeField] GameObject projectile;
    [SerializeField] float fireStrength = 1000f;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float interactionDistance = 5f;

    // Audio references
    public AudioClip fireSound;
    AudioSource audioSource;
    [SerializeField] AudioClip hazardSound;
    [SerializeField] AudioClip respawnSound;
    [SerializeField] AudioClip collectibleSound;
    [SerializeField] AudioClip finalDoorSound;

    /// <summary>Indicates whether the player has collected the LockedKey.</summary>
    public bool HasLockedKey = false;

    // UI references
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameObject gameCompletePanel;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI finalHealthText;

    /// <summary>
    /// Initializes score, health, audio, and UI elements.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoreText.text = "Score: " + score;
        UpdateHealthText();

        if (gameCompletePanel != null)
            gameCompletePanel.SetActive(false);
    }

    /// <summary>
    /// Updates the UI with the current health value.
    /// </summary>
    void UpdateHealthText()
    {
        healthText.text = "Health: " + health.ToString();
    }

    /// <summary>
    /// Modifies the player's score and updates the score UI.
    /// </summary>
    /// <param name="amount">Amount to add to the score (can be negative).</param>
    public void ModifyScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// Handles interactions with doors, collectibles, or key sockets.
    /// </summary>
    void OnInteract()
    {
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
                    ShowGameCompletePanel();
                }
                else if (doorBehaviour.CompareTag("LockedDoor") && HasLockedKey)
                {
                    doorBehaviour.Interact();
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
                    HasSpinKey = true;
                    Debug.Log("SpinKey collected! HasSpinKey: " + HasSpinKey);
                }
            }
            else if (usingSpinKeySocket != null)
            {
                usingSpinKeySocket.TryUseKey();
                return;
            }
        }
    }

    /// <summary>
    /// Called when the player collides with physical objects.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            ModifyScore(10);
            if (collectibleSound != null && audioSource != null)
                audioSource.PlayOneShot(collectibleSound);
            if (health > 100) health = 100;
            UpdateHealthText();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("hazardItems"))
        {
            ModifyScore(-5);
            health -= 10;
            UpdateHealthText();
            Destroy(collision.gameObject);
            if (hazardSound != null && audioSource != null)
                audioSource.PlayOneShot(hazardSound);
            if (health <= 0)
                HandleDeathAndRespawn();
        }
        else if (collision.gameObject.CompareTag("Hazard"))
        {
            score -= 5;
            health -= 10;
            UpdateHealthText();
            if (hazardSound != null && audioSource != null)
                audioSource.PlayOneShot(hazardSound);
            if (score <= 0) score = 0;
            if (health <= 0)
                HandleDeathAndRespawn();
        }
    }

    /// <summary>
    /// Triggered when the player enters a trigger collider.
    /// </summary>
    /// <param name="other">The collider the player entered.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            collectibleBehaviour = other.GetComponent<CollectibleBehaviour>();
            canInteract = true;
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            doorBehaviour = other.GetComponent<DoorBehaviour>();
            canInteract = true;
        }
        else if (other.gameObject.CompareTag("Killer"))
        {
            health = 0;
            UpdateHealthText();
            HandleDeathAndRespawn();
        }
        else if (other.CompareTag("SpinKeySocket"))
        {
            SpinKeySocket socket = other.GetComponent<SpinKeySocket>();
            if (socket != null) SetSpinKeySocket(socket);
        }
        else if (other.gameObject.CompareTag("SpinKey"))
        {
            HasSpinKey = true;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("LockedKey"))
        {
            HasLockedKey = true;
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// Triggered while staying inside a trigger collider.
    /// </summary>
    /// <param name="other">The collider the player is inside.</param>
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SpinKeySocket"))
        {
            usingSpinKeySocket = other.GetComponent<SpinKeySocket>();
        }
        else if (other.gameObject.CompareTag("healingArea"))
        {
            health += 3;
            if (health >= 100) health = 100;
            UpdateHealthText();
        }
    }

    /// <summary>
    /// Clears the current SpinKeySocket if it matches the provided socket.
    /// </summary>
    /// <param name="socket">The SpinKeySocket to clear.</param>
    void ClearSpinKeySocket(SpinKeySocket socket)
    {
        if (usingSpinKeySocket == socket)
        {
            usingSpinKeySocket = null;
            Debug.Log("SpinKeySocket cleared.");
        }
    }

    /// <summary>
    /// Handles player death and respawning at the spawn position.
    /// </summary>
    void HandleDeathAndRespawn()
    {
        health = 100;
        score = 0;
        transform.position = spawnPosition.position;
        Physics.SyncTransforms();
        if (respawnSound != null && audioSource != null)
            audioSource.PlayOneShot(respawnSound);
    }

    /// <summary>
    /// Fires a projectile in the forward direction.
    /// </summary>
    void OnFire()
    {
        GameObject fireball = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Vector3 fireDirection = spawnPoint.forward * fireStrength;
        fireball.GetComponent<Rigidbody>().AddForce(fireDirection);
        if (audioSource != null && fireSound != null)
            audioSource.PlayOneShot(fireSound);
    }

    /// <summary>
    /// Updates interaction detection and raycasting every frame.
    /// </summary>
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
                doorBehaviour.isFinalDoor = true;
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
                    SetSpinKeySocket(socket);
                    canInteract = true;
                }
            }
        }
        else
        {
            collectibleBehaviour = null;
            doorBehaviour = null;
            canInteract = false;
        }
    }

    /// <summary>
    /// Sets the current SpinKeySocket to be used.
    /// </summary>
    /// <param name="socket">The SpinKeySocket to set.</param>
    public void SetSpinKeySocket(SpinKeySocket socket)
    {
        usingSpinKeySocket = socket;
    }

    /// <summary>
    /// Displays the game complete panel and shows final stats.
    /// </summary>
    void ShowGameCompletePanel()
    {
        if (gameCompletePanel != null)
        {
            gameCompletePanel.SetActive(true);
            finalScoreText.text = "Final Score: " + score;
            finalHealthText.text = "Final Health: " + health;
        }
    }
}
