using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMORead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      MonsterPZContainer container =   BinaryManager.Instance.Load<MonsterPZContainer>("MonsterPZ.pzb", "B:\\UnityProject\\ˮ�����༭������\\Assets\\ˮ�����༭��\\GamePlugins\\ExcelTool\\out\\MonsterPZ\\");
      TestInfoContainer container2 =   BinaryManager.Instance.Load<TestInfoContainer>("TestInfo.pzb", "B:\\UnityProject\\ˮ�����༭������\\Assets\\ˮ�����༭��\\GamePlugins\\ExcelTool\\out\\TestInfo\\");
      TowerInfoContainer container3 =   BinaryManager.Instance.Load<TowerInfoContainer>("TowerInfo.pzb", "B:\\UnityProject\\ˮ�����༭������\\Assets\\ˮ�����༭��\\GamePlugins\\ExcelTool\\out\\TowerInfo\\");
        PlayerInfoContainer container4 =   BinaryManager.Instance.Load<PlayerInfoContainer>("PlayerInfo.pzb", "B:\\UnityProject\\ˮ�����༭������\\Assets\\ˮ�����༭��\\GamePlugins\\ExcelTool\\out\\PlayerInfo\\");
        //   Debug.Log(123);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
