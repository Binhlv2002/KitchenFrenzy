using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => Hide());
    }

    private void Start()
    {
        GameMultiplayer.Instance.OnFailedToJoinGame += GameMultiplayer_OnFailedToJoinGame;
        GameLobby.Instance.OnCreateLobbyStarted += GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed += GameLobby_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted += GameLobby_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed += GameLobby_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed += GameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void GameLobby_OnJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage(GetLocalizedMessage("COULD NOT FIND A LOBBY TO QUICK JOIN", "KHÔNG THỂ TÌM THẤY PHÒNG ĐỂ THAM GIA NHANH"));
    }

    private void GameLobby_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage(GetLocalizedMessage("FAILED TO JOIN LOBBY", "KHÔNG THAM GIA ĐƯỢC PHÒNG"));
    }

    private void GameLobby_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMessage(GetLocalizedMessage("JOINING LOBBY", "ĐANG THAM GIA PHÒNG"));
    }

    private void GameLobby_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        ShowMessage(GetLocalizedMessage("FAILED TO CREATE LOBBY", "KHÔNG THỂ TẠO PHÒNG"));
    }

    private void GameLobby_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage(GetLocalizedMessage("CREATING LOBBY...", "ĐANG TẠO PHÒNG..."));
    }

    private void GameMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        string disconnectReason = NetworkManager.Singleton.DisconnectReason;
        if (string.IsNullOrEmpty(disconnectReason))
        {
            ShowMessage(GetLocalizedMessage("FAILED TO CONNECT", "KHÔNG KẾT NỐI ĐƯỢC"));
        }
        else
        {
            ShowMessage(disconnectReason);
        }
    }

    private string GetLocalizedMessage(string english, string vietnamese)
    {
        return MainMenuUI.GetCurrentLanguage() == "en" ? english : vietnamese;
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnFailedToJoinGame -= GameMultiplayer_OnFailedToJoinGame;
        GameLobby.Instance.OnCreateLobbyStarted -= GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed -= GameLobby_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted -= GameLobby_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed -= GameLobby_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed -= GameLobby_OnQuickJoinFailed;
    }
}
