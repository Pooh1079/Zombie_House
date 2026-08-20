using System.Collections.Generic;
using UnityEngine;

public class TurretShopItem : MonoBehaviour
{
    [System.Serializable]
    public class ShopTurret
    {
        public string turretName;

        public GameObject turretPrefab;

        public int price = 50;

        public Sprite icon;
    }

    [Header("Turrets")]
    public List<ShopTurret> turrets = new List<ShopTurret>();

    [Header("Selected")]
    public int selectedTurret = 0;

    public void SelectTurret()
    {
        if (selectedTurret < 0 || selectedTurret >= turrets.Count)
            return;

        BuildSystem.Instance.StartBuilding(
            turrets[selectedTurret].turretPrefab,
            turrets[selectedTurret].price
        );

        BuildMenu menu = FindObjectOfType<BuildMenu>();

        if (menu != null)
        {
            menu.buildPanel.SetActive(false);
        }

        BuildGrid.Instance.ShowGrid();
    }

    public void SelectTurret(int index)
    {
        if (index < 0 || index >= turrets.Count)
            return;

        selectedTurret = index;

        SelectTurret();
    }
}