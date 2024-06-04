using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // Player
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class Player : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Enum] Player Direction
        public enum eDirection
        {
            FORWARD, BACK, LEFT, RIGHT, Max
        }
        #endregion

        public enum eHexaDirection
        {
            FORWARD_LEFT, FORWARD_RIGHT, LEFT, RIGHT, BACK_LEFT, BACK_RIGHT
        }

        #region [Enum] Player InputSetting
        public enum ePlayerInputState
        {
            Mouse, KeyBoard
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] PlayerSetting
        [Serializable]
        public class NPlayerSetting
        {
            public float Speed;
            public float Delaytime;
        }
        public NPlayerSetting playerSetting = new NPlayerSetting();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] PlayerDirection
        public Vector3 PlayerDirection;
        public float HAxis;
        public float VAxis;
        public Transform Cameratrs;

        public eDirection PlayerDir;        // 플레이어 방향
        public eDirection CameraDir;        // 카메라가 바라보는 방향
        public eHexaDirection CameraDirHex;        // 카메라가 바라보는 방향

        private Vector3 InitPosition;
        private Vector3 fixforward = new Vector3(0, 0, 1);
        private Vector3 PlayerNextMovePosition;
        private Vector3 PlayerBeforeMovePosition;
        #endregion

        #region [Variable] Input Delay
        private float flowTime;
        #endregion

        #region [Variable] PlayerTail
        public List<Item_Trash> PlayerTail = new List<Item_Trash>();
        #endregion

        #region [Variable] PlayerRoot
        public List<PositionChange> PlayerRoute = new List<PositionChange>();
        #endregion

        #region [Variable] PlayerState 플레이어 이동 정지 상태
        public bool PlayerGameEnd = false;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] TileMap Coordinate
        public int NowPlayerTileIndexX => (int)TileMap_StageBase.Instance.WorldToTileX(transform.position.x);
        public int NowPlayerTileIndexZ => (int)TileMap_StageBase.Instance.WorldToTileZ(transform.position.z);
        public Vector3 NowPlayerTilePos => TileMap_StageBase.Instance.WorldToTile(transform.position);
        public TileBase NowPlayerTile => TileMap_StageBase.Instance.GetTileBase(NowPlayerTileIndexX, NowPlayerTileIndexZ);
        #endregion

        #region [Property] HexTileMap Coordinate
        public int NowPlayerHexTileIndexX => TileMap_StageBase.Instance.WorldToHexTileIndex(transform.position).x;
        public int NowPlayerHexTileIndexZ => TileMap_StageBase.Instance.WorldToHexTileIndex(transform.position).z;
        public Vector3 NowPlayerHexTilePos => TileMap_StageBase.Instance.WorldToHexTileIndex(transform.position);
        public TileBase NowPlayerHexTile => TileMap_StageBase.Instance.GetTileBase(NowPlayerHexTileIndexX, NowPlayerHexTileIndexZ);
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Start
        public override void Start()
        {
            base.Start();
            PlayerDir = eDirection.FORWARD;
            PlayerDirection = Vector3.forward;

            var initpos = BamGame_LevelBase.Instance.TransformSetting.StartTransform.position;
            InitPosition = new Vector3(initpos.x, 1, initpos.z);

            transform.position = InitPosition;

            PlayerBeforeMovePosition = transform.position;
            PlayerNextMovePosition = transform.position;

            //
            PositionChange data = new PositionChange(transform.position, transform.position);
            PlayerRoute.Add(data);
        }
        #endregion

        #region [Update]
        void Update()
        {
            if (PlayerGameEnd == false)
            {
                SetPlayerDirectionByCamera();
                Move();                 // 플레이어 이동
                PlayerRouteCheck();     // 경로 저장
            }
        }
        #endregion

        #region [FixedUpdate]
        private void FixedUpdate()
        {
        }

        #endregion

        #region [Init] ResetState
        public override void Reset()
        {
            // 위치 초기화
            transform.position = InitPosition;

            // 방향 초기화
            CameraDirHex = eHexaDirection.FORWARD_LEFT;
            PlayerBeforeMovePosition = transform.position;
            PlayerNextMovePosition = transform.position;

            // 시간 초기화
            flowTime = 0f;

            // 꼬리 정보 초기화
            PlayerTail.Clear();
            PlayerRoute.Clear();
            PositionChange data = new PositionChange(transform.position, transform.position);
            PlayerRoute.Add(data);

            // 플레이어 이동 상태
            PlayerGameEnd = false;
        }
        #endregion      
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Player Input
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [PlayerInput] KeyInput
        private void PlayerInput()
        {
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    PlayerDirection = new Vector3(-transform.forward.z, 0, transform.forward.x);
            //    PlayerDir = eDirection.LEFT;
            //}
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    PlayerDirection = new Vector3(transform.forward.z, 0, -transform.forward.x);
            //    PlayerDir = eDirection.RIGHT;
            //}
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    PlayerDirection = transform.forward;
            //    PlayerDir = eDirection.FORWARD;
            //}
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    PlayerDirection = -transform.forward;
            //    PlayerDir = eDirection.BACK;
            //}

            switch (PlayerDir)
            {
                case eDirection.FORWARD:
                case eDirection.BACK:
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        PlayerDirection = new Vector3(-transform.forward.z, 0, transform.forward.x);
                        PlayerDir = eDirection.LEFT;
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        PlayerDirection = new Vector3(transform.forward.z, 0, -transform.forward.x);
                        PlayerDir = eDirection.RIGHT;
                    }
                    break;
                case eDirection.LEFT:
                case eDirection.RIGHT:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        PlayerDirection = transform.forward;
                        PlayerDir = eDirection.FORWARD;
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        PlayerDirection = -transform.forward;
                        PlayerDir = eDirection.BACK;
                    }
                    break;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Player Direction Setting
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [DirectionSetting] GetCameraDirection
        public Vector3 GetCameraDirection()
        {
            //Debug.Log("현재타일 : " + NowPlayerHexTilePos);

            var CamDirection = transform.position - Cameratrs.transform.position;
            var camdirnor = new Vector3(CamDirection.x, 0, CamDirection.z).normalized;

            return camdirnor;
        }
        #endregion

        #region [DirectionSetting] SetPlayerDirection
        public void SetPlayerDirectionByCamera()
        {
            var CamDir = GetCameraDirection();
            var playerdir = fixforward;
            var playerdirright = Vector3.Cross(Vector3.up, playerdir);

            float angle = Vector3.Angle(CamDir, playerdir);
            float sign = Mathf.Sign(Vector3.Dot(CamDir, playerdirright));
            float finalAngle = sign * angle;

            switch (TileMap_StageBase.Instance.Setting.TileMapType)
            {
                case eTileType.Quad:
                    if (finalAngle > -45 && finalAngle < 45)
                    {
                        CameraDir = eDirection.FORWARD;
                        transform.forward = Vector3.forward;
                    }
                    // Right
                    else if (finalAngle >= 45 && finalAngle < 135)
                    {
                        CameraDir = eDirection.RIGHT;
                        transform.forward = Vector3.right;
                    }
                    // Back
                    else if (finalAngle >= 135 || finalAngle < -135)
                    {
                        CameraDir = eDirection.BACK;
                        transform.forward = Vector3.back;
                    }
                    // Left
                    else if (finalAngle >= -135 && finalAngle <= -45)
                    {
                        CameraDir = eDirection.LEFT;
                        transform.forward = Vector3.left;
                    }
                    break;
                case eTileType.Hexa:
                    if (finalAngle > 0 && finalAngle < 60)
                    {
                        CameraDirHex = eHexaDirection.FORWARD_RIGHT;
                        transform.rotation = Quaternion.Euler(new Vector3(0, 30, 0));
                    }
                    else if (finalAngle >= 60 && finalAngle < 120)
                    {
                        CameraDirHex = eHexaDirection.RIGHT;
                        transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    }
                    else if (finalAngle >= 120 && finalAngle <= 180)
                    {
                        CameraDirHex = eHexaDirection.BACK_RIGHT;
                        transform.forward = Vector3.back;
                        transform.rotation = Quaternion.Euler(new Vector3(0, 150, 0));
                    }
                    else if (finalAngle >= -180 && finalAngle < -120)
                    {
                        CameraDirHex = eHexaDirection.BACK_LEFT;
                        transform.rotation = Quaternion.Euler(new Vector3(0, 210, 0));
                    }
                    else if (finalAngle >= -120 && finalAngle < -60)
                    {
                        CameraDirHex = eHexaDirection.LEFT;
                        transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                    }
                    else if (finalAngle >= -60 && finalAngle <= 0)
                    {
                        CameraDirHex = eHexaDirection.FORWARD_LEFT;
                        transform.rotation = Quaternion.Euler(new Vector3(0, 330, 0));
                    }
                    break;
            }

            // Forward

        }
        #endregion

        #region [DirectionSetting] Player Move
        private void Move()
        {
            flowTime += Time.deltaTime;
            if (flowTime <= playerSetting.Delaytime)
            {
                transform.position = Vector3.Lerp(PlayerBeforeMovePosition, PlayerNextMovePosition, flowTime);
            }
            else
            {
                GameEndCheck();         // 게임 종료 체크
                PlayerBeforeMovePosition = transform.position;
                PlayerNextMovePosition = MoveByHexTile(CameraDirHex);
                PositionChange data = new PositionChange(PlayerBeforeMovePosition, PlayerNextMovePosition);
                PlayerRoute.Add(data);
                flowTime = 0;
            }
        }
        #endregion

        #region [DirectionSetting] PlayerMoveBytile
        private Vector3 MoveByTile(eDirection dir)
        {
            var targetTile = TileMap_StageBase.Instance.GetNeighborTile(NowPlayerTilePos, dir);

            if (targetTile == null)
            {
                PlayerGameEnd = true;
                Window_InGame.Instance.ShowResult(false);
                return new Vector3(0, 0, 0);
            }

            var final = targetTile.gameObject.transform.position;

            return new Vector3(final.x - 2.5f, transform.position.y, final.z + 2.5f);
        }

        private Vector3 MoveByHexTile(eHexaDirection dir)
        {
            var targetTile = TileMap_StageBase.Instance.GetNeighborHexTile(NowPlayerHexTilePos, dir);

            if (targetTile == null && NowPlayerHexTile.TileBaseSetting.TileProperty != TileBase.eTileProperty.End)
            {
                PlayerGameEnd = true;
                Window_InGame.Instance.ShowResult(false);
                //UnityEditor.EditorApplication.isPlaying = false;
                return new Vector3(0, 0, 0);
            }

            var final = targetTile.gameObject.transform.position;

            //return new Vector3(final.x - 2.5f, transform.position.y, final.z + 2.5f);
            return new Vector3(final.x, 1, final.z);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Player Collision
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Player_Collision] TriggerEnter 

        #endregion

        public void GameEndCheck()
        {
            //var window_ingame = WindowManager.Instance.windows.Find(Window_InGame);

            if (PlayerGameEnd)
                return;
            switch (TileMap_StageBase.Instance.Setting.TileMapType)
            {
                case eTileType.Quad:
                    // 장애물
                    if (NowPlayerTile.TileBaseSetting.TileProperty == TileBase.eTileProperty.Obstacle)
                    {
                        PlayerGameEnd = true;
                        Window_InGame.Instance.ShowResult(false);
                    }
                    // 꼬리
                    foreach (var data in PlayerTail)
                    {
                        var tileindex = TileMap_StageBase.Instance.WorldToTile(data.transform.position);
                        var nowtailtile = TileMap_StageBase.Instance.GetTileBase(NowPlayerTileIndexX, NowPlayerTileIndexZ);
                        // 꼬리와 장애물
                        if (nowtailtile.TileBaseSetting.TileProperty == TileBase.eTileProperty.Obstacle)
                        {
                            PlayerGameEnd = true;
                            Window_InGame.Instance.ShowResult(false);
                        }
                        //// 꼬리와 플레이어
                        //if (nowtailtile.TileIndexX == NowPlayerTileIndexX && nowtailtile.TileIndexZ == NowPlayerTileIndexZ)
                        //{
                        //    UnityEditor.EditorApplication.isPlaying = false;
                        //}
                    }
                    break;
                case eTileType.Hexa:
                    // 장애물
                    if (NowPlayerHexTile.TileBaseSetting.TileProperty == TileBase.eTileProperty.Obstacle)
                    {
                        PlayerGameEnd = true;
                        Window_InGame.Instance.ShowResult(false);
                    }
                    else if (NowPlayerHexTile.TileBaseSetting.TileProperty == TileBase.eTileProperty.End)
                    {
                        PlayerGameEnd = true;
                        Window_InGame.Instance.ShowResult(true);
                    }
                    // 꼬리
                    foreach (var data in PlayerTail)
                    {
                        var tileindex = TileMap_StageBase.Instance.WorldToHexTileIndex(data.transform.position);
                        var nowtailtile = TileMap_StageBase.Instance.GetTileBase(tileindex.x, tileindex.z);
                        // 꼬리와 장애물
                        if (nowtailtile.TileBaseSetting.TileProperty == TileBase.eTileProperty.Obstacle)
                        {
                            PlayerGameEnd = true;
                            Window_InGame.Instance.ShowResult(false);
                        }
                        // 꼬리와 플레이어
                        if (nowtailtile.TileIndexX == NowPlayerHexTileIndexX && nowtailtile.TileIndexZ == NowPlayerHexTileIndexZ)
                        {
                            PlayerGameEnd = true;
                            Window_InGame.Instance.ShowResult(false);
                        }
                    }
                    break;
            }

        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Player Root
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [PlayerRoot] PlayerRootCheck
        public void PlayerRouteCheck()
        {
            var cnt = PlayerTail.Count;
            for (int i = 0; i < cnt; i++)
            {
                PlayerTail[i].transform.position = Vector3.Lerp(PlayerRoute[PlayerRoute.Count - 2 - i].BeforePosition,
                    PlayerRoute[PlayerRoute.Count - 2 - i].NextPosition, flowTime);
            }
        }
        #endregion

    }

}

