using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GV {

    public enum PowerupTypes { bomb };
    public enum GameState { PlayerPlacing, PlayerWatchingDrop, SettleAllPieces };
    public enum Victory { None, Blue, Red };

    public static readonly int GAME_PLAYERS = 3;
    public static readonly int POWERUP_SPAWN_RATE = 5; //every n turns
    public static readonly List<Color> GAME_PLAYER_COLORS = new List<Color>() {Color.red,Color.blue,Color.cyan,Color.green,Color.yellow}; //must have greater or equal colors to number of players, else white

    public static readonly Vector2 BOARD_SIZE = new Vector2(15,6);  //board width and height
    
    public static readonly Vector2 SLOT_GUI_SIZE = new Vector2(1,1);  //width and height of sprite, 1 unit = 100 pixels of actual picture

    public static readonly float PIECE_FALL_SPEED = 3;

    public static Vector3 removeZ(Vector3 v3)
    {
        return new Vector3(v3.x, v3.y, 0);
    }
    
}
