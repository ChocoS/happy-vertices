using UnityEngine;

public class Edge {

    private static int MAX_WEIGHT = 3;
    
    private Vertex v1;
    private Vertex v2;
    private int weight = 0;
    private GameObject gameObject;

    public void SetGameObject(GameObject gameObject) {
        this.gameObject = gameObject;
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public Edge(Vertex v1, Vertex v2) {
        this.v1 = v1;
        this.v2 = v2;
    }

    public int setWeight(int weight) {
        if (weight < 0) {
            this.weight = 0;
        } else if (weight > MAX_WEIGHT) {
            this.weight = MAX_WEIGHT;
        } else {
            this.weight = weight;
        }
        return weight;
    }

    public int GetWeight() {
        return weight;
    }

    public Vertex GetVertex1() {
        return v1;
    }

    public Vertex GetVertex2() {
        return v2;
    }

    public int IncrementWeight() {
        if (weight == MAX_WEIGHT) {
            weight = 0;
        } else {
            weight++;
        }
        return weight;
    }
}