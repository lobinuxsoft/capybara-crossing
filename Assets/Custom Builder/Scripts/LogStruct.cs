#if UNITY_EDITOR

using UnityEngine;

namespace Utilities.Builder
{
    [System.Serializable]
    public struct LogStruct
    {
        public LogType logType;
        public string condition;
        public string stackTrace;

        public string FormatLog(Color logColor, Color warnningColor, Color errorColor, Color assertColor, Color exeptionColor, Color stackTraceColor)
        {
            string messageLog = string.Empty;
            Color color = Color.white;

            switch (logType)
            {
                case LogType.Error:
                    color = errorColor;
                    break;
                case LogType.Assert:
                    color = assertColor;
                    break;
                case LogType.Warning:
                    color = warnningColor;
                    break;
                case LogType.Log:
                    color = logColor;
                    break;
                case LogType.Exception:
                    color = exeptionColor;
                    break;
            }

            messageLog += $"<color=#{ColorUtility.ToHtmlStringRGB(color)}><b>[{logType}]:</b></color> <b>{condition}</b>";
            messageLog += !string.IsNullOrEmpty(stackTrace) ? $"\n<color=#{ColorUtility.ToHtmlStringRGB(stackTraceColor)}><b>Stack Trace:</b></color> {stackTrace}" : "\n";
        
            return messageLog;
        }
    }
}

#endif