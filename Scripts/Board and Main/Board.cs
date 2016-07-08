using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board
{
    static int failGaurd = 0;
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
        if (!IsOutOfBounds(gridLoc) && board[(int)gridLoc.x, (int)gridLoc.y] != null)
        {
            Piece toDestroy = board[(int)gridLoc.x, (int)gridLoc.y];
            board[(int)gridLoc.x, (int)gridLoc.y] = null;
            GameObject.Destroy(toDestroy.pieceGUI.gameObject);
            //MonoBehaviour.FindObjectOfType<GameFlow>().allPieces.Remove(toDestroy.pieceGUI);
            for (int y = (int)gridLoc.y; y < GV.BOARD_SIZE.y; y++) //since piece removed, set all above unresolved
                if (board[(int)gridLoc.x, y] != null && board[(int)gridLoc.x, (int)y].settled == true)
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


#region boardPowerups
    public void DestroyInDirection(Vector2 gridLoc, GV.Direction dir)
    {
        Vector2 locInDir = GetGridLocInDir(gridLoc, dir, false);
        if (IsOutOfBounds(locInDir))
            return;
        DestroyPiece(locInDir);
    }

    public void RotateBoard()
    {
        unresolvedPieces.Clear();
        for (int x = 0; x < GV.BOARD_SIZE.x; x++)
            for (int y = 0; y < GV.BOARD_SIZE.y; y++)
                if (board[x, y] != null)
                {
                    board[x, y].settled = false;
                }
        for (int x = 0; x < GV.BOARD_SIZE.x; x++)
            for (int y = 0; y < GV.BOARD_SIZE.y; y++)
                if (board[x, y] != null)
                {
                    Piece p = board[x, y];
                    Debug.Log("p loc: " + p.gridLoc);
                    PieceFactory.Instance.CreateAndDropPieceAtLocation(new Vector2(p.gridLoc.y, p.gridLoc.x), p.pid);
                    DestroyPiece(p.gridLoc);
                }
        
        //destroy all pieces and remake board
        //there should be safety check because of width bypass height
        
    }

#endregion


    #region Victory  //Victory Checks
    private void PlayerScored(int pid)
    {
        GV.scoreBoards[pid].ModScored(1);
    }

    public void CheckForVictory(Vector2 gridLoc, int pid)
    {
        if(VictoryinDirections(gridLoc,pid,GV.Direction.North,GV.Direction.South)
           || VictoryinDirections(gridLoc,pid,GV.Direction.NE,GV.Direction.SW)
           || VictoryinDirections(gridLoc,pid,GV.Direction.East,GV.Direction.West)
           || VictoryinDirections(gridLoc,pid,GV.Direction.NW, GV.Direction.SE))
        {
            PlayerScored(pid);
        }
    }

    public bool VictoryinDirections(Vector2 gridLoc, int pid, GV.Direction dir1, GV.Direction dir2)
    {
        int score = ScoreInDirection(gridLoc, dir1, pid) + ScoreInDirection(gridLoc, dir2, pid) + 1;// plus one for my tile
        return score >= GV.GAME_SCORE_TO_WIN;
    }

    public bool VictoryinDirections(Vector2 gridLoc, int pid, List<GV.Direction> dirs)
    {
        int score = 1;
        foreach (GV.Direction dir in dirs)
        {
            score += ScoreInDirection(gridLoc,dir,pid);
        }
        return score >= GV.GAME_SCORE_TO_WIN;
    }


    private int ScoreInDirection(Vector2 gridLoc, GV.Direction dir, int pid)
    {
        Vector2 locInDir = GetGridLocInDir(gridLoc, dir, false);
        if (!IsOutOfBounds(locInDir) && Board.Instance.GetPlayerIDInGrid(locInDir) == pid)
            return ScoreInDirection(locInDir, dir, pid) + 1;
         return 0;
    }

    public int GetPlayerIDInGrid(Vector2 gridLoc)
    {
        if (GetPieceByGridPos(gridLoc) == null)
            return -1;
        return board[(int)gridLoc.x, (int)gridLoc.y].pid;
    }
    #endregion
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

    bool IsOutOfBounds(Vector2 gridLoc)
    {
        if (gridLoc.x < 0 || gridLoc.x >= GV.BOARD_SIZE.x || gridLoc.y < 0 || gridLoc.y > GV.BOARD_SIZE.y)
            return true;
        return false;
    }

    public Vector2 GetGridLocInDir(Vector2 loc, GV.Direction dir, bool forceClamp = true)
    {
        switch (dir)
        {
            case GV.Direction.Center: //do nut
                break;
            case GV.Direction.North:
                loc += new Vector2(0, 1);
                break;
            case GV.Direction.NE:
                loc += new Vector2(1, 1);
                break;
            case GV.Direction.East:
                loc += new Vector2(1, 0);
                break;
            case GV.Direction.SE:
                loc += new Vector2(1, -1);
                break;
            case GV.Direction.South:
                loc += new Vector2(0, -1);
                break;
            case GV.Direction.SW:
                loc += new Vector2(-1, -1);
                break;
            case GV.Direction.West:
                loc += new Vector2(-1, 0);
                break;
            case GV.Direction.NW:
                loc += new Vector2(-1, 1);
                break;            
        }
        if(forceClamp)
            return ClampGridLoc(loc);
        return loc;
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
