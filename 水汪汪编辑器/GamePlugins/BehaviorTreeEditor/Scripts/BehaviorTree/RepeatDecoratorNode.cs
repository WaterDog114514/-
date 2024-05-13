using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// װ��
/// �ڵ�  �ܹ�����Ҫִ�ж��ٴ� �ӳ�ִ�е�����
/// </summary>
[System.Serializable]
public class RepeatDecoratorNode : DecoratorNode
{
    /// <summary>
    /// ִ���ܴ���
    /// </summary>
    public int TotalExecuteCount = 1;
    /// <summary>
    /// ��ǰִ�д���
    /// </summary>
    private int CurrentExecuteCount = 0;
    public RepeatDecoratorNode()
    {

    }
    public RepeatDecoratorNode(int totalExecuteCount)
    {
        TotalExecuteCount = totalExecuteCount;
    }
    public override E_NodeState Execute()
    {
       ChildState = childNode.Execute();
        if (ChildState == E_NodeState.Succeed || ChildState == E_NodeState.Faild)
        {
            //ÿ���ӽڵ�ִ�гɹ���ʧ�ܲ���һ�� ��ֹ���ѭ���ӽڵ������
            CurrentExecuteCount++;
            //ִ�е�ָ������ ������
            if (CurrentExecuteCount >= TotalExecuteCount)
            {
                CurrentExecuteCount = 0;
                //ȡ���ִ���ǴεĽ����Ϊ�����
                return ChildState;
            }
        }
        //�����ɹ�����ʧ�� �������ִ������ٴβ���
        ChildState = E_NodeState.Running;
        return E_NodeState.Running;

    }
}

