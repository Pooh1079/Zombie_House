using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public GameObject buildPanel;


    public void OpenBuild()
    {
        // нельзя строить после победы/поражения
        if (GameManager.gameEnded)
            return;


        buildPanel.SetActive(true);


        // замораживаем игру во время выбора турели
        Time.timeScale = 0f;
    }



    public void CloseBuild()
    {
        buildPanel.SetActive(false);

        // возвращаем игру
        if (!GameManager.gameEnded)
        {
            Time.timeScale = 1f;
        }
    }
}