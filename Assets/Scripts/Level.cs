using System.Collections.Generic;
using UnityEngine;

public class Level {

    private Graph graph;
    private Rect boundry;

    public Level(Graph graph, Rect boundry) {
        this.graph = graph;
        this.boundry = boundry;
    }

    public Graph GetGraph() {
        return graph;
    }

    public Rect GetBoundry() {
        return boundry;
    }
}