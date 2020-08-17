using System.Collections.Generic;
using UnityEngine;

public class Graph {

    private List<Vertex> vertices = new List<Vertex>();

    public Graph(Vertex[] vertices) {
        this.vertices.AddRange(vertices);
    }

    public List<Vertex> GetVertices() {
        return vertices;
    }

    public Vertex FindVertexByGameObject(GameObject gameObject) {
        foreach (Vertex vertex in vertices) {
            if (vertex.GetGameObject() == gameObject) {
                return vertex;
            }
        }
        return null;
    }

    public bool AllVerticesHappy() {
        foreach (Vertex vertex in vertices) {
            if (vertex.GetState() != Vertex.State.HAPPY) {
                return false;
            }
        }
        return true;
    }
}