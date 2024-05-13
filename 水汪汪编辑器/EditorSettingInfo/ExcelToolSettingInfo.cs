using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Excel���������·�������ļ����༭��ģʽר��
public class ExcelToolSettingInfo : BaseSettingData
{

    [JsonIgnore]
    public override string DirectoryPath => Application.dataPath + "\\ˮ�����༭��\\EditorAsset\\PluginsData\\Resources\\";
    [JsonIgnore]
    public override string DataName => "ExcelToolSettingInfo";

    /// <summary>
    /// ������������
    /// </summary>
    public int propertyNameRowIndex =0;
    /// <summary>
    /// ����������������
    /// </summary>
    public int propertyTypeRowIndex =1;
    /// <summary>
    /// key��ǩ������
    /// </summary>
    public int keyRowIndex = 2 ;
    /// <summary>
    /// �������ݿ�ʼ��¼������
    /// </summary>
    public int ReallyDataStartRowIndex = 4;
    public string OutPath;
    public string ExcelDirectory_Path;
    public override void IntiValue()
    {
        //��ʼ��
        ExcelDirectory_Path = Application.dataPath + "\\ˮ�����༭��\\GamePlugins\\ExcelTool\\Excel\\";
        OutPath = Application.dataPath + "\\ˮ�����༭��\\GamePlugins\\ExcelTool\\out\\";
        propertyNameRowIndex = 0;
        propertyTypeRowIndex = 1;   
        keyRowIndex = 2;
        ReallyDataStartRowIndex =4;
    }
}
