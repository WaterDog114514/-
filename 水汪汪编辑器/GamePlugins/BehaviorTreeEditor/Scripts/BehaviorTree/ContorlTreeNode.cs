using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ƽڵ㣺��Ҫ���������ӽڵ���߼���ת
/// </summary>
[System.Serializable]
public abstract class ControlTreeNode: BaseTreeNode
{
    /// <summary>
    /// ��ǰִ���߼����ӽڵ����
    /// </summary>
    public int nowIndex;

    /// <summary>
    /// ���ڴ洢�ӽڵ������ �ýڵ�������ӽڵ㶼��洢�ڸ�List��
    /// </summary>
    protected List<BaseTreeNode> childNodes = new List<BaseTreeNode>();

    /// <summary>
    /// ����ӽڵ�ķ��� ʹ�ñ䳤���� ��Ϊһ���ڵ������n���ӽڵ� ͨ���䳤���� ���ӵķ���
    /// </summary>
    public void AddNode(BaseTreeNode node)
    {
        childNodes.Add(node);
    }
    public override void ResetShowState()
    {
        base.ResetShowState();
        for (int i = 0; i < childNodes.Count; i++)
        {
            childNodes[i].ResetShowState();
        }
    }

}

