using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

//Ϊ������̫����ӷ�ף���һ��ģ����ʵ��BT�ڵ�༭���ļ��ش洢����
public class BTEditorSaveLoader
{
    private BehaviorTreeNodeEditorWindow win;
    public BTEditorSaveLoader(BehaviorTreeNodeEditorWindow win)
    {
        this.win = win;
    }
    /// <summary>
    /// ������������
    /// </summary>
    public void m_SaveAllData()
    {
        //����ı��
        BTNodeJsonData jsonData = new BTNodeJsonData();
        jsonData.dic_Info = new Dictionary<string, BTNodeInfo>();
        //�������浽������
        foreach (VisualBaseNode node in win.dic_Nodes.Values)
        {
            VisualBehaviorTreeNode Node = node as VisualBehaviorTreeNode;
            BTNodeInfo info = new BTNodeInfo();
            //������ֵ
            switch (Node.NodeType)
            {
                case E_BehaviorType.RootNode:
                    info.childsID.Add((Node as RootNode_VisualBehaviorTreeNode).ChildID);
                    break;
                case E_BehaviorType.SelectTreeNode:
                case E_BehaviorType.SequeneTreeNode:
                case E_BehaviorType.ParallelTreeNode:
                    info.childsID = (Node as ControlNode_VisualBehaviorTreeNode).ChildsId;
                    break;
                case E_BehaviorType.ActionTreeNode:
                case E_BehaviorType.ConditionNode:
                    break;
                case E_BehaviorType.DelayDecoratorNode:
                case E_BehaviorType.ReverseDecoratorNode:
                case E_BehaviorType.RepeatDecoratorNode:
                    info.childsID.Add((Node as DecoratorNode_VisualBehaviorTreeNode).ChildID);
                    break;
            }
            //��ͨ��ֵ
            info.Description = Node.Description;
            info.ID = Node.ID;
            info.NodeType = Node.NodeType;
            info.Parameters = Node.Parameter;
            //����
            jsonData.dic_Info.Add(info.ID.ToString(), info);
        }
        // ��������
        string path = EditorUtility.SaveFilePanel("������Ϊ������", Application.dataPath, null, null);
        string pathAsset = "Assets" + path.Replace(Application.dataPath, null) + ".asset";
        JsonManager.Instance.SaveDataToPath(jsonData, path + ".json");
        //����С�����ļ�
        BTNodeData btNodeData = ScriptableObject.CreateInstance<BTNodeData>();
        btNodeData.BehaviorTreePrefab = win.RootNode.BehaviorObj;
        btNodeData.TreeNodeDataPath = "Assets/" + path.Replace(Application.dataPath, null) + ".json";
        AssetDatabase.CreateAsset(btNodeData, pathAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //��΢����һ�°󶨵������ļ�
        BTNodeData importData = AssetDatabase.LoadAssetAtPath<BTNodeData>(pathAsset);
        importData.BehaviorTreeData = AssetDatabase.LoadAssetAtPath<TextAsset>(btNodeData.TreeNodeDataPath);

        //��Ԥ��������
        GameObject prefab = RootNode_VisualBehaviorTreeNode.instance.BehaviorObj;
        if (prefab != null)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets")) + "/Library/ScriptAssemblies/Assembly-CSharp.dll");
            BTNodeObjcetDriver bTNodeObjcetDriver = prefab.GetComponent(assembly.GetType("BTNodeObjcetDriver")) as BTNodeObjcetDriver;
            if (bTNodeObjcetDriver == null) bTNodeObjcetDriver = prefab.AddComponent(assembly.GetType("BTNodeObjcetDriver")) as BTNodeObjcetDriver;
            bTNodeObjcetDriver.data = importData;

        }

    }
    /// <summary>
    /// ��̬�������ݣ�ֱ���������м�����Ϊ��
    /// </summary>
    /// <param name="dic"></param>
    public void m_LoadDynamicData(Dictionary<string, BTNodeInfo> dic)
    {
        if (win.dic_Nodes.Count > 1) win.m_ClearAllNodes();
        Read_Dic = dic;
        tempData = win.CheckingDirver.data;
        LoadAllNode();
        win.b_IsCheckingMode = true;
        foreach (var node in win.dic_Nodes.Values)
        {
            (node as VisualBehaviorTreeNode).b_IsCheckingMode = true;

        }
        tempData = null;
        //װж��
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    /// <summary>
    /// ��ͣʱ����Ҫ�ر� ��Ϊ���༭���ļ���
    /// </summary>
    /// <param name="state"></param>
    private  void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            win.m_ClearAllNodes();
            EditorWindow.GetWindow<Win_BehaviorTree>().Close();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }
    }
    private BTNodeData tempData;
    /// <summary>
    /// ��̬��������
    /// </summary>
    public void m_LoadStaticData(BTNodeData data)
    {
        //�����ظ�������BUG
        if (win.dic_Nodes.Count > 1) win.m_ClearAllNodes();
        Read_Dic = JsonMapper.ToObject<BTNodeJsonData>(data.BehaviorTreeData.text).dic_Info;
        tempData = data;
        LoadAllNode();
        //�ͷ�
        tempData = null;
    }

    private Dictionary<string, BTNodeInfo> Read_Dic;
    /// <summary>
    /// �󶨰ְ�ӵ�еĺ��ӣ���̬���ز��ã�
    /// </summary>


    /// <summary>
    /// �ڵ��Ұְ�
    /// </summary>
    public void m_NodeFindSelfFather(VisualBehaviorTreeNode node, BTNodeInfo info)
    {
        //���ڵ��Ұְ֣���Ϊ���˰ְ֣����Ӳ�������
        foreach (var FatherInfo in Read_Dic.Values)
        {
            if (FatherInfo.ID == node.ID) continue;
            switch (FatherInfo.NodeType)
            {
                case E_BehaviorType.RootNode:
                    if (FatherInfo.childsID.Count > 0)
                        if (FatherInfo.childsID[0] == node.ID)
                            node.FatherID = FatherInfo.ID;
                    break;
                case E_BehaviorType.SelectTreeNode:
                case E_BehaviorType.SequeneTreeNode:
                case E_BehaviorType.ParallelTreeNode:

                    if (FatherInfo.childsID.Count != 0)
                    {
                        for (int i = 0; i < FatherInfo.childsID.Count; i++)
                        {
                            if (FatherInfo.childsID[i] == node.ID)
                                node.FatherID = FatherInfo.ID;
                        }
                    }
                    break;

                case E_BehaviorType.ActionTreeNode:
                case E_BehaviorType.ConditionNode:
                    //���������ܵ���
                    break;
                case E_BehaviorType.DelayDecoratorNode:
                case E_BehaviorType.ReverseDecoratorNode:
                case E_BehaviorType.RepeatDecoratorNode:
                    if (FatherInfo.childsID.Count > 0 && info != FatherInfo)
                        if (FatherInfo.childsID[0] == node.ID)
                            node.FatherID = FatherInfo.ID;
                    break;
            }

        }
    }
    /// <summary>
    /// ���ؽڵ�
    /// </summary>
    public void LoadAllNode()
    {
        win.dic_Nodes.Clear();
        VisualBehaviorTreeNode node;
        foreach (var info in Read_Dic.Values)
        {
            node = win.CreateBehaviorNode(info.NodeType, false);
            node.Description = info.Description;
            node.BehaviorNode = info.Node;
            node.Parameter = info.Parameters;
            node.ID = info.ID;
            win.m_AddNode(node, node.ID);
            //���ðְ־��У���̬��ȡ�ĸ����Ͳ���Ҫ�Ҷ���
            //���ò���
            switch (node.NodeType)
            {
                case E_BehaviorType.RootNode:
                    //win�󶨸��ڵ�
                    // Debug.Log( EditorAssetsLoader.Instance.FindWithID<GameObject>(int.Parse(node.Parameter[0])));
                    win.RootNode = node as RootNode_VisualBehaviorTreeNode;
                    //������󶨿���ֱ�Ӱ�data��
                    if (tempData.BehaviorTreePrefab != null)
                        RootNode_VisualBehaviorTreeNode.instance.BehaviorObj = tempData.BehaviorTreePrefab;
                    //���ö���
                    win.RootNode.ChildID = info.childsID[0];
                    break;
                case E_BehaviorType.ActionTreeNode:
                case E_BehaviorType.ConditionNode:
                    //���ض����ڵ�������ڵ�
                    m_LoadBNode(node, info);
                    break;
                case E_BehaviorType.ReverseDecoratorNode:
                case E_BehaviorType.DelayDecoratorNode:
                case E_BehaviorType.RepeatDecoratorNode:
                    //���ú���
                    (node as DecoratorNode_VisualBehaviorTreeNode).ChildID = info.childsID[0];
                    break;

                case E_BehaviorType.SelectTreeNode:
                case E_BehaviorType.SequeneTreeNode:
                case E_BehaviorType.ParallelTreeNode:
                    //���ú�����
                    (node as ControlNode_VisualBehaviorTreeNode).ChildsId = info.childsID;
                    break;
            }
            //�󶨺ðְ�
            m_NodeFindSelfFather(node, info);
        }
        //����һ��
        win.m_ClickAutoArrange();
    }
    /// <summary>
    ///���������ڵ㣬�����ڵ�ר��
    /// </summary>
    public void m_LoadBNode(VisualBehaviorTreeNode node, BTNodeInfo info)
    {
        BehaviorNode_VisualBehaviorTreeNode bNode = node as BehaviorNode_VisualBehaviorTreeNode;
        bNode.ListenNum = info.Parameters.Length;

        //����ÿ��ѡ��
        for (int i = 0; i < bNode.ListenNum; i++)
        {
            string[] TempParameter = info.Parameters[i].Split('|');
            string componentName = TempParameter[0];
            string methodName = TempParameter[1];
            //��ȡ����õ���ѡ��
            int Select1 = int.Parse(componentName.Substring(componentName.IndexOf('&') + 1, componentName.Length - componentName.IndexOf('&') - 1));
            int Select2 = int.Parse(methodName.Substring(methodName.IndexOf('&') + 1, methodName.Length - methodName.IndexOf('&') - 1));
            bNode.selectedIndex[i].Select1 = Select1;
            bNode.selectedIndex[i].Select2 = Select2;
        }
    }
}

public class BTImportSetting : AssetPostprocessor
{
    private void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName)
    {
        if (assetPath.EndsWith(".asset"))
        {
            string assetName = Path.GetFileNameWithoutExtension(assetPath);

            Debug.Log("Asset imported: " + assetName);
        }
    }
}