using UnityEngine;

public class SpinTrap : MonoBehaviour
{
    public float spinSpeed = 180f;
    bool isSpinning = true;

    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        }
    }

    public void StopSpinning()
    {
        isSpinning = false;
        Debug.Log("Spin trap stopped!");
    }
}
