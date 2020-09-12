using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Utils {
    //static MethodInfo _clearConsoleMethod;
    //static MethodInfo clearConsoleMethod {
    //    get {
    //        if (_clearConsoleMethod == null) {
    //            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
    //            Type logEntries = assembly.GetType("UnityEditor.LogEntries");
    //            _clearConsoleMethod = logEntries.GetMethod("Clear");
    //        }
    //        return _clearConsoleMethod;
    //    }
    //}

    //public static void ClearLogConsole() {
    //    clearConsoleMethod.Invoke(new object(), null);
    //}

    public static Vector3 GetVectorFromAngle(float angle) {
        //angle  = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (n < 0) {
            n += 360;
        }
        return n;
    }
}