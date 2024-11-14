using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_MiniGame_Cleaning
    // �̴ϰ��� - û��
    // - ���ѽð����� ������ ��ǥ�� û���ϱ�
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    public class DCL_MiniGame_Cleaning : DCL_MiniGame_Base
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NMiniGame_CleaningSetting
        {
            public Transform Trash_Root;
            public GameObject Trash_Prefab;
        }
        public NMiniGame_CleaningSetting NMiniGame_CleaningSet = new NMiniGame_CleaningSetting();
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public float InGameTimer;
        public int Item_MaxCount;
        public int Item_NowCount;
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

        #region [Init] Initialize
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Initialize(float time, float GameTime, Vector3 pos)
        {
            Setting.GameStatus = MiniGameStatus.Start;
            Setting.GameType = MiniGameType.Cleaning;
            Setting.EventStartTime = time;
            Setting.GameIngTime = GameTime;
            Setting.EventEndTime = Setting.EventStartTime + Setting.GameIngTime;
            Setting.Explanation = "���ѽð� �ȿ� �����⸦ û���ϼ���!";
            Setting.Position = pos;
            Item_MaxCount = 5;   // ���߿� �ܺη� ���� ���̵� ������ �߰� �ʿ�...
            Item_NowCount = 0;

        }
        #endregion

        #region [Init] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            Setting.MiniGameEnd = false;
            Setting.MiniGameClear = false;
            NMiniGame_CleaningSet.Trash_Root = gameObject.transform;
            ItemSpread(Item_MaxCount, 20);
        }
        #endregion

        #region [Init] ItemSpread
        public void ItemSpread(float count, float range)
        {
            for (int i = 0; i < count; i++)
            {
                // ������ ��ġ ���Ŀ� ���� �ʿ�
                Vector3 pos = UnityEngine.Random.insideUnitSphere * range;
                pos.y = 0;
                GameObject trash = Instantiate(NMiniGame_CleaningSet.Trash_Prefab, pos, Quaternion.identity, NMiniGame_CleaningSet.Trash_Root);
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Update
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Update] Update
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Update()
        {
            if (MiniGameTimer() == true)
            {
                if(Item_NowCount < Item_MaxCount)
                {
                    Setting.MiniGameEnd = true;
                    Setting.MiniGameClear = false;
                }
                else
                {
                    Setting.MiniGameEnd = true;
                    Setting.MiniGameClear = true;
                }
            }
            else
            {
                if(Item_NowCount >= Item_MaxCount)
                {
                    Setting.MiniGameEnd = true;
                    Setting.MiniGameClear = true;
                }
            }
        }
        #endregion

        #region [Update] GameTimer
        public bool MiniGameTimer()
        {
            Setting.GameIngTime -= Time.deltaTime;
            if (Setting.GameIngTime < 0)
                return true;
            else
                return false;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Collider
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collider] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnTriggerExit(Collider other)
        {

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Clear
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Clear] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Clear()
        {
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        #endregion

        #region [Clear] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Destroy()
        {
            Destroy(gameObject);
        }
        #endregion
    }

}
