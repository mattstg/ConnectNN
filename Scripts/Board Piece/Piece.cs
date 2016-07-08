using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece {

    public int pid;
    public PieceGUI pieceGUI;
    public Vector2 gridLoc;
    public List<PowerUp> powerups = new List<PowerUp>();
    public bool settled = false;

    public Piece(int _owner, PieceGUI pgui)
    {
        pid = _owner;
        pieceGUI = pgui;
    }

    public void AddPowerup(PowerUp pu)
    {
        pu.piece = this;
        powerups.Add(pu);
    }

    public void UseAllPowerups()
    {
        foreach (PowerUp pu in powerups)
            GameObject.FindObjectOfType<GameFlow>().QueuePowerUp(pu);
    }

    private void Settle()
    {
        UseAllPowerups();
        settled = true;
        Board.Instance.CheckForVictory(gridLoc,pid);        
    }

    public void UpdateFalling(float dt)
    {
        if (pieceGUI.PathEmpty())
        {
            if (!pieceGUI.hasEnteredBoard)
                pieceGUI.hasEnteredBoard = true;

            if (gridLoc.y == 0 || Board.Instance.HasSlotHasSettled(gridLoc, GV.Direction.South))
            {
                Settle();
            }
            else if (Board.Instance.IsSlotIsEmpty(gridLoc, GV.Direction.South))
            {
                pieceGUI.AddPathToEnd(GV.RealWorldPosByGridLoc(Board.Instance.GetGridLocInDir(gridLoc, GV.Direction.South)));
            }
        }
        else
        {
            pieceGUI.FollowPath(dt);
        }
    }
}
