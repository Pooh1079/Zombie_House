using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;


    [Header("Waves")]
    public int maxWaves = 5;
    public int currentWave = 1;


    [Header("UI")]
    public TextMeshProUGUI waveText;
    public Button startButton;


    [Header("Auto Skip")]
    public bool autoSkip = false;
    public Image autoSkipImage;

    public Color onColor = Color.green;
    public Color offColor = Color.red;


    public bool waveStarted = false;


    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        UpdateWaveText();
        UpdateAutoSkip();


        if (startButton != null)
            startButton.gameObject.SetActive(true);
    }




    public void StartWave()
    {
        if (waveStarted)
            return;


        waveStarted = true;


        if (startButton != null)
            startButton.gameObject.SetActive(false);



        Debug.Log("Старт волны: " + currentWave);



        foreach (ZombieSpawner spawner in ZombieSpawner.AllSpawners)
        {
            spawner.StartWave(currentWave - 1);
        }
    }




    public void WaveCompleted()
    {
        if (!waveStarted)
            return;


        waveStarted = false;



        currentWave++;



        if (currentWave > maxWaves)
        {
            Debug.Log("Все волны пройдены");
            return;
        }



        UpdateWaveText();



        if (autoSkip)
        {
            Invoke(nameof(StartWave), 1f);
        }
        else
        {
            if (startButton != null)
                startButton.gameObject.SetActive(true);
        }
    }





    public void ToggleAutoSkip()
    {
        autoSkip = !autoSkip;


        UpdateAutoSkip();


        Debug.Log(
            "Авто-скип: " + autoSkip
        );
    }





    void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text =
            "Волна: " + currentWave + "/" + maxWaves;
        }
    }





    void UpdateAutoSkip()
    {
        if (autoSkipImage != null)
        {
            autoSkipImage.color =
            autoSkip ? onColor : offColor;
        }
    }
}