using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Status_Choice
    // �÷��̾�� �������� �����ϴ� ����� ���� �¾��ϴ� Ŭ����
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_Status_Choice : ObjectBase
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

        #region [NestedClass] ChoicePercent&data - ��� Ȯ�� & ��޴� ������
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class ChoicePercentNdata
        {
            public float[] ChoiceGradePercent;
            public DCL_Status Bronze_Status;
            public DCL_Status Silver_Status;
            public DCL_Status Gold_Status;
        }
        public ChoicePercentNdata ChoicePercentNdataSet = new ChoicePercentNdata();
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

        #region [Variable] ChoiceData
        private List<ChoiceDataSet> ChoiceDataArr = new List<ChoiceDataSet>(); // ������ ����Ʈ
        public Sprite[] ImageArr;
        public ChoiceDataSet[] ChoiceData_val;   // 1�� ������
        #endregion

        #region [Variable] 
        private int imageloadindex;
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

            //ChoicePercentNdataSet.ChoiceGradePercent = ChoiceGradePercent;
            //Json_Utility_Extend.FileSave(ChoicePercentNdataSet, "Data/Json_Data/Stage/StatusGrade.Json");

            ChoicePercentNdataSet = Json_Utility_Extend.FileLoad<ChoicePercentNdata>("Data/Json_Data/Stage/StatusGrade.Json");
            ChoiceDataArr = Json_Utility_Extend.FileLoadList<ChoiceDataSet>("Data/Json_Data/Stage/StatusChoice.Json");
            ImageArr = new Sprite[ChoiceDataArr.Count];
            ChoiceData_val = new ChoiceDataSet[3];
            LoadAsset();
        }
        #endregion

        #region [Init] SetChoiceData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public int[] SetChoiceData()
        {
            // ���̽� ������ ���� ����
            int[] data = RandomMaker.MakeRandomNumbers(0, ChoiceDataArr.Count);
            int[] result = new int[3];
            for (int i = 0; i < 3; i++)
            {
                ChoiceData_val[i] = ChoiceDataArr[data[i]];
                float grade = RandomMaker.Choose(ChoicePercentNdataSet.ChoiceGradePercent);
                ChoiceGrade egrd = ChoiceGrade.BRONZE;
                switch (grade)
                {
                    case 1:
                        egrd = ChoiceGrade.BRONZE;
                        break;
                    case 2:
                        egrd = ChoiceGrade.SILVER;
                        break;
                    case 3:
                        egrd = ChoiceGrade.GOLD;
                        break;
                }
                ChoiceData_val[i].Grade = egrd;
                result[i] = data[i];
            }

            return result;

        }
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Load Addressable Assets
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Load Addressable Assets] LoadAsset
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        async void LoadAsset()
        {
            await LoadAssetsInList();
        }
        #endregion

        #region [Load Addressable Assets] LoadAssetsInList
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task LoadAssetsInList()
        {
            foreach (ChoiceDataSet key in ChoiceDataArr)
            {
                // ���� �ε� �� ��� ó��
                await LoadAssetAsync(key.ImagePath);
            }
        }
        #endregion

        #region [Load Addressable Assets] LoadAssetAsync
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private async Task LoadAssetAsync(string key)
        {
            // ��巹���� ������ �񵿱������� �ε��ϰ� �Ϸ�� ������ ���
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(key);
            await handle.Task;

            // �ε� �Ϸ� �� ���� ���
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite loadedObject = handle.Result;
                ImageArr[imageloadindex] = loadedObject;
                imageloadindex++;
                Debug.Log($"{key} ������ �ε�Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogError($"{key} ������ �ε��ϴ� �� �����߽��ϴ�.");
            }

            // ����� ���� ���� ����
            Addressables.Release(handle);
        }
        #endregion



        #region [Load Addressable Assets] AddLoadStart
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private void AddLoadStart(string key)
        {
            // ��巹���� ���� �ε� ����
            Addressables.LoadAssetAsync<Sprite>(key).Completed += OnAssetLoaded;
        }
        #endregion

        #region [Load Addressable Assets] AddLoadStart
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private void OnAssetLoaded(AsyncOperationHandle<Sprite> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                // ���� �ε� ���� ��, �ε�� ������ ���
                Sprite loadedObject = obj.Result;
                ImageArr[imageloadindex] = loadedObject;
                imageloadindex++;

            }
            else
            {
                // ���� �ε� ���� ��, ���� ó��
                Debug.LogError("Addressable asset failed to load.");
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Value Control
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Value Control] PrintField
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public object PrintField(string name)
        {
            var result = this.GetType().GetField(name).GetValue(this); // public������ �ƴϸ� GetField���� null�� ���ϵȴ�
            return result;
        }
        #endregion

        #region [Value Control] SetPlayerAddData
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public float SetPlayerAddData(string value, ChoiceGrade grade)
        {
            float result = 0;
            switch (value)
            {
                case "Attack_Power":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.Attack_Power;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.Attack_Power;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.Attack_Power;
                            break;
                    }
                    break;
                case "Attack_Speed":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.Attack_Speed;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.Attack_Speed;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.Attack_Speed;
                            break;
                    }
                    break;
                case "Cri_Percent":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.Cri_Percent;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.Cri_Percent;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.Cri_Percent;
                            break;
                    }
                    break;
                case "Critical_Damage":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.Critical_Damage;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.Critical_Damage;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.Critical_Damage;
                            break;
                    }
                    break;
                case "Move_Speed":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.Move_Speed;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.Move_Speed;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.Move_Speed;
                            break;
                    }
                    break;
                case "Defense":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.Defense;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.Defense;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.Defense;
                            break;
                    }
                    break;
                case "HP":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.HP;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.HP;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.HP;
                            break;
                    }
                    break;
                case "HP_Recovery":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.HP_Recovery;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.HP_Recovery;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.HP_Recovery;
                            break;
                    }
                    break;
                case "Cleaning_Speed":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.Cleaning_Speed;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.Cleaning_Speed;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.Cleaning_Speed;
                            break;
                    }
                    break;
                case "Gain_Range":
                    switch (grade)
                    {
                        case ChoiceGrade.BRONZE:
                            result = ChoicePercentNdataSet.Bronze_Status.Gain_Range;
                            break;
                        case ChoiceGrade.SILVER:
                            result = ChoicePercentNdataSet.Silver_Status.Gain_Range;
                            break;
                        case ChoiceGrade.GOLD:
                            result = ChoicePercentNdataSet.Gold_Status.Gain_Range;
                            break;
                    }
                    break;
            }
            return result;
        }
        #endregion



    }

}


