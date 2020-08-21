using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public ResourcesController resourcesController;

    private Rect defaultBorder = new Rect(-20, -10, 40, 20);

    public Level loadLevel(int level) {
        clearAll();
        switch (level) {
            case 1: return loadLevel1();
            case 2: return loadLevel2();
            case 3: return loadLevel3();
            case 4: return loadLevel4();
            case 5: return loadLevel5();
            case 6: return loadLevel6();
            case 7: return loadLevel7();
            case 8: return loadLevel8();
        }
        return null;
    }

    private Level loadLevel1() {
        Vertex v1 = new Vertex(InstantiateVertex(-2, 0));
        Vertex v2 = new Vertex(InstantiateVertex(2, 0));
        Vertex v3 = new Vertex(InstantiateVertex(0, 2));
        CreateEdgesInClique(new Vertex[] {v1, v2, v3});
        Graph graph = new Graph(new Vertex[] {v1, v2, v3});
        return new Level(1, graph, defaultBorder);
    }

    private Level loadLevel2() {
        Vertex v1 = new Vertex(InstantiateVertex(2, 2));
        Vertex v2 = new Vertex(InstantiateVertex(2, -2));
        Vertex v3 = new Vertex(InstantiateVertex(-2, -2));
        Vertex v4 = new Vertex(InstantiateVertex(-2, 2));
        CreateEdgesInBipartiteGraph(new Vertex[] {v1, v3}, new Vertex[] {v2, v4});
        Graph graph = new Graph(new Vertex[] {v1, v2, v3, v4});
        return new Level(2, graph, defaultBorder);
    }
    
    private Level loadLevel3() {
        Vertex v1 = new Vertex(InstantiateVertex(2, 2));
        Vertex v2 = new Vertex(InstantiateVertex(2, -2));
        Vertex v3 = new Vertex(InstantiateVertex(-2, -2));
        Vertex v4 = new Vertex(InstantiateVertex(-2, 2));
        CreateEdge(v1, v2);
        CreateEdge(v2, v3);
        CreateEdge(v3, v4);
        CreateEdge(v4, v1);
        CreateEdge(v2, v4);
        Graph graph = new Graph(new Vertex[] {v1, v2, v3, v4});
        return new Level(3, graph, defaultBorder);
    }

    private Level loadLevel4() {
        Vertex v1 = new Vertex(InstantiateVertex(2, 2));
        Vertex v2 = new Vertex(InstantiateVertex(2, -2));
        Vertex v3 = new Vertex(InstantiateVertex(-2, -2));
        Vertex v4 = new Vertex(InstantiateVertex(-2, 2));
        CreateEdgesInClique(new Vertex[] {v1, v2, v3, v4});
        Graph graph = new Graph(new Vertex[] {v1, v2, v3, v4});
        return new Level(4, graph, defaultBorder);
    }

    private Level loadLevel5() {
        Vertex v1 = new Vertex(InstantiateVertex(2, 2));
        Vertex v2 = new Vertex(InstantiateVertex(2, -2));
        Vertex v3 = new Vertex(InstantiateVertex(-2, -2));
        Vertex v4 = new Vertex(InstantiateVertex(-2, 2));
        Vertex v5 = new Vertex(InstantiateVertex(0, 4));
        CreateEdgesInClique(new Vertex[] {v1, v2, v3, v4});
        CreateEdge(v1, v5);
        CreateEdge(v4, v5);
        Graph graph = new Graph(new Vertex[] {v1, v2, v3, v4, v5});
        return new Level(5, graph, defaultBorder);
    }

    private Level loadLevel6() {
        Vertex v1 = new Vertex(InstantiateVertex(2, 2));
        Vertex v2 = new Vertex(InstantiateVertex(2, -2));
        Vertex v3 = new Vertex(InstantiateVertex(-2, -2));
        Vertex v4 = new Vertex(InstantiateVertex(-2, 2));
        Vertex v5 = new Vertex(InstantiateVertex(0, 4));
        CreateEdgesInClique(new Vertex[] {v1, v2, v3, v4, v5});
        Graph graph = new Graph(new Vertex[] {v1, v2, v3, v4, v5});
        return new Level(6, graph, defaultBorder);
    }

    private Level loadLevel7() {
        Vertex v1 = new Vertex(InstantiateVertex(-2, 2));
        Vertex v2 = new Vertex(InstantiateVertex(-2, 0));
        Vertex v3 = new Vertex(InstantiateVertex(-2, -2));
        Vertex v4 = new Vertex(InstantiateVertex(2, 2));
        Vertex v5 = new Vertex(InstantiateVertex(2, 0));
        Vertex v6 = new Vertex(InstantiateVertex(2, -2));
        CreateEdgesInBipartiteGraph(new Vertex[] {v1, v2, v3}, new Vertex[] {v4, v5, v6});
        Graph graph = new Graph(new Vertex[] {v1, v2, v3, v4, v5, v6});
        return new Level(7, graph, defaultBorder);
    }

    private Level loadLevel8() {
        Vertex v1 = new Vertex(InstantiateVertex(-2, 2));
        Vertex v5 = new Vertex(InstantiateVertex(-2, -2));
        Vertex v4 = new Vertex(InstantiateVertex(2, -2));
        Vertex v2 = new Vertex(InstantiateVertex(2, 2));
        Vertex v6 = new Vertex(InstantiateVertex(-3, 0));
        Vertex v3 = new Vertex(InstantiateVertex(3, 0));
        Vertex v7 = new Vertex(InstantiateVertex(0, 0));
        CreateEdge(v1, v2);
        CreateEdge(v2, v3);
        CreateEdge(v3, v4);
        CreateEdge(v4, v5);
        CreateEdge(v5, v6);
        CreateEdge(v6, v1);
        CreateEdge(v7, v1);
        CreateEdge(v7, v2);
        CreateEdge(v7, v3);
        CreateEdge(v7, v4);
        CreateEdge(v7, v5);
        CreateEdge(v7, v6);
        Graph graph = new Graph(new Vertex[] {v1, v2, v3, v4, v5, v6, v7});
        return new Level(8, graph, defaultBorder);
    }

    private void CreateEdgesInClique(Vertex[] vertices) {
        for (int i=0; i<vertices.Length; i++) {
            for (int j=i+1; j<vertices.Length; j++) {
                CreateEdge(vertices[i], vertices[j]);
            }
        }
    }

    private void CreateEdgesInBipartiteGraph(Vertex[] verticesA, Vertex[] verticesB) {
        for (int i=0; i<verticesA.Length; i++) {
            for (int j=0; j<verticesB.Length; j++) {
                CreateEdge(verticesA[i], verticesB[j]);
            }
        }
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