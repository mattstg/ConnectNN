using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece {

    public Player owner;
    public PieceGUI pieceGUI;
    public Vector2 gridLoc;
    public List<PowerUp> powerups = new List<PowerUp>();
    public bool completedActions = false;

    public Piece(Player _owner, PieceGUI pgui)
    {
        owner = _owner;
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
        completedActions = true;
    }
	
}
