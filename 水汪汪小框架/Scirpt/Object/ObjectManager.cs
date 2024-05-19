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
    private int tempID;

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
        //ֻ�ж����Լ����Ҫ
        if (info is PoolPrefabInfo)
        {
            gameObj.name = (info as PoolPrefabInfo).identity;
            obj.PoolGroup = (info as PoolPrefabInfo).PoolGroup;
        }
        gameObj.AddComponent<GameObjectInstance>().Inti(obj);
        return obj;
    }

    /// <summary>
    /// ����һ�����ݶ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T CreateDataObject<T>() where T : DataObj, new()
    {
        T obj = new T();
        return obj;
    }
    public DataObj CreateDataObject(Type type)
    {
        if (!type.IsSubclassOf(typeof(DataObj)))
        {
            Debug.LogError($"ʵ��������{type.Name}���̳���DataObj��");
            return null;
        }
        DataObj obj = Activator.CreateInstance(type) as DataObj;
        return obj;
    }

    /// <summary>
    /// ǳ���٣�����������ض���
    /// </summary>
    /// <param name="obj"></param>
    public void DestroyObj(Obj obj)
    {
        PoolManager.Instance.DestroyObj(obj);
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

