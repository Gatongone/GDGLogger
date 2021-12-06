using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GDG.Base;
using UnityEngine;
namespace GDG.Utils
{
    public class CustomErrorException : Exception
    {
        public CustomErrorException(
            string message,
            string tag = "ERROR",
            [CallerMemberNameAttribute] string invoker = "unknown",
            [CallerFilePath] string callerFilePath = "unknown",
            [CallerLineNumber] int callerLineNumber = -1
            )
        {
            var info = LogManager.MessageFormat(message, tag, invoker, callerFilePath, callerLineNumber);
            throw new Exception(info);
        }
    }
}