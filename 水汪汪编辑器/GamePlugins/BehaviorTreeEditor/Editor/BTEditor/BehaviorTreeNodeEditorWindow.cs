using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BehaviorTreeNodeEditorWindow : VisualNodeEditorWindow
{
    /// <summary>
    /// ���ڴ��ڼ��ģʽ��
    /// </summary>
    public bool b_IsCheckingMode;
    /// <summary>
    /// ���ڼ��ӵ�����
    /// </summary>
    public BTNodeObjcetDriver CheckingDirver;
    /// <summary>
    /// ��ʼ���ڵ�
    /// </summary>
    public RootNode_VisualBehaviorTreeNode RootNode;
    /// <summary>
    /// ���ر��ش浵ר��
    /// </summary>
    public BTEditorSaveLoader Loader;
    /// <summary>
    /// ��������ʱ���ȵĽڵ�
    /// </summary>
    public VisualBehaviorTreeNode Temp_LinkNode;
    /// <summary>
    /// �ǲ��Ƕ������ӵ���
    /// </summary>
    public bool b_IsSonToFather;
    public BehaviorTreeNodeEditorWindow(BaseWindow window, Rect size) : base(window, size)
    {
        // CreateRootNode();
        Loader = new BTEditorSaveLoader(this);
        CreateRootNode();
        // new RootNode_VisualBehaviorTreeNode(null, Size.position + Size.size / 2, new Vector2(120, 60));
    }

    public void CreateRootNode()
    {
        RootNode = new RootNode_VisualBehaviorTreeNode(null, E_BehaviorType.RootNode);
        RootNode.Name = "��ʼ���ڵ�";
        RootNode.Pos_Self = Vector2.right * Size.size.x / 2 + Vector2.up * Size.size.y / 3;
        RootNode.dic_Nodes = dic_Nodes;
        m_AddNode(RootNode);
    }
    //������д
    public override void Draw()
    {

       
        base.Draw();
        if (GUI.Button(
            new Rect(Size.position + new Vector2(Size.width - 80, Size.height - 50), new Vector2(75, 45)), "�Զ����нڵ�"))
        {
            m_ClickAutoArrange();
        }
    }

    public override void On_EndLinkNode(VisualBaseNode LinkedNode)
    {
        //�������ӵ���
        if (b_IsSonToFather)
        {
            m_LinkNode(LinkedNode as VisualBehaviorTreeNode, Temp_LinkNode);
        }
        else
        {
            m_LinkNode(Temp_LinkNode, LinkedNode as VisualBehaviorTreeNode);
        }

        Temp_LinkNode = null;
        b_IsLinkingNode = false;
    }
    //��ʼ����
    public override void On_StartLinkedNode()
    {
        b_IsLinkingNode = true;
        Temp_LinkNode = SelectedNode as VisualBehaviorTreeNode;
    }
    //������нڵ�
    public override void m_ClearAllNodes()
    {
        base.m_ClearAllNodes();
        CurrentIndex = 0;
        CreateRootNode();
    }

    /// <summary>
    /// ����ĳ�ڵ�Ϊ��ʼ�ڵ�
    /// </summary>
    /// <param name="node"></param>
    public void m_SettingAsStartNode(VisualBehaviorTreeNode node)
    {
        if (RootNode.ChildID != -1)
        {
            (dic_Nodes[RootNode.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
        }
        node.FatherID = RootNode.ID;
        RootNode.ChildID = node.ID;
    }

    /// <summary>
    /// ���������ڵ�
    /// </summary>
    /// <param name="Father"></param>
    /// <param name="Son"></param>
    public void m_LinkNode(VisualBehaviorTreeNode Father, VisualBehaviorTreeNode Son)
    {
        //�ж��ǲ��Ǳ��ֽڵ��Ƿ��뵱��
        if (Father is BehaviorNode_VisualBehaviorTreeNode)
        {
            EditorUtility.DisplayDialog("��������", "�����ڵ��������ӽڵ㣬�ɲ�����������ѽ����", "(�s���t)");
            return;
        }
        //�ж��Ƿ��Ǹ��ڵ��뵱����
        if (Son is RootNode_VisualBehaviorTreeNode)
        {
            EditorUtility.DisplayDialog("��������", "��ʹ��\n������Ϊ��ʼ�ڵ㡱\n�����ӵ����ڵ㡱\nѡ�������ýڵ�Ϊ��ʼ�ڵ�", "(�s���t)");
            return;
        }
        //�Լ����Լ�
        if (Father == Son)
        {
            EditorUtility.DisplayDialog("��������", "�Լ��������Լ�\n�Լ���ô�ܵ������ֵ������أ�", "(�s���t)");
            return;
        }
        //�����������ĩ�ڵ㣬���������ԭ���ĸ��ӹ�ϵ
        if (Son is BehaviorNode_VisualBehaviorTreeNode)
            Son.m_DisConnectedFather();
        if (Father is RootNode_VisualBehaviorTreeNode)
        {
            RootNode_VisualBehaviorTreeNode Node = Father as RootNode_VisualBehaviorTreeNode;
            //�Ͽ�ԭ���ĸ���ϵ
            if (Node.ChildID != -1)
            {
                (dic_Nodes[Node.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
            }
            Node.ChildID = Son.ID;
        }
        else if (Father is DecoratorNode_VisualBehaviorTreeNode)
        {
            DecoratorNode_VisualBehaviorTreeNode Node = Father as DecoratorNode_VisualBehaviorTreeNode;
            //�Ͽ�ԭ���ĸ���ϵ
            if (Node.ChildID != -1)
            {
                (dic_Nodes[Node.ChildID] as VisualBehaviorTreeNode).FatherID = -1;
            }
            Node.ChildID = Son.ID;
        }
        else if (Father is ControlNode_VisualBehaviorTreeNode)
        {
            ControlNode_VisualBehaviorTreeNode Node = Father as ControlNode_VisualBehaviorTreeNode;
            Son.m_DisConnectedFather();
            //���˲����ظ����
            if (Node.ChildsId.Contains(Son.ID))
            {
                return;
            }
            Node.ChildsId.Add(Son.ID);
        }

        //�����������µ���ѽ
        Son.FatherID = Father.ID;
    }


    /// <summary>
    /// ��ʼ������Ϊ���ڵ�
    /// </summary>
    /// <param name="index"></param>
    public VisualBehaviorTreeNode CreateBehaviorNode(E_BehaviorType type, bool b_IsAddDic = true)
    {
        //��������  Ŀǰ��������λ��Ϊ Event.current.mousePosition - Size.position - win.Pos_CurrentView
        Vector2 defaultNodePos = dropmenu.ShowPos - Size.position - Pos_CurrentView;
        VisualBehaviorTreeNode node = null;
        //���ݽڵ����ʹ�����ͬ
        switch (type)
        {
            case E_BehaviorType.RootNode:
                node = new RootNode_VisualBehaviorTreeNode(null, type);
                node.Name = "��ʼ���ڵ�";
                break;
            case E_BehaviorType.SelectTreeNode:
                node = new ControlNode_VisualBehaviorTreeNode(null, type);
                node.Name = "ѡ��ڵ�";
                break;

            case E_BehaviorType.SequeneTreeNode:
                node = new ControlNode_VisualBehaviorTreeNode(null, type);
                node.Name = "���нڵ�";
                break;

            case E_BehaviorType.ParallelTreeNode:
                node = new ControlNode_VisualBehaviorTreeNode(null, type);
                node.Name = "���нڵ�";
                break;

            case E_BehaviorType.ActionTreeNode:
                node = new BehaviorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "�����ڵ�";
                break;

            case E_BehaviorType.ConditionNode:
                node = new BehaviorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "�����ڵ�";
                break;

            case E_BehaviorType.ReverseDecoratorNode:
                node = new DecoratorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "��ת�ڵ�";
                break;

            case E_BehaviorType.DelayDecoratorNode:
                node = new DecoratorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "�ӳ�ִ�нڵ�";
                break;

            case E_BehaviorType.RepeatDecoratorNode:
                node = new DecoratorNode_VisualBehaviorTreeNode(null, type);
                node.Name = "�ظ�ִ�нڵ�";
                break;
        }
        node.dic_Nodes = dic_Nodes;
        node.Pos_Self = defaultNodePos;
        //Debug.Log($"���:{ShowPos}  ���ڴ�Сλ��:{win.Pos_CurrentView}");
        //�������
        if (b_IsAddDic)
            m_AddNode(node);
        return node;
    }
    /// <summary>
    /// ����Զ�����
    /// </summary>
    public void m_ClickAutoArrange()
    {
        Pos_CurrentView = Vector2.zero;
        dic_Arrange.Clear();
        foreach (var node in dic_Nodes.Values)
        {

            //�Ӹ��ڵ㿪ʼ����
            if ((node as VisualBehaviorTreeNode).NodeType == E_BehaviorType.RootNode)
            {
                m_AutoLayer(node as VisualBehaviorTreeNode);
                m_AutoArrangeEveryLayer();
                return;
            }
        }

    }

    /// <summary>
    /// �����õ����¼��
    /// </summary>
    private Dictionary<int, List<VisualBehaviorTreeNode>> dic_Arrange = new Dictionary<int, List<VisualBehaviorTreeNode>>();
    /// <summary>
    /// �ݹ��Զ�д��ÿһ��ڵ����Ȳ㼶
    /// </summary>
    public void m_AutoLayer(VisualBehaviorTreeNode Node, int layer = 0)
    {
        //ʹ�ò㼶��
        //���жϸò���û�����壬û���Ƚ���һ��
        if (!dic_Arrange.ContainsKey(layer))
        {
            dic_Arrange.Add(layer, new List<VisualBehaviorTreeNode>());
        }
        switch (Node.NodeType)
        {
            case E_BehaviorType.RootNode:
                if ((Node as RootNode_VisualBehaviorTreeNode).ChildID == -1) break;
                dic_Arrange[layer].Add(Node);
                m_AutoLayer(dic_Nodes[(Node as RootNode_VisualBehaviorTreeNode).ChildID] as VisualBehaviorTreeNode, layer + 1);
                break;
            case E_BehaviorType.SelectTreeNode:
            case E_BehaviorType.SequeneTreeNode:
            case E_BehaviorType.ParallelTreeNode:
                dic_Arrange[layer].Add(Node);
                for (int i = 0; i < (Node as ControlNode_VisualBehaviorTreeNode).ChildsId.Count; i++)
                {
                    m_AutoLayer(dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]] as VisualBehaviorTreeNode, layer + 1);
                }
                break;
            case E_BehaviorType.ActionTreeNode:
            case E_BehaviorType.ConditionNode:
                dic_Arrange[layer].Add(Node);
                break;
            case E_BehaviorType.DelayDecoratorNode:
            case E_BehaviorType.ReverseDecoratorNode:
            case E_BehaviorType.RepeatDecoratorNode:
                dic_Arrange[layer].Add(Node);
                if ((Node as DecoratorNode_VisualBehaviorTreeNode).ChildID == -1) break;
                m_AutoLayer(dic_Nodes[(Node as DecoratorNode_VisualBehaviorTreeNode).ChildID] as VisualBehaviorTreeNode, layer + 1);
                break;
        }

    }
    /// <summary>
    /// �Զ�����ÿһ��
    /// </summary>
    public void m_AutoArrangeEveryLayer()
    {
        int maxLayer = 0;
        List<VisualBehaviorTreeNode> LayerNodes;
        //���ҵ�������
        foreach (var item in dic_Arrange.Keys)
        {
            if (maxLayer <= item) maxLayer = item;
        }
        //���ø��ڵ�λ��
        RootNode_VisualBehaviorTreeNode.instance.Pos_Self = Vector2.right * Size.width / 4 + Vector2.up * Size.size.y / 16;
        for (int i = 1; i <= maxLayer; i++)
        {
            LayerNodes = dic_Arrange[i];
            float width = (LayerNodes.Count - 1) * 150;
            for (int j = 0; j < LayerNodes.Count; j++)
            {
                LayerNodes[j].Pos_Self = dic_Nodes[LayerNodes[j].FatherID].Pos_Self +
                    Vector2.up * 200 +
                    Vector2.right * 150F * j +
                    -width / 2 * Vector2.right
                    ;
            }
        }

    }

}
//arrangeHeightDistance = Vector2.up * Node.Size.y * 1.5f;
////ͨ���ﺢ�����а취���ݹ�
//switch (Node.NodeType)
//{
//    //���ڵ�ֱ���������
//    case E_BehaviorType.RootNode:
//        //���к���
//        Node.Pos_Self = Vector2.right * Size.width / 4 + Vector2.up * Size.size.y / 16;

//        if ((Node as RootNode_VisualBehaviorTreeNode).ChildID == -1) return;
//        //���ڵ�����������Ǹ��ڵ������+10
//        dic_Nodes[(Node as RootNode_VisualBehaviorTreeNode).ChildID].Pos_Self =
//            Node.Pos_Self + arrangeHeightDistance;
//        m_AutoLayer(dic_Nodes[(Node as RootNode_VisualBehaviorTreeNode).ChildID] as VisualBehaviorTreeNode);
//        break;
//    case E_BehaviorType.SelectTreeNode:
//    case E_BehaviorType.SequeneTreeNode:
//    case E_BehaviorType.ParallelTreeNode:
//        if ((Node as ControlNode_VisualBehaviorTreeNode).ChildsId.Count == 0) return;
//        int count = (Node as ControlNode_VisualBehaviorTreeNode).ChildsId.Count;
//        int childSonCount = getChild_ChildCount(Node as ControlNode_VisualBehaviorTreeNode);
//        float TotalWidth = 0;
//        for (int i = 0; i < count; i++)
//        {
//            if (dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]] is ControlNode_VisualBehaviorTreeNode)
//                for (int j = 0; j < (dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]] as ControlNode_VisualBehaviorTreeNode).ChildsId.Count; j++)
//                {
//                    TotalWidth += dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]].Size.x;
//                }
//            //���ƽڵ����������Ǹ���������+10��Ȼ���������ƫ��Size�Ŀ�ȳ�1.2
//            if (childSonCount <= count)
//                dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]].Pos_Self =
//                    Node.Pos_Self + arrangeHeightDistance * Vector2.up + //����ƫ��
//                    Vector2.right * arrangeWidthtDistance * 0.65F * (count / 2 - i)//���
//                    - Vector2.right * offset * arrangeWidthtDistance;//ż��ƫ��

//            else
//                dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]].Pos_Self =
//                    Node.Pos_Self + arrangeHeightDistance +  //����ƫ��
//                    Vector2.right * arrangeWidthtDistance * (1.0f * childSonCount / count) * (count / 2 - i)  //���
//                    - Vector2.right * offset * arrangeWidthtDistance;  //ż��ƫ��

//            m_AutoLayer(dic_Nodes[(Node as ControlNode_VisualBehaviorTreeNode).ChildsId[i]] as VisualBehaviorTreeNode);
//        }
//        break;
//    case E_BehaviorType.ActionTreeNode:
//    case E_BehaviorType.ConditionNode:
//        //���Ǳ�ʾû�к���
//        return;
//    case E_BehaviorType.DelayDecoratorNode:
//    case E_BehaviorType.ReverseDecoratorNode:
//    case E_BehaviorType.RepeatDecoratorNode:

//        int ChildID = (Node as DecoratorNode_VisualBehaviorTreeNode).ChildID;
//        if (ChildID == -1) return;
//        dic_Nodes[ChildID].Pos_Self = Node.Pos_Self + arrangeHeightDistance;
//        m_AutoLayer(dic_Nodes[ChildID] as VisualBehaviorTreeNode);
//        break;
//}