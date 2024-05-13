using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// ��Ϊ���༭������
/// </summary>
public class Win_BehaviorTree : SingletonBaseWindow
{
    public BehaviorTreeNodeEditorWindow editorWindow;

    
    #region ��ʼ���߼�
    [MenuItem("ˮ�������/��Ϊ�����ӻ��༭��")]
    public static void Open()
    {
        GetWindow<Win_BehaviorTree>();
    }

    protected override void OnEnable()
    {
        m_IntiTreeBehavior();
        IntiWindowsSetting("��Ϊ���༭��", "SaveIcon.png");
        base.OnEnable();
    }

    private void m_IntiTreeBehavior()
    {
        editorWindow = new BehaviorTreeNodeEditorWindow(this, new Rect(0.15F, 0, 0.85F, 1F));
    }


    #endregion
    protected override void m_DrawWindows()
    {
        editorWindow.Draw();
        DrawLeftWindows();
        //��֤ˢ����
        Repaint();
    }
    /// <summary>
    /// ������ߴ���
    /// </summary>
    private void DrawLeftWindows()
    {
        //��������б�
        GUILayout.Label("��ǰѡ�нڵ㣺", GUILayout.Width(LeftWindowWeight));
        EditorGUILayout.TextField(editorWindow.SelectedNode != null ? editorWindow.SelectedNode.Name : "��", GUILayout.Width(LeftWindowWeight));
        if(GUILayout.Button("�������нڵ�", GUILayout.Width(LeftWindowWeight)))
        {
            editorWindow.Loader.m_SaveAllData();
           // main.m_SaveAllData();
        }
    }
    /// <summary>
    /// �����ߴ��ڵĿ��
    /// </summary>
    private float LeftWindowWeight => editorWindow.Size.x;
}
