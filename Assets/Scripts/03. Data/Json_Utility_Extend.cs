using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace HSM.Game
{
    public static class Json_Utility_Extend
    {
        [Serializable]
        private class JsonWrapper<T>
        {
            public List<T> datas;
        }

        /// <summary>
        /// ���� ����
        /// </summary>
        /// <typeparam name="T">Ŭ���� Ÿ��</typeparam>
        /// <param name="data">������</param>
        /// <param name="path">���</param>
        public static void FileSave<T>(T data, string path)
        {
            JsonWrapper<T> wrapper = new JsonWrapper<T>();
            wrapper.datas = new List<T> { data };
            string json = JsonUtility.ToJson(wrapper);
            json = PrettyPrintJson(json);
            if (!path.StartsWith('/')) path = "/" + path;
            File.WriteAllText(Application.dataPath + path, json);
            //AssetDatabase.Refresh();
        }

        /// <summary>
        /// ���� �ҷ�����
        /// </summary>
        /// <typeparam name="T">Ŭ���� Ÿ��</typeparam>
        /// <param name="path">���</param>
        /// <returns>������ ��ȯ</returns>
        public static T FileLoad<T>(string path)
        {
            if (!path.StartsWith('/')) path = "/" + path;
            string json = File.ReadAllText(Application.dataPath + path);
            JsonWrapper<T> wrapper = JsonUtility.FromJson<JsonWrapper<T>>(json);
            return wrapper.datas[0];
        }

        /// <summary>
        /// ����Ʈ Ÿ�� ����
        /// </summary>
        /// <typeparam name="T">Ŭ���� Ÿ��</typeparam>
        /// <param name="datas">������ ����Ʈ</param>
        /// <param name="path">���</param>
        public static void FileSaveList<T>(List<T> datas, string path)
        {
            JsonWrapper<T> wrapper = new JsonWrapper<T>();
            wrapper.datas = datas;
            string json = JsonUtility.ToJson(wrapper);
            json = PrettyPrintJson(json);
            if (!path.StartsWith('/')) path = "/" + path;
            File.WriteAllText(Application.dataPath + path, json);
            //AssetDatabase.Refresh();
        }

        /// <summary>
        /// ����Ʈ Ÿ�� �ҷ�����
        /// </summary>
        /// <typeparam name="T">Ŭ���� Ÿ��</typeparam>
        /// <param name="path">���</param>
        /// <returns>������ ����Ʈ ��ȯ</returns>
        public static List<T> FileLoadList<T>(string path)
        {
            if (!path.StartsWith('/')) path = "/" + path;
            string json = File.ReadAllText(Application.dataPath + path);
            JsonWrapper<T> wrapper = JsonUtility.FromJson<JsonWrapper<T>>(json);
            return wrapper.datas;
        }

        /// <summary>
        /// Json ������
        /// </summary>
        /// <param name="json">json ������ �ؽ�Ʈ</param>
        /// <returns>������ json �ؽ�Ʈ</returns>
        private static string PrettyPrintJson(string json)
        {
            const int indentSpaces = 4;
            int indent = 0;
            bool quoted = false;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < json.Length; i++)
            {
                char ch = json[i];

                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append(new string(' ', ++indent * indentSpaces));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append(new string(' ', --indent * indentSpaces));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        int index = i;
                        while (index > 0 && json[--index] == '\\')
                        {
                            escaped = !escaped;
                        }
                        if (!escaped)
                        {
                            quoted = !quoted;
                        }
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            sb.Append(new string(' ', indent * indentSpaces));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.Append(" ");
                        }
                        break;
                    default:
                        if (quoted || !char.IsWhiteSpace(ch))
                        {
                            sb.Append(ch);
                        }
                        break;
                }
            }

            return sb.ToString();
        }
    }

}
