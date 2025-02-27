using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public string[] previewTextContent_;
    public Sprite[] previewPictureContent_;

    private PauseManager pauseManager_;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseManager_ = GameObject.Find("Pause Manager").GetComponent<PauseManager>();
        pauseManager_.gameObject.SetActive(false);

        ScoreboardStrings scoreboardStrings = scoreboard.formatForScoreboard(false);

        scoreboardTime_.text = scoreboardStrings.times;
        scoreboardDate_.text = scoreboardStrings.dates;

        int levelNumber = scoreboard.getLevelIndex();
        previewText_.text = previewTextContent_[levelNumber];
        previewPicture_.GetComponent<Image>().sprite = previewPictureContent_[levelNumber];
    }

    public void reactivatePauseManager()
    {
        pauseManager_.gameObject.SetActive(true);
    }

}
