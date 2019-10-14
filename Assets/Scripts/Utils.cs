using System;
using System.Collections;
using System.Collections.Generic;
using Log;
using UnityEditor;
using UnityEngine;

public static class Utils
{


    public static LogWinScriptObject GetLogWinAsset()
    {
#if UNITY_EDITOR
        var lwso = Resources.LoadAll<LogWinScriptObject>("");
        Debug.Log(lwso.Length);
        if (lwso.Length == 0)
        {
            throw new NullReferenceException("LogWinScriptObject is null");
        }

        if (lwso.Length >1)
        {
            throw new ArgumentOutOfRangeException("LogWinScriptObject lenth out range");
        }

        return lwso[0];
#endif
        
        
    }
    

}
