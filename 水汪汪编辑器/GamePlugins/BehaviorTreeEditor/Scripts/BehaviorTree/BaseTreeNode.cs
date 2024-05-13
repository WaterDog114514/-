using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
/// <summary>
/// ��Ϊ���ڵ����
/// </summary>
public abstract class BaseTreeNode
{
    public E_NodeState ChildState = E_NodeState.Faild;
    /// <summary>
    /// ִ�нڵ��߼��ĳ��󷽷� �������ȥʵ�ָ÷���
    /// �˵�ʹ���˵ݹ��˼ά
    /// </summary>
    /// <returns>���״̬</returns>
    public abstract E_NodeState Execute();
    /// <summary>
    /// ��������״̬
    /// </summary>
    public virtual void ResetShowState()
    {
        ChildState = E_NodeState.Faild;
    }
}

