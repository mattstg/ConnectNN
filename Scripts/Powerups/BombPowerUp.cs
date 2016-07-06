using UnityEngine;
using System.Collections;

public class BombPowerUp : PowerUp {

    public override void usePower()
    {
        for (int x = 0; x < GV.BOARD_SIZE.x; x++)
            Board.Instance.DestroyPiece(new Vector2(x,piece.gridLoc.y));
        for (int y = 0; y < GV.BOARD_SIZE.y; y++)
            Board.Instance.DestroyPiece(new Vector2(piece.gridLoc.x,y));
    }
}
