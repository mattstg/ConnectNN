using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceGUI : MonoBehaviour {

    public Piece piece;
    List<Vector3> path = new List<Vector3>(); //list of world pos points to follow
    //returns false if not falling
    public void Initialize(Piece _piece)
    {
        piece = _piece;
    }

    public bool FollowPath(float dt)
    {
        Vector3 moveTowards = getCurrentTarget();
        if (moveTowards == transform.position)
        {
            piece.UseAllPowerups();
            return false;
        }

        transform.position = Vector3.MoveTowards(transform.position, path[0], GV.PIECE_FALL_SPEED * dt);
        PowerUp powerup = GameObject.FindObjectOfType<PowerUpManager>().GetPowerUpAtGridLoc(Board.Instance.GetGridPosByRealWorldPos(transform.position));
            
        if (powerup != null)
        {
            GameObject.FindObjectOfType<PowerUpManager>().RemovePowerUpAtGridLoc(Board.Instance.GetGridPosByRealWorldPos(transform.position));
            piece.AddPowerup(powerup);
        }
        return true;
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
}
