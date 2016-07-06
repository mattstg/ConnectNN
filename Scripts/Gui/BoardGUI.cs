using UnityEngine;
using System.Collections;

public class BoardGUI : MonoBehaviour {

    public SlotGUI[,] slotboard;
    public Transform slotParent;

    //will create images of boards
    public void CreateBoard()
    {
        slotboard = new SlotGUI[(int)GV.BOARD_SIZE.x,(int)GV.BOARD_SIZE.y];

        for (int x = 0; x < GV.BOARD_SIZE.x; x++)
            for (int y = 0; y < GV.BOARD_SIZE.y; y++)
            {
                slotboard[x, y] = PieceFactory.Instance.MakeSlot(new Vector2(x * GV.SLOT_GUI_SIZE.x, y * GV.SLOT_GUI_SIZE.y));
                slotboard[x, y].transform.SetParent(slotParent);
            }
    }

    
    
}
