using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��ʱ������ ����洢�˼�ʱ�����������
/// </summary>
public class TimerObj : DataObj
{
    public TimerType timerType;
    public enum TimerType
    {
        ReallyTime, ScaleTime
    }
    /// <summary>
    /// ����Ҫ�ǵ�ʱ��
    /// </summary>
    public float TotalTime;
    /// <summary>
    /// �ܼ�ʱ������ʱ��
    /// </summary>
    private float CurrentTotalTime;
    /// <summary>
    /// ��ȡʣ��ʱ��
    /// </summary>
    public float GetSurplusTime => this.TotalTime - this.CurrentTotalTime;
    /// <summary>
    /// ������ڵ�������ʱ��
    /// </summary>
    private float TotalInterCallbackTime;
    /// <summary>
    /// �����������ʱ��
    /// </summary>
    private float CurrentIntervalCallbackTime;
    private event UnityAction Event_IntervalCallback;
    private event UnityAction Event_FinishCallback;
    private WaitForSeconds waitSeconds;
    private WaitForSecondsRealtime waitRealtimeSeconds;

    /// <summary>
    /// ��ʼ����ʱ��
    /// </summary>
    public void IntiTimer(TimerType timerType, float TotalTime, UnityAction callback)
    {
        this.timerType = timerType;
        this.TotalTime = TotalTime;
        this.Event_FinishCallback = callback;
        switch (timerType)
        {
            case TimerType.ReallyTime:
                waitRealtimeSeconds = new WaitForSecondsRealtime(TimerMgr.Instance.UpdateIntervalTime);
                break;
            case TimerType.ScaleTime:
                waitSeconds = new WaitForSeconds(TimerMgr.Instance.UpdateIntervalTime);
                break;
        }
    }



    /// <summary>
    /// ������ʱ������ˢ�����м�¼
    /// </summary>
    /// <returns></returns>
    public void ReStart(float TotalTime)
    {
        //�д���ɣ�����
        TimerMgr.Instance.StartTimer(this.ID);
    }
    public void Stop()
    {
       TimerMgr.Instance.StopTimer(this.ID);
    }
    /// <summary>
    /// ���ټ�ʱ������
    /// </summary>
    public void Destroy()
    {
        ClearTimer();
        TimerMgr.Instance.DestroyTimer(this);
    }
    

    public TimerObj SetIntervalCallback(float totalIntervalTime, params UnityAction[] action)
    {
        this.TotalInterCallbackTime = totalIntervalTime;
        AddIntervalCallback(action);
        return this;
    }
    public TimerObj AddIntervalCallback(params UnityAction[] action)
    {
        for (int i = 0; i < action.Length; i++)
            Event_IntervalCallback += action[i];
        return this;
    }
    public TimerObj AddFinishCallback(params UnityAction[] action)
    {
        for (int i = 0; i < action.Length; i++)
            Event_FinishCallback += action[i];
        return this;
    }
    //�����ʱ������
    public void ClearTimer()
    {
        TotalTime = -1;
        CurrentTotalTime = 0;
        CurrentIntervalCallbackTime = 0;
        Event_FinishCallback = null;
        Event_IntervalCallback = null;
    }
    /// <summary>
    /// �����Լ�ʱ��
    /// </summary>
    public IEnumerator UpdateTime()
    {
        while (true)
        {
    
            switch (timerType)
            {
                case TimerType.ReallyTime:
                    yield return waitRealtimeSeconds;
                    break;
                case TimerType.ScaleTime:
                    yield return waitSeconds;
                    break;
            }
            CurrentTotalTime += TimerMgr.Instance.UpdateIntervalTime;
            CurrentIntervalCallbackTime += TimerMgr.Instance.UpdateIntervalTime;
            //�����������
            if (CurrentIntervalCallbackTime >= TotalInterCallbackTime)
            {

                CurrentIntervalCallbackTime = 0;
                Event_IntervalCallback?.Invoke();
            }
            //����ʱ���� ֹͣ��ʱ��
            if (CurrentTotalTime >= TotalTime)
            {
                Event_FinishCallback?.Invoke();
                Destroy();
            }

        }
    }

}
