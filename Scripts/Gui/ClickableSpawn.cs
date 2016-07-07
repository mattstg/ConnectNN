using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickableSpawn : MonoBehaviour {

    public int playerIDSpawn = 0;
    public Image coinImg;
    public MouseControl mcontrol;

    public void Initialize(int playerID, MouseControl mc)
    {
        playerIDSpawn = playerID;
        coinImg.color = GV.GAME_PLAYER_COLORS[playerID];
        mcontrol = mc;
    }

    public void OnClick()
    {
        mcontrol.SpawnClicked(playerIDSpawn);
    }
}
