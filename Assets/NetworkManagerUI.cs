using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NetworkManager))]
public class NetworkManagerUI : Editor
{
    public override void OnInspectorGUI()
    {
        var t = target as NetworkManager;
        if (GUILayout.Button("Host"))
            t.Host();

        if (GUILayout.Button("Connect"))
            t.Connect();

        if (GUILayout.Button("Send Message"))
            t.Connect();
    }
}
