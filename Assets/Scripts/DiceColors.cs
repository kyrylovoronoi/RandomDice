using UnityEngine;
using System.Collections.Generic;

public static class DiceColors {
    private static readonly string[] colors = { "red", "blue", "green", "yellow", "purple", "orange" };

    public static List<string> GetAllColors() {
        return new List<string>(colors);
    }

    public static List<string> GetUniqueRandomColors(int count) {
        List<string> shuffled = new List<string>(colors);
        for (int i = 0; i < shuffled.Count; i++) {
            int rnd = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[rnd]) = (shuffled[rnd], shuffled[i]);
        }
        return shuffled.GetRange(0, Mathf.Min(count, shuffled.Count));
    }

    public static Color ToUnityColor(string name) {
        return name.ToLower() switch {
            "red" => Color.red,
            "blue" => Color.blue,
            "green" => Color.green,
            "yellow" => Color.yellow,
            "purple" => new Color(0.5f, 0f, 0.5f),
            "orange" => new Color(1f, 0.5f, 0f),
            _ => Color.white
        };
    }
}