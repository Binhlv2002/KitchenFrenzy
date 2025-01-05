using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            Debug.Log("HOST");
            GameMultiplayer.Instance.StartHost();
            Hide();
        });

        clientButton.onClick.AddListener(() =>
        {
            Debug.Log("ClIENT");
            GameMultiplayer.Instance.StartClient();
            Hide();
        });


    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
