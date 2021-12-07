using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GDG.Utils;
using UnityEngine;
namespace GDG.Base
{
    public class LogManager
    {
        private static string FilePath { get => $"{ Application.persistentDataPath}/Logger/UnityLogger.txt"; }
        public static bool EnableConsoleLog = true;
        public static bool EnableEditorLog = true;
        public static bool EnableWriteIntoLogFile = true;
        public static bool EnableTime = true;
        public static bool EnableFilePath = true;
        public static bool EnableLog = true;
        public static bool LogErrorOrThrowException = true;

        /// <summary>
        /// 日志文件最大容量(MB)，超过会自动清空
        /// </summary>
        /// 
        public static float LoggerMaxMBSize = 50;
        private static void LoggerWriteIN(string info)
        {
            if (!EnableWriteIntoLogFile)
                return;
            if(!File.Exists(FilePath))
            {
                BuildFileAsync(null);
            }
            if (new FileInfo(FilePath)?.Length > LogManager.LoggerMaxMBSize * 1024 * 1024)
            {
                ClearFile();
                LogManager.LogWarning("清空了一次日志");
            }

            if (!LogManager.EnableTime)
                Append($"[{DateTime.Now.ToString("HH:mm:ss")}]{info}");
            else
                Append(info);
        }
        private static string ColorToHex(Color color)
        {
            int r = Mathf.RoundToInt(color.r * 255.0f);
            int g = Mathf.RoundToInt(color.g * 255.0f);
            int b = Mathf.RoundToInt(color.b * 255.0f);
            int a = Mathf.RoundToInt(color.a * 255.0f);
            string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
            return hex;
        }

        #region 控制台日志
        public static string MessageFormat(object message, string tag, string invoker, string callerFilePath, int callerLineNumber)
        {
            if (!EnableLog)
                return "";
            if(message==null)
                message = "Null";
            string info;
            if (EnableTime)
                info = $"<b>[{tag}]</b>[{DateTime.Now.ToString("HH:mm:ss")}]   <b>{message.ToString()}</b>\n";
            else
                info = $"<b>[{tag}]</b>   {message.ToString()}\n";

            if (EnableFilePath)
                info = $"{info}From：{invoker}( )  |   line:{callerLineNumber}   |   {callerFilePath}";

            if (File.Exists(FilePath))
            {
                LoggerWriteIN($"[{tag}] [{DateTime.Now.ToString("HH:mm:ss")}] [File: {callerFilePath}，Method：{invoker}( )，Line：{callerLineNumber}] -{message.ToString()}");
            }
            else
            {
                BuildFileAsync(()=>
                {
                    LoggerWriteIN($"[{tag}] [{DateTime.Now.ToString("HH:mm:ss")}] [File: {callerFilePath}，Method：{invoker}( )，Line：{callerLineNumber}] -{message.ToString()}");
                });
            }
            return info;
        }
        public static void LogCustom(object message,
            string tag,
            Color color,
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (!EnableConsoleLog || !EnableLog)
                return;
            var info = MessageFormat(message, tag, invoker, callerFilePath, callerLineNumber);
            Debug.LogFormat(string.Format($"<color=#{ColorToHex(color)}>" + "{0}</color>", info));
        }
        public static void LogSucess(object message,
            string tag = "SUCESS",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (!EnableConsoleLog || !EnableLog)
                return;
            var info = MessageFormat(message, tag, invoker, callerFilePath, callerLineNumber);
            Debug.LogFormat(string.Format("<color=#8BBF41>{0}</color>", info));
        }
        public static void LogInfo(
            object message,
            string tag = "INFO",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (!EnableConsoleLog || !EnableLog)
                return;
            var info = MessageFormat(message, tag, invoker, callerFilePath, callerLineNumber);
            Debug.Log(info);
        }
        public static void LogWarning(
            object message,
            string tag = "WARNING",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (!EnableConsoleLog || !EnableLog)
                return;
            var info = MessageFormat(message, tag, invoker, callerFilePath, callerLineNumber);
            Debug.LogWarning(string.Format("<color=#E2B652>{0}</color>", info));
        }
        public static void LogError(
            object message,
            string tag = "ERROR",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            var info = MessageFormat(message, tag, invoker, callerFilePath, callerLineNumber);

            if (LogErrorOrThrowException && EnableLog && EnableConsoleLog)
                Debug.LogError(string.Format("<color=#FF534A>{0}</color>", info));
            else
                throw new CustomErrorException(info.ToString(), tag, invoker, callerFilePath, callerLineNumber);
        }
        public static void EditorLog(
            object message,
            string tag = "EDITOR",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (!EnableConsoleLog || !EnableLog)
                return;
            var info = MessageFormat(message, tag, invoker, callerFilePath, callerLineNumber);
            Debug.LogFormat(string.Format("<color=#4E6EF2>{0}</color>", info));
        }
        #endregion

        static void BuildFileAsync(Action callback)
        {
            if (!Directory.Exists($"{Application.persistentDataPath}/Logger"))
            {
                DirectoryInfo info = Directory.CreateDirectory($"{Application.persistentDataPath}/Logger");
                LogManager.LogWarning($"创建了文件夹：{Application.persistentDataPath}/Logger");
            }

            if (!File.Exists($"FilePath"))
            {
                File.Create(FilePath).Dispose();
                LogManager.LogWarning($"创建了日志文件{FilePath}");
                
            }
            callback?.Invoke();
        }
        static void Append(string infos)
        {
            if (!File.Exists(FilePath))
                return;
            try
            {
                FileStream fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(infos);
                sw.Dispose();
                fs.Dispose();
                
            }
            catch { }
        }
        static void ClearFile()
        {
            new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite).Dispose();
        }
    }
}

