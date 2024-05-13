using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���Ῠ��AI�߼���ʹ��Э����ִ��
/// </summary>
[System.Serializable]
public class DelayDecoratorNode : DecoratorNode
{

    public DelayDecoratorNode(float timeCD)
    {
        waitTime = new WaitForSeconds(timeCD);
    }
    /// <summary>
    /// �ȴ��У������ظ�ִ��
    /// </summary>
    public bool IsWaiting;
    public DelayDecoratorNode()
    {

    }
    public override E_NodeState Execute()
    {
        //�����ӳ��в�ִ��
        if (!IsWaiting)
            MonoManager.Instance.StartCoroutine(ExecuteChildNode());
        ChildState = E_NodeState.Succeed;
        return E_NodeState.Succeed;
    }
    public WaitForSeconds waitTime;
    public IEnumerator ExecuteChildNode()
    {
        IsWaiting = true;
        //��ִ��һ��
        childNode.Execute();
        yield return waitTime;
        while (true)
        {
            //��ִ֡��
            switch (childNode.Execute())
            {
                case E_NodeState.Succeed:
                case E_NodeState.Faild:
                    IsWaiting = false;
                    yield break;
                default:
                    break;
            }
            yield return null;
        }

    }
}
