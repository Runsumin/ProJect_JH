using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Status_Choice
    // �÷��̾�� �������� �����ϴ� ����� ���� �¾��ϴ� Ŭ����
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_Status_Choice
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum ChoiceGrade { BRONZE, SILVER, GOLD }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class DataSettingArr
        {

        }
        public DataSettingArr DataSettingArray = new DataSettingArr();
        #endregion


        #region [Data Class] Data Set
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class ChoiceDataSet
        {
            public string Explain;      //����
            public string AddStatSort;  // �߰��� ���� ����
            public float AddStatValue;  // �߰��� ���� ������
            public ChoiceGrade Grade;
            public string ImagePath;

            public ChoiceDataSet(string ex, string sort, float val, ChoiceGrade grade, string path)
            {
                this.Explain = ex;
                this.AddStatSort = sort;
                this.AddStatValue = val;
                this.Grade = grade;
                this.ImagePath = path;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        private List<ChoiceDataSet> ChoiceDataArr = new List<ChoiceDataSet>(); // ������ ����Ʈ
        public ChoiceDataSet ChoiceData_val1;   // 1�� ������
        public ChoiceDataSet ChoiceData_val2;   // 2�� ������
        public ChoiceDataSet ChoiceData_val3;   // 3�� ������
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        #endregion




        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] MakeChoiceData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void MakeChoiceData()
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    string explain = "���� ����";    
            //    string AddStatSort = "Attack";
            //    float AddStatValue = 1;
            //    ChoiceGrade Grade = ChoiceGrade.BRONZE;
            //    string ImagePath = "������ ���";

            //    ChoiceDataSet data = new ChoiceDataSet(explain, AddStatSort,AddStatValue,Grade,ImagePath);

            //    ChoiceDataArr.Add(data);
            //}

            //Json_Utility_Extend.FileSaveList(ChoiceDataArr, "Data/Json_Data/Stage/StatusChoice.Json");
            ChoiceDataArr = Json_Utility_Extend.FileLoadList<ChoiceDataSet>("Data/Json_Data/Stage/StatusChoice.Json");

        }
        #endregion

        #region [Init] SetChoiceData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetChoiceData()
        {

        }
        #endregion
    }

}


