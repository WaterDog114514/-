using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ���ӻ��ڵ�༭���ڣ�ͨ��Ƕ���ڱ���Ӵ���ʹ��
/// </summary>
public abstract class VisualNodeEditorWindow
{
    /// <summary>
    /// ���ڵ�༭���������˵�
    /// </summary>
    public VisualDropmenu dropmenu;
    /// <summary>
    /// ��Ƕ�׵Ĵ���
    /// </summary>
    public BaseWindow window;
    //public List<VisualBaseNode> list_nodes = new List<VisualBaseNode>();
    /// <summary>
    /// �ڵ���
    /// </summary>
    public Dictionary<int, VisualBaseNode> dic_Nodes;
    /// <summary>
    /// ����̫��ӷ�ף�дһ�������
    /// </summary>
    public NodeEditorCheck check;

    public int CurrentIndex = 0;
    /// <summary>
    /// ����ӽڵ�
    /// </summary>
    /// <param name="node"></param>
    public void m_AddNode(VisualBaseNode node, int id = -1)
    {
        //��ָ����Ӿ�������
        if (id != -1)
        {
            dic_Nodes.Add(id, node);
            return;
        }
        //ID�����һ�޶� ���˾ͼ�
        if (dic_Nodes.Count != 0)
        {
            while (dic_Nodes.ContainsKey(CurrentIndex))
            {
                CurrentIndex++;
            }
        }
        //��������
        node.ID = CurrentIndex;
        //���
        dic_Nodes.Add(CurrentIndex, node);
    }
    /// <summary>
    /// �Ѿ�ѡ�еĽڵ�
    /// </summary>
    public VisualBaseNode SelectedNode;
    public Texture BackgroundImage;

    /// <summary>
    /// ����ϵ��
    /// </summary>
    public float ScaleValue = 0.8F;
    /// <summary>
    /// ��ǰ�ӿ�����
    /// </summary>
    public Vector2 Pos_CurrentView;
    /// <summary>
    /// ����������ʹ�С
    /// </summary>
    public Rect Size
    {
        get
        {
            return new Rect(
                _size.x * window.WindowWidth,
                _size.y * window.WindowHeight,
                _size.width * window.WindowWidth,
                _size.height * window.WindowHeight
                );
        }
    }
    /// <summary>
    /// �ﵽ��Ӧ�ԣ����԰ٷֱȴ�С
    /// </summary>
    public Rect _size;

    /// <summary>
    /// ��ʼ��һ���ڵ�༭�����ڹ���
    /// </summary>
    /// <param name="window">ҪǶ��Ĵ���</param>
    /// <param name="size">��ռ�ȷ�</param>
    public VisualNodeEditorWindow(BaseWindow window, Rect size)
    {
        //���ñ�Ƕ���window��˭
        this.window = window;
        //���ü����
        check = new NodeEditorCheck(this);
        //���ô��ڴ�С
        _size = size;
        //��ʼ�������ڵ������б�
        dropmenu = new BehaviorDropMenu(this);
        //���ñ���ͼ
        BackgroundImage = WindowUtility.LoadAssetFromPath<Texture>(EM_WinSetting.Instance.SettingData.BehaviorTreeBGImage);
        //��ʼ���ڵ�洢��
        dic_Nodes = new Dictionary<int, VisualBaseNode>();
    }
    /// <summary>
    /// ������нڵ�
    /// </summary>
    public virtual void  m_ClearAllNodes()
    {
        dic_Nodes.Clear();
    }

    #region �������

    /// <summary>
    /// ���ƽڵ�༭������
    /// </summary>
    public virtual void Draw()
    {
        //���ǰ�ò���
        t_m_DrawBeginOperate();
        //����ܻ��Ʋ���
        t_m_DrawWin();
    }
    /// <summary>
    /// ����֮ǰǰ���Ը��¹���
    /// </summary>
    public void t_m_DrawBeginOperate()
    {
        //���Ʊ���ͼ
        m_DrawBackground();
        //������
        check.m_CheckUpdate();
    }
    /// <summary>
    /// �ܻ��Ʋ���
    /// </summary>
    public void t_m_DrawWin()
    {

        //���ƴ���
        GUI.BeginGroup(Size);
        //���������б����
        m_DrawNodes();
        dropmenu.m_drawDropMenu();
        GUI.EndGroup();
    }

    /// <summary>
    /// ���������ӽڵ�ķ���
    /// </summary>
    public void m_DrawNodes()
    {
        foreach (VisualBaseNode childNode in dic_Nodes.Values)
        {
            //ǰ�ø���
            childNode.t_m_DrawBeforeUpdate(m_GetTransformPos);
            //�����߼�
            childNode.t_m_DrawSelf();
        }
    }
    /// <summary>
    /// ��ȡ��ת���������
    /// </summary>
    public Vector2 m_GetTransformPos => Size.position + Pos_CurrentView;
    /// <summary>
    /// ���Ʊ���ͼ
    /// </summary>
    public void m_DrawBackground()
    {
        GUI.DrawTexture(Size, BackgroundImage);
    }

    #endregion

    #region ���ӽڵ����
    /// <summary>
    /// �������ӽڵ�
    /// </summary>
    /// <param name="LinkedNode"></param>
    public abstract void On_EndLinkNode(VisualBaseNode LinkedNode);

    /// <summary>
    /// ��ʼ���ӽڵ�
    /// </summary>
    /// <param name="LinkedNode"></param>
    public abstract void On_StartLinkedNode();
    /// <summary>
    /// �Ƿ������ӽڵ�״̬��
    /// </summary>
    public bool b_IsLinkingNode;

    #endregion



}
