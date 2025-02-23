using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewManager : MonoBehaviour
{

    [Header("//----- Scoreboard -----\\\\")]
    public S_Scoreboard scoreboard;

    public TextMeshProUGUI scoreboardTime_;
    public TextMeshProUGUI scoreboardDate_;

    [Header("//----- Preview -----\\\\")]

    public TextMeshProUGUI previewText_;
    public GameObject previewPicture_;

    [Header("//----- Content -----\\\\")]

    public string previewTextContent_ = "Hello World!";
    public Sprite previewPictureContent_;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreboardStrings scoreboardStrings = scoreboard.formatForScoreboard(false);

        scoreboardTime_.text = scoreboardStrings.times;
        scoreboardDate_.text = scoreboardStrings.dates;

        previewText_.text = previewTextContent_;
        previewPicture_.GetComponent<Image>().sprite = previewPictureContent_;
    }

}
