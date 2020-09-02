using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate
{
    private float[][] vertices;
    private int[][] edges;
    private int[] moves;

    public LevelTemplate(float[][] vertices, int[][] edges, int[] moves) {
        this.vertices = vertices;
        this.edges = edges;
        this.moves = moves;
    }

    public float[][] GetVertices() {
        return vertices;
    }

    public int[][] GetEdges() {
        return edges;
    }

    public int[] GetMoves() {
        return moves;
    }
}