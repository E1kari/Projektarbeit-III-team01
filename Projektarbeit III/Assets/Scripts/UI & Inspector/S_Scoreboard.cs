using System;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

public struct LeaderboardEntry
{
    public string name;
    public TimeSpan time;
    public DateTime date;

    public bool isEmpty()
    {
        if (name == null && time == TimeSpan.Zero && date == DateTime.MinValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public struct LevelLeaderboard
{
    public LeaderboardEntry[] entries;

    public void init(int pa_numEntries)
    {
        entries = new LeaderboardEntry[pa_numEntries];
    }
}

public struct ScoreboardStrings
{
    public string times;
    public string dates;

    public ScoreboardStrings(string pa_times, string pa_dates)
    {
        times = pa_times;
        dates = pa_dates;
    }
}

[CreateAssetMenu(fileName = "S_Scoreboard", menuName = "Scriptable Objects/S_Scoreboard")]
public class S_Scoreboard : ScriptableObject
{
    // Define a private object for locking
    private readonly object _lock = new object();
    string path_;

    S_SceneSaver sceneSaver_;

    [SerializeField]
    private int numLevel_ = 3;
    private int levelNumber_;

    private LevelLeaderboard[] allLeaderboards_;

    private int playerEntryIndex_ = -1;
    private LeaderboardEntry playerEntry_ = new LeaderboardEntry();

    private ScoreboardStrings scoreboardStrings;


    public void OnEnable()
    {
        sceneSaver_ = Resources.Load<S_SceneSaver>("Scriptable Objects/S_SceneSaver");

        path_ = Application.persistentDataPath + "/best_time.json";
        allLeaderboards_ = new LevelLeaderboard[numLevel_];
        for (int i = 0; i < numLevel_; i++)
        {
            allLeaderboards_[i].init(10);
        }

        loadJSON();
    }

    private void determineLevelNumber()
    {
        string levelName = sceneSaver_.GetCurrentLevelSceneName();
        string levelNameNumber = levelName.Substring(levelName.Length - 1, 1);
        levelNumber_ = int.Parse(levelNameNumber);
    }

    public void sortTime(TimeSpan pa_time)
    {
        lock (_lock) // Ensure only one thread can execute this block at a time
        {
            playerEntry_ = new LeaderboardEntry();

            playerEntry_.time = pa_time;
            playerEntry_.date = DateTime.Now;

            determineLevelNumber();

            LeaderboardEntry[] currentLevelEntries = allLeaderboards_[levelNumber_ - 1].entries;


            for (int i = 0; i < currentLevelEntries.Length; i++)
            {
                TimeSpan currentIterationTime = currentLevelEntries[i].time;
                if (pa_time < currentIterationTime || currentIterationTime == TimeSpan.Zero)
                {
                    playerEntryIndex_ = i;

                    for (int k = currentLevelEntries.Length - 1; k > i; k--)
                    {
                        currentLevelEntries[k] = currentLevelEntries[k - 1];
                    }

                    currentLevelEntries[i].time = pa_time;
                    currentLevelEntries[i].date = DateTime.Now;

                    return;
                }
            }
        }
    }



    public void saveJSON()
    {
        JSONObject bestTimeJSON = bestTimesToJSONObject();
        File.WriteAllText(path_, bestTimeJSON.ToString(4));
    }

    public JSONObject bestTimesToJSONObject()
    {
        JSONObject allLeaderboardsJSON = new JSONObject();
        JSONObject levelLeaderboardJSON;
        JSONObject leaderboardEntryJSON;

        for (int i = 0; i < allLeaderboards_.Length; i++)
        {
            levelLeaderboardJSON = new JSONObject();

            for (int j = 0; j < allLeaderboards_[i].entries.Length; j++)
            {
                leaderboardEntryJSON = new JSONObject();

                var currentEntry = allLeaderboards_[i].entries[j];
                leaderboardEntryJSON["time"] = currentEntry.time;
                leaderboardEntryJSON["date"] = currentEntry.date.ToString("dd-MM-yyyy");
                levelLeaderboardJSON.Add(string.Format("{0:00}.", j + 1), leaderboardEntryJSON);
            }

            allLeaderboardsJSON.Add(string.Format("level {0}", i + 1), levelLeaderboardJSON);
        }

        return allLeaderboardsJSON;
    }


    public void loadJSON()
    {
        if (!File.Exists(path_))
        {
            saveJSON();
        }
        String bestTimesJSONString = File.ReadAllText(path_);
        JSONObject json = (JSONObject)JSON.Parse(bestTimesJSONString);

        allLeaderboards_ = new LevelLeaderboard[numLevel_];

        int levelIndex = 0;

        foreach (string level in json.Keys)
        {
            // Initialize the LevelLeaderboard
            JSONNode levelData = json[level];
            int numEntries = levelData.Count;
            allLeaderboards_[levelIndex].init(numEntries);

            int entryIndex = 0;
            foreach (var entryKey in levelData.Keys)
            {
                JSONNode entryData = levelData[entryKey];

                string dateString = entryData["date"];
                DateTime parsedDate;
                if (!DateTime.TryParseExact(dateString, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    parsedDate = DateTime.MinValue; // Default value for invalid dates
                }

                // Populate LeaderboardEntry
                LeaderboardEntry entry = new LeaderboardEntry
                {
                    time = TimeSpan.Parse(entryData["time"]),
                    date = parsedDate
                };

                // Assign to the entries array
                allLeaderboards_[levelIndex].entries[entryIndex] = entry;
                entryIndex++;
            }

            levelIndex++;
        }
    }

    public ScoreboardStrings formatForScoreboard(bool markPlayer = true)
    {
        bool playerInTop10 = playerEntryIndex_ == -1 ? false : true;

        determineLevelNumber();
        LevelLeaderboard levelLeaderboard = allLeaderboards_[levelNumber_ - 1];

        scoreboardStrings.times = "";
        scoreboardStrings.dates = "";

        for (int j = 0; j < levelLeaderboard.entries.Length; j++)
        {
            LeaderboardEntry currentEntry = levelLeaderboard.entries[j];

            if (j == playerEntryIndex_ && markPlayer)
            {
                formattingAid(currentEntry, true);
                continue;
            }

            formattingAid(currentEntry);
        }

        if (!playerInTop10 && markPlayer)
        {
            formattingAid(playerEntry_, true);
        }
        else
        {
            formattingAid(new LeaderboardEntry());
        }

        return scoreboardStrings;
    }

    private void formattingAid(LeaderboardEntry pa_entry, bool isPlayerEntry = false)
    {
        if (isPlayerEntry)
        {
            scoreboardStrings.times += string.Format("<b><color=#9ECC91>{0:00}:{1:00}:{2:000}</color></b>", pa_entry.time.Minutes, pa_entry.time.Seconds, pa_entry.time.Milliseconds) + "\n";
            scoreboardStrings.dates += string.Format("<b><color=#9ECC91>{0}</color> </b>", pa_entry.date.ToString("dd.MM.yyyy")) + "\n";
        }
        else if (!pa_entry.isEmpty())
        {
            scoreboardStrings.times += string.Format("{0:00}:{1:00}:{2:000}", pa_entry.time.Minutes, pa_entry.time.Seconds, pa_entry.time.Milliseconds) + "\n";
            scoreboardStrings.dates += pa_entry.date.ToString("dd.MM.yyyy") + "\n";
        }
        else
        {
            scoreboardStrings.times += "\n";
            scoreboardStrings.dates += "\n";
        }
    }

    public ScoreboardStrings getPlayerStrings()
    {
        ScoreboardStrings playerScore;

        playerScore.times = string.Format("{0:00}:{1:00}:{2:000}", playerEntry_.time.Minutes, playerEntry_.time.Seconds, playerEntry_.time.Milliseconds) + "\n";
        playerScore.dates = playerEntry_.date.ToString("dd.MM.yyyy") + "\n";

        return playerScore;
    }
}
