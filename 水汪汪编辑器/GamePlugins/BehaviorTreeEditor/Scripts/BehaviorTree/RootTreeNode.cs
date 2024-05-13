using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��ʼ���ڵ㣺������ʼ
/// </summary>
[System.Serializable]
public class RootTreeNode : BaseTreeNode
{
    public BaseTreeNode childNode;
    public override E_NodeState Execute()
    {
        if (childNode == null) Debug.LogError("������Ϊ��AI�����ļ��Ƿ�󶨣��ڵ������Ƿ����.asset��");
        ChildState = childNode.Execute();
        return ChildState;
    }
    public override void ResetShowState()
    {
        //������û��ִ�л���ִ��ʧ�� �����оͲ��������
        if (ChildState != E_NodeState.Succeed) return;
        base.ResetShowState();
        childNode.ResetShowState();
    }
}

