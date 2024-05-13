using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��תװ�νڵ� �ܹ���ת��״̬���
/// </summary>
[System.Serializable]
public class ReverseDecoratorNode : DecoratorNode
{

    public override E_NodeState Execute()
    {
        //��ת���������BB
        switch (childNode.Execute())
        {
            case E_NodeState.Succeed:
                ChildState = E_NodeState.Faild;
                return E_NodeState.Faild;
            case E_NodeState.Faild:
                ChildState = E_NodeState.Succeed;
                return E_NodeState.Succeed;
            default:
                ChildState =  E_NodeState.Running;
                return E_NodeState.Running;
        }

    }
}

