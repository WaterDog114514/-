using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

 /// <summary>
  /// ��Ҫ������ʽ�滻ԭ�� ���ֵ��� �ø�������װ���������
  /// </summary>
public abstract class UIBasePanelInfo{ }

/// <summary>
/// ���ڴ洢�����Ϣ �ͼ�����ɵĻص�������
/// </summary>
/// <typeparam name="T">��������</typeparam>
public class PanelInfo<T> : UIBasePanelInfo where T : UIBasePanel
{
    public T panel;
    public bool isHide;
    public Res  UIRes;
    public PanelInfo()
    {
     
    }
}

