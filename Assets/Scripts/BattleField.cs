using UnityEngine;
using System.Collections.Generic;

public class BattleField : MonoBehaviour {
    public static BattleField Instance {
        get; private set;
    }

    private const int DiceCount = 3;
    private List<DiceUnit> diceUnits = new List<DiceUnit>();
    private GridGenerator grid;
    private static System.Random rnd = new System.Random();

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        grid = new GridGenerator(rows: 3, columns: 4, widthPercent: 0.8f);
        grid.DrawGrid();

        List<string> allColors = DiceColors.GetAllColors();
        if (allColors.Count == 0)
            allColors.Add("white");

        grid.GenerateCubesMatrix(DiceCount);

        for (int r = 0; r < grid.Rows; r++) {
            for (int c = 0; c < grid.Columns; c++) {
                if (grid.CubesMatrix[r, c] == 1) {
                    int colorIndex = rnd.Next(allColors.Count);
                    string colorName = allColors[colorIndex];
                    allColors.RemoveAt(colorIndex);

                    GameObject diceObject = new GameObject($"Dice_{r}_{c}");
                    DiceUnit dice = diceObject.AddComponent<DiceUnit>();
                    dice.Init(colorName, grid.CellSize);

                    Vector2 pos = grid.GetPosition(r, c);
                    diceObject.transform.position = new Vector3(pos.x, pos.y, 0f);

                    diceUnits.Add(dice);
                }
            }
        }
    }

    private void Update() {
        if (grid == null || diceUnits.Count == 0)
            return;

        if (grid.ScreenSizeChanged()) {
            grid.RecalculateCellSize();
            grid.UpdateGridVisual();

            int diceIndex = 0;
            for (int r = 0; r < grid.Rows; r++) {
                for (int c = 0; c < grid.Columns; c++) {
                    if (grid.CubesMatrix[r, c] == 1) {
                        if (diceIndex >= diceUnits.Count)
                            break;

                        Vector2 pos = grid.GetPosition(r, c);
                        diceUnits[diceIndex].transform.position = new Vector3(pos.x, pos.y, 0f);
                        diceUnits[diceIndex].UpdateSize(grid.CellSize);
                        diceIndex++;
                    }
                }
            }
        }
    }
}