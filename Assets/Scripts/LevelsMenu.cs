using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelsMenu : MonoBehaviour
{
    private static int levelButtonsPerRow = 5;
    private static Vector2 levelButtonStartingPos = new Vector2(-120, 60);
    private static float levelButtonMargin = 60;

    public GameObject backButton;
    public MenuResourcesController resources;

    private List<GameObject> levelButtons = new List<GameObject>();

    void Start() {
        for (int i=0; i<LevelManager.levelTemplates.Count; i++) {
            LevelTemplate levelTemplate = LevelManager.levelTemplates[i];
            int row = i / levelButtonsPerRow;
            int col = i % levelButtonsPerRow;
            GameObject levelButton = Instantiate(resources.levelButton, CalculatePosition(row, col), Quaternion.identity);
            levelButton.transform.SetParent(gameObject.transform, false);
            AddListener(levelButton.GetComponent<Button>(), i);
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = "" + i;
            levelButtons.Add(levelButton);
        }
        backButton.transform.position = new Vector2(backButton.transform.position.x,
            levelButtonMargin * ((LevelManager.levelTemplates.Count-1) / levelButtonsPerRow + 1));
    }

    private void AddListener(Button button, int parameter) {
        button.onClick.AddListener(delegate { PlayLevel(parameter); });
    }

    private Vector2 CalculatePosition(int row, int col) {
        return new Vector2(levelButtonStartingPos.x + col * levelButtonMargin, levelButtonStartingPos.y - row * levelButtonMargin);
    }

    public void PlayLevel(int level) {
        GameController.CURRENT_LEVEL = level;
        SceneManager.LoadScene("GameScene");
    }
}