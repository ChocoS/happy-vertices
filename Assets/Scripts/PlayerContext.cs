using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerContext {

    private Dictionary<int, int> bestNumberOfMovesPerLevel = new Dictionary<int, int>();

    public int GetBestNumberOfMovesForLevel(int level) {
        if (bestNumberOfMovesPerLevel.ContainsKey(level)) {
            return bestNumberOfMovesPerLevel[level];
        }
        return int.MaxValue;
    }

    public void UpdateBestNumberOfMoves(int level, int numberOfMoves) {
        if (numberOfMoves < GetBestNumberOfMovesForLevel(level)) {
            bestNumberOfMovesPerLevel[level] = numberOfMoves;
        }
    }

    override
    public string ToString(){
        string result = "PlayerContext{Dictionary{";
        foreach (KeyValuePair<int, int> levelNumberOfMovesPair in bestNumberOfMovesPerLevel) {
            result += levelNumberOfMovesPair.Key + ":" + levelNumberOfMovesPair.Value + ",";
        }
        return result + "}}";
    }
}