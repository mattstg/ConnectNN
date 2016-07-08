using UnityEngine;
using System.Collections;

public class PieceFactory
{

    #region Singleton
    private static PieceFactory instance;

    private PieceFactory() { }

    public static PieceFactory Instance
    {
        get{
            if(instance == null)
            {
                instance = new PieceFactory();
            }
            return instance;
        }
    }
#endregion

    //Peace gui also contains the peice, so just return the higher level
    public PieceGUI MakePiece(int pid, Vector3 atPos)
    {
        GameObject go = (MonoBehaviour.Instantiate(Resources.Load("Prefabs/Piece"),atPos,Quaternion.identity) as GameObject);
        if (!go)
            Debug.Log("yup that weird");
        go.GetComponent<SpriteRenderer>().color = GV.GAME_PLAYER_COLORS[pid];
        PieceGUI pgui = go.GetComponent<PieceGUI>();
        pgui.Initialize(new Piece(pid, pgui));
        return go.GetComponent<PieceGUI>();
    }

    public SlotGUI MakeSlot(Vector2 worldPos)
    {
        GameObject slot = MonoBehaviour.Instantiate(Resources.Load("Prefabs/Slot"), new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity) as GameObject;
        SlotGUI slotgui = slot.GetComponent<SlotGUI>();
        slotgui.slotGridPos = Board.Instance.GetGridPosByRealWorldPos(worldPos);
        slotgui.slotWorldPos = worldPos;
        return slotgui;
    }

    public void AddScoreBoardPiece(int pid)
    {
        GameObject scoreBoardPiece = MonoBehaviour.Instantiate(Resources.Load("Prefabs/ScoreBoardPiece")) as GameObject;
        ScoreGUI scoreGUI = scoreBoardPiece.GetComponent<ScoreGUI>();
        scoreBoardPiece.transform.SetParent(GV.scorePanelGrid);
        foreach(UnityEngine.UI.Image img in scoreGUI.GetComponentsInChildren<UnityEngine.UI.Image>())
              img.color = GV.GAME_PLAYER_COLORS[pid];
        GV.scoreBoards.Add(pid,scoreGUI);
    }

    public void CreateAndDropPieceAtLocation(Vector2 worldPos, int pid)
    {
        PieceGUI curPiecePlacing = MakePiece(pid, worldPos);
        Vector2 placementPos = GV.removeZ(worldPos); //initial drop
        int col = (int)((placementPos.x / GV.SLOT_GUI_SIZE.x) + .5f);  //col itll land in
        curPiecePlacing.AddPathToEnd(GV.RealWorldPosByGridLoc(new Vector2(col, GV.BOARD_SIZE.y))); //above the board
        curPiecePlacing.AddPathToEnd(GV.RealWorldPosByGridLoc(new Vector2(col, GV.BOARD_SIZE.y - 1))); //first slot

        Board.Instance.AddPiece(curPiecePlacing.piece, new Vector2(col, GV.BOARD_SIZE.y - 1));
        curPiecePlacing.piece.gridLoc = new Vector2(col, GV.BOARD_SIZE.y - 1);
    }
    

}
