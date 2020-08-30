using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate
{
    private float[][] vertices;
    private int[][] edges;
    private float[] border;
    private int[] moves;

    public LevelTemplate(float[][] vertices, int[][] edges, float[] border, int[] moves) {
        this.vertices = vertices;
        this.edges = edges;
        this.border = border;
        this.moves = moves;
    }

    public float[][] GetVertices() {
        return vertices;
    }

    public int[][] GetEdges() {
        return edges;
    }

    public float[] GetBorder() {
        return border;
    }
}