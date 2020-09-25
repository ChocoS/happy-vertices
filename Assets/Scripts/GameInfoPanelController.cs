using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInfoPanelController : MonoBehaviour
{
    public GameObject bestNumberOfMoves;
    public GameObject currentNumberOfMoves;

    public void setNumberOfMoves(int numberOfMoves) {
        currentNumberOfMoves.GetComponent<TextMeshProUGUI>().text = "MOVES: " + numberOfMoves;
    }

    public void setBestNumberOfMoves(int numberOfMoves) {
        bestNumberOfMoves.GetComponent<TextMeshProUGUI>().text = numberOfMoves == -1 ? "BEST: -" : "BEST: " + numberOfMoves;
    }
}