using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour {

    Dictionary<Vector2, PowerUpGUI> powerups = new Dictionary<Vector2, PowerUpGUI>();


    public void CreateRandomPowerup()
    {
        Vector2 placement = new Vector2(-1,-1);
        while(placement.x == -1 || placement.y == -1)
        {
            int xloc = Random.Range(0,(int)GV.BOARD_SIZE.x);
            int yloc = Random.Range(0,(int)GV.BOARD_SIZE.y);
            placement = new Vector2(xloc,yloc);
            if(powerups.ContainsKey(placement) || Board.Instance.GetPieceByGridPos(placement) != null)
                placement.x = -1;
        }
        PowerUpGUI newpu = CreateRandomPowerupPrefab(new Vector3(placement.x, placement.y, 0));
        powerups.Add(placement,newpu);
    }

    private PowerUpGUI CreateRandomPowerupPrefab(Vector3 atPos)
    {
        GameObject newPowerUpObj = Instantiate(Resources.Load("Prefabs/PowerUp"), atPos, Quaternion.identity) as GameObject;
        int myEnumMemberCount = System.Enum.GetNames(typeof(GV.PowerupTypes)).Length;
        int selection = Random.Range(0, (int)myEnumMemberCount);
        PowerUp newPowerUp;
        switch ((GV.PowerupTypes)selection)
        {
            case GV.PowerupTypes.bomb:
                 newPowerUp = new BombPowerUp();
                break;
            default:
                Debug.LogError("error, unhandled type: " + (GV.PowerupTypes)selection);
                newPowerUp = new BombPowerUp();
                break;
        }
        newPowerUpObj.GetComponent<PowerUpGUI>().powerup = newPowerUp;
        return newPowerUpObj.GetComponent<PowerUpGUI>();
    }

    public PowerUp GetPowerUpAtGridLoc(Vector2 gridLoc)
    {
        //Debug.Log("searching for powerup at gridLoc: " + gridLoc);
        if (powerups.ContainsKey(gridLoc))
        {
            Debug.Log("powerup found");
            if (powerups[gridLoc].powerup == null)
                Debug.Log("butfuk");
            return powerups[gridLoc].powerup;
        }
        return null;
    }

    public void RemovePowerUpAtGridLoc(Vector2 gridLoc)
    {
        if (powerups.ContainsKey(gridLoc))
        {
            PowerUpGUI pgui = powerups[gridLoc];
            powerups.Remove(gridLoc);
            GameObject.Destroy(pgui.gameObject);
        }
        
    }
}
