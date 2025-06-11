using UnityEngine;

public class SpinKeySocket : MonoBehaviour
{
    public SpinTrap spinTrap;  // Assign SpinTrap in inspector

    bool playerNearby = false;
    PlayerBehaviour player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            player = other.GetComponent<PlayerBehaviour>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            player = null;
        }
    }

    // Call this when player presses interact (E)
    public void TryUseKey()
    {
        if (playerNearby && player.HasSpinKey)
        {
            spinTrap.StopSpinning();
            player.HasSpinKey = false;
            Debug.Log("SpinKey used to stop the trap!");
        }
        else
        {
            Debug.Log("No SpinKey to use!");
        }
    }

}
