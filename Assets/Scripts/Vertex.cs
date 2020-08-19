using System.Collections.Generic;
using UnityEngine;

public class Vertex {

    private Dictionary<Vertex, Edge> neighbours = new Dictionary<Vertex, Edge>();
    private State state;
    private GameObject gameObject;

    public Vertex(GameObject gameObject) {
        this.gameObject = gameObject;
        this.state = State.NORMAL;
    }

    public State GetState() {
        return state;
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public Dictionary<Vertex, Edge> GetNeighbours() {
        return neighbours;
    }

    public bool IsNeighbour(Vertex other) {
        return neighbours.ContainsKey(other);
    }

    public Edge AddNeighbour(Vertex neighbour) {
        Edge edge = new Edge(this, neighbour);
        neighbours.Add(neighbour, edge);
        neighbour.GetNeighbours().Add(this, edge);
        return edge;
    }

    public Edge incrementNeighbour(Vertex neighbour) {
        Edge edge = neighbours[neighbour];
        edge.IncrementWeight();
        UpdateStatesOfRelevantVertices(neighbour);
        return edge;
    }

    private void UpdateStatesOfRelevantVertices(Vertex neighbour) {
        foreach (Vertex vertex in neighbours.Keys) {
            vertex.UpdateState();
        }
        foreach (Vertex vertex in neighbour.GetNeighbours().Keys) {
            vertex.UpdateState();
        }
    }

    public void UpdateState() {
        int totalEdgeWeight = GetTotalEdgeWeight();
        foreach (Vertex neighbour in neighbours.Keys) {
            if (neighbour.GetTotalEdgeWeight() == totalEdgeWeight) {
                state = State.NORMAL;
                return;
            }
        }
        state = State.HAPPY;
    }

    public int GetTotalEdgeWeight() {
        int totalEdgeWeight = 0;
        foreach (KeyValuePair<Vertex, Edge> neighbourEdgePair in neighbours) {
            totalEdgeWeight += neighbourEdgePair.Value.GetWeight();
        }
        return totalEdgeWeight;
    }

    public enum State {
        NORMAL = 0, // exists a neighbour with the same total weight
        HAPPY = 1, // not NORMAL
    }
}