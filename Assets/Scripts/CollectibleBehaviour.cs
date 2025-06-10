using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    public int collectibleValue = 10; //how much this coin is worth
    //this function is only called if the object has the "Collectible" tag and the player interacts with it
    public void Collect(PlayerBehaviour player)
    {
        player.ModifyScore(collectibleValue); //add the coin's value to the player's score
        Destroy(gameObject); //destroy the coin
    }
}
