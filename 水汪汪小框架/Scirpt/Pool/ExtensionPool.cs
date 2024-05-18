using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionPool : Pool
{
    public ExtensionPool(int MaxCount, Obj Prefab) : base(MaxCount, Prefab)
    {
    }

    public override Obj Operation_PoolFull()
    {
        //���ݲ���
        maxCount++;
        Debug.Log("�Ѿ����ݣ���ǰ����:"+maxCount);
        Obj obj = Operation_CreatePoolObj();
        usingQueue.Add(obj);
        return obj;
    }


}
