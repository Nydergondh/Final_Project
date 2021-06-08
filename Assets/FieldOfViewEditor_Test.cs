﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView_Test))]
public class FieldOfViewEditor_Test : Editor {
    void OnSceneGUI() {
        FieldOfView_Test fow = (FieldOfView_Test)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;

        Handles.DrawWireDisc(fow.transform.position, Vector3.up, fow.hearingRange);
        Handles.color = Color.green;

        if (fow.currentTarget != null) {
            Handles.DrawLine(fow.transform.position, fow.currentTarget.position);
        }

    }
}
#endif
