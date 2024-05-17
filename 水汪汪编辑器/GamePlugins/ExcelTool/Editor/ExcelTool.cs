using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class EM_ExcelTool : Singleton_UnMono<EM_ExcelTool>
{

    private Assembly assembly = null;
    public ExcelToolSettingData SettingInfo;
    /// <summary>
    /// excel�ļ��д�ŵ�·��
    /// </summary>
    private string ExcelDirectory_Path => SettingInfo.ExcelDirectory_Path;
    /// <summary>
    /// �����Ƶ����ļ���·��
    /// </summary>

    public EM_ExcelTool()
    {
        SettingInfo = SettingDataLoader.Instance.LoadData<ExcelToolSettingData>();
        // ��ȡ���򼯵�·��
        string assemblyPath = Path.Combine(Application.dataPath, "../Library/ScriptAssemblies/Assembly-CSharp.dll");
        // ���س���
        assembly = Assembly.LoadFile(assemblyPath);
    }


    /// <summary>
    /// ����ָ���ĵ����ļ�
    /// </summary>
    public void GenerateExcelInfo()
    {
        string path = EditorUtility.OpenFilePanelWithFilters("ѡ��Ҫת���ĵ���Excel�ļ�", SettingInfo.ExcelDirectory_Path, new string[] { "Excel files", "xlsx,xls" });
        if (path == null || path == "") return;

        ReallyGenerateExcelInfo(path);
    }


    /// <summary>
    /// ����Ŀ¼�����е�Excel�ļ�
    /// </summary>
    public void GenerateAllExcelInfo()
    {

        if (File.Exists(ExcelDirectory_Path))
            EditorUtility.DisplayDialog("����ʧ�ܣ�", "���������·���ļ���", "�ð�~");

        //����ָ��·���е�����Excel�ļ� �������ɶ�Ӧ��3���ļ�
        DirectoryInfo dInfo = Directory.CreateDirectory(ExcelDirectory_Path);
        //�õ�ָ��·���е������ļ���Ϣ �൱�ھ��ǵõ����е�Excel��
        FileInfo[] files = dInfo.GetFiles();
        //���ݱ�����

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".xlsx" &&
                files[i].Extension != ".xls")
                continue;
            ReallyGenerateExcelInfo(files[i].FullName);

        }
    }


    //��ʱ�洢λ��
    private string TempGenerateDirectoryPath;
    /// <summary>
    /// ����Excel���Ӧ�����ݽṹ��
    /// </summary>
    /// <param name="table"></param>
    private void GenerateExcelDataClass(DataTable table)
    {
        //�ֶ�����
        DataRow rowName = table.Rows[SettingInfo.propertyNameRowIndex];
        //�ֶ�������
        DataRow rowType = table.Rows[SettingInfo.propertyTypeRowIndex];

        TempGenerateDirectoryPath = SettingInfo.OutPath + "\\" + table.TableName + "\\";
        //�ж�·���Ƿ���� û�еĻ� �ʹ����ļ���
        if (!Directory.Exists(TempGenerateDirectoryPath))
            Directory.CreateDirectory(TempGenerateDirectoryPath);

        //�������Ҫ���ɶ�Ӧ�����ݽṹ��ű� ��ʵ����ͨ����������ַ���ƴ�� Ȼ�����ļ�������
        string str = null;
        //���������ַ���ƴ��
        for (int i = 0; i < table.Columns.Count; i++)
        {
            str += "    public " + rowType[i].ToString() + " " + rowName[i].ToString() + ";\n";
        }
        str = "public class " + table.TableName + "\n{\n" + str + "\n}";
        //����
        str = "[System.Serializable]\n" + str;
        //��ƴ�Ӻõ��ַ����浽ָ���ļ���ȥ
        File.WriteAllText(TempGenerateDirectoryPath + table.TableName + ".cs", str);
        //ˢ��Project����
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// ����Excel���Ӧ������������
    /// </summary>
    /// <param name="table"></param>
    private void GenerateExcelContainer(DataTable table)
    {
        //�õ���������
        int keyIndex = GetKeyIndex(table);
        //�õ��ֶ�������
        DataRow rowType = table.Rows[SettingInfo.propertyTypeRowIndex];

        string str = "using System.Collections.Generic;\n";
        //������
        str += "\n\n[System.Serializable]\n";
        str += "public class " + table.TableName + "Container : DataBaseContainer" + "\n{\n";
        str += "\tpublic Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">";
        str += " dataDic = new " + "Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">();\n";
        str += "}";

        File.WriteAllText(TempGenerateDirectoryPath + table.TableName + "Container.cs", str);


        //ˢ��Project����
        AssetDatabase.Refresh();
        // ����������ɵ��¼�

    }

    //�����������������������
    private void ReallyGenerateExcelInfo(string ExcelPath)
    {
        if (!File.Exists(ExcelPath))
        {
            Debug.LogError("����������ʧ�ܣ�������·����" + ExcelPath);
            return;
        }
        //���ݱ�����
        DataTableCollection tableConllection = null;
        using (FileStream fs = new FileStream(ExcelPath, FileMode.Open, FileAccess.Read))
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            tableConllection = excelReader.AsDataSet().Tables;
            fs.Close();
        }
        //�����ļ��е����б����Ϣ
        foreach (DataTable table in tableConllection)
        {
            //�������ݽṹ��
            GenerateExcelDataClass(table);
            //����������
            GenerateExcelContainer(table);
        }
    }



    /// <summary>
    /// ����excel�����ļ���2��������
    /// </summary>
    /// <param name="table"></param>
    public void GenerateExcelBinary()
    {
        string path = EditorUtility.OpenFilePanelWithFilters("ѡ��Ҫת���ĵ���Excel�ļ�", SettingInfo.ExcelDirectory_Path, new string[] { "Excel files", "xlsx,xls" });
        if (path == null || path == "") return;
       
        ReallyGenerateExcelBinary(path);

    }
    /// <summary>
    /// ����EXCELĿ¼������Excel�ļ���2��������
    /// </summary>
    public void GenerateAllExcelBinary()
    {
        if (File.Exists(ExcelDirectory_Path))
            EditorUtility.DisplayDialog("����ʧ�ܣ�", "���������·���ļ���", "�ð�~");

        //����ָ��·���е�����Excel�ļ� �������ɶ�Ӧ��3���ļ�
        DirectoryInfo dInfo = Directory.CreateDirectory(ExcelDirectory_Path);
        //�õ�ָ��·���е������ļ���Ϣ �൱�ھ��ǵõ����е�Excel��
        FileInfo[] files = dInfo.GetFiles();
        //���ݱ�����

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".xlsx" &&
                files[i].Extension != ".xls")
                continue;
           ReallyGenerateExcelBinary(files[i].FullName);

        }
    }

    //�������ɶ��������ݲ���
    private void ReallyGenerateExcelBinary(string ExcelPath)
    {
        if (!File.Exists(ExcelPath))
        {
            Debug.LogError("�������ļ�ת������ʧ�ܣ�������Excel·����" + ExcelPath);
            return;
        }
        //���ݱ�����
        DataTableCollection tableConllection = null;
        using (FileStream fs = new FileStream(ExcelPath, FileMode.Open, FileAccess.Read))
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
            tableConllection = excelReader.AsDataSet().Tables;
            fs.Close();
        }
        foreach (DataTable table in tableConllection)
        {
            Type ContainerType = null;
            foreach (var type in assembly.GetTypes())
            {
                if (table.TableName + "Container" == type.Name)
                    ContainerType = type;
            }
            //�ж���û�д������
            if (ContainerType == null)
            {
                Debug.Log($"{table.TableName}��δ���������������࣬�޷����ɶ������ļ�");
                continue;
            }
            // ����������ʵ��
            object ContainerInstance = Activator.CreateInstance(ContainerType);

            //Debug.Log(ContainerInstance);
            // ��ȡDictionary�ֶ�
            FieldInfo fieldInfo = ContainerType.GetField("dataDic");
            // ��ȡDictionary������
            Type dictionaryType = fieldInfo.FieldType;

            // ��ȡ����ֵ������
            Type[] typeArguments = dictionaryType.GetGenericArguments();
            Type keyType = typeArguments[0];
            Type valueType = typeArguments[1];
            // ����Dictionaryʵ��
            Type specificDictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            object dictionaryInstance = Activator.CreateInstance(specificDictionaryType);
            // ��ȡAdd����
            MethodInfo addMethod = specificDictionaryType.GetMethod("Add");
            //���д������
            for (int i = SettingInfo.ReallyDataStartRowIndex - 1; i < table.Rows.Count; i++)
            {
                // ��������ֵ��ʵ������ӵ�Dictionary
                object keyValue = ConvertFromString(table.Rows[i][GetKeyIndex(table)].ToString(), keyType);
                object ValueInstance = Activator.CreateInstance(valueType);
                //���������ĳ�Ա�ֶ���
                for (int j = 0; j < valueType.GetFields().Length; j++)
                {
                    FieldInfo field = valueType.GetFields()[j];
                    field.SetValue(ValueInstance, ConvertFromString(table.Rows[i][j].ToString(), field.FieldType));
                }
                // �洢�����ı�����
                addMethod.Invoke(dictionaryInstance, new object[] { keyValue, ValueInstance });
            }
            // ��Dictionaryʵ����ֵ���ֶ�
            fieldInfo.SetValue(ContainerInstance, dictionaryInstance);
            //ֱ��tm���л�
            if (!Directory.Exists(SettingInfo.OutPath + "\\" + table.TableName + "\\"))
            {
                Directory.CreateDirectory(SettingInfo.OutPath + "\\" + table.TableName + "\\");
            }
            BinaryManager.Instance.Save(ContainerInstance, table.TableName + "."+SettingInfo.SuffixName
                , SettingInfo.OutPath + "\\" + table.TableName + "\\");
            //����Ĭ�ϲ���
            Reset();
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// ��ȡ�������ֵ���������������
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private int GetKeyIndex(DataTable table)
    {
        if (KeyIndex != -1) return KeyIndex;
        DataRow row = table.Rows[SettingInfo.keyRowIndex];
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (row[i].ToString() == "key")
            {
                KeyIndex = i;
                return i;
            }
        }
        return 0;
    }
    private int KeyIndex = -1;
    /// <summary>
    /// װ��ת������
    /// </summary>
    /// <param name="value"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    private object ConvertFromString(string value, Type type)
    {

        object obj = null;
        try
        {
            switch (type.Name)
            {
                case "Int32":
                    obj = Convert.ToInt32(value);
                    break;
                case "String":
                    obj = Convert.ToString(value);
                    break;
                case "Single":
                    obj = Convert.ToSingle(value);
                    break;
                case "Double":
                    obj = Convert.ToDouble(value);
                    break;
                case "Boolean":
                    obj = Convert.ToBoolean(value);
                    break;
            }
        }
        catch
        {
            obj = null;
        }
        return obj;
    }
    private void Reset()
    {
        KeyIndex = -1;
    }
}