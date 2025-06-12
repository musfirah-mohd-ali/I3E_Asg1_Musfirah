using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private bool isOpen = false;
    public bool isFinalDoor = false;

    public void Interact()
    {
        Vector3 doorRotation = transform.eulerAngles;
        if (!isOpen)
        {
            doorRotation.y += 90f;
            isOpen = true;
        }
        else
        {
            doorRotation.y -= 90f;
            isOpen = false;
        }
        transform.eulerAngles = doorRotation;

    }
}


