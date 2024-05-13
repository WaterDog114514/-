using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// һ�п��ӻ���Ϊ���ڵ���ϵ�
/// </summary>
[System.Serializable]
public class VisualBehaviorTreeNode : VisualBaseNode
{

    /// <summary>
    /// �ڵ�����
    /// </summary>
    public string Description;
    public BaseTreeNode BehaviorNode;
    public E_NodeState TempState = E_NodeState.Faild;
    public E_BehaviorType NodeType;
    public int FatherID = -1;
    public string[] Parameter;
    /// <summary>
    /// ���ڴ��ڼ��ģʽ��
    /// </summary>
    public bool b_IsCheckingMode;
    /// <summary>
    /// ��Ϊ���ڵ�������ĳ�ʼ��
    /// </summary>
    /// <param name="Description"></param>
    /// <param name="type"></param>
    public VisualBehaviorTreeNode(string Description, E_BehaviorType type)
    {
        this.Description = Description;
        NodeType = type;

    }
    public override void DrawControlData()
    {
        base.DrawControlData();
        //��������
        Description = EditorGUI.TextField(m_getControlDrawRect(true), Description == null ? "�ڵ�����...." : Description);

    }
    public override void t_m_DrawBeforeUpdate(Vector2 Offset)
    {
        base.t_m_DrawBeforeUpdate(Offset);
        //�Ȼ�����������
        m_DrawLink();
    }
    /// <summary>
    /// �������ӹ�ϵ ���Լ�����
    /// </summary>
    public void m_DrawLink()
    {
        //���ϰֲ��ܻ���
        if (FatherID == -1) return;
        if (dic_Nodes[FatherID] is RootNode_VisualBehaviorTreeNode)
            Handles.color = Color.red;
        else
            Handles.color = Color.cyan;
        //�е�����1 ����Ϸ�
        Vector2 center1 = Pos_Draw + Vector2.right * Size / 2 + Vector2.up *
            (dic_Nodes[FatherID].Pos_Draw.y + dic_Nodes[FatherID].Size.y - Pos_Draw.y) / 2;
        //�е�����2 �յ��·�
        Vector2 center2 = dic_Nodes[FatherID].Pos_Draw + Vector2.up * dic_Nodes[FatherID].Size.y + Vector2.right * dic_Nodes[FatherID].Size / 2
            - Vector2.up * (dic_Nodes[FatherID].Pos_Draw.y + dic_Nodes[FatherID].Size.y - Pos_Draw.y) / 2;
        //�ֱ��Ӧ�����е�  �����Լ����е�  �����е�  
        Handles.DrawLine(Pos_Draw + Vector2.right * Size / 2, center1);
        Handles.DrawLine(
              center1
            , center2);
        Handles.DrawLine(center2,
              dic_Nodes[FatherID].Pos_Draw + Vector2.up * dic_Nodes[FatherID].Size.y + Vector2.right * dic_Nodes[FatherID].Size.x / 2);

        Handles.DrawWireCube(dic_Nodes[FatherID].Pos_Draw + Vector2.up * dic_Nodes[FatherID].Size.y + Vector2.right * dic_Nodes[FatherID].Size.x / 2, Vector2.one * 20);
    }
    public override void m_IntiSize()
    {
        Size = new Vector2(100, 60);
    }

    public override void t_m_DrawSelf()
    {

        //���ģʽ�»���
        if (b_IsCheckingMode)
        {
            //����״̬
            TempState = BehaviorNode.ChildState;
            m_DrawDynimicCheckState();
        }
        //���Ʊ���
        m_DrawBackground();
        //���Ʊ���
        m_DrawTitle();
        //���ݼ��ģʽ�Ƿ���ɫ
        EditorGUI.BeginDisabledGroup(b_IsCheckingMode);
        //�������пؼ�
        DrawControlData();
        EditorGUI.EndDisabledGroup();
    }
    public override void m_IntiDrawStyle()
    {
        base.m_IntiDrawStyle();
        CheckStyle = new GUIStyle();
        CheckStyle.normal.textColor = Color.red;
        CheckStyle.alignment = TextAnchor.MiddleCenter;
        CheckStyle.fontStyle = UnityEngine.FontStyle.Bold;
        CheckStyle.fontSize = 19;
    }
    private GUIStyle CheckStyle;
    public void m_DrawDynimicCheckState()
    {
        Vector2 size = new Vector2(Size.x, singleLineHeight * 1.55F);
        switch (TempState)
        {
            case E_NodeState.Succeed:
                if(NodeType == E_BehaviorType.DelayDecoratorNode)
                {
                    //�ӳٽڵ����ʾʣ��ʱ��
                    CheckStyle.normal.textColor = Color.cyan;
                    GUI.Label(new Rect(Pos_Draw - Vector2.up * singleLineHeight * 1.65f, size), "�ӳٵȴ���", CheckStyle);
                    return;
                }
                CheckStyle.normal.textColor = Color.green;
                GUI.Label(new Rect(Pos_Draw - Vector2.up * singleLineHeight * 1.65f, size), "ִ�гɹ�", CheckStyle);
                break;
            case E_NodeState.Faild:
                CheckStyle.normal.textColor = Color.red;
                GUI.Label(new Rect(Pos_Draw - Vector2.up * singleLineHeight * 1.65f, size), "δִ��/ʧ��", CheckStyle);
                break;
            case E_NodeState.Running:
                CheckStyle.normal.textColor = Color.yellow;
                GUI.Label(new Rect(Pos_Draw - Vector2.up * singleLineHeight * 1.65f, size), "ִ����...", CheckStyle);
                break;
        }
    }


