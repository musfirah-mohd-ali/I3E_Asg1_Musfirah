using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    MeshRenderer myMeshRenderer; // to change the color of the coin when the player looks at it
    [SerializeField]
    Material highlightMaterial; // the material to use when the player looks at the coin
    Material originalMaterial; // the original material of the coin
    [SerializeField]
    public int collectibleValue = 10; //how much this coin is worth
    //this function is only called if the object has the "Collectible" tag and the player interacts with it
    public void Collect(PlayerBehaviour player)
    {
        player.ModifyScore(collectibleValue); //add the coin's value to the player's score
        Destroy(gameObject); //destroy the coin
    }
    public void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = myMeshRenderer.material; // store the original material of the coin
    }
    public void Highlight()
    {
        myMeshRenderer.material = highlightMaterial; // change the material to the highlight material
    }
    public void Unhighlight()
    {
        myMeshRenderer.material = originalMaterial; // change the material back to the original material
    }
}
