using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϊ��AI����������
/// </summary>
public class BTNodeObjcetDriver : MonoBehaviour
{

    /// <summary>
    /// �Ƿ�����̬�����Ϊ��
    /// </summary>
    public bool b_DynamicCheck ;
    [HideInInspector]
    /// <summary>
    /// ��̬�����
    /// </summary>
    public Dictionary<string, BTNodeInfo> dynamicDic;
    /// <summary>
    /// ���ڵ�
    /// </summary>
    public RootTreeNode RootNode;
    public BTNodeData data;
    public void Start()
    {
        IntiNode();
    }
    void Update()
    {
        if (RootNode != null)
        {
            RootNode.Execute();
        }

        //�ڶ�̬�������� ÿ��ִ����һ�ξ����״̬ ִ�гɹ�0.45F�����
        if (b_DynamicCheck)
            if (RootNode.ChildState == E_NodeState.Succeed)
            {
                if (Time.time >= NextClearTime)
                {
                    RootNode.ResetShowState();
                    NextClearTime = Time.time + 0.25f;
                }
            }

    }

    float NextClearTime;
    /// <summary>
    /// ��ʼ���ڵ㣬���ؽڵ�����
    /// </summary>
    public void IntiNode()
    {
            RootNode = BTNodeLoader.Instance.Load(this);
        //Debug.Log(dynamicDic);
    }
}
