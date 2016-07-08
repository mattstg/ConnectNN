using UnityEngine;
using System.Collections;

public class ScoreGUI : MonoBehaviour {

    public UnityEngine.UI.Text text;
    int score = 0;

    public void ModScored(int mod)
    {
        score += mod;
        text.text = score.ToString();
    }
}
