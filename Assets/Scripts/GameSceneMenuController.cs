using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneMenuController : MonoBehaviour
{
    public void LoadMenu() {
        SceneManager.LoadScene("MenuScene");
    }
}