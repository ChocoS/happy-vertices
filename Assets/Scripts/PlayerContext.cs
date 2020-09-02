using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerContext {

    private Dictionary<int, int> bestNumberOfMovesPerLevel = new Dictionary<int, int>();

    public int GetBestNumberOfMovesForLevel(int level) {
        if (ExistsBestNumberOfMovesForLevel(level)) {
            return bestNumberOfMovesPerLevel[level];
        }
        return -1;
    }

    public bool ExistsBestNumberOfMovesForLevel(int level) {
        return bestNumberOfMovesPerLevel.ContainsKey(level);
    }

    public void UpdateBestNumberOfMoves(int level, int numberOfMoves) {
        if (!ExistsBestNumberOfMovesForLevel(level) || numberOfMoves < GetBestNumberOfMovesForLevel(level)) {
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