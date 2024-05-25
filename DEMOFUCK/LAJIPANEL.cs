using System;
using UnityEngine;

public class MyMonoSingleton : Singleton_AutoMono<MyMonoSingleton>
{
    private void Awake()
    {
        // ֱ�Ӵ������ݶ���ͨ�����ͣ�
        MyDataObj dataObj = ObjectManager.Instance.CreateDataObject<MyDataObj>();

        // ֱ�Ӵ������ݶ���ͨ�� Type��
        Type dataType = typeof(MyDataObj);
        MyDataObj dataObj2 = ObjectManager.Instance.CreateDataObject(dataType) as MyDataObj;
    }

}
class MyDataObj : DataObj
{

}