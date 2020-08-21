using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInfoPanelController : MonoBehaviour
{
    private static string NUMBER_OF_MOVES_PREFIX = "MOVES: ";

    public void setNumberOfMoves(int numberOfMoves) {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = NUMBER_OF_MOVES_PREFIX + numberOfMoves;
    }
}