using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// ��ʼ���ڵ�
/// </summary>
[System.Serializable]
public class RootNode_VisualBehaviorTreeNode : VisualBehaviorTreeNode
{

    public int ChildID = -1;
    /// <summary>
    /// ���ڵ�ֻ����һ������������Ϊ��̬
    /// </summary>
    public static RootNode_VisualBehaviorTreeNode instance = null;
    public GameObject BehaviorObj
    {
        //ˢ������
        set
        {
            //��һ�ξ���
            if (_obj == value) return;
            _obj = value;
            //�վͲ�������Ŷ
            if (_obj == null) return;
            Parameter[0] = BehaviorObj.GetInstanceID().ToString();
            RootMethodReflection.Instance.RefreshRootNode();
        }
        get => _obj;
    }
    private GameObject _obj;
    public override void m_IntiSize()
    {
        Size = new Vector2(100, 90);
    }
    public RootNode_VisualBehaviorTreeNode(string Description, E_BehaviorType type) : base(Description, type)
    {
        instance = this;
        Parameter = new string[1] { "-1" };
    }
    public override void m_DrawTitle()
    {
        GUI.Label(m_getTitleRect(), Name, TitleStyle);
    }
    public override void m_IntiDrawStyle()
    {
        base.m_IntiDrawStyle();
        TitleStyle.normal.textColor = Color.red;
    }
    /// <summary>
    /// ��һ����ʼ���ӽڵ�
    /// </summary>
    public override void DrawControlData()
    {
        //����Ԥ����ѡ���
        GUI.Label(m_getControlDrawRect(true), "AIԤ���壺", FontStyle);
        BehaviorObj = (GameObject)EditorGUI.ObjectField(m_getControlDrawRect(true), BehaviorObj, typeof(GameObject), true);
    }

    public override void m_DrawBackground()
    {
        //��ɫ
        EditorGUI.DrawRect(new Rect(Pos_Draw , Size), new Color(1F, 0.85F, 0.72F, 0.69f));
    }
}
