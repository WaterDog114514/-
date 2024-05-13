using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
//�������ļ����˰�
public class BTNodeLoader
{
    public static BTNodeLoader Instance = new BTNodeLoader();
    private Dictionary<string, BTNodeInfo> Read_Dic;
    private GameObject AiObj;
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public RootTreeNode Load(BTNodeObjcetDriver driver)
    {
        if (driver.data == null)
        {
            Debug.LogError("��AI����û�а���Ϊ���ļ�");
            return null;
        }
        if(driver.data.BehaviorTreeData==null)
            driver.data.BehaviorTreeData = Resources.Load<TextAsset>(driver.data.TreeNodeDataPath);
        AiObj = driver.gameObject;
        Read_Dic = JsonMapper.ToObject<BTNodeJsonData>(driver.data.BehaviorTreeData.text).dic_Info;
        foreach (var nodeInfo in Read_Dic.Values)
        {
            if (nodeInfo.NodeType == E_BehaviorType.RootNode)
            {

                RootTreeNode node = LoadNode(nodeInfo) as RootTreeNode;
                //�ֶ��ͷ��ڴ�
                if (isOpenCheck == false)
                {
                    Read_Dic = null;
                    AiObj = null;
                }
                return node;
            }
        }
        return null;
    }
    public RootTreeNode LoadAndCheck(BTNodeObjcetDriver driver, ref Dictionary<string, BTNodeInfo> dic)
    {
        isOpenCheck = true;
        RootTreeNode node = Load(driver);
        //���ض�̬�ڴ�
        dic = Read_Dic;
        isOpenCheck = false;
        //�ͷ��ڴ�
        Read_Dic = null;
        return node;
    }
    /// <summary>
    /// �������ڵ�������ڵ��ί��ʱ�����ʱ����
    /// </summary>
    private string[] TempParameter = new string[2];
    private MethodInfo TempMethod;

    /// <summary>
    /// ��̬���
    /// </summary>
    private bool isOpenCheck;
    //�ݹ�˼ά����
    public BaseTreeNode LoadNode(BTNodeInfo info)
    {
        BaseTreeNode node = null;
        switch (info.NodeType)
        {
            case E_BehaviorType.RootNode:
                node = new RootTreeNode();
                if (info.childsID.Count == 0) return null;
                (node as RootTreeNode).childNode = LoadNode(Read_Dic[info.childsID[0].ToString()]);
                break;
            case E_BehaviorType.SelectTreeNode:
                node = new SelectTreeNode();
                if (info.childsID.Count == 0) return node;
                for (int i = 0; i < info.childsID.Count; i++)
                {
                    (node as ControlTreeNode).AddNode(LoadNode(Read_Dic[info.childsID[i].ToString()]));
                }

                break;
            case E_BehaviorType.SequeneTreeNode:
                node = new SequeneTreeNode();
                if (info.childsID.Count == 0) return node;
                for (int i = 0; i < info.childsID.Count; i++)
                {
                    (node as ControlTreeNode).AddNode(LoadNode(Read_Dic[info.childsID[i].ToString()]));
                }
                break;
            case E_BehaviorType.ParallelTreeNode:

                node = new ParallelTreeNode();
                if (info.childsID.Count == 0) return node;
                for (int i = 0; i < info.childsID.Count; i++)
                {
                    (node as ControlTreeNode).AddNode(LoadNode(Read_Dic[info.childsID[i].ToString()]));
                }
                break;
            case E_BehaviorType.ActionTreeNode:
                node = new ActionTreeNode();
                for (int i = 0; i < info.Parameters.Length; i++)
                {
                    //�������
                    TempParameter = info.Parameters[i].Split('|');
                    string CompoentName = TempParameter[0].Substring(0, TempParameter[0].IndexOf('&'));
                    string methodName = TempParameter[1].Substring(0, TempParameter[1].IndexOf('&'));
                    TempMethod = AiObj.GetComponent(CompoentName).GetType().GetMethod(methodName);
                    //ֱ����Ӽ���
                    (node as ActionTreeNode).AddEvent((UnityAction)Delegate.CreateDelegate(typeof(UnityAction), AiObj.GetComponent(CompoentName), TempMethod));
                }
                // (node as ActionTreeNode)
                break;
            case E_BehaviorType.ConditionNode:
                node = new ConditionNode();
                for (int i = 0; i < info.Parameters.Length; i++)
                {
                    //�������
                    TempParameter = info.Parameters[i].Split('|');
                    string CompoentName = TempParameter[0].Substring(0, TempParameter[0].IndexOf('&'));
                    string methodName = TempParameter[1].Substring(0, TempParameter[1].IndexOf('&'));
                    TempMethod = AiObj.GetComponent(CompoentName).GetType().GetMethod(methodName);
                    //ֱ����Ӽ���
                    (node as ConditionNode).AddEvent((Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), AiObj.GetComponent(CompoentName), TempMethod));
                }

                break;
            case E_BehaviorType.DelayDecoratorNode:
                //��ֵ
                node = new DelayDecoratorNode();
                try
                {
                    (node as DelayDecoratorNode).waitTime = new WaitForSeconds(float.Parse(info.Parameters[0]));
                }
                catch
                {
                    Debug.LogError("�в����ַ�");
                }

                //����ѽ
                if (info.childsID.Count == 0) return node;
                (node as DecoratorNode).childNode = LoadNode(Read_Dic[info.childsID[0].ToString()]);
                break;
            case E_BehaviorType.ReverseDecoratorNode:
                node = new ReverseDecoratorNode();
                //����
                if (info.childsID.Count == 0) return node;
                (node as DecoratorNode).childNode = LoadNode(Read_Dic[info.childsID[0].ToString()]);
                break;

            case E_BehaviorType.RepeatDecoratorNode:
                node = new RepeatDecoratorNode();
                try
                {
                    (node as RepeatDecoratorNode).TotalExecuteCount = int.Parse(info.Parameters[0]);
                }
                catch
                {
                    Debug.LogError("�в����ַ�");
                }
                //����
                if (info.childsID.Count == 0) return node;
                (node as DecoratorNode).childNode = LoadNode(Read_Dic[info.childsID[0].ToString()]);
                break;
        }
        //�Ƿ����˶�̬��� �����˾ͱ��淵��
        if (isOpenCheck)
            info.Node = node;
        return node;
    }
}
