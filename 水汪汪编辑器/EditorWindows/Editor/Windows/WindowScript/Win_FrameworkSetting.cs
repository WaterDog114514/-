using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Win_FrameworkSetting : SingletonBaseWindow
{
    //������ɫ����
    private GUIStyle TitleStyle = new GUIStyle();
    private FrameworkSettingData settingData;


    private void IntiTitleStyle()
    {
        TitleStyle.normal.textColor = Color.cyan;
        TitleStyle.fontSize = 16;
    }

    protected override void OnEnable()
    {
        if (settingData == null) settingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
        IntiTitleStyle();
        base.OnEnable();
        IntiWindowsSetting("С�������", "YuSheIcon.png");
    }
    [MenuItem("ˮ�������/С������趨")]
    protected static void OpenWindow()
    {
        EditorWindow.GetWindow<Win_FrameworkSetting>();
    }
    protected override void m_DrawWindows()
    {
        //Excel�����ļ���������
        DrawExcelSetting();
        //AB��������ػ���
        DrawABSetting();

    }
    private bool IsFoldExcel = false;
    private void DrawExcelSetting()
    {
        IsFoldExcel = EditorGUILayout.Foldout(IsFoldExcel, "Excel�����ļ���ȡ���ã�");
        if (!IsFoldExcel)
        {
            //����·�����
            GUILayout.Label("����·������", TitleStyle);
            EditorGUILayout.BeginHorizontal();
            settingData.loadContainerSetting.DataPath = EditorGUILayout.TextField("Excel�������ļ���·����", settingData.loadContainerSetting.DataPath);
            settingData.loadContainerSetting.IsDebugStreamingAssetLoad = EditorGUILayout.Toggle("����Stream���Լ���", settingData.abLoadSetting.IsStreamingABLoad);
            EditorGUILayout.EndHorizontal();
        }
    }
    private bool IsFoldAB = false;
    private void DrawABSetting()
    {
        IsFoldAB = EditorGUILayout.Foldout(IsFoldAB, "AB��������ã�");
        if (!IsFoldAB)
        {
            //����·�����
            GUILayout.Label("����·������", TitleStyle);

            EditorGUILayout.BeginHorizontal();
            settingData.abLoadSetting.ABLoadPath = EditorGUILayout.TextField("AB������·����", settingData.abLoadSetting.ABLoadPath);
            settingData.abLoadSetting.IsStreamingABLoad = EditorGUILayout.Toggle("����Stream���Լ���", settingData.abLoadSetting.IsStreamingABLoad);
            EditorGUILayout.EndHorizontal();

            //���ñ༭������
            EditorGUILayout.BeginHorizontal();
            settingData.abLoadSetting.IsDebugABLoad = EditorGUILayout.Toggle("�����༭�����Լ���", settingData.abLoadSetting.IsDebugABLoad);
            GUILayout.Label("(�����󣬽�ͨ��Editorͬ������AB��)");
            EditorGUILayout.EndHorizontal();
            if (settingData.abLoadSetting.IsDebugABLoad)
            {
                settingData.abLoadSetting.ABEditorLoadPath = EditorGUILayout.TextField("�༭��AB������·����", settingData.abLoadSetting.ABEditorLoadPath);
            }

            //�������
            GUILayout.Label("����������", TitleStyle);
            settingData.abLoadSetting.ABMainName = EditorGUILayout.TextField("������", settingData.abLoadSetting.ABMainName);
            GUILayout.Label("�ǰ�������", TitleStyle);
            settingData.abLoadSetting.ObjPrefabPackName = EditorGUILayout.TextField("����Ԥ�������", settingData.abLoadSetting.ObjPrefabPackName);
            settingData.abLoadSetting.UIPrefabPackName = EditorGUILayout.TextField("UIԤ�������", settingData.abLoadSetting.UIPrefabPackName);
            EditorGUILayout.Space(20);
            if (GUILayout.Button("���������޸�"))
            {
                SettingDataLoader.Instance.SaveData(settingData);
                AssetDatabase.Refresh();
            }
        }
    }
}
