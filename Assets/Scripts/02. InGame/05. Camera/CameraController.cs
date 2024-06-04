using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // PlayerCameraController
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class CameraController : MonoBehaviour
    {

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public enum eTouchRange
        {
            All, Left, Right, Max,
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // NestedClass
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nesged] CameraSetting
        [Serializable]
        public class NCameraSetting
        {
            // 0, 1.5, 2
            // 12, 180, 0
            public Vector3 OffsetPos;
            public Vector3 OffsetRot;
            public Transform Follow;
            public float MinX;          // 거리 최소값
            public float MaxX;          // 거리 최대값
            public float Distance;      // 카메라 거리  
            public float ZoomSpeed;     // 마우스 휠 줌 속도
            public float MaxDisY;       // Y축 이동 최소값
            public float MinDisY;       // Y축 이동 최대값
            public float CamRotSpeed;   // 카메라 회전 속도
        }
        public NCameraSetting CamSetting = new NCameraSetting();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Variable] 3rd Person Camera
        public eTouchRange TouchRange;
        private Vector3 _mousePosition;
        private Vector3 _mouseMovement;
        private Vector3 _mouseDownPosition;
        private Vector3 _prevMouseDownPosition;
        private float _xRotate;
        private float _yRotate;
        private float finaly;
        #endregion

        #region [Variable] TargetLock Camera
        private Camera CameraCom;
        public Transform target;
        public Vector2 ScreenSize;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Start
        public void Start()
        {
            CameraCom = GetComponentInChildren<Camera>();
        }
        #endregion

        #region [Update]
        void Update()
        {
            ScreenSize = new Vector2(Screen.width, Screen.height);
            _mousePosition = SetMousePositionCenter(Input.mousePosition, ScreenSize);

            // 카메라 Follow
            OrthoTargetLookAtCamera(CamSetting.Follow.position, _mousePosition);
            // Ortho 사이즈 조절
            OrthoSizeControll();
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. 3rd Person Camera
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Mouse] CheckMouseEvent
        private void CheckMouseEvent()
        {
            if ((_mousePosition.x < 0) || (_mousePosition.y < 0) || (_mousePosition.x > Screen.width) || (_mousePosition.y > Screen.height))
            {
                _mouseMovement = Vector3.zero;
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                _mouseDownPosition = _prevMouseDownPosition = _mousePosition;
            }

            _mouseMovement = Vector2.zero;
            {
                _mouseMovement = _mousePosition - _prevMouseDownPosition;
                _prevMouseDownPosition = _mousePosition;
            }

            if (Input.GetMouseButtonUp(1))
            {

            }

        }
        #endregion

        #region [Camera] RotateCamera
        private void RotateCamera()
        {
            if (Input.GetMouseButton(1))
            {
                _xRotate -= _mouseMovement.y * Time.deltaTime * CamSetting.CamRotSpeed;
                _yRotate += _mouseMovement.x * Time.deltaTime * CamSetting.CamRotSpeed;
                _xRotate = Mathf.Clamp(_xRotate, CamSetting.MinX, CamSetting.MaxX);
            }

            var wheelScroll = Input.mouseScrollDelta.y;
            CamSetting.Distance -= (wheelScroll * CamSetting.ZoomSpeed);
            CamSetting.Distance = Mathf.Clamp(CamSetting.Distance, CamSetting.MinX, CamSetting.MaxX);

            var rotation = Quaternion.Euler(_xRotate, _yRotate, 0);
            var position = rotation * (new Vector3(0, 0, -CamSetting.Distance)) + (CamSetting.Follow.position);
            transform.position = position;
            transform.LookAt(CamSetting.Follow.position);

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Target Lock Camera
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Camera] TargetLookAt
        private void OrthoTargetLookAtCamera(Vector3 followpos, Vector3 mousepos)
        {
            Vector3 dis = -mousepos;

            //transform.position = Vector3.Lerp(transform.position, followpos - (dis / 110), Time.deltaTime);
            transform.position = followpos;// - (dis / 100);
        }
        #endregion

        #region [Camera] OrthographicSize Controll
        private void OrthoSizeControll()
        {
            var wheelScroll = Input.mouseScrollDelta.y;
            CamSetting.Distance -= (wheelScroll * CamSetting.ZoomSpeed);
            CamSetting.Distance = Mathf.Clamp(CamSetting.Distance, CamSetting.MinX, CamSetting.MaxX);
            CameraCom.orthographicSize = Mathf.Lerp(CameraCom.orthographicSize, CamSetting.Distance, Time.deltaTime);
        }
        #endregion

        #region [Camera] TargetChange
        public void ChangeLookAtTarget(Transform target)
        {
            //var nowpos = transform.position;
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Utill
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Utill] SetMouseCenterpos
        public Vector3 SetMousePositionCenter(Vector3 mousepos, Vector2 ScreenSize)
        {
            return  new Vector3(mousepos.x - ScreenSize.x / 2, mousepos.y - ScreenSize.y / 2, 0);
        }
        #endregion
    }

}
