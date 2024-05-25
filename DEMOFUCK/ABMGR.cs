using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//֪ʶ��
//�ֵ�
//Э��
//AB�����API
//ί��
//lambda���ʽ
//����ģʽ���ࡪ��>�ۿ�UnityС�����Ƶ ����ѧϰ
public class ABMgr : Singleton_AutoMono<ABMgr>
{
    //����
    private AssetBundle mainAB = null;
    //����������ȡ�����ļ�
    private AssetBundleManifest manifest = null;

    //ѡ��洢 AB��������
    //AB�����ܹ��ظ����� ����ᱨ��
    //�ֵ�֪ʶ �����洢 AB������
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// ��ȡAB������·��
    /// </summary>
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// ������ ����ƽ̨��ͬ ������ͬ
    /// </summary>
    private string MainName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
#endif
        }
    }

    /// <summary>
    /// �������� �� �����ļ�
    /// ��Ϊ�������а��� �����ж� ͨ�������ܵõ�������Ϣ
    /// ����дһ������
    /// </summary>
    private void LoadMainAB()
    {
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }

    /// <summary>
    /// ����ָ������������
    /// </summary>
    /// <param name="abName"></param>
    private void LoadDependencies(string abName)
    {
        //��������
        LoadMainAB();
        //��ȡ������
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
    }

    ///// <summary>
    ///// ������Դͬ������
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="abName"></param>
    ///// <param name="resName"></param>
    ///// <returns></returns>
    //public T LoadRes<T>(string abName, string resName) where T:Object
    //{
    //    //����������
    //    LoadDependencies(abName);
    //    //����Ŀ���
    //    if ( !abDic.ContainsKey(abName) )
    //    {
    //        AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
    //        abDic.Add(abName, ab);
    //    }

    //    //�õ����س�������Դ
    //    T obj = abDic[abName].LoadAsset<T>(resName);
    //    //�����GameObject ��ΪGameObject 100%������Ҫʵ������
    //    //��������ֱ��ʵ����
    //    if (obj is GameObject)
    //        return Instantiate(obj);
    //    else
    //        return obj;
    //}

    ///// <summary>
    ///// Typeͬ������ָ����Դ
    ///// </summary>
    ///// <param name="abName"></param>
    ///// <param name="resName"></param>
    ///// <param name="type"></param>
    ///// <returns></returns>
    //public Object LoadRes(string abName, string resName, System.Type type) 
    //{
    //    //����������
    //    LoadDependencies(abName);
    //    //����Ŀ���
    //    if (!abDic.ContainsKey(abName))
    //    {
    //        AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
    //        abDic.Add(abName, ab);
    //    }

    //    //�õ����س�������Դ
    //    Object obj = abDic[abName].LoadAsset(resName, type);
    //    //�����GameObject ��ΪGameObject 100%������Ҫʵ������
    //    //��������ֱ��ʵ����
    //    if (obj is GameObject)
    //        return Instantiate(obj);
    //    else
    //        return obj;
    //}

    ///// <summary>
    ///// ���� ͬ������ָ����Դ
    ///// </summary>
    ///// <param name="abName"></param>
    ///// <param name="resName"></param>
    ///// <returns></returns>
    //public Object LoadRes(string abName, string resName)
    //{
    //    //����������
    //    LoadDependencies(abName);
    //    //����Ŀ���
    //    if (!abDic.ContainsKey(abName))
    //    {
    //        AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
    //        abDic.Add(abName, ab);
    //    }

    //    //�õ����س�������Դ
    //    Object obj = abDic[abName].LoadAsset(resName);
    //    //�����GameObject ��ΪGameObject 100%������Ҫʵ������
    //    //��������ֱ��ʵ����
    //    if (obj is GameObject)
    //        return Instantiate(obj);
    //    else
    //        return obj;
    //}

    /// <summary>
    /// �����첽������Դ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack, bool isSync = false) where T : Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack, isSync));
    }
    //�����˾��� Э�̺���
    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack, bool isSync) where T : Object
    {
        //��������
        LoadMainAB();
        //��ȡ������
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //��û�м��ع���AB��
            if (!abDic.ContainsKey(strs[i]))
            {
                //ͬ������
                if (isSync)
                {
                    AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                    abDic.Add(strs[i], ab);
                }
                //�첽����
                else
                {
                    //һ��ʼ�첽���� �ͼ�¼ �����ʱ�ļ�¼�е�ֵ ��null ��֤�����ab�����ڱ��첽����
                    abDic.Add(strs[i], null);
                    AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                    yield return req;
                    //�첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
                    abDic[strs[i]] = req.assetBundle;
                }
            }
            //��֤�� �ֵ����Ѿ���¼��һ��AB�������Ϣ��
            else
            {
                //����ֵ��м�¼����Ϣ��null �Ǿ�֤�����ڼ�����
                //����ֻ��Ҫ�ȴ������ؽ��� �Ϳ��Լ���ִ�к���Ĵ�����
                while (abDic[strs[i]] == null)
                {
                    //ֻҪ�������ڼ����� �Ͳ�ͣ�ĵȴ�һ֡ ��һ֡�ٽ����ж�
                    yield return 0;
                }
            }
        }
        //����Ŀ���
        if (!abDic.ContainsKey(abName))
        {
            //ͬ������
            if (isSync)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
                abDic.Add(abName, ab);
            }
            else
            {
                //һ��ʼ�첽���� �ͼ�¼ �����ʱ�ļ�¼�е�ֵ ��null ��֤�����ab�����ڱ��첽����
                abDic.Add(abName, null);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + abName);
                yield return req;
                //�첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
                abDic[abName] = req.assetBundle;
            }
        }
        else
        {
            //����ֵ��м�¼����Ϣ��null �Ǿ�֤�����ڼ�����
            //����ֻ��Ҫ�ȴ������ؽ��� �Ϳ��Լ���ִ�к���Ĵ�����
            while (abDic[abName] == null)
            {
                //ֻҪ�������ڼ����� �Ͳ�ͣ�ĵȴ�һ֡ ��һ֡�ٽ����ж�
                yield return 0;
            }
        }

        //ͬ������AB���е���Դ
        if (isSync)
        {
            //��ʹ��ͬ������ Ҳ��Ҫʹ�ûص����������ⲿ����ʹ��
            T res = abDic[abName].LoadAsset<T>(resName);
            callBack(res);
        }
        //�첽���ذ�����Դ
        else
        {
            AssetBundleRequest abq = abDic[abName].LoadAssetAsync<T>(resName);
            yield return abq;

            callBack(abq.asset as T);
        }
    }

    /// <summary>
    /// Type�첽������Դ
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="type"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack, bool isSync = false)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type, callBack, isSync));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack, bool isSync)
    {
        //��������
        LoadMainAB();
        //��ȡ������
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //��û�м��ع���AB��
            if (!abDic.ContainsKey(strs[i]))
            {
                //ͬ������
                if (isSync)
                {
                    AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                    abDic.Add(strs[i], ab);
                }
                //�첽����
                else
                {
                    //һ��ʼ�첽���� �ͼ�¼ �����ʱ�ļ�¼�е�ֵ ��null ��֤�����ab�����ڱ��첽����
                    abDic.Add(strs[i], null);
                    AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                    yield return req;
                    //�첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
                    abDic[strs[i]] = req.assetBundle;
                }
            }
            //��֤�� �ֵ����Ѿ���¼��һ��AB�������Ϣ��
            else
            {
                //����ֵ��м�¼����Ϣ��null �Ǿ�֤�����ڼ�����
                //����ֻ��Ҫ�ȴ������ؽ��� �Ϳ��Լ���ִ�к���Ĵ�����
                while (abDic[strs[i]] == null)
                {
                    //ֻҪ�������ڼ����� �Ͳ�ͣ�ĵȴ�һ֡ ��һ֡�ٽ����ж�
                    yield return 0;
                }
            }
        }
        //����Ŀ���
        if (!abDic.ContainsKey(abName))
        {
            //ͬ������
            if (isSync)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
                abDic.Add(abName, ab);
            }
            else
            {
                //һ��ʼ�첽���� �ͼ�¼ �����ʱ�ļ�¼�е�ֵ ��null ��֤�����ab�����ڱ��첽����
                abDic.Add(abName, null);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + abName);
                yield return req;
                //�첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
                abDic[abName] = req.assetBundle;
            }
        }
        else
        {
            //����ֵ��м�¼����Ϣ��null �Ǿ�֤�����ڼ�����
            //����ֻ��Ҫ�ȴ������ؽ��� �Ϳ��Լ���ִ�к���Ĵ�����
            while (abDic[abName] == null)
            {
                //ֻҪ�������ڼ����� �Ͳ�ͣ�ĵȴ�һ֡ ��һ֡�ٽ����ж�
                yield return 0;
            }
        }

        if (isSync)
        {
            Object res = abDic[abName].LoadAsset(resName, type);
            callBack(res);
        }
        else
        {
            //�첽���ذ�����Դ
            AssetBundleRequest abq = abDic[abName].LoadAssetAsync(resName, type);
            yield return abq;

            callBack(abq.asset);
        }

    }

    /// <summary>
    /// ���� �첽���� ָ����Դ
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callBack"></param>
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack, bool isSync = false)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack, isSync));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack, bool isSync)
    {
        //��������
        LoadMainAB();
        //��ȡ������
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //��û�м��ع���AB��
            if (!abDic.ContainsKey(strs[i]))
            {
                //ͬ������
                if (isSync)
                {
                    AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                    abDic.Add(strs[i], ab);
                }
                //�첽����
                else
                {
                    //һ��ʼ�첽���� �ͼ�¼ �����ʱ�ļ�¼�е�ֵ ��null ��֤�����ab�����ڱ��첽����
                    abDic.Add(strs[i], null);
                    AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                    yield return req;
                    //�첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
                    abDic[strs[i]] = req.assetBundle;
                }
            }
            //��֤�� �ֵ����Ѿ���¼��һ��AB�������Ϣ��
            else
            {
                //����ֵ��м�¼����Ϣ��null �Ǿ�֤�����ڼ�����
                //����ֻ��Ҫ�ȴ������ؽ��� �Ϳ��Լ���ִ�к���Ĵ�����
                while (abDic[strs[i]] == null)
                {
                    //ֻҪ�������ڼ����� �Ͳ�ͣ�ĵȴ�һ֡ ��һ֡�ٽ����ж�
                    yield return 0;
                }
            }
        }
        //����Ŀ���
        if (!abDic.ContainsKey(abName))
        {
            //ͬ������
            if (isSync)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
                abDic.Add(abName, ab);
            }
            else
            {
                //һ��ʼ�첽���� �ͼ�¼ �����ʱ�ļ�¼�е�ֵ ��null ��֤�����ab�����ڱ��첽����
                abDic.Add(abName, null);
                AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(PathUrl + abName);
                yield return req;
                //�첽���ؽ����� ���滻֮ǰ��null  ��ʱ ��Ϊnull ��֤�����ؽ�����
                abDic[abName] = req.assetBundle;
            }
        }
        else
        {
            //����ֵ��м�¼����Ϣ��null �Ǿ�֤�����ڼ�����
            //����ֻ��Ҫ�ȴ������ؽ��� �Ϳ��Լ���ִ�к���Ĵ�����
            while (abDic[abName] == null)
            {
                //ֻҪ�������ڼ����� �Ͳ�ͣ�ĵȴ�һ֡ ��һ֡�ٽ����ж�
                yield return 0;
            }
        }

        if (isSync)
        {
            Object obj = abDic[abName].LoadAsset(resName);
            callBack(obj);
        }
        else
        {

            //�첽���ذ�����Դ
            AssetBundleRequest abq = abDic[abName].LoadAssetAsync(resName);
            yield return abq;

            callBack(abq.asset);
        }

    }

    //ж��AB���ķ���
    public void UnLoadAB(string name, UnityAction<bool> callBackResult)
    {
        if (abDic.ContainsKey(name))
        {
            if (abDic[name] == null)
            {
                //���������첽���� û��ж�سɹ�
                callBackResult(false);
                return;
            }
            abDic[name].Unload(false);
            abDic.Remove(name);
            //ж�سɹ�
            callBackResult(true);
        }
    }

    //���AB���ķ���
    public void ClearAB()
    {
        //����AB�������첽������ ���������֮ǰ ֹͣЭͬ����
        StopAllCoroutines();
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        //ж������
        mainAB = null;
    }
}
