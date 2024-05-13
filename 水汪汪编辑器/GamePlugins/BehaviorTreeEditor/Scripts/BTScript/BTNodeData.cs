using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BTNodeData : ScriptableObject
{
    public string TreeNodeDataPath;
    [SerializeField]
    public TextAsset BehaviorTreeData;
    [SerializeField]
    public GameObject BehaviorTreePrefab;
    public string LoadData()
    {
        return BehaviorTreeData.text;
    }
}

//json����
public class BTNodeJsonData
{
    public Dictionary<string, BTNodeInfo> dic_Info;
}