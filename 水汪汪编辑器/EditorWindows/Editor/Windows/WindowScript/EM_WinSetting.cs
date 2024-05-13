using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
/// <summary>
/// ���༭���������ù����࣬�����洢���趨�༭�������Ϣ��Ҳ�����������ɱ༭����
/// </summary>
class EM_WinSetting : EditorMain, ISaveLoadWindowMain
{
    public static EM_WinSetting Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EM_WinSetting();
            return _instance;
        }
    }

    public Data_WinSetting SettingData;
    public string DirectoryPath => Application.dataPath + @"\ˮ�����༭��\Editor\Windows\EditorAsset\EditorData\";

    public string DataName => "WinEditorSetting.json";

    /// <summary>
    /// Ψһ����
    /// </summary>
    private static EM_WinSetting _instance = new EM_WinSetting();
    public Texture WindowsBackground;
    public EM_WinSetting()
    {
        m_LoadData();
        WindowsBackground = WindowUtility.LoadAssetFromPath<Texture>(SettingData.BackgroundImagePath);
        //�浵�߼� ��

    }

    public void m_SaveData()
    {
        //�ٴμ��
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }
        JsonManager.Instance.SaveDataToPath(SettingData, DirectoryPath + DataName);

    }

    public void m_LoadData()
    {
        if (File.Exists(DirectoryPath + DataName))
        {
            SettingData = JsonManager.Instance.LoadDataFromPath<Data_WinSetting>(DirectoryPath + DataName);
            return;
        }
        //��һ�δ���������ΪĬ��
        SettingData = new Data_WinSetting();

        //Ĭ�ϱ���
        WindowUtility.SettingPathAndCreateDirectory(ref SettingData.BackgroundImagePath, Application.dataPath + "/ˮ�����༭��/EditorAsset/ArtEditorAsset/bg.png");
        //Ĭ�ϴ浵λ��
        WindowUtility.SettingPathAndCreateDirectory(ref SettingData.AutoSavePath, Application.dataPath + "/ˮ�����༭��/EditorAsset/EditorData/");
        //Ĭ�ϵ���Ϊ��ͼƬ
        WindowUtility.SettingPathAndCreateDirectory(ref SettingData.BehaviorTreeBGImage, Application.dataPath + "/ˮ�����༭��/EditorAsset/ArtEditorAsset/TreeRight.jpg");
        //�༭��ͼ��
        WindowUtility.SettingPathAndCreateDirectory(ref SettingData.EditorIcon, Application.dataPath + "/ˮ�����༭��/EditorAsset/ArtEditorAsset/Icon.jpg");

    }
}
[Serializable]
class Data_WinSetting
{
    /// <summary>
    /// �༭��������Դ�洢·��
    /// </summary>
    public string BackgroundImagePath = null;
    /// <summary>
    /// ��Ϊ���༭���ı���ͼ
    /// </summary>
    public string BehaviorTreeBGImage;
    public string AutoSavePath = null;
    /// <summary>
    /// �༭��ͼ������
    /// </summary>
    public string EditorIcon;
}