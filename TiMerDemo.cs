using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiMerDemo : MonoBehaviour
{
    TimerObj timerObj;
    TimerObj timerObj2;
    // Start is called before the first frame update
    void Start()
    {
        timerObj = TimerMgr.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, 6, () =>
        {
            print("�����");
        }).SetIntervalCallback(0.15F, () =>
        {
            print("ʣ��ʱ�䣺" + timerObj.GetSurplusTime);
        });
        //int i = 0;
        //timerObj2 = TimerMgr.Instance.StartNewTimer(TimerObj.TimerType.ScaleTime, 3, () => {
        //    print("�����");
        //}).SetIntervalCallback(0.5F, () => {
        //    print("����籩��" + i++);
        //});
    }

    private void OnGUI()
    {
        if (GUILayout.Button("����"))
        {
            TimerMgr.Instance.StopTimer(timerObj.ID);
        }   if (GUILayout.Button("DADAA��"))
        {
            TimerMgr.Instance.StartTimer(timerObj.ID);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
