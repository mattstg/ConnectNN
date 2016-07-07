using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece {

    public int pidOwner;
    public PieceGUI pieceGUI;
    public Vector2 gridLoc;
    public List<PowerUp> powerups = new List<PowerUp>();
    public bool settled = false;

    public Piece(int _owner, PieceGUI pgui)
    {
        pidOwner = _owner;
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
            pu.usePower();
    }

    private void Settle()
    {
        UseAllPowerups();
        settled = true;
        Board.Instance.CheckForVictory(gridLoc);        
    }

    public void UpdateFalling(float dt)
    {
        if (pieceGUI.PathEmpty())
        {
            if (!pieceGUI.hasEnteredBoard)
                pieceGUI.hasEnteredBoard = true;

            if (gridLoc.y == 0 || Board.Instance.HasSlotHasSettled(gridLoc, GV.Direction.Down))
            {
                Settle();
            }
            else if (Board.Instance.IsSlotIsEmpty(gridLoc, GV.Direction.Down))
            {
                pieceGUI.AddPathToEnd(GV.RealWorldPosByGridLoc(Board.Instance.GetGridLocInDir(gridLoc, GV.Direction.Down)));
            }
        }
        else
        {
            pieceGUI.FollowPath(dt);
        }
    }
}
