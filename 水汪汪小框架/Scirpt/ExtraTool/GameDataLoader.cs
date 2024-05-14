using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Excel��Ϸ���������ļ����ع��ߣ����Excel���ع���ʹ��
/// </summary>
public class GameExcelDataLoader : Singleton_UnMono<GameExcelDataLoader>
{
    private Dictionary<string, DataBaseContainer> dic_LoadedContainer = new Dictionary<string, DataBaseContainer>();
    private FrameworkSettingData FKsettingData = null;
    private ExcelToolSettingData ExcelsettingData = null;
    public GameExcelDataLoader()
    {
        FKsettingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
        ExcelsettingData= SettingDataLoader.Instance.LoadData<ExcelToolSettingData>();
        
    }
    /// <summary>
    /// ȡ��Excel���ñ����������������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetDataContainer<T>() where T : DataBaseContainer
    {
        string key =typeof(T).Name.Replace("Container",null);
        if (!dic_LoadedContainer.ContainsKey(key))
        {
            string path = FKsettingData.loadContainerSetting.IsDebugStreamingAssetLoad? Application.streamingAssetsPath+"/settinginfo": FKsettingData.loadContainerSetting.DataPath;
            //�ȼ���
           T Container = BinaryManager.Instance.Load<T>($"{path}/{key}.{ExcelsettingData.SuffixName}");
            dic_LoadedContainer.Add(key, Container);
        }
        return dic_LoadedContainer[key] as T;
    }

}
