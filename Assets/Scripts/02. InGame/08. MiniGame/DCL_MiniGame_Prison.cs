using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_MiniGame_Base
    // 미니게임 - 감옥
    // - 제한시간동안 정해진 위치에서 벗어나지 않고 살아남기
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_MiniGame_Prison : DCL_MiniGame_Base
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class NMiniGame_PrisionSetting
        {
            public DecalProjector Decal_RangeEffect;
            public Collider HitCollider;
        }
        public NMiniGame_PrisionSetting NMiniGame_PrisionSet = new NMiniGame_PrisionSetting();
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public float InGameTimer;
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
            Setting.GameType = MiniGameType.Prison;
            Setting.EventStartTime = time;
            Setting.GameIngTime = GameTime;
            Setting.EventEndTime = Setting.EventStartTime + Setting.GameIngTime;
            Setting.Explanation = "제한시간 동안 감옥 안에서 살아남으세요!";
            Setting.Position = pos;
        }
        #endregion

        #region [Init] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            Setting.MiniGameEnd = false;
            Setting.MiniGameClear = false;
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
                Setting.MiniGameEnd = true;
                Setting.MiniGameClear = true;
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
        // 2. Decal
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Decal] ChangeDecalSize
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void ChangeDecalSize(float w, float h)
        {
            NMiniGame_PrisionSet.Decal_RangeEffect.size = new Vector3(w, h, 1);
        }
        #endregion

        #region [Decal] ChangeColliderSize
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void ChangeColliderSize(float r)
        {
            NMiniGame_PrisionSet.HitCollider.GetComponent<SphereCollider>().radius = r;
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
            if (other.gameObject.layer == PLAYERCOLLIDER)
            {
                // 실패
                //Destroy();
                Setting.MiniGameEnd = true;
                Debug.Log("아웃");
            }
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
