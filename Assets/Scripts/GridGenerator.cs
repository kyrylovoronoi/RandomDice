using UnityEngine;
using System.Collections.Generic;

public class GridGenerator {
    private int rows;
    private int columns;
    private const float gapPercent = 0.05f; // GAP в процентах от размера ячейки
    private float widthPercent;
    private float gridWidth;
    private float cellSize;
    private float gap;
    private Vector2 fieldOffset;

    private int[,] cubesMatrix; // логическая матрица кубов
    private List<Vector2> cellPositions = new List<Vector2>();

    private int lastScreenWidth;
    private int lastScreenHeight;

    public int Rows => rows;
    public int Columns => columns;
    public float Gap => gap;
    public float CellSize => cellSize;
    public int[,] CubesMatrix => cubesMatrix;

    public GridGenerator(int rows = 3, int columns = 3, float widthPercent = 0.8f) {
        this.rows = rows;
        this.columns = columns;
        this.widthPercent = widthPercent;

        RecalculateCellSize();
        GeneratePositions();
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }

    public void RecalculateCellSize() {
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        float minSide = Mathf.Min(screenWidth, screenHeight);
        gridWidth = minSide * widthPercent;

        cellSize = gridWidth / (columns + gapPercent * (columns - 1));
        gap = cellSize * gapPercent;

        float fieldActualSize = columns * cellSize + (columns - 1) * gap;
        fieldOffset = new Vector2(-fieldActualSize / 2 + cellSize / 2, -fieldActualSize / 2 + cellSize / 2);
    }

    public void GeneratePositions() {
        cellPositions.Clear();
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < columns; c++) {
                Vector2 pos = new Vector2(c * (cellSize + gap), r * (cellSize + gap)) + fieldOffset;
                cellPositions.Add(pos);
            }
        }
    }

    // Возвращает координату для конкретной ячейки
    public Vector2 GetPosition(int row, int col) {
        return new Vector2(col * (cellSize + gap), row * (cellSize + gap)) + fieldOffset;
    }

    // Создание случайной матрицы кубов
    public void GenerateCubesMatrix(int diceCount) {
        cubesMatrix = new int[rows, columns];

        int placed = 0;
        System.Random rnd = new System.Random();
        while (placed < diceCount) {
            int r = rnd.Next(rows);
            int c = rnd.Next(columns);
            if (cubesMatrix[r, c] == 0) {
                cubesMatrix[r, c] = 1;
                placed++;
            }
        }
    }

    public void DrawGrid() {
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < columns; c++) {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
                cell.name = $"Cell_{r}_{c}";
                Vector2 pos = GetPosition(r, c);
                cell.transform.position = new Vector3(pos.x, pos.y, 1f);
                cell.transform.localScale = Vector3.one * cellSize;

                Renderer rend = cell.GetComponent<Renderer>();
                rend.material = new Material(Shader.Find("Sprites/Default"));
                rend.material.color = Color.white;
            }
        }
    }

    public void UpdateGridVisual() {
        for (int r = 0; r < rows; r++) {
            for (int c = 0; c < columns; c++) {
                GameObject cell = GameObject.Find($"Cell_{r}_{c}");
                if (cell != null) {
                    Vector2 pos = GetPosition(r, c);
                    cell.transform.position = new Vector3(pos.x, pos.y, 1f);
                    cell.transform.localScale = Vector3.one * cellSize;
                }
            }
        }
    }

    public bool ScreenSizeChanged() {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight) {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            return true;
        }
        return false;
    }
}