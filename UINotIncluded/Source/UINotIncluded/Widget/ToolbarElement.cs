using RimWorld;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using Verse;

namespace UINotIncluded.Widget
{
    public class ToolbarElement
    {
        public readonly Widget.Configs.ElementConfig config;
        private WidgetWorker _worker;

        public ToolbarElement(Widget.Configs.ElementConfig config)
        {
            this.config = config;
        }

        public bool Configurable => this.config.Configurable;

        public WidgetWorker Worker
        {
            get
            {
                if (_worker == null) _worker = config.Worker;
                return _worker;
            }
        }
    }
}