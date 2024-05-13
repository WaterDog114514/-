using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Win_ExcelTool : SingletonBaseWindow
{
    private ExcelToolSettingData SettingInfo;


    //������ɫ����
    private GUIStyle TitleStyle = new GUIStyle();
    private void IntiTitleStyle()
    {
        TitleStyle.normal.textColor = Color.red;
        TitleStyle.fontSize = 16;
    }

    protected override void OnEnable()
    {
        if (SettingInfo == null) SettingInfo = EM_ExcelTool.Instance.SettingInfo;

        IntiTitleStyle();
        base.OnEnable();
        IntiWindowsSetting("Excel����ת�����ƹ���", "YuSheIcon.png");

    }
    [MenuItem("ˮ��������/Excel���ع���")]
    protected static void OpenWindow()
    {
        EditorWindow.GetWindow<Win_ExcelTool>();
    }
    protected override void m_DrawWindows()
    {
        GUILayout.Label("����������ã�", TitleStyle);
        SettingInfo.propertyNameRowIndex = EditorGUILayout.IntField("�ֶ��������У�",SettingInfo.propertyNameRowIndex);
        SettingInfo.propertyTypeRowIndex = EditorGUILayout.IntField("�ֶ������������У�",SettingInfo.propertyTypeRowIndex);
        SettingInfo.keyRowIndex= EditorGUILayout.IntField("������key��������У�",SettingInfo.keyRowIndex);
        SettingInfo.ReallyDataStartRowIndex = EditorGUILayout.IntField("�������ݴ洢��ʼ��ȡ�У�",SettingInfo.ReallyDataStartRowIndex);
        SettingInfo.SuffixName = EditorGUILayout.TextField("�Զ����׺��:",SettingInfo.SuffixName);
        GUILayout.Label("��Excel������������������ࣺ", TitleStyle);
        if (GUILayout.Button("���ɵ���Excel�ļ�������������..."))
        {
           EM_ExcelTool.Instance.GenerateExcelInfo();
        }
        if (GUILayout.Button("��������Excel�ļ�������������")) 
        { 
            EM_ExcelTool.Instance.GenerateAllExcelInfo();
        }

        GUILayout.Label("�����ɺõ��������������ת��Ϊ�����ƣ�", TitleStyle);
        if (GUILayout.Button("ת������Excel�ļ�..."))
        {
           EM_ExcelTool.Instance.GenerateExcelBinary();
        }
        if (GUILayout.Button("����ת��Excel�ļ�"))
        {
            EM_ExcelTool.Instance.GenerateAllExcelBinary();
        }

        //����·�����
        GUILayout.Label("·������", TitleStyle);
        //EXCEL�ļ�·������
        EditorGUILayout.BeginHorizontal();
        SettingInfo.ExcelDirectory_Path = EditorGUILayout.TextField("Excel�ļ�·����",SettingInfo.ExcelDirectory_Path);
        if (GUILayout.Button("����·��"))
        {
            string path = EditorUtility.SaveFolderPanel("����Ҫת��Excel���ļ���·��",Application.dataPath,null);
            SettingInfo.ExcelDirectory_Path = path;
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        SettingInfo.OutPath = EditorGUILayout.TextField("���·����", SettingInfo.OutPath);
        if (GUILayout.Button("����·��"))
        {
            string path = EditorUtility.SaveFolderPanel("����Ҫת��Excel���ļ���·��", Application.dataPath, null);
            SettingInfo.OutPath = path;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("�򿪵����ļ���"))
        {
            if(Directory.Exists(SettingInfo.OutPath)) 
            System.Diagnostics.Process.Start("explorer.exe", SettingInfo.OutPath);
            else
               EditorUtility.DisplayDialog("����","�����ڸ�·���ļ���","�ð�~");
        }

        //���ñ༭������
        EditorGUILayout.Space(20);
        if (GUILayout.Button("���������޸�"))
        {
            SettingDataLoader.Instance.SaveData(SettingInfo);
            AssetDatabase.Refresh();
        }

    }
}
