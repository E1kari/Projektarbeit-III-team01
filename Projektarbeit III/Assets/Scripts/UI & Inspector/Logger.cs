using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public string logFilePath;

    void Awake()
    {
        logFilePath = Path.Combine(Application.dataPath, "game_log.txt");
        Debug.Log("Logger started. Log file path: " + logFilePath);
        
        DontDestroyOnLoad(this.gameObject);
        Application.logMessageReceived += Log;
        Log("Logger started (Log Message to test if Log function works)", "Logger", LogType.Log);
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        string logEntry = $"{System.DateTime.Now} [{type}]: {logString}";

        Debug.Log("Log-Message received. Original Message (if not written in the Log): " + logString);

        // Log-Nachricht in die Datei schreiben
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine(logEntry);
        }
        
        // Ausgabe in der Konsole
        Debug.Log(logEntry);
    }

    void OnDestroy()
    {
        Debug.Log("Logger stopped");
        Application.logMessageReceived -= Log;
    }
}
