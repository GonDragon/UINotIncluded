using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

using UINotIncluded.Widget;

namespace UINotIncluded
{
    public sealed class UIManager
    {
        
        private ExtendedToolbar clock;
        public float nextPosition = 5f;
        public float ClockHeight => clock.height;

        private UIManager()
        {
            this.clock = new ExtendedToolbar(0, 0, (float)UI.screenWidth / 4);
        }
        
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
            clock.ExtendedToolbarOnGUI();
        }
        public void MainUIOnGUI()
        {
            
        }

        public void After_MainUIOnGUI()
        {
            this.Clear();
        }
        private void Clear()
        {
            this.nextPosition = 5f;
        }
    }
}
