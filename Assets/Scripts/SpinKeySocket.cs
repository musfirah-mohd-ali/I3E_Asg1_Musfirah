using UnityEngine;

public class SpinKeySocket : MonoBehaviour
{
    public SpinTrap spinTrap;  // Assign SpinTrap in inspector

    PlayerBehaviour player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerBehaviour>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

    // Call this when player presses interact (E)
    public void TryUseKey()
    {
        if (player.HasSpinKey)
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