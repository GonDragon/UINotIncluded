using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UINotIncluded.Widget
{
    /* Empty worker to load on errors */
    class Erroring_Worker : WidgetWorker
    {
        public override bool WidgetVisible => false;
        public override void OnGUI(Rect rect)
        {}
    }
}
