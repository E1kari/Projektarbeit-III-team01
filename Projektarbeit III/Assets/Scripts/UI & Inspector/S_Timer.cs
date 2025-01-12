using System;
using UnityEngine;
using SimpleJSON;
using System.IO;


[CreateAssetMenu(fileName = "Timer", menuName = "Scriptable Objects/Timer")]
public class S_Timer : ScriptableObject
{
    [SerializeField]
    private S_Scoreboard scoreboard_;
    private float startTime_;
    private float time_;
    private bool running_;



    public void StartTimer()
    {
        time_ = 0f;
        startTime_ = Time.time;
        running_ = true;
    }

    public void StopTimer()
    {
        time_ = Time.time - startTime_;
        running_ = false;
        scoreboard_.sortTime(TimeSpan.FromSeconds(time_));
        scoreboard_.saveJSON();
    }

    public float retrieveTime()
    {
        if (running_)
        {
            return Time.time - startTime_;
        }
        return time_;
    }

    public TimeSpan retrieveTimeAsTimeSpan()
    {
        float time = retrieveTime();
        return TimeSpan.FromSeconds(time);
    }

    public string retrieveTimeAsString()
    {
        TimeSpan timeSpan = retrieveTimeAsTimeSpan();
        return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
    }

}
