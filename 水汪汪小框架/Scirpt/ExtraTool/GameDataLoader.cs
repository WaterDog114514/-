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
        string key = typeof(T).Name;
        if (!dic_LoadedContainer.ContainsKey(key))
        {
            //�ȼ���
           T Container = BinaryManager.Instance.Load<T>($"{FKsettingData.loadContainerSetting.DataPath}/{key}.{ExcelsettingData.SuffixName}");
            dic_LoadedContainer.Add(key, Container);
        }
        return dic_LoadedContainer[key] as T;
    }

}
