using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����ôӷ�ִ֡�еĽǶ�ȥ�˽����

/// <summary>
/// ���нڵ� 
/// �ص㣺
/// 1.������ִ���Լ����ӽڵ�
/// 2.���ĳһ���ӽڵ�ִ��ʧ���� �ͻ�ͣ���� Ȼ�󷵻�ʧ��
/// 3.���û��һ���ڵ�ʧ�ܣ���ô��ִ���������ӽڵ���߼� ���ҷ��سɹ�
/// </summary>
[System.Serializable]
public class SequeneTreeNode : ControlTreeNode
{
    /// <summary>
    /// ��˳��ִ���ӽڵ�
    /// </summary>
    /// <returns></returns>
    public override E_NodeState Execute()
    {
        if (childNodes.Count == 0)
        {
            Debug.LogError("û�������κε��ӽڵ�");
            ChildState = E_NodeState.Succeed;
            return E_NodeState.Succeed;
        }
        switch (childNodes[nowIndex].Execute())
        {
            case E_NodeState.Succeed:
                //�����ǰ�ڵ�ִ�гɹ��ˣ���ô����ִ����һ���ڵ���߼�
                nowIndex++;
                if (nowIndex >= childNodes.Count)
                {
                    //ִ�����ˣ��´δ�0��ʼ
                    nowIndex = 0;
                    ChildState = E_NodeState.Succeed;
                    return E_NodeState.Succeed;
                }
                break;
            case E_NodeState.Faild:
                //���ʧ���ˣ���һ��ҲӦ�ô�ͷ��ʼִ��
                nowIndex = 0;
                ChildState = E_NodeState.Faild;
                return E_NodeState.Faild;
            case E_NodeState.Running:
                ChildState = E_NodeState.Running;
                return E_NodeState.Running;
        }
        //ֻ��һ���������ⷵ�� 
        //�ɹ� ���ҽڵ�֮����Ҫ����ִ��
        return E_NodeState.Succeed;
    }
}