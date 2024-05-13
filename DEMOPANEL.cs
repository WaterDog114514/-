using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMOPANEL : UIBasePanel
{

    protected override void Awake()
    {
        base.Awake();
        AddListenerButtonClickEvent("EnterBtn", () => {
            Debug.Log("���ˣ�");
        });
        AddListenerChangeSliderEvent("SaoSlider", (value) =>
        {
            Debug.Log("����"+value);
        });        
        AddListenerChangeToggelEvent("LaoNai", (value) =>
        {
            Debug.Log("��������"+value);
        });
    }
}
