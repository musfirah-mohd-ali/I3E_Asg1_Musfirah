using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    int score = 0;
    int health = 100;
    DoorBehaviour doorBehaviour;
    bool canInteract = false;
    void OnInteract()
    {
        // This method will be called when the player interacts with an object
        Debug.Log("Player interacted " + gameObject.name);
        if (canInteract)
        {
            if (doorBehaviour != null)
            {
                doorBehaviour.Interact();
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // This method will be called when the player collides with another object
        Debug.Log("Player collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Door"))
        {
            doorBehaviour = collision.gameObject.GetComponent<DoorBehaviour>();
            canInteract = true;
        }
        else if (collision.gameObject.CompareTag("Collectible"))
        {
            score += 10;
            Debug.Log("Collected: " + gameObject.name);
            Destroy(collision.gameObject);
        }
    }
}
