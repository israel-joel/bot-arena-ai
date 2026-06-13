using UnityEngine;

public enum CellType
{
    Empty,
    Obstacle,
    Bot1,
    Bot2,
    BonusHealth,
    BonusAmmo
}

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;

    public CellType[,] grid;
    public GameObject[,] cellObjects;

    // Sprites à associer
    public Sprite emptySprite;
    public Sprite obstacleSprite;
    public Sprite bot1Sprite;
    public Sprite bot2Sprite;
    public Sprite bonusHealthSprite;
    public Sprite bonusAmmoSprite;

    void Start()
    {
        InitGrid();
        GenerateVisualGrid();
    }

    void InitGrid()
    {
        grid = new CellType[width, height];
        cellObjects = new GameObject[width, height];

        // Initialisation à Empty
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = CellType.Empty;

        // Exemples de placement manuel (à adapter)
        grid[2, 2] = CellType.Obstacle;
        grid[1, 1] = CellType.Bot1;
        grid[8, 8] = CellType.Bot2;
        grid[3, 4] = CellType.BonusHealth;
        grid[5, 5] = CellType.BonusAmmo;
    }

    void GenerateVisualGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cell = new GameObject($"Cell_{x}_{y}");
                cell.transform.position = new Vector3(x * cellSize, y * cellSize, 0);
                cell.transform.parent = this.transform;

                SpriteRenderer sr = cell.AddComponent<SpriteRenderer>();
                sr.sprite = GetSpriteForCell(grid[x, y]);

                cellObjects[x, y] = cell;
            }
        }
    }

    Sprite GetSpriteForCell(CellType type)
    {
        switch (type)
        {
            case CellType.Obstacle: return obstacleSprite;
            case CellType.Bot1: return bot1Sprite;
            case CellType.Bot2: return bot2Sprite;
            case CellType.BonusHealth: return bonusHealthSprite;
            case CellType.BonusAmmo: return bonusAmmoSprite;
            default: return emptySprite;
        }
    }
}

