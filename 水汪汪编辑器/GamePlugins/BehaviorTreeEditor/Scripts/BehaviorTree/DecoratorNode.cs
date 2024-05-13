using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// װ�νڵ�  �ܹ�����Ҫִ�ж��ٴ� �ӳ�ִ�е�����
/// </summary>
[System.Serializable]
public abstract class DecoratorNode : BaseTreeNode
{
    public BaseTreeNode childNode;
    public override void ResetShowState()
    {
        base.ResetShowState();
        childNode.ResetShowState();
    }
}

