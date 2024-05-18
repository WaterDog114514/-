using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoaderManager : Singleton_UnMono<PrefabLoaderManager>
{

    public PrefabLoaderManager()
    {
        SettingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();

    }

    private FrameworkSettingData SettingData;


    public Dictionary<string, PrefabInfo> dic_PrefabsFromName = new Dictionary<string, PrefabInfo>();
    public Dictionary<int, PrefabInfo> dic_PrefabsFromID = new Dictionary<int, PrefabInfo>();
    public enum E_LoadType
    {
        UIPrefab, GameObjPrefab
    }
    public PrefabInfo GetPrefabInfoFromName(string GameObjectName)
    {
        if (dic_PrefabsFromName.ContainsKey(GameObjectName))
            return dic_PrefabsFromName[GameObjectName];
        else
        {
            Debug.LogError($"��ȡ{GameObjectName}Ԥ����ʧ�ܣ������ڴ�Ԥ�������");
            return null;
        }
    }
    public PrefabInfo GetPrefabInfoFromID(int id)
    {
        if (dic_PrefabsFromID.ContainsKey(id))
            return dic_PrefabsFromID[id];
        else
        {
            Debug.LogError($"��ȡidΪ{id}��Ԥ����ʧ�ܣ������ڴ�Ԥ�������");
            return null;
        }
    }

    /// <summary>
    /// ����Excel����ر��е�Ԥ����
    /// </summary>
    /// <typeparam name="T">Ԥ�������ڱ�</typeparam>
    public void PreLoadPrefabFrmoExcel<T>() where T : DataBaseContainer
    {
        //�Ȼ�ȡԤ������е�·���Ͷ�������Լ�Ψһid
        string[] paths = GameExcelDataLoader.Instance.GetDataPropertyInfo<T>(SettingData.loadPrefabSetting.ExcelArtPathName);
        string[] groups = GameExcelDataLoader.Instance.GetDataPropertyInfo<T>(SettingData.loadPrefabSetting.ExcelPoolGroupName);
        string[] IDs = GameExcelDataLoader.Instance.GetDataPropertyInfo<T>(SettingData.loadPrefabSetting.ExcelIDName);


        GameObject[] LoadPrefab = new GameObject[paths.Length];
        ResLoader.Instance.CreatePreloadTaskFromPaths(paths, (resCollection) =>
        {
            Debug.Log("�������");
            for (int i = 0; i < resCollection.Length; i++)
            {
                GameObject obj = resCollection[i].GetAsset<GameObject>();
                if (obj == null)
                {
                    Debug.LogError($"����Ԥʱ�����ص���Ԥ������Դ��{resCollection[i].Asset.name}");
                    continue;
                }
                //�ɹ����ؿ�ʼ�����¼
                LoadPrefab[i] = obj;
            }


            //��ֵ����
            for (int i = 0; i < LoadPrefab.Length; i++)
            {
                PrefabInfo info = null;
                //�ж���û���飬��·��
                if (groups == null || groups[i] == null || groups[i] == "")
                {
                    info = new UnLimitedPrefabInfo();
                }
                else
                {
                    info = new PoolPrefabInfo() { PoolGroup = groups[i] , identity = LoadPrefab[i].name };
                }
                info.res = LoadPrefab[i];


                //�Ž����ֵ��ֵ�
                if (!dic_PrefabsFromName.ContainsKey(LoadPrefab[i].name))
                    dic_PrefabsFromName.Add(LoadPrefab[i].name, info);
                else
                    Debug.LogError($"��������Ԥ����{LoadPrefab[i].name}������Ԥ���壬����������");

                //�Ž�id���ֵ�
                if (IDs != null)
                {
                    int id = 0;
                    try
                    {
                        id = int.Parse(IDs[i]);
                    }
                    catch
                    {
                        Debug.LogError("Excel�����зǷ��ַ����޷�ת��IDΪ���֣�����");
                        continue;
                    }
                    //ת���ɹ����жϷŽ���
                    if (!dic_PrefabsFromID.ContainsKey(id))
                    {
                        dic_PrefabsFromID.Add(id, info);
                    }
                    else
                    {
                        Debug.LogError($"�Ѵ���{id}��Ԥ����ID���뱣֤Ԥ����id��Ψһ�ԣ�");
                    }

                }

            }

        });
      


    }
    public void DemoTEst()
    {

    }
}
//Ԥ������Ϣ
//Ԥ������UIԤ�������ͨ��Ϸ����Ԥ����֮��
public class PrefabInfo
{
    public GameObject res;
}
/// <summary>
/// ���ᱻ��������Ƶ�Ԥ����
/// </summary>
public class UnLimitedPrefabInfo : PrefabInfo
{

}
/// <summary>
/// �������������Ԥ����
/// </summary>
public class PoolPrefabInfo : PrefabInfo
{
    public string identity;
    public string PoolGroup;
}