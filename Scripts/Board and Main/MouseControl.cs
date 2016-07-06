using UnityEngine;
using System.Collections;

public class MouseControl : MonoBehaviour {

    GameFlow gameflow;
    PiecePlacer piecePlacer;
     
    void Start()
    {
        gameflow = GetComponent<GameFlow>();
    }
	// Update is called once per frame

    public bool Clicked()
    {
        return Input.GetMouseButtonDown(0);
    }
}
