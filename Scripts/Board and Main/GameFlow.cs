﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour {

    static int nextAvailableID = 0;
    public int piecesPlayed = 0;   

    public GV.GameState curGameState = GV.GameState.PlayerWatchingDrop;
    int curPlayerID;

    BoardGUI boardgui;
    MouseControl mouseControl;
    List<Player> players = new List<Player>();
    public List<PieceGUI> allPieces = new List<PieceGUI>();
    List<PowerUp> powerupsQueued = new List<PowerUp>();
    public Transform gridLayout;
    PowerUpManager powerupManager;
    
	// Use this for initialization
	void Start () {
        boardgui = GetComponent<BoardGUI>();
        mouseControl = GetComponent<MouseControl>();
        powerupManager = GetComponent<PowerUpManager>();
        InitializeBoard();
        InitializePlayers();
	}
	
	// Update is called once per frame
	void Update () {
        Board.Instance.UpdateUnresolved(Time.deltaTime);
        ResolvePowerUps();
	}

    public void QueuePowerUp(PowerUp pup)
    {
        powerupsQueued.Add(pup);
    }

    
    public void ResolvePowerUps()
    {
        foreach (PowerUp pu in powerupsQueued)
            pu.usePower();
        powerupsQueued.Clear();
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
            GameObject spawnButton = Instantiate(Resources.Load("Prefabs/ClickableSpawn")) as GameObject;
            spawnButton.transform.SetParent(gridLayout);
            spawnButton.GetComponent<ClickableSpawn>().Initialize(i,GetComponent<MouseControl>());
            PieceFactory.Instance.AddScoreBoardPiece(i);
        }
    }

    public void PieceDropped()
    {
        piecesPlayed++;
        if (piecesPlayed % GV.POWERUP_SPAWN_RATE == 0)
            powerupManager.CreateRandomPowerup();
    }

    //hacks cuz button, make debug class later
    public void Rotate()
    {
        Board.Instance.RotateBoard();
    }
}
