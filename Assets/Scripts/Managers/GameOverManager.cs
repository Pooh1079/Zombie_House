using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Lose UI")]
    public GameObject gameOverPanel;


    [Header("Build UI")]
    public GameObject buildButton;
    public GameObject buildPanel;



    public void GameOver()
    {
        // такое же состояние как при победе
        GameManager.gameEnded = true;



        // убрать кнопку строительства
        if (buildButton != null)
        {
            buildButton.SetActive(false);
        }



        // закрыть меню турелей
        if (buildPanel != null)
        {
            buildPanel.SetActive(false);
        }



        // открыть проигрыш
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }



        // остановить игру
        Time.timeScale = 0f;



        Debug.Log("ПОРАЖЕНИЕ");
    }





    public void BackToMenu()
    {
        GameManager.gameEnded = false;

        Time.timeScale = 1f;


        SceneManager.LoadScene("MainMenu");
    }
}