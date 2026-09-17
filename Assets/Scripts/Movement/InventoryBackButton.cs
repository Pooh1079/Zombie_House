using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryBackButton : MonoBehaviour
{
    public void BackToMenu()
    {
        Debug.Log("СОХРАНЯЕМ ИНВЕНТАРЬ И ВОЗВРАЩАЕМСЯ В MAINMENU");

        // Сохраняем выбранные турели
        if (GameSaveSystem.Instance != null)
        {
            GameSaveSystem.Instance.SaveAll();

            Debug.Log("ТУРЕЛИ СОХРАНЕНЫ");
        }
        else
        {
            Debug.LogWarning(
                "GameSaveSystem не найден. Сохранение не выполнено."
            );
        }

        // Открываем MainMenu
        SceneManager.LoadScene("MainMenu");
    }
}