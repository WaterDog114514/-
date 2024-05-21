using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��ʱ�������� ��Ҫ���ڿ�����ֹͣ�����õȵȲ����������ʱ��
/// </summary>
public class TimerMgr : Singleton_UnMono<TimerMgr>
{
    public TimerMgr()
    {
        IntiMgr();
    }
    //��ʱ����
    private Dictionary<int, TimerObj> dic_TimerObj = new Dictionary<int, TimerObj>();
    public float UpdateIntervalTime;
    public TimerObj StartNewTimer(TimerObj.TimerType type, float totaltime, UnityAction Callback = null)
    {
        //��ȡһ��
        TimerObj timerObj = ObjectManager.Instance.getDataObjFromPool<TimerObj>();
        timerObj.IntiTimer(type, totaltime, Callback);
        //�����ֵ�
        dic_TimerObj.Add(timerObj.ID, timerObj);
        StartTimer(timerObj.ID);
        return timerObj;
    }

    //��ʼ��������
    public void IntiMgr()
    {
        UpdateIntervalTime = 0.1f;
        waitSeconds = new WaitForSeconds(UpdateIntervalTime);
        waitSecondsRealtime = new WaitForSecondsRealtime(UpdateIntervalTime);
    }

    public WaitForSeconds waitSeconds;
    public WaitForSecondsRealtime waitSecondsRealtime;
    //�Ƴ���ʱ��
    public void DestroyTimer(TimerObj obj)
    {
        StopTimer(obj.ID);
        ObjectManager.Instance.DestroyObj(obj);
        dic_TimerObj.Remove(obj.ID);
    }

    //������ͣ��ǰ��ʱ����ʱ�����߼�
    public void StartOrPauseAllTimer(bool IsStart)
    {
        if (IsStart)
            foreach (var id in dic_TimerObj.Keys)
                StopTimer(id);
        else
            foreach (var id in dic_TimerObj.Keys)
                StartTimer(id);

    }
    //���м�ʱ�������߼�

    public void StartTimer(int id)
    {
        //���������⣬��Ҫ����dic���
        MonoManager.Instance.StartCoroutine(dic_TimerObj[id].UpdateTime());
    }
    public void StopTimer(int id)
    {
        dic_TimerObj[id].Stop();
    }
}

