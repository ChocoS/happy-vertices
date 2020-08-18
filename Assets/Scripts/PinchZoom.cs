using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public static float MIN_ZOOM = 2.0f;
    public static float MAX_ZOOM = 5.0f;

    private DebugController debug;

    void Start() {
        debug = GameObject.FindGameObjectWithTag("Debug").GetComponent<DebugController>();
    }

    void Update() {
        if (Input.touchCount == 2) {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize + deltaMagnitudeDiff * sensitivity, 0.1f);
            Camera.main.orthographicSize *= prevTouchDeltaMag / touchDeltaMag;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, MIN_ZOOM, MAX_ZOOM);
        }        
    }
}