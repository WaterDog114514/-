using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class NodeEditorCheck
{

    public VisualNodeEditorWindow win;
    public Rect Size => win.Size;
    public VisualDropmenu dropmenu => win.dropmenu;
    public NodeEditorCheck(VisualNodeEditorWindow win)
    {
        this.win = win;
    }

    private bool b_isEditorState;
    public void m_CheckUpdate()
    {
        //�����ּ��
        //  m_MouseScrollCheck();
        //��������
        m_mouseLeftClickColliderCheck();
        //�ӿ���ק���
        m_DragViewCheck();
        //�����ק��⵽
        m_DragSingleNodeCheck();
        //�Ҽ��������˵����
        m_DropMenuCheck();
    }

    /// <summary>
    /// �Ƿ��ڱ༭ģʽ�£��ڵ�༭ģʽ�Ƿ�ֹ�Ҽ��ڵ� �㲻��
    /// </summary>
    public bool b_IsInEditing;
    /// <summary>
    /// ����Ƿ�����ק�����䲻�����˵�
    /// </summary>
    private bool b_mousedeltaNoOpen;
    /// <summary>
    /// ��ʱ���λ��
    /// </summary>
    public Vector2 TempCurrentMousePos;
    /// <summary>
    /// �ж����λ���Ƿ��ڴ��ڷ�Χ��
    /// </summary>
    public bool b_IsInWindowSize =>
            TempCurrentMousePos.x >= Size.x &&
            TempCurrentMousePos.x <= (Size.x + Size.width) &&
            TempCurrentMousePos.y >= Size.y &&
            TempCurrentMousePos.y <= (Size.y + Size.height);
    /// <summary>
    /// ����ڵ������ڷ�Χ����
    /// </summary>
    public bool b_IsInDropMenuSize =>
                 TempCurrentMousePos.x >= dropmenu.ShowPos.x + Size.x &&
                TempCurrentMousePos.x <= (dropmenu.ShowPos.x + Size.x + dropmenu.Size.x) &&
                TempCurrentMousePos.y >= dropmenu.ShowPos.y + Size.y &&
                TempCurrentMousePos.y <= (dropmenu.ShowPos.y + Size.y + dropmenu.Size.y);
    /// <summary>
    /// �ӿ��϶����
    /// </summary>
    public bool b_IsDragChecking;
    /// <summary>
    /// �ӿ��϶����
    /// </summary>
    public void m_DragViewCheck()
    {

        //ˮ���޵������룺ֻ����Size�ڲż���϶�

        //��ס���ɿ���
        if (b_IsDragChecking)
        {
            win.Pos_CurrentView -= Event.current.delta * 0.1F;
            if (Event.current.button != 1) b_IsDragChecking = false;
            //���ٸ���,ֱ��return
            return;
        }
        TempCurrentMousePos = Event.current.mousePosition;
        //�ڱ༭���ӿ��ڲ���
        if (Event.current.button == 1 && b_IsInWindowSize) b_IsDragChecking = true;


    }

    /// <summary>
    /// �����˵����
    /// </summary>
    public void m_DropMenuCheck()
    {
        //������ģʽ��ȡ������
        if (b_isEditorState && Event.current.button == 1)
        {
            b_isEditorState = false;
            return;
        }
        //�������ȡ������
        if (Event.current.type == EventType.KeyUp || Event.current.type == EventType.MouseUp)
        {
            if (!b_IsInDropMenuSize)
            {
                dropmenu.m_HideDropmenu();
            }

        }
        //ֻҪ����ק���Ͳ�tm�İ����˵���
        if (Event.current.type == EventType.MouseDrag)
        {
            b_mousedeltaNoOpen = false;
        }
        //�����Ҽ������˵�
        if (Event.current.type == EventType.MouseUp)
        {


            //�����ѡ�нڵ�
            if (Event.current.button == 1 && Event.current.delta == Vector2.zero && b_mousedeltaNoOpen)
            {      //������ģʽ��ȡ������
                if (b_isEditorState)
                {
                    b_isEditorState = false;
                    return;
                }

                //����ڵ�༭ģʽ
                if (b_m_IsInControlSize())
                {
                    dropmenu.Page = VisualDropmenu.E_DropMenuPage.EditNode;
                    //�ڵ�༭ģʽ�Ƿ�ֹ�Ҽ��ڵ�㲻��
                    b_IsInEditing = true;
                }
                dropmenu.m_ShowDropmenu();
            }
            b_mousedeltaNoOpen = true;
        }







    }

    private double lastClickTime;
    /// <summary>
    /// ��������ײ���
    /// </summary>
    public void m_mouseLeftClickColliderCheck()
    {

        //˫�����
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown)
        {
            //����˫����������
            if (EditorApplication.timeSinceStartup - lastClickTime < 0.3)
            {
                b_isEditorState = true;
            }
            lastClickTime = EditorApplication.timeSinceStartup;
        }
        if (b_isEditorState == false&& win.SelectedNode != RootNode_VisualBehaviorTreeNode.instance)
            GUIUtility.keyboardControl = 0;


        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            //������ģʽ�� ���ӵ��߼�
            //ʹ���߼��Ƿ��·�İ취���
            if (b_m_IsInControlSize())
            {
                if (win.b_IsLinkingNode)
                    win.On_EndLinkNode(win.SelectedNode);
            }
            else
            {
                //ʧȥ����
                b_isEditorState = false;
            }
        }

    }

    public void m_MouseScrollCheck()
    {
        //if (Event.current.isScrollWheel && Event.current.delta != Vector2.zero)
        //{
        //    win. -= Event.current.delta.y * 0.05F;
        //    Pos_CurrentView -= Vector2.one * 7;
        //    if (win.SelectedNode < 0.35f)
        //    {
        //        win.SelectedNode = 0.35f;
        //        Pos_CurrentView += Vector2.one * 7;
        //    }
        //    else if (win.SelectedNode > 1.3f)
        //    {
        //        Pos_CurrentView += Vector2.one * 7;
        //        win.SelectedNode = 1.3f;
        //    }
        //}

    }
    /// <summary>
    /// ��ק�����ڵ�
    /// </summary>
    public void m_DragSingleNodeCheck()
    {
        //�����������ʱ�򣬲����ƶ�
        if (GUIUtility.keyboardControl != 0) return;
        //��ק�������
        //1.��ѡ�����岻Ϊ��ʱ
        //2.��ס������
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && win.SelectedNode != null)
        {
            win.SelectedNode.Pos_Self = m_getNodeCenter(win.SelectedNode);
        }
    }
    /// <summary>
    /// �ж�����Ƿ���ĳ���ڵ��ڣ��ڵĻ��ͷ���trueȻ������SelectNode
    /// </summary>
    /// <returns>�ڲ�����</returns>
    public bool b_m_IsInControlSize()
    {
        Vector2 fixedMousePos = TempCurrentMousePos - Size.position;
        //ʧ�����б�ѡ�нڵ㣬������б�ѡ�нڵ�
        //ֻ�в��ڱ��༭����²�ʧ��
        if (!b_IsInEditing)
        {
            if (win.SelectedNode != null)
            {
                win.SelectedNode.b_IsSelected = false;
                win.SelectedNode = null;
            }
        }
        //ѡ�б����㵽�Ľڵ�
        foreach (VisualBaseNode childNode in win.dic_Nodes.Values)
        {
            if (
                fixedMousePos.x >= childNode.Pos_Draw.x &&
                fixedMousePos.x <= (childNode.Pos_Draw.x + childNode.Size.x) &&
                fixedMousePos.y >= childNode.Pos_Draw.y &&
                fixedMousePos.y <= (childNode.Pos_Draw.y + childNode.Size.y))
            {
                win.SelectedNode = childNode;
                win.SelectedNode.b_IsSelected = true;
                return true;
            }
        }

        return false;
        // Debug.Log("���λ�ã�"+fixedMousePos+" �ڵ�λ��" + node.Pos_Draw);

    }
    public Vector2 m_getNodeCenter(VisualBaseNode node)
    {
        return Event.current.mousePosition - Size.position - win.Pos_CurrentView - Vector2.right * (node.Size.x / 2 + Size.x) - Vector2.up * node.Size.y / 2;
    }
}
