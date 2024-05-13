using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//һ������PC��������Ϣ��ÿ��������Ϣ������ÿ���¼�

/// <summary>
/// ������Ϣ
/// </summary>
public abstract class InputInfo
{
    public InputInfo(E_InputEvent Event)
    {
        this.inputEvent = Event;
    }
    protected E_InputEvent inputEvent;
    /// <summary>
    /// ��λ���£������Щ�����Ƿ�����
    /// </summary>
    public abstract void KeyUpdate();
    /// <summary>
    /// ��ֵ����
    /// </summary>
    public abstract void ValueUpdate();
    /// <summary>
    /// ����ȫ�������¼�����
    /// </summary>
    public abstract void TriggerUpdate();
}

public class ButtonKeyInputInfo : InputInfo
{
    public E_KeyInputType inputType;
    public InputKey ButtonKey;

    public ButtonKeyInputInfo(E_InputEvent Event,InputKey Buttonkey , E_KeyInputType inputType) : base(Event)
    {
        this.inputType = inputType;
        this.ButtonKey = Buttonkey;
    }

    public override void KeyUpdate()
    {
        switch (inputType)
        {
            case E_KeyInputType.Down:
                if (ButtonKey.isKeyBoradInput)
                    ButtonKey.isDown = Input.GetKeyDown(ButtonKey.keyCode);
                else
                    ButtonKey.isDown = Input.GetMouseButtonDown(ButtonKey.MouseID);
                break;

            case E_KeyInputType.Up:
                if (ButtonKey.isKeyBoradInput)
                    ButtonKey.isDown = Input.GetKeyUp(ButtonKey.keyCode);
                else
                    ButtonKey.isDown = Input.GetMouseButtonUp(ButtonKey.MouseID);
                break;

            case E_KeyInputType.Always:
                if (ButtonKey.isKeyBoradInput)
                    ButtonKey.isDown = Input.GetKey(ButtonKey.keyCode);
                else
                    ButtonKey.isDown = Input.GetMouseButton(ButtonKey.MouseID);
                break;
        }
    }
    public override void ValueUpdate()
    {

    }
    public override void TriggerUpdate()
    {
        if (ButtonKey.isDown)
            EventCenterManager.Instance.TriggerInputEvent(inputEvent);
    }


}

public class DirectionKeyInputInfo : InputInfo
{
    public InputKey positiveKey;
    public InputKey negativeKey;
    /// <summary>
    /// ���º��ı�ֵ
    /// </summary>
    public float Value;
    /// <summary>
    /// ��ֵ�仯�Ƿ�Ϊ���� ��Ļ�ֻ����-1��1
    /// </summary>
    public bool IsFaded = true;
    public DirectionKeyInputInfo(E_InputEvent Event, InputKey positive, InputKey negative, bool isFaded = true) : base(Event)
    {
        positiveKey = positive;
        negativeKey = negative;
        IsFaded = isFaded;
    }

    public override void KeyUpdate()
    {
        if (positiveKey.isKeyBoradInput)
            positiveKey.isDown = Input.GetKey(positiveKey.keyCode);
        else
            positiveKey.isDown = Input.GetMouseButton(positiveKey.MouseID);

        if (negativeKey.isKeyBoradInput)
            negativeKey.isDown = Input.GetKey(negativeKey.keyCode);
        else
            negativeKey.isDown = Input.GetMouseButton(negativeKey.MouseID);

    }

    public override void ValueUpdate()
    {
        // �������ӻ���ٵ�ֵ
        float changeValue = IsFaded ? 2f * Time.deltaTime : 1;

        // ����������
        if (positiveKey.isDown)
        {
            Value += changeValue;
        }
        // ��⸺�����
        else if (negativeKey.isDown)
        {
            Value -= changeValue;
        }
        // ���û�а������£��𽥽�Value���ٵ�0
        else
        {
            if (Value > 0)
            {
                Value -= changeValue;
                if (Value < 0) Value = 0; // ��ֹValue��Ϊ����
            }
            else if (Value < 0)
            {
                Value += changeValue;
                if (Value > 0) Value = 0; // ��ֹValue��Ϊ����
            }
        }

        // ȷ��Value��-1��1֮��
        Value = Mathf.Clamp(Value, -1, 1);
    }

    public override void TriggerUpdate()
    {
        if (positiveKey.isDown || negativeKey.isDown)
            EventCenterManager.Instance.TriggerInputEvent(inputEvent);
    }
}
/// <summary>
/// ����������룬������ƶ� �����ֻ���
/// </summary>
public class MouseMoveOrScrollInfo : InputInfo
{
    /// <summary>
    /// ���ר����������
    /// </summary>
    public E_MouseInputType inputType;
    /// <summary>
    /// ����ƶ����������ֻ���ֵ
    /// </summary>
    public Vector2 mouseDelta;
    public MouseMoveOrScrollInfo(E_InputEvent Event, E_MouseInputType inputType) : base(Event)
    {
        this.inputType = inputType;
    }
    public bool IsChange;
    public override void KeyUpdate()
    {
        switch (inputType)
        {
            case E_MouseInputType.MouseMove:
                IsChange = Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0 ? false : true;
                break;
            case E_MouseInputType.MouseScroll:
                IsChange = Input.mouseScrollDelta == Vector2.zero ? false : true;
                break;
        }

    }

    public override void ValueUpdate()
    {
        switch (inputType)
        {
            case E_MouseInputType.MouseScroll:
                mouseDelta = Input.mouseScrollDelta;
                break;
            case E_MouseInputType.MouseMove:
                mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                break;
        }
    }

    public override void TriggerUpdate()
    {
        if (IsChange)
            EventCenterManager.Instance.TriggerInputEvent(inputEvent);
    }
}


/// <summary>
/// ���̻���갴�� ���� ̧�� ���ŵļ�λ��Ϣ
/// </summary>
public class InputKey
{
    public InputKey(KeyCode key)
    {
        this.keyCode = key;
        this.MouseID = -1;
    }
    public InputKey(int MouseID)
    {
        this.MouseID = MouseID;
        this.keyCode = KeyCode.None;
    }
    //�������ͼ�������ֻ�ܴ���һ������һ����Ϊnull
    public int MouseID { get; private set; }
    public KeyCode keyCode { get; private set; }
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool isDown;
    public bool isKeyBoradInput => MouseID == -1 && keyCode != KeyCode.None;

}
public enum E_KeyInputType
{
    /// <summary>
    /// ����
    /// </summary>
    Down,
    /// <summary>
    /// ̧��
    /// </summary>
    Up,
    /// <summary>
    /// ����
    /// </summary>
    Always,
}
public enum E_MouseInputType
{
    MouseMove,
    MouseScroll
}
