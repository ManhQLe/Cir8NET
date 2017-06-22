using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cir8NET
{
    public abstract class CComp : IComp
    {

        public CComp() {
            Name = DateTime.Now.ToFileTime().ToString();
        }

        string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }

        public abstract void Connect(IComp Comp, string Contact);

        public abstract void DisconnectWith(IComp Comp, string Contact);

        public abstract void OnVibrate(IComp Comp, string Contact, object Val);
    }
}
