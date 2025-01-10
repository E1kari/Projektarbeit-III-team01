using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Timer", menuName = "Scriptable Objects/Timer")]
public class Timer : ScriptableObject
{
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
    }

    public float retrieveTime()
    {
        if (running_)
        {
            return Time.time - startTime_;
        }

        return time_;
    }

    public string retrieveTimeAsString()
    {
        float time = retrieveTime();
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
    }
}
