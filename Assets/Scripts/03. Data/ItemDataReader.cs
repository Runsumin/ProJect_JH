using UnityEngine;
//using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HSM.Game
{
//    [Serializable]
//    public struct ItemData
//    {
//        public int index;
//        public string parameter;
//        public float data;

//        public ItemData(int id, string parameter, float data)
//        {
//            this.index = id;
//            this.parameter = parameter;
//            this.data = data;
//        }
//    }

//    [CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/ItemDataReader", order = int.MaxValue)]
//    public class ItemDataReader : DataReaderBase
//    {
//        [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")] [SerializeField] public List<ItemData> DataList = new List<ItemData>();

//        internal void UpdateStats(List<GSTU_Cell> list, int itemID)
//        {
//            int id = 0;
//            string name = null;
//            float data = 0;

//            for (int i = 0; i < list.Count; i++)
//            {
//                switch (list[i].columnId)
//                {
//                    case "Index":
//                        {
//                            id = int.Parse(list[i].value);
//                            break;
//                        }
//                    case "Parameter":
//                        {
//                            name = list[i].value;
//                            break;
//                        }
//                    case "lv0":
//                        {
//                            data = float.Parse(list[i].value);
//                            break;
//                        }
//                }
//            }

//            DataList.Add(new ItemData(id, name, data));
//        }
//    }

//#if UNITY_EDITOR
//    [CustomEditor(typeof(ItemDataReader))]
//    public class ItemDataReaderEditor : Editor
//    {
//        ItemDataReader data;

//        void OnEnable()
//        {
//            data = (ItemDataReader)target;
//        }

//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//            GUILayout.Label("\n\n스프레드 시트 읽어오기");

//            if (GUILayout.Button("데이터 읽기(API 호출)"))
//            {
//                UpdateStats(UpdateMethodOne);
//                data.DataList.Clear();
//            }
//        }

//        void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
//        {
//            SpreadsheetManager.Read(new GSTU_Search(data.associatedSheet, data.associatedWorksheet), callback, mergedCells);
//        }

//        void UpdateMethodOne(GstuSpreadSheet ss)
//        {
//            for (int i = data.START_ROW_LENGTH; i <= data.END_ROW_LENGTH; ++i)
//            {
//                data.UpdateStats(ss.rows[i], i);
//            }

//            EditorUtility.SetDirty(target);
//        }
//    }
//#endif

}