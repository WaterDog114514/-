using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// �����л�������
/// </summary>
public class GameSceneManager : Singleton_UnMono<GameSceneManager>
{

    //ͬ���л������ķ���
    public void LoadScene(E_Scene sceneName, UnityAction callBack = null)
    {
        //�л�����
        SceneManager.LoadScene(sceneName.ToString());
        //���ûص�
        callBack?.Invoke();
        callBack = null;
    }

    //�첽�л������ķ���
    public void LoadSceneAsyn(E_Scene sceneName, UnityAction callBack = null)
    {
        MonoManager.Instance.StartCoroutine(ReallyLoadSceneAsyn(sceneName.ToString(), callBack));
        EventCenterManager.Instance.TriggerGameEvent<E_Scene>(E_GameEvent.LoadScene, sceneName);
    }

    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction callBack)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //��ͣ����Эͬ������ÿ֡����Ƿ���ؽ��� ������ؽ����Ͳ�������ѭ��ÿִ֡����
        //���ؽ��ȵĴ����Ⱥ���Ū
        while (!ao.isDone)
        {
            //���������������¼����� ÿһ֡�����ȷ��͸���Ҫ�õ��ĵط�
            yield return 0;
        }
        //�������һֱ֡�ӽ����� û��ͬ��1��ȥ

        callBack?.Invoke();
    }
}
