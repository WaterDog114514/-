using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// �㼶ö��
/// </summary>
public enum E_UILayer
{
    /// <summary>
    /// ��ײ�
    /// </summary>
    Bottom,
    /// <summary>
    /// �в�
    /// </summary>
    Middle,
    /// <summary>
    /// �߲�
    /// </summary>
    Top,
    /// <summary>
    /// ϵͳ�� ��߲�
    /// </summary>
    System,
}

/// <summary>
/// ��������UI���Ĺ�����
/// ע�⣺���Ԥ������Ҫ���������һ�£���������
/// </summary>
public class UIManager : Singleton_UnMono<UIManager>
{

    //ui�����ؼ�
    private Camera uiCamera;
    private Canvas uiCanvas;
    private EventSystem uiEventSystem;

    //�㼶������
    private Transform bottomLayer;
    private Transform middleLayer;
    private Transform topLayer;
    private Transform systemLayer;

    /// <summary>
    /// ���ڴ洢���е�������
    /// </summary>
    private Dictionary<string, UIBasePanelInfo> panelDic = new Dictionary<string, UIBasePanelInfo>();

    /// <summary>
    /// ��ʼ����������ʵ����������UI�ؼ�
    /// </summary>
    public void IntiManager()
    {
        //��̬����Ψһ��Canvas��EventSystem���������
        uiCamera = GameObject.Instantiate(ResLoader.Instance.LoadRes_Sync<GameObject>("UI/UICamera")).GetComponent<Camera>();

        //��̬����Canvas
        uiCanvas = GameObject.Instantiate(ResLoader.Instance.LoadRes_Sync<GameObject>("UI/UICanvas")).GetComponent<Canvas>();
        //����ʹ�õ�UI�����
        uiCanvas.worldCamera = uiCamera;
        //�ҵ��㼶������
        bottomLayer = uiCanvas.transform.Find("Bottom");
        middleLayer = uiCanvas.transform.Find("Middle");
        topLayer = uiCanvas.transform.Find("Top");
        systemLayer = uiCanvas.transform.Find("System");

        //��̬����EventSystem
        uiEventSystem = GameObject.Instantiate(ResLoader.Instance.LoadRes_Sync<GameObject>("UI/UIEventSystem")).GetComponent<EventSystem>();

        //���������Ƴ������ؼ�
        Object.DontDestroyOnLoad(uiEventSystem.gameObject);
        Object.DontDestroyOnLoad(uiCanvas.gameObject);
        Object.DontDestroyOnLoad(uiCamera.gameObject);
    }




    public UIManager()
    {
        IntiManager();
    }

