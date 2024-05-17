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

    //�����洢�����еĶ��� ��¼û������ʹ�õĶ���
    private Queue<Obj> poolQueue = new Queue<Obj>();
    //����
    public int Count => poolQueue.Count;
    public int maxCount;
    /// <summary>
    /// �Ƿ��п��еĶ���
    /// </summary>
    public abstract bool IsHaveFreeObj { get; }


    /// <summary>
    /// ֻ�е�һ�δ�������ʱ��ŵ���
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="volume">�������</param>
    public Pool()
    {
       
    }
    /// <summary>
    /// �ӳ�����ȡ�����󣬲��Ƴ������еĶ���
    /// </summary>
    /// <returns>��Ҫ�Ķ�������</returns>

    public Obj QuitPool()
    {
        Obj obj = null;
        //���ݳ�������ȡ����ͬ�߼�
        //ѭ��������޿����ˣ��ӵ�һ��ȡ��
        if (IsHaveFreeObj)
        {
            obj = poolQueue.Dequeue();
            recordQuitPool(obj);
        }
        obj = NoFreeObjOperate();

        obj.QuitPoolCallback?.Invoke();
        //���������û�ж�������ʹ���е����峬�������������ʱ��
        return obj;
    }
    /// <summary>
    /// ��ʹ����Ķ���������
    /// </summary>
    /// <param name="obj"></param>
    public void EnterPool(Obj obj)
    {
        recordEnterPoolObj(obj);
        poolQueue.Enqueue(obj);
        obj.EnterPoolCallback?.Invoke();
        //�ڿ���ʱ��Ӧ�ÿɿ����ķ�������������ù۲�
#if UNITY_EDITOR
      //  if (obj is GameObj)
           // (obj as GameObj).transform.SetParent(VolumeTransform);
#endif

    }
    /// <summary>
    /// ��¼�Ѿ���һ�����������ӣ���Ϊ����״̬
    /// </summary>
    public abstract void recordEnterPoolObj(Obj obj);
    /// <summary>
    /// ��¼�Ѿ���һ�������Ѿ�������
    /// </summary>
    public abstract void recordQuitPool(Obj obj);
    /// <summary>
    /// û�п�������Ĵ����߼�
    /// </summary>
    public abstract Obj NoFreeObjOperate();
}

/// <summary>
/// ѭ�������
/// </summary>
public class CircuPool : Pool
{
    //������¼ʹ���еĶ���� 
    private List<Obj> usingQueue = new List<Obj>();

    public CircuPool()
    {
    }

    public override bool IsHaveFreeObj => usingQueue.Count < maxCount;

    public override Obj NoFreeObjOperate()
    {
        if (usingQueue.Count <= 0)
        {
            Debug.LogError("����ʹ�ó�����Ϊ0");
            return null;
        }
        //������ʹ�õĵ�һ��������
        Obj obj = usingQueue[0];
        //ˢ��˳�򣬰����ŵ�ĩβ
        usingQueue.Remove(obj);
        usingQueue.Add(obj);
        return obj;
    }

    public override void recordEnterPoolObj(Obj obj)
    {
        usingQueue.Remove(obj);
    }

    public override void recordQuitPool(Obj obj)
    {
        usingQueue.Add(obj);
    }
}