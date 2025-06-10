using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    int score = 0;
    int health = 100;
    DoorBehaviour doorBehaviour;
    CollectibleBehaviour collectibleBehaviour;
    bool canInteract = false;

    public void ModifyScore(int amount)
    {
        // This method will be called to modify the player's score
        score += amount;
        Debug.Log("Score: " + score);
    }
    void OnInteract()
    {
        // This method will be called when the player interacts with an object
        // Debug.Log("Player interacted " + gameObject.name);
        if (canInteract)
        {
            if (doorBehaviour != null)
            {
                doorBehaviour.Interact();
            }
            else if (collectibleBehaviour != null)
            {
                collectibleBehaviour.Collect(this);
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
        }
        else if (collision.gameObject.CompareTag("Hazard"))
        {
            score -= 5;
            Debug.Log("Hit a hazard: " + gameObject.name);
            health -= 10;
            Debug.Log("Health: " + health);
            Debug.Log("Score: " + score);
            if (health <= 0)
            {
                Debug.Log("Player is dead.");
                // Handle player death (e.g., respawn, game over)
                Destroy(gameObject); // For simplicity, destroy the player object
            }
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
            Destroy(gameObject); // For simplicity, destroy the player object
        }
        else if (other.gameObject.CompareTag("healingArea"))
        {
            health += 3;
            if (health > 100)
            {
                health = 100; // Cap health at 100
                Debug.Log("Player healed. Health: " + health);
            }
        }
    }
}
