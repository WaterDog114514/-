using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������Ϸ�������������¼������ʵ���������Ϸ���󣬽�������GameObject�ͷ�Mono�����ݶ���
/// </summary>
public class ObjectMgr : Singleton_UnMono<ObjectMgr>
{
    private Dictionary<int,Obj> dicObj = new Dictionary<int, Obj>();


    private int tempID;
    private void AddObjToDic(Obj obj)
    {
        tempID++;
        obj.ID= tempID;
        dicObj.Add(obj.ID, obj);
    }
    public GameObj CreateGameObject(GameObject prefab)
    {
        GameObject gameObj = Object.Instantiate(prefab);
        GameObj obj = new GameObj(gameObj);
        return obj;
    }
    public GameObj CreateGameObject(GameObject prefab, Vector3 pos)
    {
        GameObj obj = CreateGameObject(prefab);
        obj.transform.position = pos;
        return obj;
    }

    public T CreateDataObject<T>() where T : DataObj, new()
    {
        T obj = new T();

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
        if(obj is GameObj)
        {
            obj?.DestroyCallback();
        }
        dicObj.Remove(obj.ID);
    }
}

