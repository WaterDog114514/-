using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ѭ�������
/// </summary>
public class CircuPool : Pool
{
    public CircuPool(int MaxCount, Obj Prefab) : base(MaxCount, Prefab)
    {
    }

    public override Obj Operation_QuitObjPoolFull()
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

    public override void Operation_EnterObjPoolFull(Obj obj)
    {
        //ѭ���� ֱ��ɾ�������˿�
        ObjectManager.Instance.ReallyDestroyObj(obj);
    }
}