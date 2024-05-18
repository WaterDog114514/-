using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;

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
    public List<PreLoadTask> preloadResTasks = new List<PreLoadTask>();

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
        //�ȸ�����Ԥ��������ֳ�С��֧һ��ͬ��ļ�������
        for (int j = 0; j < preloadResTasks.Count; j++)
        {
            //���ȸ���֮����

            PreLoadTask preLoadTask = preloadResTasks[j];
            //�ص���Դ
            Res[] LoadedRes = new Res[preLoadTask.TaskList.Count];
            //�ٸ����������е��첽�������
            for (int i = 0; i < preLoadTask.TaskList.Count; i++)
            {

                AsyncLoadTask task = preLoadTask.TaskList[i];
                //������ɲ��ؼ���
                if (task.isFinish) continue;
                currentCoroutine = task.StartAsyncLoad();
                yield return currentCoroutine;
                //��¼���غ�Ļص���Դ
                LoadedRes[i] = task.ResInfo;
                //����Դ�������
                LoadedResNum++;
                //�����������߼�С����ȣ�ʹ���¼�����
                Debug.Log($"���ؽ���{LoadedResNum}/{TotalResNum}");
            }
            //������һ��Ԥ���ش�����ͻ����һ��
            preLoadTask.callback?.Invoke(LoadedRes);

        }



        //������ϣ������������
        ClearAllTasks();
        //�ص�һ��Ԥ���غõ���ԴŶ
    }
    /// <summary>
    /// ����Ԥ��������
    /// </summary>
    public void CreatePreLoadTask(PreLoadTask task)
    {
        preloadResTasks.Add(task);
    }
    //��ʱ�Ե�·���洢����ֹ�ظ�����
    private List<string> TempPath = new List<string>();

    public void CreatePreloadTaskFromPaths(string[] paths, UnityAction<Res[]> callback = null)
    {
        PreLoadTask preLoadTask = new PreLoadTask();
        for (int i = 0; i < paths.Length; i++)
        {
            // ���������ʹ��nameFieldValue
            string path = paths[i];
            //���ݷ�ʽ����
            if (IsABOrResLoadFromPath(path))
            {
                string abName = path.Substring(0, path.IndexOf('/'));
                string resName = path.Replace(abName + "/", null);
                AsyncLoadTask task = ResLoader.Instance.CreateAB_Async<UnityEngine.Object>(abName, resName, null);
                preLoadTask.TaskList.Add(task);
            }
            else if (IsABOrResLoadFromPath(path))
            {
                AsyncLoadTask task = ResLoader.Instance.CreateRes_Async<UnityEngine.Object>(path, null);
                preLoadTask.TaskList.Add(task);
            }

        }
        preLoadTask.callback = callback;
        CreatePreLoadTask(preLoadTask);

    }

    /// <summary>
    ///��·���ж���AB������Res����  
    /// </summary>
    /// <param name="path"></param>
    /// <returns>trueΪAB����falseΪRes</returns>
    private bool IsABOrResLoadFromPath(string path)
    {
       string temp = path.Substring(0,3);
        if (temp == "Res")
        {
            return false;
        }
        return true;    
    }
}


public class PreLoadTask
{
    public UnityAction<Res[]> callback;
    public List<AsyncLoadTask> TaskList = new List<AsyncLoadTask>();
}