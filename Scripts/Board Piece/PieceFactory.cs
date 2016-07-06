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
    public PieceGUI MakePiece(Player playerOwner, Vector3 atPos)
    {
        GameObject go = (MonoBehaviour.Instantiate(Resources.Load("Prefabs/Piece"),atPos,Quaternion.identity) as GameObject);
        if (!go)
            Debug.Log("yup that weird");
        go.GetComponent<SpriteRenderer>().color = playerOwner.color;
        PieceGUI pgui = go.GetComponent<PieceGUI>();
        pgui.Initialize(new Piece(playerOwner, pgui));
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

}