    public override void m_DrawTitle()
    {
        if (!b_IsCheckingMode)
            base.m_DrawTitle();
        else
        {
            EditorGUI.DrawRect(m_getTitleRect(), new Color(0.56F, 0.43F, 0.81F, 0.96f));
            EditorGUI.LabelField(m_getTitleRect(), Name, TitleStyle);
        }
    }
    /// <summary>
    /// ��������뺢���ǵ���ϵ
    /// </summary>
    public void m_DisConnectedSons()
    {

        if (this is RootNode_VisualBehaviorTreeNode)
        {
            RootNode_VisualBehaviorTreeNode Node = this as RootNode_VisualBehaviorTreeNode;
            if (Node.ChildID != -1)
                (dic_Nodes[Node.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
            Node.ChildID = -1;
        }
        else if (this is DecoratorNode_VisualBehaviorTreeNode)
        {
            DecoratorNode_VisualBehaviorTreeNode Node = this as DecoratorNode_VisualBehaviorTreeNode;
            if (Node.ChildID != -1)
                (dic_Nodes[Node.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
            Node.ChildID = -1;
        }
        else if (this is ControlNode_VisualBehaviorTreeNode)
        {
            ControlNode_VisualBehaviorTreeNode Node = this as ControlNode_VisualBehaviorTreeNode;
            //�Ȱ����ӹ�ϵ����
            if (Node.ChildsId.Count == 0) return;
            for (int i = 0; i < Node.ChildsId.Count; i++)
            {
                (dic_Nodes[Node.ChildsId[i]] as VisualBehaviorTreeNode).FatherID = -1;
            }
            Node.ChildsId.Clear();
        }
        //�����ڵ��ǲ������Ǹ��ڵ����

    }
    /// <summary>
    /// ����Լ����ϵ�����ϵ
    /// </summary>
    public void m_DisConnectedFather()
    {
        if (FatherID == -1) return;
        if (dic_Nodes[FatherID] is RootNode_VisualBehaviorTreeNode)
        {
            (dic_Nodes[FatherID] as RootNode_VisualBehaviorTreeNode).ChildID = -1;
        }
        else if (dic_Nodes[FatherID] is DecoratorNode_VisualBehaviorTreeNode)
        {
            (dic_Nodes[FatherID] as DecoratorNode_VisualBehaviorTreeNode).ChildID = -1;
        }
        else if (dic_Nodes[FatherID] is ControlNode_VisualBehaviorTreeNode)
        {
            (dic_Nodes[FatherID] as ControlNode_VisualBehaviorTreeNode).ChildsId.Remove(ID);
        }
        //�����ڵ��ǲ������Ǹ��ڵ����
        FatherID = -1;
    }
}


//public void DemoDrawSon()
//{
//    if(this is ControlNode_VisualBehaviorTreeNode)
//    {
//        for (int i = 0; i < (this as ControlNode_VisualBehaviorTreeNode).ChildsId.Count; i++)
//        {
//            EditorGUI.LabelField(m_getLabelRect(), $"����{i}:",FontStyle);
//            EditorGUI.LabelField(m_getControlDrawRect(), (this as ControlNode_VisualBehaviorTreeNode).ChildsId[i].ToString());
//        }
//    }
//    else if (this is DecoratorNode_VisualBehaviorTreeNode)
//    {
//        EditorGUI.LabelField(m_getLabelRect(), $"����:",FontStyle);
//        EditorGUI.LabelField(m_getControlDrawRect(), (this as DecoratorNode_VisualBehaviorTreeNode).ChildID.ToString());
//    }

//}