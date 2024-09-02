using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Status_Choice
    // 플레이어에게 선택지를 제공하는 경우의 수를 셋업하는 클래스
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

            //Json_Utility_Extend.FileSaveList(ChoiceDataArr, "Data/Json_Data/Stage/StatusChoice.Json");
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
            for (int i =0; i < 3; i++)
            {
                ChoiceData_val[i] = ChoiceDataArr[data[i]];
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
    }

}


