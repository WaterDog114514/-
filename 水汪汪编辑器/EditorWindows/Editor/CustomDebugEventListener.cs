using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// ���������޸Ŀ��ӻ��¼�������
/// </summary>
[CustomEditor(typeof(DebugEventListenerGizmos))]
public class CustomDebugEventListener : Editor
{
    private SerializedProperty list;
    private Transform ListennerTransform;

    private void OnEnable()
    {
        ListennerTransform = (target as DebugEventListenerGizmos).event_Listener;
    }
    public override void OnInspectorGUI()
    {
        GUILayout.Label("���ӻ������󶨲鿴��");
        EditorGUILayout.ObjectField(new GUIContent("����������"), ListennerTransform, typeof(Transform), true);
        if (GUILayout.Button("��λ������������"))
        {
            EditorGUIUtility.PingObject(ListennerTransform);
        }

    }
}
