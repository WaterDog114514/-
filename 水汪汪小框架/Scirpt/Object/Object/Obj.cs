using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//为了给框架更加方便的调用，开发一套关联Object的伪Obj来使用
/// <summary>
/// 所有物体基类
/// </summary>
public abstract class Obj
{
    /// <summary>
    /// 所属对象池标识，数据对象通过Type区分，游戏对象通过名字区分
    /// </summary>
    public abstract string PoolIdentity { get;}
    /// <summary>
    /// 所属池子对象组
    /// </summary>
    public string PoolGroup;
    /// <summary>
    /// 对象的唯一id
    /// </summary>
    public int ID;
    //进出循环池回调
    public UnityAction EnterPoolCallback;
    public UnityAction QuitPoolCallback;
    // 销毁时候回调
    public UnityAction DestroyCallback;

}
