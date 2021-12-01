using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace UINotIncluded.Widget.Configs
{
    public class ElementConfig : IExposable
    {
        public string defName;
        public virtual bool Configurable => false;

        public virtual bool Repeatable => false;

        public virtual WidgetWorker Worker => throw new NotImplementedException();

        public virtual void Reset() { }

        public virtual Def Def
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public virtual void ExposeData()
        {
            Scribe_Values.Look(ref defName,"defname");
        }
    }
}
