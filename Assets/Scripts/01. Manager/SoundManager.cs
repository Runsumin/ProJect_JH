using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // JSoundManager
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Enum
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    #region [Enum]
    public enum eSounds
    {
        ALL,
        BGM1,
        BGM2,
        EFFECT1,
        EFFECT2,
        Max,
    }

    public enum eType
    {
        Scale,
        Mute,
        Max,
    }

    public enum eSet
    {
        Off,
        On,
        Max,
    }
    #endregion

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Variable
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Property
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // 0. Base Methods
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    #region [Init]
    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {

    }
    #endregion
}
