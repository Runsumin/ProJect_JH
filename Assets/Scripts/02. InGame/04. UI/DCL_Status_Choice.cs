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
            // ���̽� ������ ���� ����
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
    }

}

