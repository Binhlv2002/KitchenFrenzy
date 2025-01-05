using Unity.Netcode;
using Cinemachine;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (virtualCamera != null)
            {
                virtualCamera.Follow = transform;
                virtualCamera.LookAt = transform;
            }
            else
            {
                Debug.LogError("Virtual Camera not found in the scene!");
            }
        }
    }
}
