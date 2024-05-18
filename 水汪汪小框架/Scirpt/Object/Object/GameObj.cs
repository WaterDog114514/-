using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��Ϸ����  �Ͷ���ؽ��ܹ�����Ҳ�������Դ���ܹ���
/// </summary>
public class GameObj : Obj
{

    /// <summary>
    /// ʵ����Ϸ����
    /// </summary>
    private GameObject Instance;
    public GameObj(GameObject Instance,UnityAction IntiCallback = null)
    {
        if (Instance == null)
            Debug.LogError("ʵ��������ʱ�����Ϊnull��");

        this.Instance = Instance;

        IntiCallback?.Invoke();
        //������ʼ��
        EnterPoolCallback += () =>
        {
            Instance.SetActive(false);
        };
        QuitPoolCallback += () =>
        {
            Instance.SetActive(true);
            IntiCallback?.Invoke();
        };

        DestroyCallback += () =>
        {
            Object.Destroy(Instance);
        };

    }
    public Transform transform => Instance.transform;
    public string name => Instance.name;

    public override string PoolIdentity => name;

    public T GetComponent<T>() where T : Component
    {
        return Instance.GetComponent<T>();
    }
}
