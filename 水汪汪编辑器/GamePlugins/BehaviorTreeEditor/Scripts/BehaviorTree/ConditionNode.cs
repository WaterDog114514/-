using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �����ڵ㣬�����ж�����������ɹ��ͷ���Succee��ͨ������˳��ڵ���ʹ��
/// </summary>
[System.Serializable]
public class ConditionNode : BehaviorTreeNode
{
    /// <summary>
    /// ��������
    /// </summary>
    public Func<bool> ConditionAction;
    public ConditionNode(Func<bool> conditionAction)
    {
        ConditionAction = conditionAction;
    }
    public ConditionNode()
    {

    }
    public override E_NodeState Execute()
    {
        if (ConditionAction == null)
        {
            Debug.LogError("û�����ýڵ�ί��");
            return E_NodeState.Succeed;
        }
        //��Ҫ�����жϵ���Ϊ
        if(ConditionAction.Invoke())
        {
            ChildState =  E_NodeState.Succeed;
            return E_NodeState.Succeed;
        }
        else
        {
            ChildState = E_NodeState.Faild;
            return E_NodeState.Faild;
        }


    }
    public void AddEvent(Func<bool> conditionAction)
    {
        this.ConditionAction += conditionAction;
    }
}

