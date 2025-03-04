using TMPro;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{

    public S_Scoreboard scoreboard;

    public TextMeshProUGUI scoreboardTime_;
    public TextMeshProUGUI scoreboardDate_;

    public TextMeshProUGUI latestTime_;
    public TextMeshProUGUI latestDate_;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreboardStrings scoreboardStrings = scoreboard.formatForScoreboard();

        scoreboardTime_.text = scoreboardStrings.times;
        scoreboardDate_.text = scoreboardStrings.dates;

        ScoreboardStrings playerStrings = scoreboard.getPlayerStrings();

        latestTime_.text = playerStrings.times;
        latestDate_.text = playerStrings.dates;
    }

}
