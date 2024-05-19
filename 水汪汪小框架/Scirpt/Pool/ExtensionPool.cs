using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionPool : Pool
{
    public ExtensionPool(int MaxCount, Obj Prefab) : base(MaxCount, Prefab)
    {
    }

    public override Obj Operation_QuitObjPoolFull()
    {
        maxCount++;
        Debug.Log("�Ѿ����ݣ���ǰ����:" + maxCount);
        Obj obj = Operation_CreatePoolObj();
        usingQueue.Add(obj);
        return obj;
    }

    public override void Operation_EnterObjPoolFull(Obj obj)
    {
        //���ݲ���
        maxCount++;
        poolQueue.Enqueue(obj);
        //�ص��������
        obj.EnterPoolCallback?.Invoke();
    }
}
