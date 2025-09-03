using UnityEngine;
using UnityEngine.UIElements;

public class DiceInfo : MonoBehaviour {
    public static DiceInfo Instance {
        get; private set;
    }

    private Label diceNameLabel;
    private Label diceValueLabel;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        var root = GetComponent<UIDocument>().rootVisualElement;
        diceNameLabel = root.Q<Label>("dice-name");
        diceValueLabel = root.Q<Label>("dice-value");

        if (diceNameLabel == null)
            Debug.LogError("dice-name label not found in UXML");
        if (diceValueLabel == null)
            Debug.LogError("dice-value label not found in UXML");
    }

    public void ShowDiceInfo(string colorName, int value) {
        if (diceNameLabel != null)
            diceNameLabel.text = $"Color: {colorName}";
        if (diceValueLabel != null)
            diceValueLabel.text = $"Value: {value}";
    }
}