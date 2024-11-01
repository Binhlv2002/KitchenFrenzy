using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button exitToMenuButton;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        playAgainButton.onClick.AddListener(RestartGame);
        exitToMenuButton.onClick.AddListener(ExitToMenu);
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void RestartGame()
    {
        // Tải lại cảnh hiện tại để bắt đầu lại trò chơi
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitToMenu()
    {
        // Tải cảnh menu chính (MainMenuScene) 
        SceneManager.LoadScene("MainMenuScene");
    }
}
