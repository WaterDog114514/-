using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
/// <summary>
/// �޷�����ı༭��
/// </summary>
public class Win_UIListener: SingletonBaseWindow
{
    private EW_UIListener main;
    [MenuItem("ˮ��������/UI�������������")]
    protected static void OpenWindow()
    {
        GetWindow<Win_UIListener>();
    }
    /// <summary>
    /// ��������Ϣ
    /// </summary>
    private Vector2 scrollValue;
    protected override void OnEnable()
    {
        main = new EW_UIListener();
        base.OnEnable();
        IntiWindowsSetting("UI����", "YuSheIcon.png");
        //����һ��S
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        main = null;
    }
    protected override void m_DrawWindows()
    {
        if(main==null) main = new EW_UIListener();
        //�������壬�����û���ע��Щ��Ҫ����
        if (GUILayout.Button("�����ø��������������ӿؼ�", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight)))
        {
            main.ClickFind();
        }
        main.isFoladed = EditorGUILayout.Foldout(main.isFoladed, "չ��");
        if (main.isFoladed && main.list_CheckChilds.Count > 0)
        {
            EditorGUI.DrawRect(new Rect(0, EditorGUIUtility.singleLineHeight * 2.3F, WindowWidth, EditorGUIUtility.singleLineHeight * 10), Color.grey);
            scrollValue = GUILayout.BeginScrollView(scrollValue, GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight * 10));
            for (int i = 0; i < main.list_CheckChilds.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(main.list_CheckChilds[i].ShowInfo);
                main.list_CheckChilds[i].IsListened = GUILayout.Toggle(main.list_CheckChilds[i].IsListened, "�Ƿ����");
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
        //ȷ������
        if (main.list_CheckChilds.Count > 0)
        {
            #region �����������

            EditorGUILayout.BeginHorizontal();
            //�����������
            main.ListenerParents = EditorGUILayout.ObjectField(new GUIContent("����������"), main.ListenerParents, typeof(GameObject), true) as GameObject;
            //��������
            if (GUILayout.Button("��������������"))
            {
                EditorGUIUtility.ShowObjectPicker<GameObject>(null, true, null, 0);
            }
            GUILayout.EndHorizontal();
            if (Event.current.commandName == "ObjectSelectorUpdated")
                main.ListenerParents = EditorGUIUtility.GetObjectPickerObject() as GameObject;

            #endregion
            //����������
            main.ClassName = EditorGUILayout.TextField("����������",main.ClassName);
            if (GUILayout.Button("��ʼ���ɼ�������"))
            {
                main.m_ClickCreateScript();
            }
        }
    }
}
