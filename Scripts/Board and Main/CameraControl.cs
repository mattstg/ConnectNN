using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public Camera mainCamera;
    public float zoomLevel = -10;
    public Vector2 cameraOffset; // offset to make cameras bottom left corner be on edge of (0,0)
	// Use this for initialization
	void Start () {
        mainCamera.transform.position = new Vector3(cameraOffset.x - GV.SLOT_GUI_SIZE.x,cameraOffset.y - GV.SLOT_GUI_SIZE.y,zoomLevel);
	}
	
}
