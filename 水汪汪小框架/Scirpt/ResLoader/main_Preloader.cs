using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Ԥ���غ���
/// </summary>
public class main_Preloader
{
    /// <summary>
    /// ����Ҫ������Դ������
    /// </summary>
    public int TotalResNum;

    /// <summary>
    /// �Ѽ�����Դ����
    /// </summary>
    public int LoadedResNum;
    /// <summary>
    /// ���ڼ��ص�������
    /// </summary>
    public string CurrentTaskName;
    public List<AsyncLoadTask> preloadResTasks = new List<AsyncLoadTask>();

    /// <summary>
    /// ��ʼ����Ԥ����
    /// </summary>
    public void StartLoad()
    {
        //�ȼ�鿴������Щ�����Ѿ��Լ������˵ģ�
        foreach (var task in preloadResTasks)
        {

        }

        if (preloadResTasks.Count == 0) Debug.LogError("Ԥ��������Ϊ0�������Ԥ�����������ִ��");
        MonoManager.Instance.StartCoroutine(ReallyPreLoadRes());
    }
    /// <summary>
    /// ������м�������
    /// </summary>
    private void ClearAllTasks()
    {
        //�ͷ����м��ؼ�¼����Ϣ
        preloadResTasks.Clear();
        TotalResNum = 0;
        LoadedResNum = 0;
        CurrentTaskName = null;
        TempPath.Clear();
    }

    /// <summary>
    /// Ԥ������Դ һ���Ǽ��س���ʱ����� ֻ��Ԥ������ϲ��ܼ����³���
    /// </summary>
    public IEnumerator ReallyPreLoadRes()
    {
        //��ͳ������Ҫ������Դ������
        TotalResNum += preloadResTasks.Count;
        Coroutine currentCoroutine = null;
        //�ȸ���ÿ����������Լ���Э��
        foreach (var task in preloadResTasks)
        {
            //������ɲ��ؼ���
            if (task.isFinish) continue;

            currentCoroutine = task.StartAsyncLoad();
            yield return currentCoroutine;
            //����Դ�������
            LoadedResNum++;
            Debug.Log($"���ؽ���{LoadedResNum}/{TotalResNum}");
            //�����������߼���ʹ���¼�����
        }

        //������ϣ������������
        ClearAllTasks();

    }
    /// <summary>
    /// ����Ԥ��������
    /// </summary>
    public void CreatePreLoadTask(AsyncLoadTask task)
    {
        preloadResTasks.Add(task);
    }

    //��ʱ�Ե�·���洢����ֹ�ظ�����
    private List<string> TempPath = new List<string>();

    //��������AB��������ָ��������Դ
    /// <summary>
    /// ����һ������İ�������Դ�������������ƥ���Լ��ļ���Э��
    /// </summary>

    public void PreloadFromExcel<T>(string ResPathName = "Res" +
        "Path", E_LoadType loadType = E_LoadType.AB) where T : DataBaseContainer
    {
        T container = GameExcelDataLoader.Instance.GetDataContainer<T>();
        // �����ȡdataDic�ֶ�
        var dataDicField = typeof(T).GetField("dataDic");
        if (dataDicField == null)
        {
            Debug.LogError($"��ȡ�����벻Ҫ��{typeof(T).Name}�и�dataDic������");
            return;
        }
        // ��ȡdataDic��ֵ
        var dataDicValue = dataDicField.GetValue(container) as IDictionary;

        // ��ȡ�ֵ��ֵ���ͣ�����2��
        var valueType = dataDicField.FieldType.GetGenericArguments()[1];

        // ͨ������2�õ���Ϊname���ֶ�
        var nameField = valueType.GetField(ResPathName);
        if (nameField == null)
        {
            Debug.LogError($"��ȡ�������ݶ�������{valueType.Name}������{ResPathName}���ֶ�");
            return;
        }

        // �����ֵ䣬��ȡ��Ϊname���ֶε�ֵ
        foreach (DictionaryEntry pair in dataDicValue)
        {
            object valueObject = pair.Value;
            object nameFieldValue = nameField.GetValue(valueObject);
            // ���������ʹ��nameFieldValue
            string path = nameFieldValue.ToString();
            //��ֹ�ظ�����ͬ·������
            if (TempPath.Contains(path))
                continue;
            else
                TempPath.Add(path);

            //���ݷ�ʽ����
            if (loadType == E_LoadType.AB)
            {
                Debug.Log(path);
                string abName = path.Substring(0, path.IndexOf('/'));
                string resName = path.Replace(abName + "/", null);
                AsyncLoadTask task = ResLoader.Instance.CreateAB_Async<UnityEngine.Object>(abName, resName, null);
                CreatePreLoadTask(task);
            }
            else if (loadType == E_LoadType.Res)
            {
                AsyncLoadTask task = ResLoader.Instance.CreateRes_Async<UnityEngine.Object>(path, null);
                CreatePreLoadTask(task);
            }
        }
    }
    /// <summary>
    /// ���ط�ʽ ��AB�����أ�����Res����
    /// </summary>
    public enum E_LoadType
    {
        AB,
        Res
    }
}
