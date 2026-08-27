using UnityEngine;

public static class Logger
{
    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void Log(string message)
    {
        Debug.Log(message);
    }
}
