using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace UINotIncluded.Widget
{
    class BlankSpace_Worker : WidgetWorker
    {
        private bool _fixedWidth = false;
        private float _width = 10f;

        public override float GetWidth() => _fixedWidth ? _width : -1f;
        public override bool FixedWidth => _fixedWidth;
        public override bool WidgetVisible => true;
        public override void OnGUI(Rect rect)
        { }
    }
}