    /// <summary>
    /// ��ȡ��Ӧ�㼶�ĸ�����
    /// </summary>
    /// <param name="layer">�㼶ö��ֵ</param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UILayer layer)
    {
        switch (layer)
        {
            case E_UILayer.Bottom:
                return bottomLayer;
            case E_UILayer.Middle:
                return middleLayer;
            case E_UILayer.Top:
                return topLayer;
            case E_UILayer.System:
                return systemLayer;
            default:
                return null;
        }
    }

    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="layer">�����ʾ�Ĳ㼶</param>
    /// <param name="callBack">���ڿ������첽���� ���ͨ��ί�лص�����ʽ ��������ɵ���崫�ݳ�ȥ����ʹ��</param>
    /// <param name="isSync">�Ƿ����ͬ������ Ĭ��Ϊfalse</param>
    public void ShowPanel<T>(E_UILayer layer = E_UILayer.Middle, UnityAction<T> callBack = null, bool isSync = false) where T : UIBasePanel
    {
        //ͨ���������ȡ��� Ԥ������������������һ�� 
        PanelInfo<T> panelInfo = GetPanel<T>(isSync);
        //�����Ƿ��첽������ɶ��ж�
        //if (panelInfo.PanelLoadTask.isFinish)
        //    ReallyShowPanel<T>(panelInfo, layer, callBack);
        //else
        //    panelInfo.PanelLoadTask.AddCallbackCommand(() =>
        //    {
        //        ReallyShowPanel<T>(panelInfo, layer, callBack);
        //    });
    }
    /// <summary>
    /// �������������ʾ���ط���
    /// </summary>
    private void ReallyShowPanel<T>(PanelInfo<T> panelInfo, E_UILayer layer = E_UILayer.Middle, UnityAction<T> callBack = null) where T : UIBasePanel
    {
        //�����Ԥ���崴������Ӧ�������� ���ұ���ԭ�������Ŵ�С
        panelInfo.panel.gameObject.transform.SetParent(GetLayerFather(layer), false);
        //����һ�о��� ���Ҳ���ˣ���ֱ�Ӳ�����
        //���Ҫ��ʾ��� ��ִ��һ������Ĭ����ʾ�߼�
        panelInfo.panel.ShowMe();
        //������ڻص� ֱ�ӷ��س�ȥ����
        callBack?.Invoke(panelInfo.panel);
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    public void HidePanel<T>() where T : UIBasePanel
    {
        if (!panelDic.ContainsKey(typeof(T).Name))
        {
            Debug.LogWarning($"����{typeof(T).Name}���ʧ�ܣ������ڴ����");
            return;
        }
        //��ȡ
        PanelInfo<T> panelInfo = panelDic[typeof(T).Name] as PanelInfo<T>;

        //����
        panelInfo.panel.HideMe();
        panelInfo.panel.gameObject.SetActive(false);
    }
    /// <summary>
    /// ������壬���Ǽ��ص���Դ�����ڴ��ֻ����ֻ�ǻ����ټ�����
    /// </summary>
    public void DestroyPanel<T>() where T : UIBasePanel
    {
        if (!panelDic.ContainsKey(typeof(T).Name))
        {
            Debug.LogWarning($"����{typeof(T).Name}���ʧ�ܣ������ڴ����");
            return;
        }
        //��ȡ
        PanelInfo<T> panelInfo = panelDic[typeof(T).Name] as PanelInfo<T>;

        //�������
        Object.Destroy(panelInfo.panel.gameObject);
        //���������Ƴ�
        panelDic.Remove(typeof(T).Name);
    }

    /// <summary>
    /// ��һ�β������ʱ�����û�б����أ�����Ҫ�ȼ���
    /// </summary>
    //public LoadTask LoadPanel<T>(bool isSync = false) where T : UIBasePanel
    //{
    //    //���ط�null��֤
    //    string panelName = typeof(T).Name;
    //    //���������
    //    if (!panelDic.ContainsKey(panelName))
    //        panelDic.Add(panelName, new PanelInfo<T>());

    //    //ȡ���ֵ����Ѿ�ռ��λ�õ�����
    //    PanelInfo<T> panelInfo = panelDic[panelName] as PanelInfo<T>;

    //    //���سɹ���ص�
    //    UnityAction<GameObject> CreateCallback = (obj) =>
    //    {
    //        //ʵ����
    //        GameObject panelObj = Object.Instantiate(obj);
    //        Object.DontDestroyOnLoad(panelObj);
    //        T panel = panelObj.GetComponent<T>();
    //        //ȡ���ֵ����Ѿ�ռ��λ�õ�����
    //        PanelInfo<T> panelInfo = panelDic[panelName] as PanelInfo<T>;
    //        //�洢panel
    //        panelInfo.panel = panel;
    //    };
    //    //ѡ���Ƿ�ͬ�������첽����
    //    LoadTask task = null;
    //    if (!isSync)
    //    {

    //        //ʵսʱ�򣬴���һ�����أ�Ҫ��������һ��ֻ���ص�������ɵ�Ƶ�Ŷ
    //        //ʵսʱ�򣬴���һ�����أ�Ҫ��������һ��ֻ���ص�������ɵ�Ƶ�Ŷ
    //        //ʵսʱ�򣬴���һ�����أ�Ҫ��������һ��ֻ���ص�������ɵ�Ƶ�Ŷ

    //        //task = ResLoader.Instance.LoadAB_Async<GameObject>(FrameworkSetting.Instance.data.UIPrefabPackName, panelName, CreateCallback);
    //    }
    //    else
    //    {
    //       // task = ResLoader.Instance.LoadAB_Sync<GameObject>(FrameworkSetting.Instance.data.UIPrefabPackName, panelName);
    //        CreateCallback.Invoke((task.ResInfo as Res<GameObject>).asset);
    //    }
    //    //�Զ���ʼ����
    //    task.StartLoadTask();
    //    return task;
    //}

    /// <summary>
    /// ��ȡ���
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public PanelInfo<T> GetPanel<T>(bool isSync = false) where T : UIBasePanel
    {
        string panelName = typeof(T).Name;
        //���������
        if (!panelDic.ContainsKey(panelName))
        {   //��������� �ȴ����ֵ䵱�� ռ��λ�� ֮���������ʾ �Ҳ��ܵõ��ֵ��е���Ϣ�����ж�
            panelDic.Add(panelName, new PanelInfo<T>());
        }

        //ȡ���ֵ����Ѿ�ռ��λ�õ�����
        PanelInfo<T> panelInfo = panelDic[panelName] as PanelInfo<T>;
        //���û���أ��Ǿ��ȼ������
        if (panelInfo.panel == null)
        {
            //panelInfo.PanelLoadTask = LoadPanel<T>(isSync);
        }

        return panelInfo;

    }


    /// <summary>
    /// Ϊ�ؼ�����Զ����¼�
    /// </summary>
    /// <param name="control">��Ӧ�Ŀؼ�</param>
    /// <param name="type">�¼�������</param>
    /// <param name="callBack">��Ӧ�ĺ���</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        //�����߼���Ҫ�����ڱ�֤ �ؼ���ֻ�����һ��EventTrigger
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}
