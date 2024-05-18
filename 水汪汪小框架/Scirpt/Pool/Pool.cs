using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������
/// </summary>
public enum PoolType
{
    /// <summary>
    /// ѭ��ʹ�ð汾�ĳ��룬���ʹ���еĶ���ĵ�һ��ȡ����ʹ��
    /// </summary>
    Circulate,
    /// <summary>
    /// ���ݵĳ��룬���ʹ���еĶ���ĵ�һ��ȡ����ʹ��
    /// </summary>
    Expansion,
    /// <summary>
    /// ���ݳأ��̶������������޿��������ˣ���������κβ���
    /// </summary>
    Fixed

}
//����ֻ��Ҫ����û�п��� ��¼ʹ���߼� ȡ����¼ʹ���߼�
public abstract class Pool
{
    //�ó���ͬ���Ԥ������Ϣ��������ʱ�����ݴ�������
    protected string Identity;
    protected Type ObjType;
    //�����洢�����еĶ��� ��¼û������ʹ�õĶ���
    private Queue<Obj> poolQueue = new Queue<Obj>();
    //����
    public int Count => poolQueue.Count;
    public int maxCount;
    /// <summary>
    /// �Ƿ��п��еĶ���
    /// </summary>
    //������¼ʹ���еĶ���� 
    protected List<Obj> usingQueue = new List<Obj>();
    public bool IsHaveFreeObj => usingQueue.Count < maxCount && poolQueue.Count > 0;

    /// <summary>
    /// ֻ�е�һ�δ�������ʱ��ŵ���
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="volume">�������</param>
    public Pool(int MaxCount, Obj Prefab)
    {
        this.maxCount = MaxCount;
        Identity = Prefab.PoolIdentity;
        ObjType = Prefab.GetType();
    }
    /// <summary>
    /// �ӳ�����ȡ�����󣬲��Ƴ������еĶ���
    /// </summary>
    /// <returns>��Ҫ�Ķ�������</returns>

    public Obj Operation_QuitPool()
    {
        Obj obj = null;
        //���ݳ����Ƿ��п��ж��������в���
        if (IsHaveFreeObj)
        {
            //�о�ֱ�ӳ���Ȼ���¼һ��
            obj = poolQueue.Dequeue();
            usingQueue.Add(obj);
        }
        //û�п��ж��󣬾ͽ����޿��еĲ���
        else
        {
            //�����ˣ��׸�����������
            if (usingQueue.Count >= maxCount)
                obj = Operation_PoolFull();
            //����ض���Ϊ0������ʹ��û�б������������Ҫ������
            else
            {
                obj = Operation_CreatePoolObj();
                usingQueue.Add(obj);
            }
        }
        //�����ɹ��󣬽��лص������Ӳ���
        obj.QuitPoolCallback?.Invoke();
        //���������û�ж�������ʹ���е����峬�������������ʱ��
        return obj;
    }
    /// <summary>
    /// ��ʹ����Ķ���������
    /// </summary>
    /// <param name="obj"></param>
    public void Operation_EnterPool(Obj obj)
    {
        //��¼һ��
        usingQueue.Remove(obj);
        poolQueue.Enqueue(obj);
        //�ص��������
        obj.EnterPoolCallback?.Invoke();
    }

    /// <summary>
    /// û�п�������Ĵ����߼�
    /// </summary>
    public abstract Obj Operation_PoolFull();
    //����Ҫ����ʱ���Ҫ�����Ŷ
    public Obj Operation_CreatePoolObj()
    {
        if (ObjType.IsSubclassOf(typeof(DataObj))) return ObjectManager.Instance.CreateDataObject(ObjType);
        else return ObjectManager.Instance.CreateGameObject(
            PrefabLoaderManager.Instance.GetPrefabInfoFromName(Identity)
            );
    }
}
