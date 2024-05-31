using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ��Դ���صĺ����࣬��װ�˸�����ģ��ļ���
/// </summary>
public class ResLoader : Singleton_UnMono<ResLoader>
{
    //������ģ��
    private main_ResoucesLoader resourcesLoader;
    private main_ABLoader abLoader;
    private main_EditorABLoader editorLoader;
    private main_Preloader preloader;
    //�������������ļ�
    private FrameworkSettingData settingData;
    /// <summary>
    /// ���е��첽�������񣬰����Ѿ�ִ�к�δִ�е�
    /// </summary>
    private Dictionary<string, AsyncLoadTask> dic_LoadedTask = new Dictionary<string, AsyncLoadTask>();
    /// <summary>
    /// �Ѿ����ع�����Դ
    /// </summary>
    private Dictionary<string, Res> dic_LoadedRes = new Dictionary<string, Res>();
    public ResLoader()
    {
        settingData = SettingDataLoader.Instance.LoadData<FrameworkSettingData>();
        //��ʼ�����бر�������ģ��
        resourcesLoader = new main_ResoucesLoader();
        abLoader = new main_ABLoader();
        preloader = new main_Preloader();
#if UNITY_EDITOR
        editorLoader = new main_EditorABLoader();
#endif
    }

    #region AB���������
    //��ȡ������ϵ�AB����Դ
    public T GetRes_FromAB<T>(string abName, string resName) where T : UnityEngine.Object
    {
        string key = $"AB_{abName}_{resName}";
        if (!dic_LoadedRes.ContainsKey(key))
        {
            Debug.LogError($"��ȡ{abName}_{resName}��Դʧ�ܣ���Ϊ����ʼ���ش���Դ��");
            return null;
        }
        return dic_LoadedRes[key].GetAsset<T>();
    }
    // ͨ�����ͼ���AB��
    public void LoadAB_Async<T>(string abName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
    {
        AsyncLoadTask task = CreateAB_Async<T>(abName, resName, callback);
        if (task != null)
            task.StartAsyncLoad();
    }
    // ͨ�������첽����AB����Դ
    public void LoadAB_Async(string abName, string resName, UnityAction<UnityEngine.Object> callback)
    {
        AsyncLoadTask task = CreateAB_Async<UnityEngine.Object>(abName, resName, callback);
        if (task != null)
            task.StartAsyncLoad();
    }
    //����AB�����񣬲����������أ���Ҫ�ֶ����ز���
    public AsyncLoadTask CreateAB_Async<T>(string abName, string resName, UnityAction<T> callback) where T : UnityEngine.Object
    {

        string key = $"AB_{abName}_{resName}";
        //�������Ƿ��һ�μ����������
        if (!IsFirstAsyncLoad(key, callback))
            return null;

#if UNITY_EDITOR
        if (settingData.abLoadSetting.IsDebugABLoad)
        {
            EditorLoadABRes(abName, resName, callback);
            return null;
        }
#endif

        //��һ�ξ��½�����
        AsyncLoadTask task = new AsyncLoadTask();
        task.IntiLoadOperation(abLoader.ReallyLoadAsync<T>(abName, resName, task));
        task.AddCallbackCommand(new LoadResAsyncCommand<T>(callback, task));
        //������Դռλ
        dic_LoadedRes.Add(key, null);
        //��¼���ֵ���
        dic_LoadedTask.Add(key, task);
        //�첽������ɼ�¼
        task.AddCallbackCommand(() =>
        {
            dic_LoadedRes[key] = task.ResInfo;
            dic_LoadedTask.Remove(key);
        });

        return task;
    }

    /// <summary>
    /// (��ʼ�������������)ͨ�����ͽ���ͬ������AB��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public T LoadAB_Sync<T>(string abName, string resName) where T : UnityEngine.Object
    {
        string key = $"AB_{abName}_{resName}";
        if (dic_LoadedRes.ContainsKey(key))
        {
            if (dic_LoadedRes[key] != null)
                return dic_LoadedRes[key].GetAsset<T>();
            else Debug.LogError("��ȡ��Դʧ�ܣ����������ڽ����첽�����У�����ͨ���첽��ȡ");
        }
#if UNITY_EDITOR
        //�༭�����Լ���
        if (settingData.abLoadSetting.IsDebugABLoad)
        {
            return EditorLoadABRes<T>(abName, resName);
        }
#endif
        //���ز���
        T res = abLoader.ReallyLoadSync<T>(abName, resName);
        Res ResInfo = new Res(typeof(T));
        ResInfo.Asset = res;
        dic_LoadedRes.Add(key, ResInfo);
        return res;

    }
    /// <summary>
    /// ͨ�����ֽ���ͬ������AB��
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object LoadAB_Sync(string abName, string resName)
    {
        return LoadAB_Sync<UnityEngine.Object>(abName, resName);
    }
    #endregion

    #region Resources����ģ��

    public void LoadRes_Async<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
    {
        AsyncLoadTask asyncLoadTask = CreateRes_Async<T>(path, callback);
        asyncLoadTask?.StartAsyncLoad();
    }
    //����Res�첽����
    public AsyncLoadTask CreateRes_Async<T>(string path, UnityAction<T> callback) where T : UnityEngine.Object
    {
        //�������Ƿ��һ�μ����������
        if (!IsFirstAsyncLoad(path, callback))
        {
            return null;
        }
        //��һ�ξ��½�����
        AsyncLoadTask task = new AsyncLoadTask();
        task.IntiLoadOperation(resourcesLoader.reallyLoadAsync<T>(path, task));
        task.AddCallbackCommand(new LoadResAsyncCommand<T>(callback, task));
        //������Դռλ
        dic_LoadedRes.Add(path, null);
        //��¼���ֵ���
        dic_LoadedTask.Add(path, task);
        //�첽������ɼ�¼
        task.AddCallbackCommand(() =>
        {
            dic_LoadedRes[path] = task.ResInfo;
            dic_LoadedTask.Remove(path);
        });
        return task;
    }
    /// <summary>
    /// ͬ�����ټ��������ļ��ȣ���Ҫ�ǳ�Ѹ�٣����Բ���Task�ˣ�ֱ�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T LoadRes_Sync<T>(string path) where T : UnityEngine.Object
    {
        if (dic_LoadedRes.ContainsKey(path))
        {
            if (dic_LoadedRes[path] != null)
                return dic_LoadedRes[path].GetAsset<T>();
            else Debug.LogError($"��ȡ��Դ{path}ʧ�ܣ����������ڽ����첽�����У���ͨ���첽��ȡ");
        }
        //����Դ��ȥ
        T res = resourcesLoader.LoadSync<T>(path);
        Res resInfo = new Res(typeof(T));
        resInfo.Asset = res;
        dic_LoadedRes.Add(path, resInfo);
        return res;
    }

    public T GetRes_FromRes<T>(string path) where T : UnityEngine.Object
    {

        if (!dic_LoadedRes.ContainsKey(path))
        {
            Debug.LogError($"��ȡ{path}��Դʧ�ܣ���Ϊ����ʼ���ش���Դ��");
            return null;
        }
        return dic_LoadedRes[path].GetAsset<T>();
    }
    #endregion

    #region Ԥ����ģ�����
    /// <summary>
    /// ��ʼ����Ԥ����  (��Ҫ�ȴ���Ԥ�������񣡣�)
    /// </summary>
    public void StartPreload()
    {
        preloader.StartLoad();
    }

    //Ԥ���������Ϊ �첽�������񣬿��԰���ͨ����������ӵ�Ԥ����֮�У���ʱ������Ԥ���ؾͻ�һ�������
    /// <summary>
    /// ����Ԥ��������
    /// </summary>
    public void CreatePreloadTask(params AsyncLoadTask[] tasks)
    {

        PreLoadTask preLoadTask = new PreLoadTask();
        preLoadTask.TaskList.AddRange(tasks);
        preloader.CreatePreLoadTask(preLoadTask);
    }
    public void CreatePreloadTaskFromExcel<T>(string ResPathName = "ResPath", UnityAction<Res[]> callback = null) where T : DataBaseContainer
    {
        //Ԥ����������
        string[] paths = GameExcelDataLoader.Instance.GetDataPropertyInfo<T>(ResPathName);
        CreatePreloadTaskFromPaths(paths);
    }
    #endregion
    //ÿ��Excel����Դ���һ��Ԥ����������
    public void CreatePreloadTaskFromPaths(string[] paths, UnityAction<Res[]> callback = null)
    {
        preloader.CreatePreloadTaskFromPaths(paths, callback);
    }

    /// <summary>
    /// �ж��ǲ��ǵ�һ��ִ���첽����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private bool IsFirstAsyncLoad<T>(string key, UnityAction<T> callback = null) where T : UnityEngine.Object
    {
        //���ж��Ƿ��ǵ�һ�μ���ĳ����
        if (!dic_LoadedRes.ContainsKey(key)) return true;
        //�Ѿ����£�ֱ����
        if (!dic_LoadedTask.ContainsKey(key) && dic_LoadedRes[key] != null)
        {
            callback?.Invoke(dic_LoadedRes[key].Asset as T);
            return false;
        }
        //û�����£����ڼ������������Ҫ�����������ʹ��
        else
        {
            //�������������ж�
            AsyncLoadTask task = dic_LoadedTask[key];
            task.AddCallbackCommand(new LoadResAsyncCommand<T>(callback, task));
            return false;
        }

        //�༭���߼�
#if UNITY_EDITOR
        //if (settingData.abLoadSetting.IsDebugABLoad && !task.isFinish )
        //{
        //    if (!task.isFinish) (task as SyncLoadTask).AddOperation(() =>
        //    {
        //        callback(task.getRes<T>());
        //    });
        //    return task;
        //}
#endif

    }
    //ʹ�ñ༭������ʱ���¼��Դ
    private T EditorLoadABRes<T>(string abName, string resName, UnityAction<T> callback = null) where T : UnityEngine.Object
    {
        AsyncLoadTask task = new AsyncLoadTask();
        T res = editorLoader.LoadEditorRes<T>(abName + "/" + resName);

        //ֱ�������������
        Res resInfo = new Res(typeof(T));
        resInfo.Asset = res;
        callback?.Invoke(res);
        task.FinishTask(resInfo);
        dic_LoadedRes.Add($"AB_{abName}_{resName}", resInfo);
        dic_LoadedTask.Add($"AB_{abName}_{resName}", task);
        return res;
    }

    public void Demo()
    {
    }

}