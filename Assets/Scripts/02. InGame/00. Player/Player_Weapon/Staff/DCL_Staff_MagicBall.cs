using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//
// DCL_Staff_MagicBall
// 원거리 무기 - 매직볼 클래스
// 
//
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

namespace HSM.Game
{
    public class DCL_Staff_MagicBall : MonoBehaviour
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Enum] MagicBallLevel
        public enum MagicBallType { DIRECTION, TARGET }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] MagicBallSetting
        [Serializable]
        public class NMagicBallSetting
        {
            public MagicBallType MagicBall_Level;
            public Vector3 Direction;
            public Transform Target;
            public GameObject EndPorisionEffect;
        }
        public NMagicBallSetting MagicBallSetting = new NMagicBallSetting();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] PlayerTransform
        public float LimitTime;
        public float ingtm;
        public float MagicBallSpeed;
        public bool StopMoving;
        #endregion

        #region [Variable] Curve Setting
        [Range(0, 1)] private float t = 0;
        Vector3[] point = new Vector3[4];
        public float posA = 0.55f;
        public float posB = 0.45f;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Start
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        void Start()
        {
            ingtm = 0;
            LimitTime = 2;
            MagicBallSpeed = 20;
            if (MagicBallSetting.EndPorisionEffect != null)
                MagicBallSetting.EndPorisionEffect.gameObject.SetActive(false);
            StopMoving = false;
        }
        #endregion


        #region [Update] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        void Update()
        {
            switch (MagicBallSetting.MagicBall_Level)
            {
                case MagicBallType.DIRECTION:
                    MagicBallPattern_Directon();
                    break;
                case MagicBallType.TARGET:
                    MagicBallPattern_Target();
                    break;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Type Setting
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Type Setting] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Set_Direction(Vector3 direction)
        {
            MagicBallSetting.MagicBall_Level = MagicBallType.DIRECTION;
            MagicBallSetting.Direction = direction;
        }
        #endregion

        #region [Type Setting] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Set_Target(Transform target)
        {
            MagicBallSetting.MagicBall_Level = MagicBallType.TARGET;
            MagicBallSetting.Target = target;
            TargetCurveSetting();
        }
        #endregion

        #region [CurveSetting]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void TargetCurveSetting()
        {
            point[0] = transform.position; // P0
            point[1] = PointSetting(transform.position); // P1
            point[2] = PointSetting(MagicBallSetting.Target.position); // P2
            point[3] = MagicBallSetting.Target.position; // P3
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        Vector3 PointSetting(Vector3 origin)
        {
            float x, z;
            x = posA * Mathf.Cos(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad)
            + origin.x;
            z = posB * Mathf.Sin(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad)
            + origin.z;
            return new Vector3(x, origin.y, z);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        private float FourPointBezier(float a, float b, float c, float d)
        {
            return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. MagicBall Pattern
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [MagicBall Pattern]  Directon
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void MagicBallPattern_Directon()
        {
            ingtm += Time.deltaTime;
            if (ingtm > 2)
            {
                Destroy(gameObject);
            }
            transform.Translate(MagicBallSetting.Direction * MagicBallSpeed * Time.deltaTime);

        }
        #endregion

        #region [MagicBall Pattern]  Target
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void MagicBallPattern_Target()
        {
            t += Time.deltaTime;
            if (t > 1)
            {
                StopMoving = true;
                MagicBallEnd();
                if (t > 2)
                {
                    Destroy(gameObject);
                }
            }

            if (StopMoving == false)
            {
                transform.position = new Vector3(
                    FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x),
                    transform.position.y,
                    FourPointBezier(point[0].z, point[1].z, point[2].z, point[3].z));
            }

        }
        #endregion

        #region[MagicBall Pattern] End Pattern
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void MagicBallEnd()
        {
            MagicBallSetting.EndPorisionEffect.gameObject.SetActive(true);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Collision Check
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Collision]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                other.gameObject.GetComponent<DCL_MonsterBase>().Hit(5);
                Destroy(gameObject);
            }
        }
        #endregion


    }

}

