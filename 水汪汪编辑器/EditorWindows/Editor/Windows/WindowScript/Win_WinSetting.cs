using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���б༭���������ô��ڣ�������
/// </summary>
class Win_WinSetting : SingletonBaseWindow
{
    private EM_WinSetting main;
    protected override void OnEnable()
    {
        base.OnEnable();
        IntiWindowsSetting("���������", "YuSheIcon.png");
    }
    [MenuItem("ˮ�������/������趨")]
    protected static void OpenWindow()
    {
        EditorWindow.GetWindow<Win_WinSetting>();
    }
    protected override void m_DrawWindows()
    {
        if (main == null) main = EM_WinSetting.Instance;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField("���ڱ���ͼ��", main.SettingData.BackgroundImagePath);
        if(GUILayout.Button("�޸�..."))
        {
            string path = EditorUtility.OpenFilePanelWithFilters("ѡ��ͼƬ", Application.dataPath,new string[]{"png","jpg" });
            if (path!="") 
            main.SettingData.BackgroundImagePath =  path;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField("����ͼ�꣺", main.SettingData.EditorIcon);
        if (GUILayout.Button("�޸�..."))
        {
            string path = EditorUtility.OpenFilePanelWithFilters("ѡ��ͼƬ", Application.dataPath, new string[] { "png", "jpg,icon" });
            if (path!="") 
            main.SettingData.EditorIcon = path;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextField("���������ļ�����·����", main.SettingData.AutoSavePath);
        if (GUILayout.Button("�޸�..."))
        {
            string path = EditorUtility.OpenFolderPanel("ѡ��ͼƬ", Application.dataPath, "");
            if (path!="") 
            main.SettingData.AutoSavePath = path;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("���������޸�"))
        {
            main.m_SaveData();
        }

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
 
    }

    public void m_SaveSetting()
    {

    }

}
