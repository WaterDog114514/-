using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// ��Ϸ������ӻ����(�༭ģʽ����)
/// </summary>
[CustomEditor(typeof(GameObjectInstance))]

public class GameObjectInspector : Editor
{
    private GameObj gameObj;
    private void OnEnable()
    {
        gameObj = (target as GameObjectInstance).gameObj;
    }
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        if (gameObj == null)
        {
            GUILayout.Label("��ͨ�����봴������ʵ������");
            return;
        }
        base.OnInspectorGUI();

        EditorGUILayout.TextField("����ID��", gameObj.ID.ToString());
        //�������Ϣ
        //string PoolInfo = PoolManager.Instance.Dic_Pool.ContainsKey(gameObj.PoolIdentity) ? gameObj.PoolIdentity : "��δ��������";
        //string PoolLimit = PoolManager.Instance.Dic_Pool.ContainsKey(gameObj.PoolIdentity) ? $"��ǰ���У�{PoolManager.Instance.Dic_Pool[gameObj.PoolIdentity].PoolQueue.Count}  ʹ���У�{PoolManager.Instance.Dic_Pool[gameObj.PoolIdentity].UsingQueue.Count}  ������ޣ�{PoolManager.Instance.Dic_Pool[gameObj.PoolIdentity].maxCount}" : "��δ��������";
        //EditorGUILayout.TextField("����أ�", PoolInfo);
        //EditorGUILayout.LabelField("�����ʹ�ã�", PoolLimit);

    }
}