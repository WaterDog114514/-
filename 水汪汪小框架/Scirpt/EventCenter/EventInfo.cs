using UnityEngine.Events;

/// <summary>
/// ���������¼���Ϣ�ĸ��� 
/// ����ͨ���������������T������չ����������ܴ�����Ĳ�����
/// </summary>
abstract class BaseEventInfo
{

}
/// <summary>
/// �޲��޷���ֵί����Ϣ��
/// </summary>
class EventInfo : BaseEventInfo
{
    public UnityAction Event;
    public EventInfo(UnityAction _event)
    {
        Event += _event;
    }
}
/// <summary>
/// ��һ��������ί����Ϣ��
/// </summary>
class EventInfo<T> : BaseEventInfo
{
    public EventInfo(UnityAction<T> _event)
    {
        Event += _event;
    }
    public UnityAction<T> Event;
}