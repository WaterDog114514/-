using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//Ϊ�˸���ܸ��ӷ���ĵ��ã�����һ�׹���Object��αObj��ʹ��
/// <summary>
/// �����������
/// </summary>
public abstract class Obj
{
    /// <summary>
    /// ��������ر�ʶ�����ݶ���ͨ��Type���֣���Ϸ����ͨ����������
    /// </summary>
    public abstract string PoolIdentity { get;}

    /// <summary>
    /// ������������
    /// </summary>
    public int MaxCount;
    /// <summary>
    /// �����Ψһid
    /// </summary>
    public int ID;

    //����ѭ���ػص�
    public UnityAction EnterPoolCallback;
    public UnityAction QuitPoolCallback;
    //�����ʼ��������ʱ��ص�
    public UnityAction IntiCallback;
    public UnityAction DestroyCallback;

}
