using UnityEngine;
using System.Collections;

public class MouseControl : MonoBehaviour {

    GameFlow gameflow;
    PieceGUI curPiecePlacing;
    bool isPlacing = false;
     
    void Start()
    {
        gameflow = GetComponent<GameFlow>();
    }
	// Update is called once per frame

    public void Update()
    {
        if (isPlacing && Input.GetMouseButtonDown(0))
            DropPiece();
        if (isPlacing)
            PlacePieceOnMouse();
    }

    public void SpawnClicked(int pid)
    {
        if (isPlacing)
            DeleteCurrentToken();

        isPlacing = true;
         curPiecePlacing = PieceFactory.Instance.MakePiece(pid, GV.removeZ(Input.mousePosition));
    }

    private void DeleteCurrentToken()
    {
        Destroy(curPiecePlacing.gameObject);
        curPiecePlacing = null;
    }

    private void PlacePieceOnMouse()
    {
        curPiecePlacing.transform.position = GV.removeZ(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }


    private void DropPiece()
    {
        Vector2 placementPos = GV.removeZ(Camera.main.ScreenToWorldPoint(Input.mousePosition)); //initial drop
        int col = (int)((placementPos.x / GV.SLOT_GUI_SIZE.x) + .5f);  //col itll land in
        curPiecePlacing.AddPathToEnd(GV.RealWorldPosByGridLoc(new Vector2(col, GV.BOARD_SIZE.y))); //above the board
        curPiecePlacing.AddPathToEnd(GV.RealWorldPosByGridLoc(new Vector2(col, GV.BOARD_SIZE.y - 1))); //first slot

        Board.Instance.AddPiece(curPiecePlacing.piece, new Vector2(col, GV.BOARD_SIZE.y - 1));
        curPiecePlacing.piece.gridLoc = new Vector2(col, GV.BOARD_SIZE.y - 1);

        curPiecePlacing = null;
        isPlacing = false;
        gameflow.PieceDropped();
    }

    public void DropPieceAtLocation(Piece piece, Vector2 worldPos)
    {
        PieceGUI curPiecePlacing = piece.pieceGUI;
        Vector2 placementPos = GV.removeZ(Camera.main.ScreenToWorldPoint(Input.mousePosition)); //initial drop
        int col = (int)((placementPos.x / GV.SLOT_GUI_SIZE.x) + .5f);  //col itll land in
        curPiecePlacing.AddPathToEnd(GV.RealWorldPosByGridLoc(new Vector2(col, GV.BOARD_SIZE.y))); //above the board
        curPiecePlacing.AddPathToEnd(GV.RealWorldPosByGridLoc(new Vector2(col, GV.BOARD_SIZE.y - 1))); //first slot

        Board.Instance.AddPiece(curPiecePlacing.piece, new Vector2(col, GV.BOARD_SIZE.y - 1));
        curPiecePlacing.piece.gridLoc = new Vector2(col, GV.BOARD_SIZE.y - 1);
    }
}
