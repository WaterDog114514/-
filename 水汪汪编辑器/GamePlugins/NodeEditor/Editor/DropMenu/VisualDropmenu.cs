using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// �����˵�ר����չ������̫����ӷ�׶�������
/// �̳д���Ϳ��Դﵽ�ظ������˵���
/// </summary>
public abstract class VisualDropmenu
{
    public E_DropMenuPage Page = E_DropMenuPage.Main;


    public VisualNodeEditorWindow win;
    /// <summary>
    /// �����˵���С
    /// </summary>
    public Vector2 Size;
    /// <summary>
    /// �Ƿ�չʾҳ��
    /// </summary>
    private bool b_IsShowDropMenu;
    public Vector2 ShowPos;
    public VisualDropmenu(VisualNodeEditorWindow Win)
    {
        win = Win;
        Size = new Vector2(100, 50);
    }
    /// <summary>
    /// �������˵�
    /// </summary>
    public void m_drawDropMenu()
    {
        if (!b_IsShowDropMenu) return;
        //�����б���
        EditorGUI.DrawRect(new Rect(ShowPos, Size), new Color(1, 1, 1, 0.34f));
        OneButtonIndex = -1;
        //����ҳ�����滭��ͬ�˵�չʾ
        switch (Page)
        {
            case E_DropMenuPage.Main:
                m_DrawMainPage();
                break;
            case E_DropMenuPage.EditNode:
                m_DrawEditNodePage();
                break;
            case E_DropMenuPage.CreateNode:
                m_DrawCreatePage();
                break;
        }



    }
    /// <summary>
    /// �滭��ť��������λ����
    /// </summary>
    protected int OneButtonIndex;
    /// <summary>
    /// ȡ����ť��Rect
    /// </summary>
    /// <returns></returns>
    public Rect m_GetButtonRect()
    {
        OneButtonIndex++;
        return new Rect(ShowPos + Vector2.up * EditorGUIUtility.singleLineHeight * (OneButtonIndex), new Vector2(Size.x, EditorGUIUtility.singleLineHeight));
    }

    /// <summary>
    /// �����½ڵ�
    /// </summary>
    public virtual void m_DrawMainPage()
    {

    }
    /// <summary>
    /// �滭�༭ĳ�����ڵ�Ĳ˵�
    /// </summary>
    public virtual void m_DrawEditNodePage()
    {
        ////ɾ���ڵ�
        //if (GUI.Button(m_GetButtonRect(), "ɾ���ýڵ�"))
        //{
        //    win.list_nodes.Remove(win.SelectedNode);
        //    m_HideDropmenu();
        //}
    }
    /// <summary>
    /// ���ƴ����ڵ�ҳ��
    /// </summary>
    public abstract void m_DrawCreatePage();
    /// <summary>
    /// ��ǰҪ���Ƶ�ҳ��
    /// </summary>
    public enum E_DropMenuPage
    {
        Main, EditNode, CreateNode
    }
    /// <summary>
    /// չʾ�����˵�����
    /// </summary>
    public void m_ShowDropmenu()
    {
        b_IsShowDropMenu = true;
        win.b_IsLinkingNode = false;
        //����
        ShowPos = Event.current.mousePosition - win.Size.position;

    }
    /// <summary>
    /// ���ز˵�����
    /// </summary>
    public void m_HideDropmenu()
    {
        //�˳��ڵ�༭ģʽ �ڵ�༭ģʽ�Ƿ�ֹ�Ҽ��ڵ�㲻��
        b_IsShowDropMenu = false;
        //����
        win.check.b_IsInEditing = false;
        //����ҳ��
        Page = E_DropMenuPage.Main;
    }

}