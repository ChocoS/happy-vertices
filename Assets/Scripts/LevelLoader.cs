using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animator;

    public void LoadLevel(int level) {
        if (level > LevelManager.levelTemplates.Count - 1) {
            LoadCredits();
        } else {
            StartCoroutine(LoadLevelCoroutine(level));
        }
    }

    public void LoadMenu() {
        StartCoroutine(LoadMenuCoroutine());
    }

    public void LoadCredits() {
        StartCoroutine(LoadCreditsCoroutine());
    }

    IEnumerator LoadLevelCoroutine(int level) {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        GameController.CURRENT_LEVEL = level;
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator LoadMenuCoroutine() {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MenuScene");
    }

    IEnumerator LoadCreditsCoroutine() {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("CreditsScene");
    }
}