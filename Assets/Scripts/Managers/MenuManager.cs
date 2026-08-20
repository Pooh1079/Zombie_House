using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OpenSelectMenu()
    {
        SceneManager.LoadScene("SelectMenu");
    }

    public void OpenBattleground()
    {
        SceneManager.LoadScene("Battleground");
    }
}