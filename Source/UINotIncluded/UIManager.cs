using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using HarmonyLib;

using UINotIncluded.Widget;

namespace UINotIncluded
{
    public sealed class UIManager
    {
        public float ExtendedBarHeight => ExtendedToolbar.height;
        
        private static UIManager instance = null;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }

        public void Before_MainUIOnGUI() 
        {
            float posY = UINotIncludedSettings.tabsOnTop ? 0f : UI.screenHeight - ExtendedToolbar.height;
            ExtendedToolbar.ExtendedToolbarOnGUI(0, posY, (float)UI.screenWidth / 4);
        }
        public void MainUIOnGUI()
        {
            
        }

        public void After_MainUIOnGUI()
        {

        }
    }
}
