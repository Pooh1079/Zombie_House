using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildTurretSlot : MonoBehaviour
{
    [Header("UI")]
    public Image icon;

    public TMP_Text priceText;

    [Header("Selected Turret")]
    private TurretData turretData;

    public void Setup(TurretData data)
    {
        turretData = data;

        if (turretData == null)
        {
            ClearSlot();
            return;
        }

        // Иконка
        if (icon != null)
        {
            icon.sprite = turretData.icon;

            icon.enabled =
                turretData.icon != null;

            icon.preserveAspect = true;
        }

        // Цена
        if (priceText != null)
        {
            priceText.text =
                turretData.buildPrice.ToString();
        }
    }

    public void Build()
    {
        if (turretData == null)
        {
            Debug.Log(
                "В этом слоте нет турели!"
            );

            return;
        }

        if (turretData.turretPrefab == null)
        {
            Debug.LogError(
                "У турели " +
                turretData.turretName +
                " не назначен Turret Prefab!"
            );

            return;
        }

        if (BuildSystem.Instance == null)
        {
            Debug.LogError(
                "BuildSystem не найден!"
            );

            return;
        }

        BuildSystem.Instance.StartBuilding(
            turretData.turretPrefab,
            turretData.buildPrice
        );

        // Закрываем BuildMenu
        BuildMenu menu =
            FindObjectOfType<BuildMenu>();

        if (menu != null)
        {
            menu.buildPanel.SetActive(false);
        }

        if (BuildGrid.Instance != null)
        {
            BuildGrid.Instance.ShowGrid();
        }
    }

    public void ClearSlot()
    {
        turretData = null;

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        if (priceText != null)
        {
            priceText.text = "";
        }
    }

    public TurretData GetTurretData()
    {
        return turretData;
    }
}