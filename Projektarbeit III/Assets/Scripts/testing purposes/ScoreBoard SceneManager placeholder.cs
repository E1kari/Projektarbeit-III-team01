using TMPro;
using UnityEngine;

public class ScoreBoardSceneManagerplaceholder : MonoBehaviour
{

    public S_Scoreboard scoreboard;

    public TextMeshProUGUI name;
    public TextMeshProUGUI time;
    public TextMeshProUGUI date;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreboardStrings scoreboardStrings = scoreboard.formatForScoreboard();

        name.text = scoreboardStrings.names;
        time.text = scoreboardStrings.times;
        date.text = scoreboardStrings.dates;
    }

}
