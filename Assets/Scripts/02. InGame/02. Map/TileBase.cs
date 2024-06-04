using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // TileMapBase
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    public class TileBase : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum eTileProperty
        {
            None,
            Start,      // 시작 위치
            End,        // 종료 위치
            Road,       // 플레이어 이동 가능 타일
            Obstacle,   // 플레이어 이동 불가능 타일
            Portal,     // 다른 스테이지 진입 타일
            Cliff,      // 낭떠러지
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] BaseSetting
        [Serializable]
        public class NTileBaseSetting
        {
            public float TileWidth;
            public float TileHeight;
            public Vector2 TileIndex;
            public Vector3 TileWorldPos;
            public eTileProperty TileProperty;
            public eTileType TileType;
        }
        public NTileBaseSetting TileBaseSetting = new NTileBaseSetting();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable]
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property]
        public int TileIndexX => (int)TileBaseSetting.TileIndex.x;//.(int)TileMap_StageBase.Instance.WorldToTileX(transform.position.x);
        public int TileIndexZ => (int)TileBaseSetting.TileIndex.y;//TileMap_StageBase.Instance.WorldToTileZ(transform.position.z);
        #endregion


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        #region [Init] Start
        public override void Start()
        {

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Utill
        //  
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Utill] DrawGizmo
        public void OnDrawGizmos()
        {
            var pos = transform.position + new Vector3(0f, -0.5f, 0f);
            switch (TileBaseSetting.TileProperty)
            {
                case eTileProperty.None: Gizmos.color = new Color(1, 1, 1, 0.1f); break;
                case eTileProperty.Start: Gizmos.color = Color.green; break;
                case eTileProperty.End: Gizmos.color = Color.black; break;
                case eTileProperty.Road: Gizmos.color = Color.white; break;
                case eTileProperty.Obstacle: Gizmos.color = Color.cyan; break;
                case eTileProperty.Portal: Gizmos.color = Color.blue; break;
                case eTileProperty.Cliff: Gizmos.color = Color.red; break;
            }

            switch (TileBaseSetting.TileType)
            {
                case eTileType.Quad:
                    if (TileBaseSetting.TileProperty == eTileProperty.None)
                        Gizmos.DrawWireCube(pos, new Vector3(TileBaseSetting.TileWidth, 0, TileBaseSetting.TileHeight));
                    else
                        Gizmos.DrawCube(pos, new Vector3(TileBaseSetting.TileWidth, 0, TileBaseSetting.TileHeight)); break;

                case eTileType.Hexa: break;
                case eTileType.Rhombus:
                    List<Vector3> linelist = new List<Vector3>
                    {
                        new Vector3(pos.x , 0, pos.z + TileBaseSetting.TileHeight / 2),
                        new Vector3(pos.x + TileBaseSetting.TileWidth / 2 , 0,pos.z),
                        new Vector3(pos.x , 0, pos.z - TileBaseSetting.TileHeight / 2),
                        new Vector3(pos.x - TileBaseSetting.TileWidth / 2 , 0,pos.z),
                        new Vector3(pos.x , 0, pos.z + TileBaseSetting.TileHeight / 2),
                    };
                    for (int i = 0; i < linelist.Count - 1; i++)
                    {
                        Gizmos.DrawLine(linelist[i], linelist[i + 1]);
                    }
                    break;
            }
        }
        #endregion
    }
}

