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

    /// <summary>
    /// ��ñ���ĳһ������������ֵ��Ϣ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name=""></param>
    /// <returns></returns>
    public string[] GetDataPropertyInfo<T>(string PropertyName) where T : DataBaseContainer
    {
        T container =  GetDataContainer<T>();
        // �����ȡdataDic�ֶ�
        var dataDicField = typeof(T).GetField("dataDic");
        if (dataDicField == null)
        {
            Debug.LogError($"��ȡ�����벻Ҫ��{typeof(T).Name}�и�dataDic������");
            return null;
        }
        // ��ȡdataDic��ֵ
        var dataDicValue = dataDicField.GetValue(container) as IDictionary;

        // ��ȡ�ֵ��ֵ���ͣ�����2��
        var valueType = dataDicField.FieldType.GetGenericArguments()[1];

        // ͨ������2�õ���Ϊname���ֶ�
        var nameField = valueType.GetField(PropertyName);
        if (nameField == null)
        {
            Debug.LogWarning($"Excel�������ʾ��{valueType.Name}������{PropertyName}���ֶΣ��ѷ���null");
            return null;
        }
        int index =0;
        string[] propertyNames =  new string[dataDicValue.Count];
        // �����ֵ䣬��ȡ��Ϊname���ֶε�ֵ
        foreach (DictionaryEntry pair in dataDicValue)
        {
            object valueObject = pair.Value;
            propertyNames[index] = nameField.GetValue(valueObject).ToString();
            index++;
        }
        return propertyNames;
    }
}
