using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �Զ�����ʾ��Ϊ����������
/// </summary>
[CustomEditor(typeof(BTNodeData))]
public class CustomBehaviorTreeDataShow : Editor
{
    public void OnEnable()
    {
        //�����Զ�����
        if ((target as BTNodeData).BehaviorTreeData == null)
        {
            (target as BTNodeData).BehaviorTreeData = AssetDatabase.LoadAssetAtPath<TextAsset>((target as BTNodeData).TreeNodeDataPath);
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("�򿪱༭���޸�"))
        {
            if ((target as BTNodeData).BehaviorTreeData != null)
            {
                Win_BehaviorTree win = EditorWindow.GetWindow<Win_BehaviorTree>();
                win.editorWindow.Loader.m_LoadStaticData(target as BTNodeData);
            }
        }
    }
}

[CustomEditor(typeof(BTNodeObjcetDriver))]
public class CustomBehaviorTreeDriverShow : Editor
{
    public GUIStyle CheckStyle;
    public GUIStyle TitleStyle;

    public void SettingStyle()
    {
        CheckStyle = new GUIStyle(GUI.skin.button);
        CheckStyle.normal.textColor = Color.green;
        CheckStyle.fontStyle = FontStyle.Bold;
        CheckStyle.onNormal.textColor = Color.red;
        CheckStyle.onHover.textColor = Color.yellow;
    }

    private void OnEnable()
    {
        TitleStyle = new GUIStyle();
        TitleStyle.normal.textColor = Color.cyan;
        TitleStyle.fontStyle = FontStyle.Bold;
        TitleStyle.alignment = TextAnchor.MiddleCenter;

    }
    public override void OnInspectorGUI()
    {
        SettingStyle();
        EditorGUILayout.LabelField("��Ϊ��AI����������", TitleStyle);
        (target as BTNodeObjcetDriver).data = EditorGUILayout.ObjectField("��Ϊ�������ļ���", (target as BTNodeObjcetDriver).data, typeof(BTNodeData), false) as BTNodeData;
        if (GUILayout.Button("�򿪶�̬���", CheckStyle))
        {
            if (!Application.isPlaying)
            {

                Debug.LogWarning("��Ҫ������ģʽ�²��ܿ�����̬���");
                return;
            }
            (target as BTNodeObjcetDriver).b_DynamicCheck = true;
            (target as BTNodeObjcetDriver).RootNode = BTNodeLoader.Instance.LoadAndCheck((target as BTNodeObjcetDriver), ref (target as BTNodeObjcetDriver).dynamicDic);
            Dictionary<string, BTNodeInfo> dic = (target as BTNodeObjcetDriver).dynamicDic;
            if (dic != null)
            {
                Win_BehaviorTree win = EditorWindow.GetWindow<Win_BehaviorTree>();
                win.editorWindow.CheckingDirver = target as BTNodeObjcetDriver;
                win.editorWindow.Loader.m_LoadDynamicData(dic);
            }
            else
            {
                Debug.LogWarning("��Ҫ������ģʽ֮ǰ�����򹳶�̬�����ܵ��������");
            }
        }

    }
}
