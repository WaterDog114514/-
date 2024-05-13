using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ԥ���غ���
/// </summary>
class main_Preloader
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
    public List<PreloadResTask> preloadResTasks = new List<PreloadResTask>();
    /// <summary>
    /// ��ʼ����Ԥ����
    /// </summary>
    public void StartLoad()
    {
        if (preloadResTasks.Count == 0) Debug.LogError("Ԥ��������Ϊ0�������Ԥ�����������ִ��");
        MonoManager.Instance.StartCoroutine(ReallyPreLoadRes());
    }
    /// <summary>
    /// ������м�������
    /// </summary>
    private void ClearAllTasks()
    {
        preloadResTasks.Clear();
        TotalResNum = 0;
        LoadedResNum = 0;
        CurrentTaskName = null;
        waitLoadAssetBundle.Clear();

    }

    /// <summary>
    /// Ԥ������Դ һ���Ǽ��س���ʱ����� ֻ��Ԥ������ϲ��ܼ����³���
    /// </summary>
    public IEnumerator ReallyPreLoadRes()
    {
        //���ڼ������������ȵȼ������
        foreach (var wait in waitLoadAssetBundle)
        {
            yield return wait;
        }

        //��ͳ������Ҫ������Դ������
        foreach (var task in preloadResTasks)
        {
            TotalResNum += task.ResInfos.Length;
        }

        //�ȸ���ÿ����������Լ���Э��
        foreach (var task in preloadResTasks)
        {
            //���õ�ǰ������
            CurrentTaskName = task.taskName;
            //�ȷ���Э��
            DistributeCoroutine(task);
            //���ݵ��������Э�̽��м�¼
            foreach (var coroutine in task.coroutines)
            {
                yield return coroutine;
                //����Դ�������
                LoadedResNum++;
                Debug.Log($"���ؽ���{LoadedResNum}/{TotalResNum}");
                //�����������߼���ʹ���¼�����
                // do do do

            }
        }

        //������ϣ������������
        ClearAllTasks();

    }
    /// <summary>
    /// ����Ԥ��������
    /// </summary>
    public void CreatePreLoadTask(PreloadResTask task, Type type = null)
    {
        task.LoadType = type;
        preloadResTasks.Add(task);
    }
    public List<Coroutine> waitLoadAssetBundle = new List<Coroutine>();
    //��������AB��������ָ��������Դ
    public IEnumerator ReallyCreatePreloadABTask(string taskName, string ABName, Type type, main_ABLoader abLoader)
    {

        string[] AllResName = null;
        //�첽��ȡAB�������е���Դ��
        yield return MonoManager.Instance.StartCoroutine(abLoader.getABAllResName(ABName, (names) => { 
            
            AllResName = names;
        
        }));
        if (AllResName.Length == 0)
        {
            Debug.LogWarning("��AB��Դ������Դ����Ϊ0����");
            yield break;
        }
        PreLoadInfo[] infos = new PreLoadInfo[AllResName.Length];
        for (int i = 0; i < AllResName.Length; i++)
        {
            infos[i] = new PreLoadInfo() { ABName = ABName, ResName = AllResName[i] };
        }
        PreloadResTask task = new PreloadResTask() { taskName = taskName, ResInfos = infos };
        CreatePreLoadTask(task, type);
    }
    /// <summary>
    /// ����һ������İ�������Դ�������������ƥ���Լ��ļ���Э��
    /// </summary>
    private void DistributeCoroutine(PreloadResTask task)
    {
        task.coroutines = new Coroutine[task.ResInfos.Length];
        //������Դ����
        for (int i = 0; i < task.coroutines.Length; i++)
        {
            //����ÿһ�������Э��
           //  task.coroutines[i] = ResLoader.Instance.LoadAB_Async(task.ResInfos[i].ABName, task.ResInfos[i].ResName, null);
        }
    }
}
/// <summary>
/// �����ͽ��з���Ԥ����
/// </summary>
public class PreloadResTask
{
    //��������
    public Type LoadType;
    public Coroutine[] coroutines;
    /// <summary>
    /// ������
    /// </summary>
    public string taskName;
    /// <summary>
    /// �����ص���Դ�ǵ���Ϣ
    /// </summary>
    public PreLoadInfo[] ResInfos;

}
public class PreLoadInfo
{
    public string ABName;
    public string ResName;
}