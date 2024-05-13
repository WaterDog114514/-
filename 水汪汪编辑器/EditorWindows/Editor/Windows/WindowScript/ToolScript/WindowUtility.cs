using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ˮ������ʵ�ù����� ��չ�� ���淽����
/// </summary>
public static class WindowUtility
{
    #region �������� �Ѿ����ã�ʹ��Json���ؼ���
    /// <summary>
    /// ��¼��ǰ������ݣ���������
    /// </summary>
    // public static object[] RecordWindowData()
    // {
    //     //����һ�� ��ȡ��������
    //     Type t = this.GetType();
    //     FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
    //     object[] recordValues = new object[fields.Length];
    //     for (int i = 0; i < fields.Length; i++)
    //     {
    //         //��ɸ��һ��
    //         if (fields[i].Name == "Title" || fields[i].Name == "Icon" ||
    //             fields[i].Name == "m_TitleContent" ||
    //             fields[i].Name == "m_Parent" || fields[i].Name == "m_Pos" ||
    //             fields[i].Name == "m_Notification" || fields[i].Name == "m_FadeoutTime" || fields[i].Name == "originWindowData") //continue;
    //         recordValues[i] = fields[i].GetValue(this);
    //     }
    //     return recordValues;
    // }


    // protected void SetWindowsData(object[] datas)
    // {
    //     //����һ�� ��ȡ��������
    //     Type t = this.GetType();
    //     FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
    //     for (int i = 0; i < fields.Length; i++)
    //     {
    //         //��ɸ��һ��
    //         if (fields[i].Name == "Title" || fields[i].Name == "Icon" ||
    //             fields[i].Name == "m_TitleContent" ||
    //             fields[i].Name == "m_Parent" || fields[i].Name == "m_Pos" ||
    //             fields[i].Name == "m_Notification" || fields[i].Name == "m_FadeoutTime" || fields[i].Name == "originWindowData") continue;
    //         fields[i].SetValue(this, originWindowData[i]);
    //     }
    // }
    #endregion

    /// <summary>
    /// �ø����main�͸��ര��һһ��Ӧ�ĸ�ֵ
    /// </summary>
    public static Type getMainType(this BaseWindow win)
    {
        Type type = win.GetType();
        if (type == typeof(Win_WinSetting))
            return typeof(EM_WinSetting);
        else if (type == typeof(Win_UIListener))
            return typeof(EW_UIListener);
        return null;
    }

    /// <summary>
    /// �����ĵ������û��Զ���ѡ�񱣴�·��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void SaveWindowsDataCustom(this BaseWindow win)
    {
        string path = EditorUtility.SaveFilePanelInProject("ѡ�񱣴�·��", "", "json", "");
        SaveWindowsDataToPath(win, path);
    }
    /// <summary>
    /// �����ĵ���ָ��·��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="win"></param>
    /// <param name="path"></param>
    public static void SaveWindowsDataToPath(this BaseWindow win, string path)
    {
        object data = win.editorMain;
        JsonManager.Instance.SaveDataToPath(data, path, JsonType.JsonUtlity);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// ���ش�������ʹ��������
    /// </summary>
    /// <param name="win"></param>
    public static void LoadWindowsData(this BaseWindow win)
    {
        //�ǳ����� ��ʷת��������ʹ��
        string path = EditorUtility.OpenFilePanel("ѡ��򿪵�·��", "", "json");
        LoadWindowsData(win, path);
    }
    /// <summary>
    /// ���ش�������
    /// </summary>
    /// <param name="win"></param>
    public static EditorMain LoadWindowsData(this BaseWindow win, string path)
    {
        //�ǳ����� ��ʷת��������ʹ��
        object data = JsonManager.Instance.LoadDataFromPath(win.MainType, path);
        return data as EditorMain;
    }

    /// <summary>
    /// ���Ʊ��� �������֮���
    /// </summary>
    /// <typeparam name="T">EditorMain������</typeparam>
    /// <param name="win">Ҫ���ƵĴ���</param>
    public static void SettingSaveLoadDraw(this BaseWindow win)
    {
        EditorGUILayout.Space(40);
        if (GUILayout.Button(new GUIContent("�������ݵ�ĳ�ļ���")))
        {
            SaveWindowsDataCustom(win);
        }
        //�����������
        if (GUILayout.Button(new GUIContent("��...�����������")))
        {
            LoadWindowsData(win);
        }
        if (GUILayout.Button(new GUIContent("������������")))
        {
            win.editorMain = Activator.CreateInstance(win.MainType) as EditorMain;
        }

    }

    /// <summary>
    /// ����·����ͬʱ�������ļ��У������ڱ༭���ı���·��string��
    /// </summary>
    /// <param name="ParamterPath"></param>
    /// <param name="targetPath"></param>
    public static void SettingPathAndCreateDirectory(ref string ParamterPath, string targetPath)
    {
        ParamterPath = targetPath;
        string DirectoryPath = null;
        if (targetPath.Contains('/'))
            DirectoryPath = targetPath.Substring(0, targetPath.LastIndexOf('/'));
        if (!Directory.Exists(DirectoryPath))
            Directory.CreateDirectory(DirectoryPath);
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// ţ�Ƶļ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ast"></param>
    /// <param name="loadPath"></param>
    /// <returns></returns>
    public static T LoadAssetFromPath<T>(string loadPath) where T : UnityEngine.Object
    {
        if(loadPath==null) return null;
        string path = "Assets/" + loadPath.Replace(Application.dataPath + "/", null);
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }
}
