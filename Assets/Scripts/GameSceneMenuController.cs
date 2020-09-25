using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMenuController : MonoBehaviour
{
    public LevelLoader levelLoader;

    public void LoadMenu() {
        levelLoader.LoadMenu();
    }

    public void ReloadLevel() {
        levelLoader.LoadLevel(GameController.CURRENT_LEVEL);
    }
}