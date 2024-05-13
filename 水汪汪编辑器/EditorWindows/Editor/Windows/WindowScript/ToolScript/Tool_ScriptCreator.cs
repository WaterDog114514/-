using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �ű�������  Ϊ��ÿ��ģ�鶼��ʹ�ã����ɽű�����ʹ�ô���
/// </summary>
public static class Tool_ScriptCreator
{
    /// <summary>
    /// �ű���Ϣ
    /// </summary>
    public static ScriptInfo info;

    /// <summary>
    /// ���ɽű�
    /// </summary>
    /// <param name="path">����·��</param>
    /// <param name="ClassName">����</param>
    /// <param name="InheritName">�̳���</param>
    /// <param name="usingInfo">������Ϣ</param>
    /// <param name="Members">��Ա������</param>
    /// <param name="Methods">������</param>
    public static void m_CreateScript(string path, string ClassName, string InheritName = null, string usingInfo = null, List<ScriptFieldInfo> Members = null, List<ScriptMethodInfo> Methods = null)
    {
        //��ֵ
        info = new ScriptInfo();
        info.UsingInfo = usingInfo;
        info.ClassName = ClassName;
        info.InheritClassName = InheritName;
        info.MemberInfos = Members;
        info.MethodInfos = Methods;
        //  if (Methods != null)
        //  {
        //      info.MethodInfos = new ScriptFieldInfo[Methods.Count];
        //      for (int i = 0; i < Methods.Count; i++)
        //      {
        //          info.MethodInfos[i] = Methods[i];
        //      }
        //  }
        //  if (Members != null)
        //  {
        //      info.MemberInfos = new ScriptFieldInfo[Members.Count];
        //      for (int i = 0; i < Members.Count; i++)
        //      {
        //          info.MethodInfos[i] = Members[i];
        //      }
        //
        //  }
        m_CreateScript(info, path);
    }
    /// <summary>
    ///  ���ɽű�����������
    /// </summary>
    public static void m_CreateScript(ScriptInfo info, string path)
    {
        StringBuilder content = new StringBuilder();

        //������
        if (info.ClassName == null) { 
            Debug.LogError("���ɴ�������Ϊ�գ����飡��ֹͣ����"); 
            return;
        }

        //����������Ϣ
        if(info.UsingInfo=="")
        content.AppendLine(DefaultUsingInfo);
        else content.AppendLine(info.UsingInfo);
        content.Append("\n\n\n");


        //д����
        if (info.InheritClassName == null)
            content.AppendLine("public class " + info.ClassName);
        else
            content.AppendLine("public class " + info.ClassName + " : " + info.InheritClassName);
        //��һ��������
        content.AppendLine("{");

        #region дÿ�пճ�Ա�Ϳշ���
     
        //дÿ�г�Ա�ֶ�
        if (info.MethodInfos != null)
        {
            //ûд������
            for (int i = 0; i < info.MemberInfos.Count; i++)
            {
                if (info.MemberInfos[i].FieldType == "" || info.MemberInfos[i].FieldName == "") { continue; }
                content.AppendLine("\t"+info.MemberInfos[i].GetTypeName() + " " + info.MemberInfos[i].FieldType + " " + info.MemberInfos[i].FieldName + ";");
            }
        }
        content.Append("\n");
        //дÿ�еķ���
        if (info.MethodInfos != null)
        {
            for (int i = 0; i < info.MethodInfos.Count; i++)
            {
                //ûд������
                if (info.MethodInfos[i].FieldType == "" || info.MethodInfos[i].FieldName == "") continue;
                content.AppendLine("\t" + info.MethodInfos[i].GetTypeName() + " " + info.MethodInfos[i].FieldType + " " + info.MethodInfos[i].FieldName + $"({info.MethodInfos[i].ParameterInfo})");
                content.AppendLine("{\n}\n");
            }
        }
        #endregion
        //д��������
        if (info.ExtraContent != null)
        {
            content.AppendLine("\n");
            content.Append(info.ExtraContent);
        }

        //��������
        content.AppendLine("\n\n}");

        //��ʼд��ű�
        if (path == null) path = AssetsPath;
        File.WriteAllText(path + "/" + info.ClassName + ".cs", content.ToString());
        //ˢ��һ�� ���ܿ���
        AssetDatabase.Refresh();
    }




    /// <summary>
    /// �ؼ�����
    /// </summary>
    private const string DefaultUsingInfo = "using UnityEngine;";
    /// <summary>
    /// ����·��
    /// </summary>
    private static string Path;
    //�̶�·��
    public static string AssetsPath = Application.dataPath;
}



#region ���ɴ������Ϣ����
//�����Ǵ��������Ϣ
/// <summary>
/// ���ڱ༭���洢�ֶ���Ϣ
/// </summary>
public class ScriptMemberInfo
{
    /// <summary>
    /// �������η�
    /// </summary>
    public E_AccessModifiers Modifiers;
    public string FieldName;
    public string FieldType; 
    public string GetTypeName()
    {
        switch (Modifiers)
        {
            case E_AccessModifiers.Public:
                return "public";
            case E_AccessModifiers.Private:
                return "private";
            case E_AccessModifiers.Protected:
                return "protected";
        }
        return "public";
    }
}

[System.Serializable]
public class ScriptFieldInfo : ScriptMemberInfo
{

    
}
/// <summary>
/// ���ɽű�������Ϣ
/// </summary>
[System.Serializable]

public class ScriptMethodInfo : ScriptMemberInfo
{
    /// <summary>
    /// �����Ĳ�����Ϣ�����Ϊ�ֶοɺ��Դ˰�
    /// </summary>
    public string ParameterInfo;
}
public class ScriptInfo
{/// <summary>
/// �ű�������Ϣ
/// </summary>
    public string UsingInfo;
    public string ClassName;
    public string InheritClassName;
    /// <summary>
    /// ��Ա��Ϣ
    /// </summary>
    public List<ScriptFieldInfo> MemberInfos;
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public List<ScriptMethodInfo> MethodInfos;
    /// <summary>
    /// ��������
    /// </summary>
    public string ExtraContent;
 }

/// <summary>
/// �������η�
/// </summary>
public enum E_AccessModifiers
{
    Public,
    Private,
    Protected
}

#endregion
