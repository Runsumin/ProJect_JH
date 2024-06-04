using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // Item_Trash
    // 
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class PositionChange
    {
        public Vector3 BeforePosition;
        public Vector3 NextPosition;

        public PositionChange(Vector3 before, Vector3 Next) { BeforePosition = before; NextPosition = Next; }
    }


    public class Item_Trash : InterObject_Base
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum eTrashState
        {
            OnField,
            PlayerStack,
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //[Serializable]
        //public class NSetting
        //{

        //}
        //public NSetting Setting = new NSetting();
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        // 
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public eTrashState Item_TrashState;
        public int Index;
        #endregion

        #region [Variable] Position
        public Vector3 NextPosition;
        public Vector3 BeforePosition;
        public Vector3 InitPosition;
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
        public override void Start()
        {
            base.Start();
            ObjectType = InterObject_Base.etype.Item;
            Item_TrashState = eTrashState.OnField;
            InitPosition = transform.localPosition;
        }
        #endregion

        #region [Init] ReSetItemState
        public override void Reset()
        {
            // 아이템 속성 초기화
            ObjectType = InterObject_Base.etype.Item;
            Item_TrashState = eTrashState.OnField;

            // 위치 초기화
            transform.parent = BamGame_LevelBase.Instance.RootSetting.MapRoot;
            transform.position = InitPosition;
        }
        #endregion

        #region [Destroy]
        public override void Destroy()
        {
            base.Destroy();
            Destroy(gameObject);
        }
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Item Collision
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private void OnTriggerEnter(Collider other)
        {
            if (Item_TrashState == eTrashState.OnField && other.gameObject.name == "Player")
            {
                var playerTail = other.gameObject.GetComponent<Player>().PlayerTail;
                var playerRoute = other.gameObject.GetComponent<Player>().PlayerRoute;

                switch (ObjectType)
                {
                    case InterObject_Base.etype.Item:
                        Index = playerTail.Count;
                        playerTail.Add(this);
                        Item_TrashState = eTrashState.PlayerStack;
                        transform.SetParent(other.transform);
                        transform.position = playerRoute[playerRoute.Count - playerTail.Count].BeforePosition;
                        break;
                    case InterObject_Base.etype.Obstacle:
                        break;
                    case InterObject_Base.etype.Portal:
                        break;
                }

            }
        }
    }

}
