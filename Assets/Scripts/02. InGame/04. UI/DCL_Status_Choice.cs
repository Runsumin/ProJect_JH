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
    // 플레이어에게 선택지를 제공하는 경우의 수를 셋업하는 클래스
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

        #region [NestedClass] ChoicePercent&data - 등급 확률 & 등급당 데이터
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
            public string Explain;      //설명
            public string AddStatSort;  // 추가될 스탯 종류
            public float AddStatValue;  // 추가될 스텟 데이터
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
        private List<ChoiceDataSet> ChoiceDataArr = new List<ChoiceDataSet>(); // 선택지 리스트
        public Sprite[] ImageArr;
        public ChoiceDataSet[] ChoiceData_val;   // 1번 선택지
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
            //    string explain = "설명 예시";    
            //    string AddStatSort = "Attack";
            //    float AddStatValue = 1;
            //    ChoiceGrade Grade = ChoiceGrade.BRONZE;
            //    string ImagePath = "섬네일 경로";

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
            // 초이스 데이터 랜덤 섞기
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
                // 에셋 로드 및 결과 처리
                await LoadAssetAsync(key.ImagePath);
            }
        }
        #endregion

        #region [Load Addressable Assets] LoadAssetAsync
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private async Task LoadAssetAsync(string key)
        {
            // 어드레서블 에셋을 비동기적으로 로드하고 완료될 때까지 대기
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(key);
            await handle.Task;

            // 로드 완료 후 에셋 사용
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite loadedObject = handle.Result;
                ImageArr[imageloadindex] = loadedObject;
                imageloadindex++;
                Debug.Log($"{key} 에셋이 로드되었습니다.");
            }
            else
            {
                Debug.LogError($"{key} 에셋을 로드하는 데 실패했습니다.");
            }

            // 사용이 끝난 에셋 해제
            Addressables.Release(handle);
        }
        #endregion



        #region [Load Addressable Assets] AddLoadStart
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private void AddLoadStart(string key)
        {
            // 어드레서블 에셋 로드 시작
            Addressables.LoadAssetAsync<Sprite>(key).Completed += OnAssetLoaded;
        }
        #endregion

        #region [Load Addressable Assets] AddLoadStart
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private void OnAssetLoaded(AsyncOperationHandle<Sprite> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                // 에셋 로드 성공 시, 로드된 에셋을 사용
                Sprite loadedObject = obj.Result;
                ImageArr[imageloadindex] = loadedObject;
                imageloadindex++;

            }
            else
            {
                // 에셋 로드 실패 시, 에러 처리
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
            var result = this.GetType().GetField(name).GetValue(this); // public변수가 아니면 GetField에서 null이 리턴된다
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


