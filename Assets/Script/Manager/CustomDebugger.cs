using UnityEngine;

namespace OthelloGameProj
{
    /// <summary>
    /// カスタムされたデバッグログ
    /// </summary>
    public static class CustomDebugger
    {
        public static void NormalLog(string msg)
        {
            Debug.Log(msg);
        }

        public static void Warninglog(string msg)
        {
            Debug.LogWarning(msg);
        }

        public static void Errorlog(string msg)
        {
            Debug.LogError(msg);
        }

        public static void BoldLog(string msg, bool isBold)
        {
            string log = isBold ? $"<b>{msg}</b>" : msg;
            Debug.Log(log);
        }

        public static void ColorLog(string msg, GameConst.LogLevel logLevel)
        {
            if (logLevel == GameConst.LogLevel.Lime)
                Debug.Log($"<color=lime>{msg}</color>");
            else if (logLevel == GameConst.LogLevel.Yellow)
                Debug.LogWarning($"<color=yellow>{msg}</color>");
            else
                Debug.LogError($"<color=red>{msg}</color>");
        }

        public static void CustomLog(string msg, GameConst.LogLevel logLevel, bool isBold)
        {
            if (logLevel == GameConst.LogLevel.Lime)
            {
                string log = isBold ? $"<b><color=lime>{msg}</color></b>" : $"<color=lime>{msg}</color>";
                Debug.Log(log);
            }
            else if (logLevel == GameConst.LogLevel.Yellow)
            {
                string log = isBold ? $"<b><color=yellow>{msg}</color></b>" : $"<color=yellow>{msg}</color>";
                Debug.LogWarning(log);
            }
            else
            {
                string log = isBold ? $"<b><color=red>{msg}</color></b>" : $"<color=red>{msg}</color>";
                Debug.LogError(log);
            }
        }
    }
}
