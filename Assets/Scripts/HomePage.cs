using UnityEngine;
using UnityEngine.UIElements;

public class HomePage : MonoBehaviour {
    public UIDocument uiDocument;

    private Button playBtn;
    // private Label gameTitle;

    void Start() {
        var root = uiDocument.rootVisualElement;
        playBtn = root.Q<Button>("play");
        // gameTitle = root.Q<Label>("game-title");

        if (playBtn != null) {
            playBtn.clicked += OnClickStartButton;
        }
    }

    // Update is called once per frame
    void Update() {
        if (playBtn != null) {
            if (Screen.width < Screen.height) {
                // Debug.Log("Portrait");
                playBtn.style.marginTop = 80;

                // if (Screen.width < 600) {
                //     gameTitle.style.fontSize = 32;
                // }
            }
            else {
                // Debug.Log("Landscape");
                playBtn.style.marginTop = 40;
            }
        }
    }

    public void OnClickStartButton() {
        Debug.Log("Start Button Clicked.");
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattlefieldPage");
    }
}
