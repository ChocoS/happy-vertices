using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private float defaultBorderMargin = 10;

    public ResourcesController resourcesController;

    public static List<LevelTemplate> levelTemplates = LoadLevelTemplates();

    public Level LoadLevel(int number) {
        ClearAll();
        return LoadLevelFromTemplate(number);
    }

    private Level LoadLevelFromTemplate(int number) {
        LevelTemplate levelTemplate = levelTemplates[number];
        List<Vertex> vertices = new List<Vertex>();
        foreach (float[] vertex in levelTemplate.GetVertices()) {
            vertices.Add(new Vertex(InstantiateVertex(vertex[0], vertex[1])));
        }
        foreach (int[] edge in levelTemplate.GetEdges()) {
            CreateEdge(vertices[edge[0]], vertices[edge[1]]);
        }
        Graph graph = new Graph(vertices);
        return new Level(number, graph, calculateBorder(levelTemplate.GetVertices()));
    }

    private Rect calculateBorder(float[][] vertices) {
        float minX = vertices[0][0];
        float maxX = vertices[0][0];
        float minY = vertices[0][1];
        float maxY = vertices[0][1];
        for (int i=1; i<vertices.Length; i++) {
            if (vertices[i][0] < minX) {
                minX = vertices[i][0];
            }
            if (vertices[i][0] > maxX) {
                maxX = vertices[i][0];
            }
            if (vertices[i][1] < minY) {
                minY = vertices[i][1];
            }
            if (vertices[i][1] > maxY) {
                maxY = vertices[i][1];
            }
        }
        return new Rect(minX - defaultBorderMargin, minY - defaultBorderMargin,
            maxX - minX + 2 * defaultBorderMargin, maxY - minY + 2 * defaultBorderMargin);
    }

    private static List<LevelTemplate> LoadLevelTemplates() {
        List<LevelTemplate> result = new List<LevelTemplate>();

        float[][] vertices = new float[][] { V(-2, 0), V(2, 0), V(0, 2) };
        int[][] edges = new int[][] { E(0, 1), E(1, 2), E(2, 0) };
        int[] moves = new int[2] { 3, 6 };
        result.Add(new LevelTemplate(vertices, edges, moves));

        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0) };
        moves = new int[2] { 2, 5 };
        result.Add(new LevelTemplate(vertices, edges, moves));
        
        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0), E(1, 3) };
        moves = new int[2] { 2, 5 };
        result.Add(new LevelTemplate(vertices, edges, moves));
        
        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0), E(1, 3), E(0, 2) };
        moves = new int[2] { 3, 6 };
        result.Add(new LevelTemplate(vertices, edges, moves));

        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2), V(0, 4) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0), E(1, 3), E(0, 2), E(0, 4), E(3, 4) };
        moves = new int[2] { 2, 5 };
        result.Add(new LevelTemplate(vertices, edges, moves));

        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2), V(0, 4) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0), E(1, 3), E(0, 2), E(0, 4), E(1, 4), E(2, 4), E(3, 4) };
        moves = new int[2] { 5, 8 };
        result.Add(new LevelTemplate(vertices, edges, moves));

        vertices = new float[][] { V(-2, 2), V(-2, 0), V(-2, -2), V(2, 2), V(2, 0), V(2, -2) };
        edges = new int[][] { E(0, 3), E(0, 4), E(0, 5), E(1, 3), E(1, 4), E(1, 5), E(2, 3), E(2, 4), E(2, 5) };
        moves = new int[2] { 3, 6 };
        result.Add(new LevelTemplate(vertices, edges, moves));

        vertices = new float[][] { V(-2, 2), V(2, 2), V(3, 0), V(2, -2), V(-2, -2), V(-3, 0), V(0, 0) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 4), E(4, 5), E(5, 0), E(6, 0), E(6, 1), E(6, 2), E(6, 3), E(6, 4), E(6, 5) };
        moves = new int[2] { 3, 6 };
        result.Add(new LevelTemplate(vertices, edges, moves));

        vertices = new float[][] { V(0, 0), V(0, 2), V(2, 0), V(0, -2), V(-2, 0), V(-3, 3), V(3, 3), V(3, -3), V(-3, -3) };
        edges = new int[][] { E(0, 1), E(0, 2), E(0, 3), E(0, 4), E(2, 3), E(4, 1),  E(5, 1), E(5, 6), E(5, 4), E(5, 8), E(6, 1), E(6, 2), E(6, 7), E(7, 2), E(7, 3), E(7, 8), E(8, 3), E(8, 4) };
        moves = new int[2] { 7, 10 };
        result.Add(new LevelTemplate(vertices, edges, moves));

        return result;
    }

    private static float[] V(float x, float y) {
        return new float[] { x, y };
    }

    private static int[] E(int v1, int v2) {
        return new int[] {v1, v2};
    }

    private GameObject InstantiateVertex(float x, float y) {
        return Instantiate(resourcesController.vertexPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    private void CreateEdge(Vertex v1, Vertex v2) {
        v1.AddNeighbour(v2).SetGameObject(InstantiateEdge(v1.GetGameObject(), v2.GetGameObject()));
    }

    private GameObject InstantiateEdge(GameObject v1, GameObject v2) {
        GameObject line = Instantiate(resourcesController.edgePrefab, Vector3.zero, Quaternion.identity);
        line.GetComponent<LineRenderer>().SetPosition(0, v1.transform.position);
        line.GetComponent<LineRenderer>().SetPosition(1, v2.transform.position);
        line.GetComponent<LineRenderer>().material = resourcesController.edgeWeight1Material;
        return line;
    }

    private void ClearAll() {
        GameObject[] vertices = GameObject.FindGameObjectsWithTag(resourcesController.vertexTag);
        GameObject[] edges = GameObject.FindGameObjectsWithTag(resourcesController.edgeTag);
        foreach (GameObject vertex in vertices) {
            Destroy(vertex);
        }
        foreach (GameObject edge in edges) {
            Destroy(edge);
        }
    }
}