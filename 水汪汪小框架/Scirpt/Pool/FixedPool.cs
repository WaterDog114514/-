using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPool : Pool {
    public FixedPool(int MaxCount, Obj Prefab) : base(MaxCount, Prefab)
    {
    }


    public override Obj Operation_PoolFull()
    {
        Debug.LogWarning($"���ݳ�{Identity}�Ѿ�û�п��ж����ˣ�����null����");
        return null;
    }

}


