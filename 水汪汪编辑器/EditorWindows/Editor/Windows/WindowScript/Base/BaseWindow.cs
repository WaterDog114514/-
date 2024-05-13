using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ˮ�����༭���ڻ���  ��չʾ��� ����������ݿ��� �����ݵ���EditorMain�йص�
/// </summary>
public abstract class BaseWindow : EditorWindow
{

    #region ���ں������
    /// <summary>
    /// �Ӵ�������
    /// </summary>
    public Type WinType;
    /// <summary>
    /// ��������
    /// </summary>
    public Type MainType;
    [SerializeField]

    /// <summary>
    /// ÿ���༭�����ڵĺ��� 
    /// </summary>
    public EditorMain editorMain;
    /// <summary>
    /// ȡ������ ��Ҫ�Լ�as������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public EditorMain p_GetMainValue()
    {
        if (editorMain == null) { 
            editorMain = Activator.CreateInstance(MainType) as EditorMain;
            editorMain.SelfWindow = this;
        }
        return editorMain;
    }
    #endregion
    #region �������� ��ʾ���

    /// <summary>
    /// ������
    /// </summary>
    protected string Title;
    /// <summary>
    /// ͼ��
    /// </summary>
    protected Texture Icon;
    /// <summary>
    /// ����ͼƬ
    /// </summary>
    private Texture textrue;
    /// <summary>
    /// �Ƿ�ʹ�ñ�����ɫ
    /// </summary>
    public bool isUseBlackground;
    /// <summary>
    /// ��ǰ���ڴ�С
    /// </summary>
    private Vector2 currentWindowSize;
    /// <summary>
    /// ��ȡ���ڿ�
    /// </summary>
    [SerializeField]
    public float WindowWidth => position.width;
    /// <summary>
    /// ��ȡ���ڸ�
    /// </summary>
    [SerializeField]
    public float WindowHeight => position.height;
    private Vector2 _originSize = Vector2.zero;
    /// <summary>
    /// ��ʼ���ڴ�С ��Ҫ��OnEnableǰ����
    /// </summary>
    protected Vector2 OriginWindowSize
    {
        get
        {
            if(_originSize == Vector2.zero)
                return new Vector2( EM_WinSetting.Instance.WindowsBackground.width/2,EM_WinSetting.Instance.WindowsBackground.height / 2);
            return _originSize;
        }
        set
        {
            _originSize = value;
        }
    }
    /// <summary>
    /// ��ʼ���༭�����ڵ����� ��������������base�ķ������μ���
    /// </summary>
    /// <param name="Title">���ڱ���</param>
    /// <param name="IconPath">����ͼ��</param>
    public void IntiWindowsSetting(string Title, string IconPath)
    {
        this.Title = Title;
        this.Icon =WindowUtility.LoadAssetFromPath<Texture>(EM_WinSetting.Instance.SettingData.EditorIcon);
        titleContent = new GUIContent(this.Title, this.Icon);
    }


    #endregion

    #region ��������ִ����ط���
    public BaseWindow()
    {

    }

    /// <summary>
    /// ��������ִ�з���
    /// </summary>
    protected virtual void OnEnable()
    {
        //Ԥ�����úô������ͣ������Ժ�ò���
        WinType = GetType();
        //Ԥ�����úô��ں������ͣ������Ժ�ò���
        MainType = this.getMainType();

        textrue = EM_WinSetting.Instance.WindowsBackground;
        //���õ�ǰλ�úͳ�ʼ��С
        position = new Rect(new Vector2(position.x,position.y),OriginWindowSize);
    }



    /// <summary>
    /// �رմ���ִ�з���
    /// </summary>
    protected virtual void OnDestroy()
    {
    }

    /// <summary>
    /// ÿ�λ��ƵĻ����߼�
    /// </summary>
    protected virtual void OnGUI()
    {
        //���Ʊ���ͼ
        currentWindowSize = new Vector2(this.position.width, this.position.height);
        GUI.DrawTexture(new Rect(Vector2.zero, currentWindowSize), textrue, ScaleMode.StretchToFill);
        //�Լ����ƴ����߼�
        m_DrawWindows();

        //���Ʊ��� ���� ������Ϣ�İ�ť
    }

    /// <summary>
    /// �����Ļ����Լ��Ĵ��ڷ���  ���д��ڱ����Լ�ʵ��
    /// </summary>
    protected abstract void m_DrawWindows();


    #endregion



}
