using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Json
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // Json_Utility
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    public class Json_Utility : MonoBehaviour
    {
        public static Json_Utility instance { get; set; } = null;

        public string filepath;

        private void Awake()
        {
            instance = this;
        }

        public string ObjectToJson(object obj)
        {
            return JsonUtility.ToJson(obj, true);
        }
        public T JsonToOject<T>(string jsonData)
        {
            return JsonUtility.FromJson<T>(jsonData);
        }
        public void CreateJsonFile(string createPath, string fileName, string jsonData)
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }
        public void SaveData(object obj, string filepath, int StageID)
        {
            string jsonData = ObjectToJson(obj);
            CreateJsonFile(Application.dataPath + filepath, "StageID_" + StageID.ToString(), jsonData);
        }

        public T LoadJsonFile<T>(string filepath, string filename)
        {
            FileStream filestream = new FileStream(string.Format("{0}/{1}.json", filepath, filename), FileMode.Open);
            byte[] data = new byte[filestream.Length];
            filestream.Read(data, 0, data.Length);
            filestream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(jsonData);
        }
    }

}
