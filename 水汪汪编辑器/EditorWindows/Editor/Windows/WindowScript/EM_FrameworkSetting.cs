using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
/// <summary>
/// ���༭���������ù����࣬�����洢���趨�༭�������Ϣ��Ҳ�����������ɱ༭����
/// </summary>
class EM_FrameworkSetting : EditorMain
{
    public static EM_FrameworkSetting Instance
    {
        get
        {
            if (_instance == null)
                _instance = new EM_FrameworkSetting();
            return _instance;
        }
    }

    

    /// <summary>
    /// Ψһ����
    /// </summary>
    private static EM_FrameworkSetting _instance = new EM_FrameworkSetting();
    public Texture WindowsBackground;
    public EM_FrameworkSetting()
    {

    }

    public void m_SaveData()
    {

    }
}