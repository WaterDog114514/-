using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.Events;

public abstract class VisualBaseNode
{
    public string Name;
    protected GUIStyle FontStyle;
    protected GUIStyle TitleStyle;
    /// <summary>
    /// �ڵ�ΨһID
    /// </summary>
    public int ID = -1;
    public Vector2 Pos_Self;
    /// <summary>
    /// �ڵ�滭��λ��
    /// </summary>
    public Vector2 Pos_Draw;
    /// <summary>
    /// �ڵ㴰�ڴ�С
    /// </summary>
    public Vector2 Size
    {
        get { return _size; }
        set { _size = value; }
    }
    protected Vector2 _size = Vector2.zero;
    [NonSerialized]
    public Dictionary<int, VisualBaseNode> dic_Nodes = new Dictionary<int, VisualBaseNode>();

    /// <summary>
    /// �ڵ����������
    /// </summary>
    public virtual void t_m_DrawSelf()
    {
        //���Ʊ���
        m_DrawBackground();
        //���Ʊ���
        m_DrawTitle();
        //�������пؼ�
        DrawControlData();

    }
    /// <summary>
    /// ��ʼ��������ʽ
    /// </summary>
    public virtual void m_IntiDrawStyle()
    {
        //���ÿؼ�
        FontStyle = new GUIStyle();
        FontStyle.fontSize = 13;
        FontStyle.normal.textColor = Color.black;
        FontStyle.fontStyle = UnityEngine.FontStyle.Bold;
        //���ñ���
        TitleStyle = new GUIStyle();
        TitleStyle.normal.textColor = new Color(0.93F, 0.86F, 0.501F);
        TitleStyle.alignment = TextAnchor.MiddleCenter;
        TitleStyle.fontSize = 18;
    }
    /// <summary>
    /// �滭ǰ���ø����߼�
    /// </summary>
    public virtual void t_m_DrawBeforeUpdate(Vector2 Offset)
    {

        //���û滭λ��
        Pos_Draw = Offset + Pos_Self;
        //��ѡ�л���ɫ���
        if (b_IsSelected)
        {
            Handles.color = Color.green;
            Handles.DrawWireCube(Pos_Draw + Size / 2, Size + Vector2.one * 2);
        }
        //��������
        IndexControl = 0;
        IndexLabel = 0;
    }
    /// <summary>
    /// ���Ʊ���
    /// </summary>
    public virtual void m_DrawBackground()
    {
        EditorGUI.DrawRect(new Rect(Pos_Draw, Size), new Color(0.325F, 0.525F, 0.111F, 0.59f));
    }
    /// <summary>
    /// �����ڵ�
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="size"></param>
    public VisualBaseNode()
    {
        m_IntiDrawStyle();
        m_IntiSize();
    }
    /// <summary>
    /// ����ڵ��Ƿ�ѡ���ˣ�
    /// </summary>
    public bool b_IsSelected;
    /// <summary>
    /// ���Ʊ��ⷽ��
    /// </summary>
    public virtual void m_DrawTitle()
    {
        //�Ȼ����ⱳ��ɫ
        EditorGUI.DrawRect(m_getTitleRect(), new Color(0.56F, 0.43F, 0.81F, 0.7f));
        Name = EditorGUI.TextField(m_getTitleRect(), Name, TitleStyle);
    }
    /// <summary>
    /// ���ƿؼ���Ϣ
    /// </summary>
    public virtual void DrawControlData()
    {

    }
    /// <summary>
    /// ��ʼ�������С
    /// </summary>
    public virtual void m_IntiSize()
    {
        Size = new Vector2(100, 50);
    }
    protected int IndexLabel = 0;
    protected int IndexControl = 0;
    protected float singleLineHeight = EditorGUIUtility.singleLineHeight * 1.45f;
    /// <summary>
    /// ȡ�ÿؼ��ı�ǩ���
    /// </summary>
    /// <returns></returns>
    public Rect m_getLabelRect(float width = 1.0f / 3)
    {
        return new Rect(new Vector2(Pos_Draw.x, Pos_Draw.y + (++IndexLabel) * singleLineHeight), new Vector2(Size.x * width, singleLineHeight));
    }
    /// <summary>
    /// ȡ�ÿؼ�����Ŀ��
    /// </summary>
    /// <param name="b_IsLine">�Ƿ�ռһ��</param>
    /// <returns></returns>
    public Rect m_getControlDrawRect(bool b_IsLine)
    {
        ++IndexLabel;
        return new Rect(new Vector2(Pos_Draw.x, Pos_Draw.y + (++IndexControl) * singleLineHeight), new Vector2(Size.x, singleLineHeight));
    }
    public Rect m_getControlDrawRect(float width = 2.0f / 3)
    {

        return new Rect(new Vector2(Pos_Draw.x + (Size.x - Size.x * width), Pos_Draw.y + (++IndexControl) * singleLineHeight), new Vector2(Size.x * width, singleLineHeight));
    }
    public Rect m_getTitleRect()
    {
        return new Rect(new Vector2(Pos_Draw.x , Pos_Draw.y - 10), new Vector2(Size.x, singleLineHeight * 1.5F));
    }
    public void m_getLineRect()
    {
        IndexLabel++;
        IndexControl++;

    }
}
