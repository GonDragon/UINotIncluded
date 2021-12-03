﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace UINotIncluded.Widget
{
    public class WidgetDef : Def
    {
        public override TaggedString LabelCap
        {
            get
            {
                if (this.label == null) this.label = defName;

                return base.LabelCap + " (widget)";
            }
        }

        public Type configType;

        public Configs.ElementConfig GetNewConfig()
        {
            return (Configs.ElementConfig)Activator.CreateInstance(configType);
        }
    }
}
