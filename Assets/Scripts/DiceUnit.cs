using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class DiceUnit : MonoBehaviour, IPointerClickHandler {
    private string colorName;
    public string ColorName => colorName;

    private int value;
    public int Value => value;

    private static System.Random rnd = new System.Random();
    private SpriteRenderer spriteRenderer;
    private TextMeshPro numberText;
    private Sprite currentSprite;
    public Sprite CurrentSprite => currentSprite;

    private static Sprite placeholderSprite;

    private readonly Dictionary<int, string> diceSpriteMap = new Dictionary<int, string> {
        {1, "dice_6"},
        {2, "dice_14"},
        {3, "dice_22"},
        {4, "dice_30"},
        {5, "dice_38"},
        {6, "dice_46"}
    };

    public void Init(string colorName, float cellSize) {
        this.colorName = colorName;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        spriteRenderer.material = new Material(Shader.Find("Sprites/Default"));
        spriteRenderer.color = DiceColors.ToUnityColor(colorName);

        value = rnd.Next(1, 7);

        Sprite[] allDiceSprites = Resources.LoadAll<Sprite>("Dices");
        string spriteName = diceSpriteMap.ContainsKey(value) ? diceSpriteMap[value] : "";
        currentSprite = System.Array.Find(allDiceSprites, s => s.name == spriteName);

        if (currentSprite == null) {
            if (placeholderSprite == null)
                placeholderSprite = CreatePlaceholderSprite();
            currentSprite = placeholderSprite;
            spriteRenderer.sprite = currentSprite;
            CreateNumberText();
        } else {
            spriteRenderer.sprite = currentSprite;
        }

        UpdateSize(cellSize);

        // Коллайдер автоматически создаётся через RequireComponent
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = Vector2.one;
        collider.offset = Vector2.zero;
    }

    private static Sprite CreatePlaceholderSprite() {
        Texture2D tex = new Texture2D(16, 16);
        Color[] pixels = new Color[16 * 16];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.white;
        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.width);
    }

    private void CreateNumberText() {
        if (numberText == null) {
            GameObject textObj = new GameObject("NumberText");
            textObj.transform.parent = transform;
            textObj.transform.localPosition = Vector3.zero;

            numberText = textObj.AddComponent<TextMeshPro>();
            numberText.alignment = TextAlignmentOptions.Center;
            numberText.fontSize = 10f;
            numberText.color = Color.black;
            numberText.sortingOrder = 2;
        }

        numberText.text = value.ToString();
        numberText.gameObject.SetActive(true);
    }

    public void UpdateSize(float newCellSize) {
        float scale = newCellSize / currentSprite.bounds.size.x;
        transform.localScale = new Vector3(scale, scale, 1f);
        GetComponent<BoxCollider2D>().size = Vector2.one;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log($"Clicked dice: {colorName} {value}");
        if (DiceInfo.Instance != null) {
            DiceInfo.Instance.ShowDiceInfo(colorName, value);
        }
    }
}