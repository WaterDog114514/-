using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����ڵ�  �����ӽڵ㷵��ʧ�ܻ�ɹ������������
/// </summary>
[System.Serializable]
public sealed class ParallelTreeNode : ControlTreeNode
{
    /// <summary>
    /// ������¼ÿ���ڵ��Ƿ�ִ�����
    /// </summary>
    public bool[] FinishIndex;
    private E_NodeState tempState;
    public override E_NodeState Execute()
    {
        //��һ��ִ�У�������һ��
        if (FinishIndex == null)
        {
            FinishIndex = new bool[childNodes.Count];
        }
        for (int i = 0; i < FinishIndex.Length; i++)
        {
            //û����ɾ�ִ�� Ȼ�����
            if (FinishIndex[i] == false)
            {
                tempState = childNodes[i].Execute();
                //������
                switch (tempState)
                {
                    //����˼�¼����
                    case E_NodeState.Succeed:
                    case E_NodeState.Faild:
                        FinishIndex[i] = true;
                        break;
                    //�����������
                    case E_NodeState.Running:
                        break;
                }
            }
        }
        //������е������ô
        for (int i = 0; i < FinishIndex.Length; i++)
        {
            //ֻҪ��һ����û����� ���˳�����������״̬
            if (FinishIndex[i] == false)
            {
                ChildState = E_NodeState.Running;
                return E_NodeState.Running;
            }
        }
        //ȫ��ִ����ϣ������Ǳ��false����״̬
        for (int i = 0; i < FinishIndex.Length; i++)
        {
            FinishIndex[i] = false;
        }
        //���ж������ˣ����سɹ�
        ChildState = E_NodeState.Succeed;
        return E_NodeState.Succeed;

    }

}
