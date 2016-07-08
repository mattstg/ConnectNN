using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombPowerUp : PowerUp {

    public override void usePower()
    {
        Debug.Log("bomb goes off");
        List<GV.Direction> directions = new List<GV.Direction>();
        for(int i = 0; i < GV.PU_BOMB_DIRECTIONS; i++)
        {
            GV.Direction dir = (GV.Direction)Random.Range(0, System.Enum.GetNames(typeof(GV.Direction)).Length);
            if(dir != GV.Direction.Center)
                directions.Add(dir);
        }

        foreach (GV.Direction dir in directions)
        {
            Debug.Log("dir: " + dir.ToString());
            Board.Instance.DestroyInDirection(piece.gridLoc, dir);
        }
    }
}
