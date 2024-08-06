using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Item_Trash
    // �÷��̾ û���ؾ��� ������- ������ Ŭ����
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_Item_Trash : DCL_ItemBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region[Enum] TrashState
        public enum TrashState { ONFIELD, INTERACTION, END }    // ����, �ʵ�, ��ȣ�ۿ�, ��
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Nested]
        [Serializable]
        public class Trash_Setting
        {
            public TrashState Trash_State;      // ������ ����
            public Collider Inter_Range;        // ��ȣ�ۿ� ����
            public float Inter_Maxtime;         // ��ȣ�ۿ� �ɸ��� �ð�
            public float Inter_DelayTime;       // ������
            public GameObject InGameWorldUI;    // ������
        }
        public Trash_Setting TrashSetting = new Trash_Setting();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        private float Inter_inTime;
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

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            Setting.ItemType = ItemType.TRASH;
            TrashSetting.Trash_State = TrashState.ONFIELD;
            TrashSetting.Inter_Range = transform.GetComponent<Collider>();
            TrashSetting.Inter_Maxtime = 4;
            Inter_inTime = 0;
            TrashSetting.Inter_DelayTime = 1;
        }
        #endregion

        #region [Update]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Update()
        {
            if (TrashSetting.Trash_State == TrashState.END)
                Death();

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. InterAction
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [InterAction]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void InterAction()
        {

        }
        #endregion

        #region [InterAction] TriggerEnter
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.name == "Inter_Range" &&
                TrashSetting.Trash_State == TrashState.ONFIELD)
            {
                TrashSetting.Trash_State = TrashState.INTERACTION;
                Inter_inTime = 0;
            }
        }
        #endregion

        #region [InterAction] TriggerEnter
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnTriggerStay(Collider coll)
        {
            if (coll.gameObject.name == "Inter_Range" && 
                TrashSetting.Trash_State == TrashState.INTERACTION)
            {
                Inter_inTime += Time.deltaTime;

                if(Inter_inTime >= TrashSetting.Inter_Maxtime)
                {
                    TrashSetting.Trash_State = TrashState.END;
                }
            }
        }
        #endregion

        #region [InterAction] TriggerEnter
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnTriggerExit(Collider coll)
        {
            if(coll.gameObject.name == "Inter_Range")
            {
                if (TrashSetting.Trash_State != TrashState.END)
                {
                    TrashSetting.Trash_State = TrashState.ONFIELD;
                }
                else 
                {
                    Death(); 
                }
                Inter_inTime = 0;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. UI
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        //#region [InterAction]
        ////------------------------------------------------------------------------------------------------------------------------------------------------------
        //public override void InterAction()
        //{

        //}
        //#endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Death
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collider] Monster_Death
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Death()
        {
            Window_InGame.Instance.Show_ChoiceList();
            Destroy(gameObject);
        }
        #endregion


    }
}