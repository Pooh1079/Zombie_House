using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public static bool gameEnded = false;



    [Header("Zombies")]
    public int totalZombies = 10;

    private int killedZombies = 0;



    [Header("UI")]
    public TextMeshProUGUI zombieText;

    public GameObject winPanel;

    public GameObject losePanel;

    public GameObject buildButton;

    public GameObject buildPanel;



    private void Awake()
    {
        Instance = this;
    }



    void Start()
    {
        gameEnded = false;

        Time.timeScale = 1f;


        if (winPanel != null)
            winPanel.SetActive(false);


        if (losePanel != null)
            losePanel.SetActive(false);


        if (buildButton != null)
            buildButton.SetActive(true);


        UpdateZombieText();
    }





    public void ZombieKilled()
    {
        if (killedZombies >= totalZombies)
        {
            WaveManager.Instance.WaveCompleted();
        }
        if (gameEnded)
            return;


        killedZombies++;


        UpdateZombieText();


        if (killedZombies >= totalZombies)
        {
            WinGame();
        }
    }





    void UpdateZombieText()
    {
        if (zombieText != null)
        {
            zombieText.text =
            killedZombies + "/" + totalZombies;
        }
    }





    public void WinGame()
    {
        if (gameEnded)
            return;


        gameEnded = true;


        StopGame();


        if (winPanel != null)
            winPanel.SetActive(true);


        Debug.Log("ПОБЕДА");
    }





    public void LoseGame()
    {
        if (gameEnded)
            return;


        gameEnded = true;


        StopGame();


        if (losePanel != null)
            losePanel.SetActive(true);


        Debug.Log("ПОРАЖЕНИЕ");
    }





    void StopGame()
    {
        // убираем кнопку строить
        if (buildButton != null)
        {
            buildButton.SetActive(false);
        }



        // закрываем меню турелей
        if (buildPanel != null)
        {
            buildPanel.SetActive(false);
        }



        // стоп всей игры
        Time.timeScale = 0f;
    }





    public void NextLevel()
    {
        gameEnded = false;

        Time.timeScale = 1f;


        SceneManager.LoadScene(
        SceneManager.GetActiveScene().buildIndex + 1);
    }
    // ===== Система волн =====

    [HideInInspector]
    public int aliveZombies = 0;

    [HideInInspector]
    public bool spawningFinished = false;

    public void ZombieSpawned()
    {
        aliveZombies++;
    }

    public void ZombieDead()
    {
        aliveZombies--;

        if (aliveZombies < 0)
            aliveZombies = 0;

        Debug.Log("Живых зомби: " + aliveZombies);

        if (spawningFinished && aliveZombies == 0)
        {
            spawningFinished = false;
            ZombieSpawner.finishedSpawners = 0;

            if (WaveManager.Instance != null)
                WaveManager.Instance.WaveCompleted();
        }
    }
}