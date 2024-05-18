using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.AI;



/// <summary>
/// ����������� ���Ի�ȡAB������Դ·������־�ļ�·���ȵ� 
/// </summary>
[Serializable]
public class FrameworkSettingData : BaseSettingData
{
    public ABLoadSettingData abLoadSetting;
    public LoadContainerSettingData loadContainerSetting;
    public LoadPrefabSetting loadPrefabSetting;
    public override void IntiValue()
    {
        abLoadSetting = new ABLoadSettingData();
        abLoadSetting.ABLoadPath = "gameAssets/";
        abLoadSetting.VoicePackName = "voice";
        abLoadSetting.ObjPrefabPackName = "obj_prefab";
        abLoadSetting.UIPrefabPackName = "ui_prefab";
        abLoadSetting.ABEditorLoadPath = "Assets/Editor/ArtRes/";

        loadContainerSetting = new LoadContainerSettingData();
        loadPrefabSetting = new LoadPrefabSetting()
        {
            ExcelArtPathName = "ResPath",
            ExcelIDName = "PrefabID",
            ExcelPoolGroupName = "PoolGroup",
            DefaultGroupPoolSize = 15

        };

    }

    public override string DirectoryPath => Application.dataPath + @"\ˮ����С���\SettingInfo\Resources\";
    public override string DataName => "FrameworkSetting";

}
/// <summary>
/// AB����������
/// </summary>
[Serializable]
public class ABLoadSettingData
{
    /// <summary>
    /// AB��������
    /// </summary>
    public string ABMainName = null;
    /// <summary>
    ///������Դ��
    /// </summary>
    public string VoicePackName;
    /// <summary>
    /// ��Ϸ����Ԥ�����
    /// </summary>
    public string ObjPrefabPackName;
    /// <summary>
    /// UIԤ�����
    /// </summary>
    public string UIPrefabPackName;
    /// <summary>
    /// �Ƿ���AB�����ԣ��������Editor��ʼ��ȡ
    /// </summary>
    public bool IsDebugABLoad = false;
    /// <summary>
    /// ������Streaming����AB��
    /// </summary>
    public bool IsStreamingABLoad = false;
    /// <summary>
    /// AB���ڱ༭���м���λ��
    /// </summary>
    public string ABEditorLoadPath;
    /// <summary>
    /// AB����ϷĿ¼�ж�ȡ·��
    /// </summary>
    public string ABLoadPath;
}

/// <summary>
/// ��Ϸ�����ļ���������
/// </summary>
[Serializable]
public class LoadContainerSettingData
{

    public string DataPath;
    public string SuffixName;
    public bool IsDebugStreamingAssetLoad = false;

}

[Serializable]
public class LoadPrefabSetting
{
    public string ExcelArtPathName;
    public string ExcelPoolGroupName;
    public string ExcelIDName;
    public int DefaultGroupPoolSize;
}