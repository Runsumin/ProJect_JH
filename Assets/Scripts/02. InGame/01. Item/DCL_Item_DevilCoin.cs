using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Item_DevilCoin
    // 몬스터 & 퀘스트에서 생성되는 플레이어 성장에 도움이 되는 재화.
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    public class DCL_Item_DevilCoin : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NEXPSetting
        {
            public GameObject CoinModel;
            public int Coin_Amount;           // 플레이어에게 부여할 코인 총량
        }
        public NEXPSetting Setting = new NEXPSetting();
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base

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

        #region [Init] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public new void Start()
        {
            base.Start();
        }
        #endregion

        #region [Init] SetEXP
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void SetCoin(int amount)
        {
            Setting.Coin_Amount= amount;

            Setting.CoinModel.SetActive(true);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Collision
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collision] OnTriggerEnter
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnTriggerEnter(Collider coll)
        {
            // Layer = 7 : monstercoll, 8 = MonsterWeaponcoll
            if (coll.gameObject.layer == PLAYERCOLLIDER)
            {
                coll.gameObject.GetComponent<DCL_PlayerBase>().Add_Coin(Setting.Coin_Amount);
                Destroy(gameObject);
            }
        }
        #endregion
    }

}
