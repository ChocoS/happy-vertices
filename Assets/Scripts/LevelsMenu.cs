using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{
    public void PlayLevel(int level) {
        GameController.CURRENT_LEVEL = level;
        SceneManager.LoadScene("GameScene");
    }
}