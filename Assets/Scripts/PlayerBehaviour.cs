using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    void OnInteract()
    {
        // This method will be called when the player interacts with an object
        Debug.Log("Player interacted " + gameObject.name);
    }
    void OnCollisionEnter(Collision collision)
    {
        // This method will be called when the player collides with another object
        Debug.Log("Player collided with " + collision.gameObject.name);
    }
}
