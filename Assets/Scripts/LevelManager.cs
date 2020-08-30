using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public ResourcesController resourcesController;

    private static Dictionary<int, LevelTemplate> levelTemplates = LoadLevelTemplates();

    public Level loadLevel(int level) {
        clearAll();
        return LoadLevelFromTemplate(levelTemplates[level], level);
    }

    private Level LoadLevelFromTemplate(LevelTemplate levelTemplate, int number){
        List<Vertex> vertices = new List<Vertex>();
        foreach (float[] vertex in levelTemplate.GetVertices()) {
            vertices.Add(new Vertex(InstantiateVertex(vertex[0], vertex[1])));
        }
        foreach (int[] edge in levelTemplate.GetEdges()) {
            CreateEdge(vertices[edge[0]], vertices[edge[1]]);
        }
        Graph graph = new Graph(vertices);
        return new Level(number, graph, new Rect(levelTemplate.GetBorder()[0], levelTemplate.GetBorder()[1], levelTemplate.GetBorder()[2], levelTemplate.GetBorder()[3]));
    }

    private static Dictionary<int, LevelTemplate> LoadLevelTemplates() {
        Dictionary<int, LevelTemplate> result = new Dictionary<int, LevelTemplate>();
        float[] defaultBorderFloat = new float[] {-20, -10, 40, 20};

        float[][] vertices = new float[][] { V(-2, 0), V(2, 0), V(0, 2) };
        int[][] edges = new int[][] { E(0, 1), E(1, 2), E(2, 0) };
        int[] moves = new int[2] { 3, 5 };
        result.Add(1, new LevelTemplate(vertices, edges, defaultBorderFloat, moves));

        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0) };
        moves = new int[2] { 2, 4 };
        result.Add(2, new LevelTemplate(vertices, edges, defaultBorderFloat, moves));
        
        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0), E(1, 3) };
        moves = new int[2] { 2, 4 };
        result.Add(3, new LevelTemplate(vertices, edges, defaultBorderFloat, moves));
        
        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0), E(1, 3), E(0, 2) };
        moves = new int[2] { 3, 5 };
        result.Add(4, new LevelTemplate(vertices, edges, defaultBorderFloat, moves));

        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2), V(0, 4) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0), E(1, 3), E(0, 2), E(0, 4), E(3, 4) };
        moves = new int[2] { 3, 5 };
        result.Add(5, new LevelTemplate(vertices, edges, defaultBorderFloat, moves));

        vertices = new float[][] { V(2, 2), V(2, -2), V(-2, -2), V(-2, 2), V(0, 4) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 0), E(1, 3), E(0, 2), E(0, 4), E(1, 4), E(2, 4), E(3, 4) };
        moves = new int[2] { 5, 7 };
        result.Add(6, new LevelTemplate(vertices, edges, defaultBorderFloat, moves));

        vertices = new float[][] { V(-2, 2), V(-2, 0), V(-2, -2), V(2, 2), V(2, 0), V(2, -2) };
        edges = new int[][] { E(0, 3), E(0, 4), E(0, 5), E(1, 3), E(1, 4), E(1, 5), E(2, 3), E(2, 4), E(2, 5) };
        moves = new int[2] { 3, 5 };
        result.Add(7, new LevelTemplate(vertices, edges, defaultBorderFloat, moves));

        vertices = new float[][] { V(-2, 2), V(2, 2), V(3, 0), V(2, -2), V(-2, -2), V(-3, 0), V(0, 0) };
        edges = new int[][] { E(0, 1), E(1, 2), E(2, 3), E(3, 4), E(4, 5), E(5, 0), E(6, 0), E(6, 1), E(6, 2), E(6, 3), E(6, 4), E(6, 5) };
        moves = new int[2] { 3, 5 };
        result.Add(8, new LevelTemplate(vertices, edges, defaultBorderFloat, moves));

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
        v1.AddNeighbour(v2).SetGameObject(instantiateEdge(v1.GetGameObject(), v2.GetGameObject()));
    }

    private GameObject instantiateEdge(GameObject v1, GameObject v2) {
        GameObject line = Instantiate(resourcesController.edgePrefab, Vector3.zero, Quaternion.identity);
        line.GetComponent<LineRenderer>().SetPosition(0, v1.transform.position);
        line.GetComponent<LineRenderer>().SetPosition(1, v2.transform.position);
        line.GetComponent<LineRenderer>().material = resourcesController.edgeWeight1Material;
        return line;
    }

    private void clearAll() {
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