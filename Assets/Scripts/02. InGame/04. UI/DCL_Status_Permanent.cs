using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // DCL_Status_Permanent
    // 캐릭터 영구 능력치 관리 클래스
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class DCL_Status_Permanent
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Status_Permanent] 영구 능력치 레벨업 베이스 클래스
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class Status_Per_Base
        {
            public int Expense;
            public int Level;
            public float AddStatus;
        }
        #endregion

        #region [Status_Permanent] 능력치 리스트
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class Status_Per_List
        {
            public int Att_Power_Now_Level;
            public int Att_Speed_Now_Level;
            public int Cri_Percent_Now_Level;
            public int Cri_Damage_Now_Level;
            public int Move_Speed_Now_Level;
            public int Defense_Now_Level;
            public int HP_Now_Level;
            public int HP_Recovery_Now_Level;
            public int Cln_Speed_Now_Level;
            public int Gain_Range_Now_Level;

            public List<Status_Per_Base> Att_Power_List = new List<Status_Per_Base>();
            public List<Status_Per_Base> Att_Speed_List = new List<Status_Per_Base>();
            public List<Status_Per_Base> Cri_Percent_List = new List<Status_Per_Base>();
            public List<Status_Per_Base> Cri_Damage_List = new List<Status_Per_Base>();
            public List<Status_Per_Base> Move_Speed_List = new List<Status_Per_Base>();
            public List<Status_Per_Base> Defense_List = new List<Status_Per_Base>();
            public List<Status_Per_Base> HP_List = new List<Status_Per_Base>();
            public List<Status_Per_Base> HP_Recovery_List = new List<Status_Per_Base>();
            public List<Status_Per_Base> Cln_Speed_List = new List<Status_Per_Base>();
            public List<Status_Per_Base> Gain_Range_List = new List<Status_Per_Base>();
        }
        public Status_Per_List Stat_Per_List = new Status_Per_List();
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public DCL_PlayerBase.Player_PermanentData Player_PermanentData;
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------

        public Status_Per_Base Now_Att_Power_Data => Stat_Per_List.Att_Power_List[Stat_Per_List.Att_Power_Now_Level]; // 현재 공격력 데이터
        public Status_Per_Base Now_Att_Speed_Data => Stat_Per_List.Att_Speed_List[Stat_Per_List.Att_Speed_Now_Level]; // 현재 공격속도 데이터
        public Status_Per_Base Now_Cln_Speed_Data => Stat_Per_List.Cln_Speed_List[Stat_Per_List.Cln_Speed_Now_Level]; // 현재 청소속도 데이터
        public Status_Per_Base Now_Cri_Damage_Data => Stat_Per_List.Cri_Damage_List[Stat_Per_List.Cri_Damage_Now_Level]; // 현재 치명타 데미지 데이터
        public Status_Per_Base Now_Cri_Percent_Data => Stat_Per_List.Cri_Percent_List[Stat_Per_List.Cri_Percent_Now_Level]; // 현재 치명타 확률 데이터
        public Status_Per_Base Now_Defense_Data => Stat_Per_List.Defense_List[Stat_Per_List.Defense_Now_Level]; // 현재 방어력 데이터
        public Status_Per_Base Now_Gain_Range_Data => Stat_Per_List.Gain_Range_List[Stat_Per_List.Gain_Range_Now_Level]; // 현재 획득범위 데이터
        public Status_Per_Base Now_HP_Data => Stat_Per_List.HP_List[Stat_Per_List.HP_Now_Level]; // 현재 체력 데이터
        public Status_Per_Base Now_HP_Recovery_Data => Stat_Per_List.HP_Recovery_List[Stat_Per_List.HP_Recovery_Now_Level]; // 현재 체력재생 데이터
        public Status_Per_Base Now_Move_Speed_Data => Stat_Per_List.Move_Speed_List[Stat_Per_List.Move_Speed_Now_Level]; // 현재 체력재생 데이터

        #endregion




        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Initialize_Status_Permanent()
        {
            //for (int i = 1; i < 8; i++)
            //{
            //    Status_Per_Base data = new Status_Per_Base();
            //    data.AddStatus = 1;
            //    data.Expense = 1000 * i;
            //    data.Level = i;

            //    Stat_Per_List.Att_Power_List.Add(data);
            //    Stat_Per_List.Att_Speed_List.Add(data);
            //    Stat_Per_List.Cri_Percent_List.Add(data);
            //    Stat_Per_List.Cri_Damage_List.Add(data);
            //    Stat_Per_List.Move_Speed_List.Add(data);
            //    Stat_Per_List.Defense_List.Add(data);
            //    Stat_Per_List.HP_List.Add(data);
            //    Stat_Per_List.HP_Recovery_List.Add(data);
            //    Stat_Per_List.Cln_Speed_List.Add(data);
            //    Stat_Per_List.Gain_Range_List.Add(data);
            //}

            //Json_Utility_Extend.FileSave(Stat_Per_List, "Data/Json_Data/Stage/PermanentStatus.Json");
            Stat_Per_List = Json_Utility_Extend.FileLoad<Status_Per_List>("Data/Json_Data/Stage/PermanentStatus.Json");
            Player_PermanentData = Json_Utility_Extend.FileLoad<DCL_PlayerBase.Player_PermanentData>("Data/Json_Data/Player/Player_Status_Permanent.Json");
        }
        #endregion

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public string Show_Status_PermanentData(int data, int type/* 1 = level, 2 = */)
        {
            //for (int i = 1; i < 8; i++)
            //{
            //    Status_Per_Base data = new Status_Per_Base();
            //    data.AddStatus = 1;
            //    data.Expense = 1000 * i;
            //    data.Level = i;

            //    Stat_Per_List.Att_Power_List.Add(data);
            //    Stat_Per_List.Att_Speed_List.Add(data);
            //    Stat_Per_List.Cri_Percent_List.Add(data);
            //    Stat_Per_List.Cri_Damage_List.Add(data);
            //    Stat_Per_List.Move_Speed_List.Add(data);
            //    Stat_Per_List.Defense_List.Add(data);
            //    Stat_Per_List.HP_List.Add(data);
            //    Stat_Per_List.HP_Recovery_List.Add(data);
            //    Stat_Per_List.Cln_Speed_List.Add(data);
            //    Stat_Per_List.Gain_Range_List.Add(data);
            //}

            //Json_Utility_Extend.FileSave(Stat_Per_List, "Data/Json_Data/Stage/PermanentStatus.Json");
            //Stat_Per_List = Json_Utility_Extend.FileLoad<Status_Per_List>("Data/Json_Data/Stage/PermanentStatus.Json");
            //Player_PermanentData = Json_Utility_Extend.FileLoad<DCL_PlayerBase.Player_PermanentData>("Data/Json_Data/Player/Player_Status_Permanent.Json");
            return "";
        }
        #endregion
    }

}
