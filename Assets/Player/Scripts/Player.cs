using UnityEngine;

public class Player : MonoBehaviour
{
    // note: GetComponentInParent<Player> can be used to get this component from ANY child of the player, even those who have a sub-parent

    [Header("References")]
    public Rigidbody2D rb;

    [Header("Player Components")]
    public PlayerMovement playerMovement;
    public PlayerVisuals playerVisuals;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // there is probably a better way to do this but i think its sufficient cuz we wont have too many components - amnotagoose
        playerMovement.Initialize();
        playerVisuals.Initialize();
    }
}
