using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public string logFilePath;

    void Awake()
    {
        logFilePath = Path.Combine(Application.dataPath, "game_log.txt");

        #if UNITY_EDITOR
        //
        #else

        Debug.Log("Logger started. Log file path: " + logFilePath);
        
        DontDestroyOnLoad(this.gameObject);
        Application.logMessageReceived += Log;
        Log("Logger started (Log Message to test if Log function works)", "Logger", LogType.Log);
        #endif
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        string logEntry = $"{System.DateTime.Now} [{type}]: {logString}";

        Debug.Log("Log-Message received. Original Message (if not written in the Log): " + logString);

        // Writing to the log file
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine(logEntry);
        }
        
        // Writing to the console
        Debug.Log(logEntry);
    }

    void OnDestroy()
    {
        Debug.Log("Logger stopped");
        Application.logMessageReceived -= Log;
    }
}
