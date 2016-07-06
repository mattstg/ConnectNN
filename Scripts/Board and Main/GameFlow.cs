using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour {

    static int nextAvailableID = 0;
    int currentGameTurn = 0;

    public GV.GameState curGameState = GV.GameState.PlayerWatchingDrop;
    int curPlayerID;

    BoardGUI boardgui;
    PiecePlacer piecePlacer;
    MouseControl mouseControl;
    List<Player> players = new List<Player>();
    public List<PieceGUI> allPieces = new List<PieceGUI>();
    PowerUpManager powerupManager;
    
	// Use this for initialization
	void Start () {
        boardgui = GetComponent<BoardGUI>();
        mouseControl = GetComponent<MouseControl>();
        piecePlacer = GetComponent<PiecePlacer>();
        powerupManager = GetComponent<PowerUpManager>();
        InitializeBoard();
        InitializePlayers();
        StartCurrentPlayersTurn();
	}
	
	// Update is called once per frame
	void Update () {
        switch (curGameState)
        {
            case GV.GameState.PlayerPlacing:
                piecePlacer.PlacePieceOnMouse();
                if(mouseControl.Clicked())
                    PieceWasDropped();
                break;
            case GV.GameState.PlayerWatchingDrop:
                UpdateFallingPieces();
                break;
            case GV.GameState.SettleAllPieces:
                break;
        }
	}

    public void InitializeBoard()
    {
        Board.Instance.Initialize(); //just creating the instance will create the board
        boardgui.CreateBoard();
    }

    public void InitializePlayers()
    {
        for (int i = 0; i < GV.GAME_PLAYERS; i++)
        {
            Color playersColor = (i < GV.GAME_PLAYER_COLORS.Count) ? GV.GAME_PLAYER_COLORS[i] : Color.white;
            players.Add(new Player(playersColor, nextAvailableID++));

        }
    }

    public void StartCurrentPlayersTurn()
    {
        if (currentGameTurn % GV.POWERUP_SPAWN_RATE == 0)
            powerupManager.CreateRandomPowerup();
        Player curPlayer = players[curPlayerID];
        PieceGUI newPiece = PieceFactory.Instance.MakePiece(curPlayer, GV.removeZ(Input.mousePosition));
        piecePlacer.ConnectPieceToMouse(newPiece);
        curGameState = GV.GameState.PlayerPlacing;
    }

    public void PieceWasDropped()
    {

        curGameState = GV.GameState.PlayerWatchingDrop;
        allPieces.Add(piecePlacer.piecePlacing);
        piecePlacer.PlacePiece();
    }

    public void AllPiecesPlaced()
    {
        AllPiecesSettled();
    }

    public void AllPiecesSettled()
    {
        curPlayerID++;
        curPlayerID %= GV.GAME_PLAYERS;
        currentGameTurn++;
        StartCurrentPlayersTurn();
    }

    public void UpdateFallingPieces()
    {
        bool allComplete = true;
        foreach (PieceGUI pgui in allPieces)
        {
            if (pgui.FollowPath(Time.deltaTime))
                allComplete = false;
        }
        if (allComplete)
        {
            AllPiecesPlaced();
        }
    }
}
