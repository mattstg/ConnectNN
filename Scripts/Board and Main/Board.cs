using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board
{
    Piece[,] board;
    List<Piece> unresolvedPieces = new List<Piece>();

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
        unresolvedPieces.Add(piece);
    }

    public void MovePiece(Vector2 fromLoc, Vector2 toLoc)
    {
        board[(int)toLoc.x, (int)toLoc.y] = board[(int)fromLoc.x, (int)fromLoc.y];
        board[(int)fromLoc.x, (int)fromLoc.y] = null;

    }

    public void UpdateUnresolved(float dt)
    {
        foreach (Piece p in unresolvedPieces)
            p.UpdateFalling(dt);
        for (int i = unresolvedPieces.Count - 1; i >= 0; i--)
            if (unresolvedPieces[i].settled)
                unresolvedPieces.RemoveAt(i);
    }

    public void DestroyPiece(Vector2 gridLoc)
    {
        if (board[(int)gridLoc.x, (int)gridLoc.y] != null)
        {
            Piece toDestroy = board[(int)gridLoc.x, (int)gridLoc.y];
            board[(int)gridLoc.x, (int)gridLoc.y] = null;
            GameObject.Destroy(toDestroy.pieceGUI.gameObject);
            MonoBehaviour.FindObjectOfType<GameFlow>().allPieces.Remove(toDestroy.pieceGUI);
            for (int y = (int)gridLoc.y; y < GV.BOARD_SIZE.y; y++) //since piece removed, set all above unresolved
                if (board[(int)gridLoc.x, y] != null)
                {
                    board[(int)gridLoc.x, (int)y].settled = false;
                    if(!unresolvedPieces.Contains(board[(int)gridLoc.x, (int)y]))
                        unresolvedPieces.Add(board[(int)gridLoc.x, (int)y]);
                }
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

    //careful handles -1 weird
    public bool IsSlotIsEmpty(Vector2 loc, GV.Direction dir = GV.Direction.Center)
    {
        Vector2 finalLoc = GetGridLocInDir(loc, dir);
        return (board[(int)finalLoc.x, (int)finalLoc.y] == null);
    }

    public bool HasSlotHasSettled(Vector2 loc, GV.Direction dir = GV.Direction.Center)
    {
        Vector2 finalLoc = GetGridLocInDir(loc, dir);
        if (board[(int)finalLoc.x, (int)finalLoc.y] == null)
        {
            return false;
        }
        return (board[(int)finalLoc.x, (int)finalLoc.y].settled);
    }

    public void CheckForVictory(Vector2 gridLoc)
    {

    }
    //helper funcs

    Vector2 ClampGridLoc(Vector2 v1)
    {
        Vector2 toRet = v1;
        toRet.x = (toRet.x < 0) ? 0 : toRet.x;
        toRet.x = (toRet.x >= GV.BOARD_SIZE.x) ? GV.BOARD_SIZE.x - 1 : toRet.x;
        toRet.y = (toRet.y < 0) ? 0 : toRet.y;
        toRet.y = (toRet.y >= GV.BOARD_SIZE.y) ? GV.BOARD_SIZE.y - 1 : toRet.y;
        return toRet;
    }

    public Vector2 GetGridLocInDir(Vector2 loc, GV.Direction dir)
    {
        switch (dir)
        {
            case GV.Direction.Center: //do nut
                break;
            case GV.Direction.Down:
                loc.y--;
                break;
            case GV.Direction.Left:
                loc.x--;
                break;
            case GV.Direction.Right:
                loc.x++;
                break;
            case GV.Direction.Up:
                loc.y++;
                break;
        }
        return ClampGridLoc(loc);
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
