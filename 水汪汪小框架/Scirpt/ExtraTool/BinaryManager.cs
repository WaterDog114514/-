using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryManager : Singleton_UnMono<BinaryManager>
{

    /// <summary>
    /// �洢���������
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fileName"></param>
    public void Save(object obj, string fileName, string path = null)
    {
        if (path == null)
            path = Application.persistentDataPath + "/";
        //���ж�·���ļ�����û��
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        using (FileStream fs = new FileStream(path + "/" + fileName, FileMode.OpenOrCreate, FileAccess.Write))
        {

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
            fs.Close();
        }
    }

    /// <summary>
    /// ��ȡ2��������ת���ɶ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T Load<T>( string path) where T : class
    {
        //���ж�·���ļ���û��
        if (!File.Exists(path))
        {
            Debug.LogError($"���л�����ʧ�ܣ������ڴ�·���µ��ļ�{path}");
            return default(T);
        }

        T obj = default(T);

        using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs) as T;
            fs.Close();
        }
        return obj;
    }
    /// <summary>
    /// ֱ�Ӵ�persistent�����ļ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T LoadFromName<T>(string fileName) where T : class
    {
        string path = Application.persistentDataPath + "/" + fileName;
        return Load<T>(path);   
    }
}
