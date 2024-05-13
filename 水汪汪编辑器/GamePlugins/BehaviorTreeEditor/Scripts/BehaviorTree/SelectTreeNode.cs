using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����ôӷ�ִ֡�еĽǶ�ȥ�˽����

/// <summary>
/// ѡ��ڵ�
/// �ص㣺
/// 1.������ִ���Լ����ӽڵ�
/// 2.�����ǰ�ڵ�ִ�гɹ��� �Ͳ������ִ�к����ڵ�
/// 3.�����ǰ�ڵ�ִ��ʧ���� �ͻ��������ִ�� ֱ���ɹ�
/// 4.��һ����û�гɹ��򷵻�ʧ��
/// </summary>
[System.Serializable]
public class SelectTreeNode : ControlTreeNode
{
    //���ﲻ����whileѭ������Ϊwhileѭ����һ֡�������߼�����
    //���ǵ������Ƕ�֡�����һ���ֻأ�����ʹ��Я��ȥ�ȴ�ʱ��ȥ����ִ����ȥ
    public override E_NodeState Execute()
    {
        //���ѡ��ڵ��� ��ĳ���ڵ�ִ�гɹ� �Ͳ��ؼ�������ִ����
        //ֱ�ӷ��سɹ�����
        if (childNodes.Count == 0)
        {
            Debug.LogError("�˿��ƽڵ�û���κε��ӽڵ�");
            ChildState = E_NodeState.Succeed;
            return E_NodeState.Succeed;

        }
        switch (childNodes[nowIndex].Execute())
        {
            //�ɹ��� 
            case E_NodeState.Succeed:
                //���¿�ʼ
                    nowIndex = 0;
                ChildState = E_NodeState.Succeed;
                return E_NodeState.Succeed;

            case E_NodeState.Faild:
                //ѡ��ڵ㣬ʧ��������һ��
                nowIndex++;
                //�Ѿ�û�и���Ľڵ����ִ����
                //��֤��ǰ��Ķ�ʧ����
                if (nowIndex == childNodes.Count)
                {
                    nowIndex = 0;
                    ChildState = E_NodeState.Faild;
                    return E_NodeState.Faild;
                }
                break;

            case E_NodeState.Running:
                ChildState = E_NodeState.Running;
                return E_NodeState.Running;
        }
        //ֻ�е�ѡ��ڵ�û��ִ����ʱ ���ҵ�ǰ�ڵ�ʧ��ʱ �Ż�������
        //֤����ϣ������һ֡��������ִ�� �������ﷵ�سɹ�
        return E_NodeState.Succeed;
    }
}
