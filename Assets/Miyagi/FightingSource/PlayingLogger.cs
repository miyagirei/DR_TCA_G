using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayingLogger : MonoBehaviour
{
    [SerializeField] Text log_text;
    string log_messages = "";
    int max_lines = 10;

    public void Log(string message , Color color) {
        string color_message = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>";
        log_messages += color_message + "\n";

        string[] lines = log_messages.Split("\n");
        if (lines.Length > max_lines) {
            log_messages = string.Join("\n", lines, lines.Length - max_lines, max_lines);
        }

        log_text.text = log_messages;
    }

    public void ClearLog() {
        log_messages = "";
        log_text.text = "";
    }

    private static PlayingLogger instance;

    private void Awake()
    {
        instance = this;
    }

    public static void LogStatic(string message, Color color) {
        if (instance != null) {
            instance.Log(message, color);
        }
    }
}
