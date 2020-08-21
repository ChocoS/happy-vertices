using System.Collections.Generic;
using UnityEngine;

public class Level {

    private int number;
    private Graph graph;
    private Rect boundry;

    public Level(int number, Graph graph, Rect boundry) {
        this.number = number;
        this.graph = graph;
        this.boundry = boundry;
    }

    public int GetNumber() {
        return number;
    }

    public Graph GetGraph() {
        return graph;
    }

    public Rect GetBoundry() {
        return boundry;
    }
}