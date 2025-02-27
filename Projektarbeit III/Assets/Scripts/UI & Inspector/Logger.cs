using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private static Logger instance;
    public static Logger Instance
    {
        get
        {
            return instance;
        }
    }

    private string logFilePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            logFilePath = Path.Combine(Application.dataPath, "game_log.txt");
            Debug.Log($"Logger started. Log file path: {logFilePath}");
            Application.logMessageReceived += Log;
            Log("Logger initialized successfully", "Logger", LogType.Log);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
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

    public void FindLogger()
    {
        instance = this.gameObject.GetComponent<Logger>();
        #if !UNITY_EDITOR
        if (instance == null)
        {
            Debug.LogError("Logger not found");
            return;
        }
        else if (instance != null)
        {
            if (instance.logFilePath == null)
            {
                instance.logFilePath = System.IO.Path.Combine(Application.dataPath, "game_log.txt");
                instance.Log("Logger started over script. Log file path: " + instance.logFilePath, "Logger", LogType.Log);
            }
        }
        #endif
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= Log;
        Debug.Log("Logger stopped");
    }
}
