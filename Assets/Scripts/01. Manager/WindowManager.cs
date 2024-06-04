using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // WindowManager
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class WindowManager : MonoBehaviour
    {
        public static WindowManager Instance;
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // NestedClass
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public Image Fade_Image;
        public Canvas BaseCanvas;
        #endregion

        #region [Variable] Base
        public List<WindowBase> windows = new List<WindowBase>();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] parent
        public Transform CanvasRoot => BaseCanvas.transform;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Awake
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Awake()
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        void Start()
        {

        }
        #endregion

        #region [Init] Generate Window
        public void GenerateWindow(WindowBase window)
        {
            window.InstantiateWindow(window.gameObject, CanvasRoot);
            windows.Add(window);
        }
        #endregion
        #region [Window] Open
        public void OpenWindow(WindowBase window) => window.OpenWindow();
        #endregion

        #region [Window] Close
        public void CloseWindow(WindowBase window, bool destroy = true) => window.CloseWindow(destroy);
        #endregion
    }

}
