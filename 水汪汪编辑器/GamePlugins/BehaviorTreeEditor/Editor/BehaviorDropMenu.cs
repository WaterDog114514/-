using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BehaviorDropMenu : VisualDropmenu
{

    public BehaviorDropMenu(VisualNodeEditorWindow Win) : base(Win)
    {
        Size = new Vector2(100, EditorGUIUtility.singleLineHeight * 8);
    }

    public override void m_DrawMainPage()
    {
        if (GUI.Button(m_GetButtonRect(), "������Ϊ���ڵ�"))
        {
            Page = E_DropMenuPage.CreateNode;
        }
        //���
        if (GUI.Button(m_GetButtonRect(), "��սڵ�"))
        {
           win.m_ClearAllNodes();
            m_HideDropmenu();
        }
        //3.����
        if (GUI.Button(m_GetButtonRect(), "����ҳ��"))
        {
            m_HideDropmenu();
        }
    }
    public override void m_DrawEditNodePage()
    {
        if (GUI.Button(m_GetButtonRect(), "���ӵ����ڵ�"))
        {
            //��ʼ����
            (win as BehaviorTreeNodeEditorWindow).b_IsSonToFather = true;
            win.On_StartLinkedNode();
            m_HideDropmenu();
        }
        if (GUI.Button(m_GetButtonRect(), "����ӽڵ�"))
        {
            //��ʼ����
            (win as BehaviorTreeNodeEditorWindow).b_IsSonToFather = false;
            win.On_StartLinkedNode();
            m_HideDropmenu();
        }
        if (GUI.Button(m_GetButtonRect(), "����Ϊ��ʼ�ڵ�"))
        {
            (win as BehaviorTreeNodeEditorWindow).m_SettingAsStartNode(win.SelectedNode as VisualBehaviorTreeNode);
            m_HideDropmenu();
        }
        if (GUI.Button(m_GetButtonRect(), "ɾ���ýڵ�"))
        {
            m_RemoveNode();
            m_HideDropmenu();
        }
    }
    /// <summary>
    /// �Ƴ�ѡ�нڵ㷽��
    /// </summary>
    public void m_RemoveNode()
    {
        VisualBehaviorTreeNode SelectedNode = win.SelectedNode as VisualBehaviorTreeNode;

        //�ж���ϵ��ɾ��
        SelectedNode.m_DisConnectedFather();
        SelectedNode.m_DisConnectedSons();
        //�����Ƴ�
        win.dic_Nodes.Remove(SelectedNode.ID);
    }

    public override void m_DrawCreatePage()
    {
        if (GUI.Button(m_GetButtonRect(), "ѡ��ڵ�"))
        {
            //�ȹش���
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.SelectTreeNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "˳��ڵ�"))
        {
            //�ȹش���
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.SequeneTreeNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "���нڵ�"))
        {
            //�ȹش���
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.ParallelTreeNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "�����ڵ�"))
        {
            //�ȹش���
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.ActionTreeNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "�����ڵ�"))
        {
            //�ȹش���
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.ConditionNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "��תװ�νڵ�"))
        {
            //�ȹش���
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.ReverseDecoratorNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "�ӳ�ִ�нڵ�"))
        {
            //�ȹش���
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.DelayDecoratorNode);
        }
        else if (GUI.Button(m_GetButtonRect(), "�ظ��ڵ�"))
        {
            //�ȹش���
            m_HideDropmenu();
            (win as BehaviorTreeNodeEditorWindow).CreateBehaviorNode(E_BehaviorType.RepeatDecoratorNode);
        }
    }





}
