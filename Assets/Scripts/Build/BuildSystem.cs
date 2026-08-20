using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem Instance;

    [Header("Selected Turret")]
    public GameObject selectedTurret;
    public int selectedPrice;

    [Header("Build")]
    public bool building;

    void Awake()
    {
        Instance = this;
    }

    public void StartBuilding(GameObject turret, int price)
    {
        if (GameManager.gameEnded)
            return;

        selectedTurret = turret;
        selectedPrice = price;

        building = true;

        Time.timeScale = 0f;

        if (BuildGrid.Instance != null)
            BuildGrid.Instance.ShowGrid();
    }

    public void PlaceTurret(Vector3 position)
    {
        if (!building)
            return;

        if (selectedTurret == null)
            return;

        MoneyManager money =
            FindObjectOfType<MoneyManager>();

        if (money != null)
        {
            bool bought =
                money.SpendMoney(selectedPrice);

            if (!bought)
            {
                Debug.Log("Недостаточно денег");

                CancelBuilding();

                return;
            }
        }

        Instantiate(
            selectedTurret,
            position,
            Quaternion.identity
        );

        GameObject block =
            new GameObject("BuildBlock");

        block.transform.position = position;

        block.AddComponent<BuildBlock>();

        BoxCollider2D col =
            block.AddComponent<BoxCollider2D>();

        col.isTrigger = true;
        col.size = Vector2.one * 0.8f;

        building = false;

        selectedTurret = null;

        if (BuildGrid.Instance != null)
            BuildGrid.Instance.HideGrid();

        Time.timeScale = 1f;
    }

    public void CancelBuilding()
    {
        building = false;

        selectedTurret = null;

        if (BuildGrid.Instance != null)
            BuildGrid.Instance.HideGrid();

        Time.timeScale = 1f;
    }
}