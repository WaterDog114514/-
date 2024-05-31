using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DEMORead : MonoBehaviour
{
    public Text text;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        //���ع�����е�������Դ·���µ�Ԥ����
        PrefabLoaderManager.Instance.PreLoadPrefabFrmoExcel<MonsterPZContainer>();
        ResLoader.Instance.StartPreload();
        PrefabLoaderManager.Instance.PreLoadPrefabFrmoExcel<MonsterPZContainer>();

        //   ResLoader.Instance.CreatePreloadTaskFromExcel<MonsterPZContainer>("ArtPath");
        // ResLoader.Instance.CreatePreloadFromExcel<TestInfoContainer>("resPath");
    }


    private void OnGUI()
    {
        if (GUILayout.Button("���Դ���"))
        {
            //PrefabLoaderManager.Instance.DemoTEst();
        }
        if (GUILayout.Button("ţ��"))
        {
            ObjectManager.Instance.GetGameObjFromPool("OO1").SetPosition(Vector3.one * 3);
            ObjectManager.Instance.GetGameObjFromPool("smg1").SetPosition(Vector3.one * -4);
            ObjectManager.Instance.GetGameObjFromPool("sniper2").SetPosition(Vector3.one * 13);
            ObjectManager.Instance.GetGameObjFromPool("OO2").SetPosition(Vector3.one * 5);
        }
    }
    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        int total = (int)time;
        text.text =TextTool.SecondToHMS_Semicolon(total);
    }
}
