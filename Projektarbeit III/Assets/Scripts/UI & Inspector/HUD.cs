using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private bool showFps = false;

    [SerializeField]
    private bool showTime = true;

    [SerializeField]
    private Timer timer_;


    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int width = Screen.width, height = Screen.height;
        GUIStyle style = new GUIStyle();


        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height / 20;
        style.normal.textColor = Color.white;

        if (showFps)
        {
            Rect rect = new Rect(20, 20, width, height / 50);
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.} fps", fps);
            GUI.Label(rect, text, style);
        }
        if (showTime)
        {
            Rect rect = new Rect(width - (width / 7), 20, width, height / 50);
            string text = timer_.retrieveTimeAsString();
            GUI.Label(rect, text, style);
        }
    }
}
