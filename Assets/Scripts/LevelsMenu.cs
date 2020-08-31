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

    public MenuResourcesController resources;

    void Start() {
        foreach (KeyValuePair<int, LevelTemplate> levelTemplate in LevelManager.levelTemplates) {
            int row = (levelTemplate.Key-1) / levelButtonsPerRow;
            int col = (levelTemplate.Key-1) % levelButtonsPerRow;
            GameObject levelButton = Instantiate(resources.levelButton, CalculatePosition(row, col), Quaternion.identity);
            levelButton.transform.SetParent(gameObject.transform, false);
            levelButton.GetComponent<Button>().onClick.AddListener(() => PlayLevel(levelTemplate.Key));
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = "" + levelTemplate.Key;
        }
    }

    private Vector2 CalculatePosition(int row, int col) {
        return new Vector2(levelButtonStartingPos.x + col * levelButtonMargin, levelButtonStartingPos.y - row * levelButtonMargin);
    }

    public void PlayLevel(int level) {
        GameController.CURRENT_LEVEL = level;
        SceneManager.LoadScene("GameScene");
    }
}