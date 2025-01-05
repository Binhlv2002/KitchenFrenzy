using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player;
    private Animator animator;

    private void Awake()
    {
        if (player != null && player.PlayerVisual != null)
        {
            animator = player.PlayerVisual.GetAnimator();
            if (animator == null)
            {
                Debug.LogError("Animator không được tìm thấy!");
            }
        }
    }


    private void Update()
    {
        if (!IsOwner) return;

        if (animator != null)
        {
            animator.SetBool(IS_WALKING, player.IsWalking());
        }

        Debug.Log("IsWalking: " + player.IsWalking());
        Debug.Log("IsOwner: " + IsOwner);
    }
}
