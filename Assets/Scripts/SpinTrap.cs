using UnityEngine;

public class SpinTrap : MonoBehaviour
{
    [SerializeField]
    float spinSpeed = 180f; // degrees per second

    void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
