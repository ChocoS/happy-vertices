using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesController : MonoBehaviour
{
    public GameObject vertexPrefab;
    public GameObject edgePrefab;
    public GameObject borderPrefab;
    public Material edgeTempMaterial;
    public Material edgeWeight0Material;
    public Material edgeWeight1Material;
    public Material edgeWeight2Material;
    public Material edgeWeight3Material;
    public Material borderMaterial;
    public Sprite vertexAsleepSprite;
    public Sprite vertexNormalSprite;
    public Sprite vertexNotHappySprite;
    public Sprite vertexHappySprite;
    public string vertexTag = "Vertex";
    public string edgeTag = "Edge";
}