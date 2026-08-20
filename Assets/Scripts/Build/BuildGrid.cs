using UnityEngine;

public class BuildGrid : MonoBehaviour
{
    public static BuildGrid Instance;

    [Header("Grid")]
    public GameObject cellPrefab;

    public int width = 40;
    public int height = 25;

    public float cellSize = 1f;

    public Vector2 startPosition;

    private BuildCell[,] cells;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowGrid()
    {
        HideGrid();

        cells = new BuildCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(
                    startPosition.x + x * cellSize,
                    startPosition.y + y * cellSize,
                    0f);

                GameObject obj = Instantiate(
                    cellPrefab,
                    pos,
                    Quaternion.identity,
                    transform);

                obj.transform.localScale =
                    Vector3.one * (cellSize * 0.9f);

                BuildCell cell = obj.GetComponent<BuildCell>();

                cells[x, y] = cell;

                bool blocked = false;

                Collider2D[] hits = Physics2D.OverlapBoxAll(
                    pos,
                    Vector2.one * cellSize * 0.8f,
                    0f);

                foreach (Collider2D hit in hits)
                {
                    if (hit == null)
                        continue;

                    // Круг радиуса не блокирует строительство
                    if (hit.GetComponent<Turret>() != null)
                    {
                        continue;
                    }

                    if (hit.GetComponent<BuildBlock>() != null)
                    {
                        blocked = true;
                        break;
                    }
                }

                cell.SetBlocked(blocked);
            }
        }
    }

    public void HideGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}