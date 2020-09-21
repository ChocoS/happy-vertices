using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelsMenu : MonoBehaviour
{
    private static int levelButtonsPerRow = 5;
    private static Vector2 levelButtonStartingPos = new Vector2(-120, 60);
    private static float levelButtonMargin = 60;

    public GameObject backButton;
    public MenuResourcesController resources;
    public LevelLoader levelLoader;

    private List<GameObject> levelButtons = new List<GameObject>();

    void Start() {
        for (int i=0; i<LevelManager.levelTemplates.Count; i++) {
            LevelTemplate levelTemplate = LevelManager.levelTemplates[i];
            int row = i / levelButtonsPerRow;
            int col = i % levelButtonsPerRow;
            GameObject levelButton = Instantiate(resources.levelButton, CalculatePosition(row, col), Quaternion.identity);
            levelButton.transform.SetParent(gameObject.transform, false);
            AddListener(levelButton.GetComponent<Button>(), i);
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = "" + (i + 1);
            levelButtons.Add(levelButton);
        }
        AdjustBackButtonPosition();
        UpdateLevelButtonsStates();
    }

    private void OnEnable() {
        UpdateLevelButtonsStates();
    }

    private void AdjustBackButtonPosition() {
        backButton.transform.localPosition = new Vector2(backButton.transform.localPosition.x,
            levelButtonStartingPos.y - levelButtonMargin * ((LevelManager.levelTemplates.Count-1) / levelButtonsPerRow + 1));
    }

    private void UpdateLevelButtonsStates() {
        for (int i=0; i<levelButtons.Count; i++) {
            UpdateLevelButtonState(levelButtons[i], i);
        }
    }

    private void UpdateLevelButtonState(GameObject levelButton, int levelNumber) {
        if (PlayerContextManager.GetCurrentContext().ExistsBestNumberOfMovesForLevel(levelNumber)) {
            int playerBest = PlayerContextManager.GetCurrentContext().GetBestNumberOfMovesForLevel(levelNumber);
            int[] moveThreshold = LevelManager.levelTemplates[levelNumber].GetMoves();
            if (playerBest <= moveThreshold[0]) {
                MarkMedals(levelButton, 3);
            } else if (playerBest <= moveThreshold[1]) {
                MarkMedals(levelButton, 2);
            } else {
                MarkMedals(levelButton, 1);
            }
        } else {
            MarkMedals(levelButton, 0);
            if (levelNumber > 0) {
                bool enableButton = PlayerContextManager.GetCurrentContext().ExistsBestNumberOfMovesForLevel(levelNumber - 1);
                if (enableButton) {
                    EnableButton(levelButton);
                } else {
                    DisableButton(levelButton);
                }
            }
        }
    }

    private void EnableButton(GameObject levelButton) {
        levelButton.GetComponent<Button>().interactable = true;
        levelButton.GetComponentInChildren<TextMeshProUGUI>().colorGradientPreset = resources.activeButtonGradient;
        foreach (Image image in levelButton.GetComponentsInChildren<Image>()) {
            image.enabled = true;
        }
    }

    private void DisableButton(GameObject levelButton) {
        levelButton.GetComponent<Button>().interactable = false;
        levelButton.GetComponentInChildren<TextMeshProUGUI>().colorGradientPreset = resources.inactiveButtonGradient;
        foreach (Image image in levelButton.GetComponentsInChildren<Image>()) {
            image.enabled = false;
        }
    }

    private void MarkMedals(GameObject levelButton, int markedMedals) {
        List<Image> images = new List<Image>();
        levelButton.GetComponentsInChildren<Image>(images);
        Image image1 = images.Find(image => image.name == "Image1");
        Image image2 = images.Find(image => image.name == "Image2");
        Image image3 = images.Find(image => image.name == "Image3");
        switch (markedMedals) {
            case 0: image1.sprite = resources.vertexAsleepSprite; image2.sprite = resources.vertexAsleepSprite; image3.sprite = resources.vertexAsleepSprite; break;
            case 1: image1.sprite = resources.vertexNormalSprite; image2.sprite = resources.vertexAsleepSprite; image3.sprite = resources.vertexAsleepSprite; break;
            case 2: image1.sprite = resources.vertexNormalSprite; image2.sprite = resources.vertexNormalSprite; image3.sprite = resources.vertexAsleepSprite; break;
            case 3: image1.sprite = resources.vertexHappySprite; image2.sprite = resources.vertexHappySprite; image3.sprite = resources.vertexHappySprite; break;
        }
    }

    private void AddListener(Button button, int parameter) {
        button.onClick.AddListener(delegate { PlayLevel(parameter); });
    }

    private Vector2 CalculatePosition(int row, int col) {
        return new Vector2(levelButtonStartingPos.x + col * levelButtonMargin, levelButtonStartingPos.y - row * levelButtonMargin);
    }

    public void PlayLevel(int level) {
        levelLoader.LoadLevel(level);
    }
}