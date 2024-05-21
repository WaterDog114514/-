using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������Ϸ�������������¼������ʵ���������Ϸ���󣬽�������GameObject�ͷ�Mono�����ݶ���
/// </summary>
public class ObjectManager : Singleton_UnMono<ObjectManager>
{
    // private Dictionary<int, Obj> dicObj = new Dictionary<int, Obj>();
    private PoolManager poolManager;
    public ObjectManager()
    {
        poolManager = new PoolManager();
    }


    //����Ԥ�������
    public GameObj CreateGameObject(string prefabName)
    {
        PrefabInfo prefabInfo = PrefabLoaderManager.Instance.GetPrefabInfoFromName(prefabName);
        return CreateGameObject(prefabInfo);
    }
    public GameObj CreateGameObject(int id)
    {
        PrefabInfo prefabInfo = PrefabLoaderManager.Instance.GetPrefabInfoFromID(id);
        return (CreateGameObject(id));
    }
    public GameObj CreateGameObject(PrefabInfo info)
    {
        //ԭʼ����
        GameObject gameObj = UnityEngine.Object.Instantiate(info.res);
        GameObj obj = new GameObj(gameObj);
        //����id
        GiveObjID(obj);
        //ֻ�ж����Լ����Ҫ
        if (info is PoolPrefabInfo)
        {
            gameObj.name = (info as PoolPrefabInfo).identity;
            obj.PoolGroup = (info as PoolPrefabInfo).PoolGroup;
        }
        gameObj.AddComponent<GameObjectInstance>().Inti(obj);
        return obj;
    }

    //�Ӷ�����л������
    public GameObj GetGameObjFromPool(PrefabInfo info)
    {
        return poolManager.GetGameObj(info);
    }
    public GameObj GetGameObjFromPool(int id)
    {
        return poolManager.GetGameObj(PrefabLoaderManager.Instance.GetPrefabInfoFromID(id));
    }
    public GameObj GetGameObjFromPool(string PrefabName)
    {
        return poolManager.GetGameObj(PrefabLoaderManager.Instance.GetPrefabInfoFromName(PrefabName));
    }

    //��ȡ���ݶ���Ӷ����֮��
    public T getDataObjFromPool<T>() where T : DataObj
    { 
        return poolManager.GetDataObj(typeof(T)) as T;
    }

    public DataObj getDataObjFromPool(Type type)  
    {
        return poolManager.GetDataObj(type);
    }

    /// <summary>
    /// ����һ�����ݶ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T CreateDataObject<T>() where T : DataObj, new()
    {
        return CreateDataObject(typeof(T)) as T; 
    }
    public DataObj CreateDataObject(Type type)
    {
        if (!type.IsSubclassOf(typeof(DataObj)))
        {
            Debug.LogError($"ʵ��������{type.Name}���̳���DataObj��");
            return null;
        }
        DataObj obj = Activator.CreateInstance(type) as DataObj;
        //����ID
        GiveObjID(obj);
        return obj;
    }

    private int CurrentNewObjID=0;
    /// <summary>
    /// �����¶���ΨһID��һ�㴴��ʱ��ʹ��
    /// </summary>
    private void GiveObjID(Obj obj)
    {
       obj.ID = CurrentNewObjID++;
    }

    /// <summary>
    /// ǳ���٣�����������ض���
    /// </summary>
    /// <param name="obj"></param>
    public void DestroyObj(Obj obj)
    {
        poolManager.DestroyObj(obj);
    }
    /// <summary>
    /// ������٣�������ȫ���ڴ����Ƴ�
    /// </summary>
    public void ReallyDestroyObj(Obj obj)
    {
        if (obj is GameObj)
        {
            obj?.DestroyCallback();
        }
    }
}

