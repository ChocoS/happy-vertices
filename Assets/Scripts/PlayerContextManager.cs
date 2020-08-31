using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class PlayerContextManager {

    private static string path = Application.persistentDataPath + "/happy-vertices.dat";
    private static BinaryFormatter formatter = new BinaryFormatter();

    private static PlayerContext playerContext;

    public static void UpdateBestNumberOfMoves(int level, int numberOfMoves) {
        playerContext.UpdateBestNumberOfMoves(level, numberOfMoves);

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, playerContext);
        stream.Close();
    }

    public static PlayerContext Load() {
        if (File.Exists(path)) {
            Debug.Log("save file found in path: " + path);
            FileStream stream = new FileStream(path, FileMode.Open);
            playerContext = formatter.Deserialize(stream) as PlayerContext;
            stream.Close();
            return playerContext;
        }
        Debug.Log("save file not found in path: " + path);
        playerContext = new PlayerContext();
        return playerContext;
    }

    public static PlayerContext GetCurrentContext() {
        if (playerContext == null) {
            return Load();
        }
        return playerContext;
    }
}