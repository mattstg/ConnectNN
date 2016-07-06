using UnityEngine;
using System.Collections;

public class Board
{
    Piece[,] board;


    public void Initialize()
    {
        board = new Piece[(int)GV.BOARD_SIZE.x, (int)GV.BOARD_SIZE.y]; //it should all be initialized null;
        for (int x = 0; x < GV.BOARD_SIZE.x; x++)
            for (int y = 0; y < GV.BOARD_SIZE.y; y++)
                board[x, y] = null;
    }

    public void AddPiece(Piece piece, Vector2 location)
    {
        board[(int)location.x, (int)location.y] = piece;
    }

    public void DestroyPiece(Vector2 gridLoc)
    {
        if (board[(int)gridLoc.x, (int)gridLoc.y] != null)
        {
            Piece toDestroy = board[(int)gridLoc.x, (int)gridLoc.y];
            board[(int)gridLoc.x, (int)gridLoc.y] = null;
            GameObject.Destroy(toDestroy.pieceGUI.gameObject);
            MonoBehaviour.FindObjectOfType<GameFlow>().allPieces.Remove(toDestroy.pieceGUI);
        }
    }

    public Vector2 GetGridPosByRealWorldPos(Vector2 realWorldPosition)
    {
        Vector2 result =  new Vector2((int)(realWorldPosition.x / GV.SLOT_GUI_SIZE.x), (int)(realWorldPosition.y / GV.SLOT_GUI_SIZE.y));
        //Debug.Log("result: " + result);
        return result;

    }

    public Piece GetPieceByWorldPos(Vector2 realWorldPosition)
    {
        return GetPieceByGridPos(GetGridPosByRealWorldPos(realWorldPosition)); //lol
    }

    public Piece GetPieceByGridPos(Vector2 gridPosition)
    {
        return board[(int)gridPosition.x, (int)gridPosition.y];
    }

    public Vector2 GetDropGridPosByCol(int col)
    {
        for (int i = (int)GV.BOARD_SIZE.y - 1; i >= 0; --i)
        {
            if (board[col, i] != null)
            {
                return new Vector2(col, i + 1);
            }
        }
        return new Vector2(col, 0);
    }

    #region Singleton
    private static Board instance;

    private Board() { }

    public static Board Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Board();
            }
            return instance;
        }
    }
    #endregion
}
