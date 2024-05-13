using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// �����ļ���������ר�ż��������ļ�
/// </summary>
public class SettingDataLoader : Singleton_UnMono<SettingDataLoader>
{
    /// <summary>
    /// �Ѿ����ع��������ļ����еĻ�ֱ�Ӵ�������
    /// </summary>
    private Dictionary<string, BaseSettingData> dic_LoadedData = new Dictionary<string, BaseSettingData>();
    /// <summary>
    /// ֻ���ڱ༭��ģʽ�²��ܱ���
    /// </summary>
    public void SaveData<T>(T data) where T : BaseSettingData
    {
#if UNITY_EDITOR
        //�ٴμ��
        if (!Directory.Exists(data.DirectoryPath))
        {
            Directory.CreateDirectory(data.DirectoryPath);
        }
        JsonManager.Instance.SaveDataToPath(data, data.DirectoryPath + data.DataName + ".json", JsonType.JsonNet);
        //ж����Դ����ֹBUG
        Resources.UnloadAsset(Resources.Load<TextAsset>(data.DataName));
#endif
    }

    public T LoadData<T>() where T : BaseSettingData, new()
    {

        //���ع�ֱ������ֱ������
        if (dic_LoadedData.ContainsKey(typeof(T).Name))
            return dic_LoadedData[typeof(T).Name] as T;

        //��һ�μ���
        T data = new T();
        TextAsset asset = Resources.Load<TextAsset>(data.DataName);
        if (asset != null)
        {
            data = JsonManager.Instance.LoadDataFromText<T>(asset.text, JsonType.JsonNet);
        }
        else
        {
            Debug.LogWarning("�ѵ�һ�δ��������ļ���"+typeof(T).Name);
        }
        return data;

    }


}

public abstract class BaseSettingData
{
    public BaseSettingData()
    {
        IntiValue();
    }

    //����Resources������δ�����ȡ
    [JsonIgnore]
    public abstract string DirectoryPath { get; }// =>
    [JsonIgnore]
    public abstract string DataName { get; } //=> 
    /// <summary>
    /// ��һ�γ�ʼ������
    /// </summary>
    public abstract void IntiValue();
}