using UnityEngine;
using System.Collections;

public class UILinks : MonoBehaviour {

    public Transform spawnCoinGrid;
    public Transform scoreBoardGrid;

    public void Start()
    {
        GV.coinPanelGrid = spawnCoinGrid;
        GV.scorePanelGrid = scoreBoardGrid;
    }
}
