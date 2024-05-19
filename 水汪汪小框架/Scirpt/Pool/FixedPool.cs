using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPool : Pool {
    public FixedPool(int MaxCount, Obj Prefab) : base(MaxCount, Prefab)
    {
    }

    public override void Operation_EnterObjPoolFull(Obj obj)
    {
        ObjectManager.Instance.ReallyDestroyObj(obj);
    }

    public override Obj Operation_QuitObjPoolFull()
    {
        Debug.LogWarning($"���ݳ�{Identity}�Ѿ�û�п��ж����ˣ�����null����");
        return null;
    }

}


