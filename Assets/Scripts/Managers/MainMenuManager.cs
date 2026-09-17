using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scenes")]
    public string inventorySceneName = "Inventory";

    public void OpenInventory()
    {
        Debug.Log("Открываем инвентарь");

        SceneManager.LoadScene(inventorySceneName);
    }
}