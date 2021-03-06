﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static int CURRENT_LEVEL = 0;
    private static float MAX_VERTEX_PROXIMITY = 1.0f;

    public ResourcesController resourcesController;
    public LevelManager levelManager;
    public GameInfoPanelController gameInfoPanelController;
    public LevelLoader levelLoader;
    public GameObject nextLevelButton;

    private GameObject currentEdge;
    private Level currentLevel;
    private Vector3 dragStartingPosition;
    private Vector3 cameraStartDragPosition;
    private float zoomSensitivity = 0.2f;
    private int currentLevelMoveCounter = 0;
    private List<GameObject> borders = new List<GameObject>();
    private bool levelFinished = false;

    private bool multiTouchInPreviousFrame = false;

    void Awake() {
        PrepareNewLevel();
    }

    void Update() {
        if (currentLevel.GetGraph().AllVerticesHappy() && !levelFinished) {
            FinishLevel();
        }

        if (Input.touchSupported) {
            if (Input.touchCount == 1) {
                Touch touch = Input.GetTouch(0);
                Vector3 activePosition = getTouchActivePosition(touch);
                if (touch.phase == TouchPhase.Began) {
                    HandleClickedDown(activePosition);
                } else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
                    if (multiTouchInPreviousFrame) {
                        dragStartingPosition = activePosition;
                        multiTouchInPreviousFrame = false;
                    }
                    HandleClicked(activePosition);
                } else if (touch.phase == TouchPhase.Ended) {
                    HandleClickedUp();
                }
            } else if (Input.touchCount > 1) {
                multiTouchInPreviousFrame = true;
                Camera.main.transform.position -= getMiddleDeltaPosition(Input.GetTouch(0), Input.GetTouch(1));
            } else {
                multiTouchInPreviousFrame = false;
            }
        } else {
            if (Input.GetMouseButtonDown(0)) {
                HandleClickedDown(GetMousePosition());
            } else if (Input.GetMouseButton(0)) {
                HandleClicked(GetMousePosition());
            } else if (Input.GetMouseButtonUp(0)) {
                HandleClickedUp();
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize-zoomSensitivity, PinchZoom.MIN_ZOOM);
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize+zoomSensitivity, PinchZoom.MAX_ZOOM);
        }
    }

    private Vector3 getTouchActivePosition(Touch touch) {
        Vector3 activePosition = Camera.main.ScreenToWorldPoint(touch.position);
        activePosition.z = 0;
        return activePosition;
    }

    private Vector3 getMiddleDeltaPosition(Touch touch1, Touch touch2) {
        Vector3 activePosition1 = Camera.main.ScreenToWorldPoint(touch1.position);
        Vector3 activePosition2 = Camera.main.ScreenToWorldPoint(touch2.position);        
        Vector3 previousPosition1 = Camera.main.ScreenToWorldPoint(touch1.position - touch1.deltaPosition);
        Vector3 previousPosition2 = Camera.main.ScreenToWorldPoint(touch2.position - touch2.deltaPosition);
        Vector3 previousMiddle = (previousPosition1 + previousPosition2) / 2;
        Vector3 activeMiddle = (activePosition1 + activePosition2) / 2;
        Vector3 result = activeMiddle - previousMiddle;
        result.z = 0;
        return result;
    }

    private void HandleClickedDown(Vector3 activePosition) {
        if (GetVertexThatContainsPoint(GetAllVertexGameObjects(), activePosition) != null) {
            currentEdge = SpawnTempEdge(activePosition);
        } else {
            dragStartingPosition = activePosition;
        }
    }

    private void HandleClicked(Vector3 activePosition) {
        if (currentEdge != null) {
            currentEdge.GetComponent<LineRenderer>().SetPosition(1, activePosition);
        } else {
            Camera.main.transform.position += dragStartingPosition - activePosition;
            ClampCameraTransformPosition();
        }
    }

    private void ClampCameraTransformPosition() {
        if (currentLevel != null) {
            Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, currentLevel.GetBoundry().xMin, currentLevel.GetBoundry().xMax),
                Mathf.Clamp(Camera.main.transform.position.y, currentLevel.GetBoundry().yMin, currentLevel.GetBoundry().yMax), Camera.main.transform.position.z);
        }
    }

    private void HandleClickedUp() {
        if (currentEdge != null) {
            LineRenderer lineRenderer = currentEdge.GetComponent<LineRenderer>();
            Vector2 point0 = lineRenderer.GetPosition(0);
            Vector2 point1 = lineRenderer.GetPosition(1);
            GameObject[] vertices = GameObject.FindGameObjectsWithTag(resourcesController.vertexTag);
            GameObject vertexGameObject0 = GetVertexThatContainsPoint(vertices, point0);
            GameObject vertexGameObject1 = GetClosestVertexToPoint(vertices, point1);
            if (vertexGameObject0 != null && vertexGameObject1 != null && vertexGameObject0 != vertexGameObject1) {
                lineRenderer.SetPosition(0, vertexGameObject0.transform.position);
                lineRenderer.SetPosition(1, vertexGameObject1.transform.position);
                Vertex v0 = currentLevel.GetGraph().FindVertexByGameObject(vertexGameObject0);
                Vertex v1 = currentLevel.GetGraph().FindVertexByGameObject(vertexGameObject1);
                if (v0.IsNeighbour(v1)) {
                    Edge edge = v0.incrementNeighbour(v1);
                    UpdateAffectedGameObjects(edge);
                    UpdateNumberOfMoves();
                }
            }
            Destroy(currentEdge);
            currentEdge = null;
        }
    }

    private void UpdateNumberOfMoves() {
        currentLevelMoveCounter++;
        gameInfoPanelController.setNumberOfMoves(currentLevelMoveCounter);
    }

    private GameObject[] GetAllVertexGameObjects() {
        return GameObject.FindGameObjectsWithTag(resourcesController.vertexTag);
    }

    private void UpdateAffectedGameObjects(Edge edge) {
        UpdateEdgeGameObject(edge);
        UpdateVertexGameObjects(edge.GetVertex1(), edge.GetVertex2());
    }

    private void UpdateVertexGameObjects(Vertex vertex1, Vertex vertex2) {
        foreach (Vertex vertex in vertex1.GetNeighbours().Keys) {
            UpdateVertexGameObject(vertex);
        }
        foreach (Vertex vertex in vertex2.GetNeighbours().Keys) {
            UpdateVertexGameObject(vertex);
        }
    }

    private void PrepareNewLevel() {
        currentLevel = levelManager.LoadLevel(CURRENT_LEVEL);
        UpdateAllVerticesGameObjects();
        currentLevelMoveCounter = 0;
        gameInfoPanelController.setNumberOfMoves(0);
        gameInfoPanelController.setBestNumberOfMoves(PlayerContextManager.GetCurrentContext().GetBestNumberOfMovesForLevel(CURRENT_LEVEL));
        levelFinished = false;
    }

    private void FinishLevel() {
        levelFinished = true;
        PlayerContextManager.UpdateBestNumberOfMoves(CURRENT_LEVEL, currentLevelMoveCounter);
        gameInfoPanelController.setBestNumberOfMoves(PlayerContextManager.GetCurrentContext().GetBestNumberOfMovesForLevel(CURRENT_LEVEL));
        nextLevelButton.SetActive(true);
    }

    private void UpdateAllVerticesGameObjects() {
        foreach (Vertex vertex in currentLevel.GetGraph().GetVertices()) {
            UpdateVertexGameObject(vertex);
        }
    }

    private void UpdateVertexGameObject(Vertex vertex) {
        vertex.GetGameObject().GetComponentInChildren<Animator>().SetInteger("State", (int) vertex.GetState());
        vertex.GetGameObject().GetComponentInChildren<TextMesh>().text = vertex.GetTotalEdgeWeight().ToString();
    }

    private void UpdateEdgeGameObject(Edge edge) {
        switch (edge.GetWeight()) {
            case 0: edge.GetGameObject().GetComponent<LineRenderer>().material = resourcesController.edgeWeight0Material; break;
            case 1: edge.GetGameObject().GetComponent<LineRenderer>().material = resourcesController.edgeWeight1Material; break;
            case 2: edge.GetGameObject().GetComponent<LineRenderer>().material = resourcesController.edgeWeight2Material; break;
            case 3: edge.GetGameObject().GetComponent<LineRenderer>().material = resourcesController.edgeWeight3Material; break;
        }
    }

    private GameObject GetVertexThatContainsPoint(GameObject[] vertices, Vector2 point) {
        foreach (GameObject vertex in vertices) {
            CircleCollider2D circleCollider = vertex.GetComponentInChildren<CircleCollider2D>();
            if (ColliderContainsPoint(circleCollider, point)) {
                return vertex;
            }
        }
        return null;
    }

    private GameObject GetClosestVertexToPoint(GameObject[] vertices, Vector2 point) {
        GameObject closestVertex = null;
        float closestDistance = float.MaxValue;
        foreach (GameObject vertex in vertices) {
            CircleCollider2D collider = vertex.GetComponentInChildren<CircleCollider2D>();
            float currentDistance = (collider.ClosestPoint(point) - point).magnitude;
            if (currentDistance < closestDistance) {
                closestDistance = currentDistance;
                closestVertex = vertex;
            }
        }
        return closestDistance < MAX_VERTEX_PROXIMITY ? closestVertex : null;
    }

    private bool ColliderContainsPoint(Collider2D collider, Vector2 point) {
        return (collider.ClosestPoint(point) - point).magnitude < 0.001;
    }

    private Vector3 GetMousePosition() {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        return position;
    }

    private GameObject SpawnTempEdge(Vector2 position) {
        GameObject tempEdge = Instantiate(resourcesController.edgePrefab, Vector2.zero, Quaternion.identity);
        tempEdge.GetComponent<LineRenderer>().SetPosition(0, position);
        tempEdge.GetComponent<LineRenderer>().SetPosition(1, position);
        tempEdge.GetComponent<LineRenderer>().material = resourcesController.edgeTempMaterial;
        tempEdge.GetComponent<LineRenderer>().sortingOrder = 1;
        return tempEdge;
    }
}