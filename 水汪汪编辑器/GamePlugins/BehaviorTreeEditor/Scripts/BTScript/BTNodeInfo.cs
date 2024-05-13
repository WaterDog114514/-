using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// ����һ�ױ༭���ɶ� ��Ϸ��Ҳ�ܶ�ȡ�����������ࣨ�Ƚ������ڴ棬�����ȡ����ֶ��ͷţ�
/// </summary>
public class BTNodeInfo 
{
    /// <summary>
    /// �����洢�ڴ��ַ�����ܸ��༭��ͳһ��
    /// </summary>
    public BaseTreeNode Node;
    /// <summary>
    /// �����Һ���
    /// </summary>
    public E_BehaviorType NodeType;
    /// <summary>
    /// ����
    /// </summary>
    public string Description;
    public int ID; // �洢���ڵ������
    public List<int> childsID; // �洢�ӽڵ����������
    /// <summary>
    /// �ýڵ�����������
    /// </summary>
    public string[] Parameters;
    public BTNodeInfo()
    {
        childsID = new List<int>();
    }
}
/// <summary>
/// ��Ϊ���ڵ�����
/// </summary>
public enum E_BehaviorType
{
    RootNode,
    SelectTreeNode, SequeneTreeNode, ParallelTreeNode,
    ActionTreeNode, ConditionNode,
    DelayDecoratorNode, ReverseDecoratorNode, RepeatDecoratorNode
}