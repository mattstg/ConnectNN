using UnityEngine;
using System.Collections;

public class PiecePlacer : MonoBehaviour {

    public PieceGUI piecePlacing;
	// Update is called once per frame
	
    public void PlacePieceOnMouse() {
        if (!piecePlacing)
            Debug.LogError("trying to place a null piece");
        
        piecePlacing.transform.position = GV.removeZ(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

    public void ConnectPieceToMouse(PieceGUI _piecePlacing)
    {
        piecePlacing = _piecePlacing;
    }
    
    public void PlacePiece()
    {//places it on mouse
        //Right here give the pieceGUI is list of goals, also set the final desitination
        Vector2 placementPos = GV.removeZ(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float boardtop = GV.SLOT_GUI_SIZE.y * GV.BOARD_SIZE.y;
        int col = (int)((placementPos.x / GV.SLOT_GUI_SIZE.x) + .5f);
        if (boardtop < placementPos.y)
        {
            piecePlacing.AddPathToEnd(new Vector2(placementPos.x, boardtop));
        }
        piecePlacing.AddPathToEnd(new Vector2(col, GV.BOARD_SIZE.y)); 
        Vector2 dropUntilFreeSpot = Board.Instance.GetDropGridPosByCol(col);
        piecePlacing.AddPathToEnd(dropUntilFreeSpot);
        piecePlacing.piece.gridLoc = Board.Instance.GetGridPosByRealWorldPos(dropUntilFreeSpot);
        Board.Instance.AddPiece(piecePlacing.piece, piecePlacing.piece.gridLoc);
        piecePlacing = null;
        
    }

    
}
