using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceGUI : MonoBehaviour {

    public Piece piece;
    List<Vector3> path = new List<Vector3>(); //list of world pos points to follow
    public bool hasEnteredBoard = false;
    //returns false if not falling
    public void Initialize(Piece _piece)
    {
        piece = _piece;
    }

    public void FollowPath(float dt)
    {
        Vector3 moveTowards = getCurrentTarget();
        if (moveTowards == transform.position)
            return;

        transform.position = Vector3.MoveTowards(transform.position, path[0], GV.PIECE_FALL_SPEED * dt);
        if (!hasEnteredBoard)
            return;

        PowerUp powerup = GameObject.FindObjectOfType<PowerUpManager>().GetPowerUpAtGridLoc(Board.Instance.GetGridPosByRealWorldPos(transform.position));
        if (powerup != null)
        {
            GameObject.FindObjectOfType<PowerUpManager>().RemovePowerUpAtGridLoc(Board.Instance.GetGridPosByRealWorldPos(transform.position));
            piece.AddPowerup(powerup);
        }

        Vector2 curGridPiece = Board.Instance.GetGridPosByRealWorldPos(transform.position);
        if (curGridPiece != piece.gridLoc)
        {
            Board.Instance.MovePiece(piece.gridLoc, curGridPiece);
            piece.gridLoc = curGridPiece;
        }
    }

    public void AddPathToEnd(Vector3 pathPos)
    {
        path.Add(pathPos);
    }

    public void AddPathToEnd(Vector2 pathPos)
    {
        path.Add(new Vector3(pathPos.x, pathPos.y, 0));
    }

    private Vector3 getCurrentTarget()
    {
        if (path.Count <= 0)
            return transform.position;

        if (transform.position == path[0])
        {
            path.RemoveAt(0);
            if (path.Count <= 0)
            {
                return transform.position;
            }
            else
            {
                return path[0];
            }
        }
        else
        {
            return path[0];
        }
    }

    public bool PathEmpty()
    {
        return path.Count == 0;
    }
}
