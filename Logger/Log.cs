using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using GDG.Base;

namespace GDG.Utils
{
    public class Log
    {
        public static void Info(
            object message,
            string tag = "INFO",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            LogManager.LogInfo(message, tag, invoker, callerFilePath, callerLineNumber);
        }
        public static void Sucess(
            object message,
            string tag = "SUCCESS",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            LogManager.LogSucess(message, tag, invoker, callerFilePath, callerLineNumber);
        }
        public static void Warning(
            object message,
            string tag = "WARNING",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            LogManager.LogWarning(message, tag, invoker, callerFilePath, callerLineNumber);
        }
        public static void Error(
            object message,
            string tag = "ERROR",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            LogManager.LogError(message, tag, invoker, callerFilePath, callerLineNumber);
        }
        public void Custom(object message,
        string tag,
        Color color,
        [CallerMemberNameAttribute] string invoker = "unknown",
        [CallerFilePath] string callerFilePath = "unknown",
        [CallerLineNumber] int callerLineNumber = -1)
        {
            LogManager.LogCustom(message,tag,color,invoker, callerFilePath, callerLineNumber);
        }
        public static void Editor(
            object message,
            string tag = "EDITOR",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1)
        {

            LogManager.EditorLog(message, tag, invoker, callerFilePath, callerLineNumber);
        }
    }
}