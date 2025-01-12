using System;
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
    public string names;
    public string times;
    public string dates;

    public ScoreboardStrings(string pa_names, string pa_times, string pa_dates)
    {
        names = pa_names;
        times = pa_times;
        dates = pa_dates;
    }
}

[CreateAssetMenu(fileName = "S_Scoreboard", menuName = "Scriptable Objects/S_Scoreboard")]
public class S_Scoreboard : ScriptableObject
{
    // Define a private object for locking
    private readonly object _lock = new object();
    string path;

    [SerializeField]
    private int numLevel_ = 1;
    private LevelLeaderboard[] allLeaderboards_;
    private LeaderboardEntry playerEntry = new LeaderboardEntry();

    private ScoreboardStrings scoreboardStrings;


    public void OnEnable()
    {
        path = Application.persistentDataPath + "/best_time.json";
        Debug.Log(numLevel_);
        allLeaderboards_ = new LevelLeaderboard[numLevel_];
        for (int i = 0; i < numLevel_; i++)
        {
            allLeaderboards_[i].init(10);
        }

        loadJSON();
    }

    public void sortTime(TimeSpan pa_time)
    {
        lock (_lock) // Ensure only one thread can execute this block at a time
        {
            try
            {
                playerEntry = new LeaderboardEntry();

                int sceneID = 0; // Warning: Replace with current scene ID
                int index = -1;

                LeaderboardEntry[] currentLevelEntries = allLeaderboards_[sceneID].entries;



                for (int i = 0; i < currentLevelEntries.Length; i++)
                {
                    TimeSpan currentIterationTime = currentLevelEntries[i].time;
                    if (pa_time < currentIterationTime || currentIterationTime == TimeSpan.Zero)
                    {

                        //seems to overrite the entry on the better place => 00:00:02.20, 00:00:06.20 => add 00:00:04.20 => 00:00:02.20, 00:00:04.20, 00:00:04.20 (I think)

                        index = i;
                        for (int k = currentLevelEntries.Length - 1; k > i + 1; k--)
                        {
                            currentLevelEntries[k] = currentLevelEntries[k - 1];
                        }

                        currentLevelEntries[i].time = pa_time;
                        currentLevelEntries[i].date = DateTime.Now;

                        return;
                    }
                }

                playerEntry.time = pa_time;
                playerEntry.date = DateTime.Now;

            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }



    public void saveJSON()
    {
        JSONObject bestTimeJSON = bestTimesToJSONObject();
        File.WriteAllText(path, bestTimeJSON.ToString(4));
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
                leaderboardEntryJSON["name"] = currentEntry.name;
                leaderboardEntryJSON["time"] = currentEntry.time;
                leaderboardEntryJSON["date"] = currentEntry.date.ToString("dd-MM-yyyy");
                levelLeaderboardJSON.Add(string.Format("{0:00}.", j + 1), leaderboardEntryJSON);
            }

            allLeaderboardsJSON.Add(string.Format("level {0}", i), levelLeaderboardJSON);
        }

        return allLeaderboardsJSON;
    }



    public void loadJSON()
    {
        if (!File.Exists(path))
        {
            saveJSON();
        }
        String bestTimesJSONString = File.ReadAllText(path);
        JSONObject json = (JSONObject)JSON.Parse(bestTimesJSONString);

        allLeaderboards_ = new LevelLeaderboard[numLevel_];

        int levelIndex = 0;

        foreach (String level in json.Keys)
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
                    name = entryData["name"],
                    time = TimeSpan.Parse(entryData["time"]),
                    date = parsedDate
                };

                // Assign to the entries array
                allLeaderboards_[levelIndex].entries[entryIndex] = entry;
                entryIndex++;
            }

            levelIndex++;
        }

        Debug.Log("Loading completed");
    }

    public ScoreboardStrings formatForScoreboard()
    {
        LevelLeaderboard levelLeaderboard = allLeaderboards_[0];

        scoreboardStrings.names = "";
        scoreboardStrings.times = "";
        scoreboardStrings.dates = "";

        for (int j = 0; j < levelLeaderboard.entries.Length; j++)
        {
            LeaderboardEntry currentEntry = levelLeaderboard.entries[j];

            formattingAid(currentEntry);
        }

        formattingAid(playerEntry);

        return scoreboardStrings;
    }

    private void formattingAid(LeaderboardEntry pa_entry)
    {
        if (!pa_entry.isEmpty())
        {
            scoreboardStrings.names += pa_entry.name + "\n";
            scoreboardStrings.times += string.Format("{0:00}:{1:00}:{2:00}", pa_entry.time.Minutes, pa_entry.time.Seconds, pa_entry.time.Milliseconds) + "\n";
            scoreboardStrings.dates += pa_entry.date.ToString("dd.MM.yyyy") + "\n";
        }
        else
        {
            scoreboardStrings.names += "\n";
            scoreboardStrings.times += "\n";
            scoreboardStrings.dates += "\n";
        }
    }
}
