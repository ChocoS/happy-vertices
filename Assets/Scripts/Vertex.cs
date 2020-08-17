using System.Collections.Generic;
using UnityEngine;

public class Vertex {

    private Dictionary<Vertex, Edge> neighbours = new Dictionary<Vertex, Edge>();
    private State state;
    private GameObject gameObject;

    public Vertex(GameObject gameObject) {
        this.gameObject = gameObject;
        this.state = State.ASLEEP;
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
        if (totalEdgeWeight == 0) {
            state = State.ASLEEP;
            return;
        }
        foreach (Vertex neighbour in neighbours.Keys) {
            if (neighbour.GetTotalEdgeWeight() == totalEdgeWeight) {
                state = State.NOT_HAPPY;
                return;
            }
        }
        foreach (KeyValuePair<Vertex, Edge> neighbourEdgePair in neighbours) {
            if (neighbourEdgePair.Value.GetWeight() == 0) {
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
        ASLEEP = 0, // total edge weight 0
        NOT_HAPPY = 1, // exists a neighbour with the same total weight
        NORMAL = 2, // not NOT_HAPPY and exists an edge with weight 0
        HAPPY = 3, // not NOT_HAPPY and all edges with weight > 0
    }
}