using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// �༭��ר����Դ����ģ��
/// </summary>
public class EditorAssetsLoader
{
    public static EditorAssetsLoader Instance { get; private set; } = new EditorAssetsLoader();
    /// <summary>
    /// ͨ��id����ȡ����������ԴAsset�еĶ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T FindWithID<T>(int id) where T : Object
    {
        //�Ȼ�Asset�е�ȡԤ�������
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            T prefab = AssetDatabase.LoadAssetAtPath<T>(path);
            if (prefab != null && prefab.GetInstanceID() == id)
            {
                return prefab;
            }
        }
        //��ͨ��������ȥ����
        //ֻ��Gameobject���ܷŵ�������ѽ
        if (typeof(T) != typeof(GameObject)) return null;
        GameObject[] sceneGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in sceneGameObjects)
        {
            if (obj.GetInstanceID() == id)
            {
                return obj as T;
            }
        }
        return null;
    }
}
