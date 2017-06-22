using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cir8NET
{
    public class CPort: CComp
    {
        internal ConcurrentDictionary<string, IComp> Contacts = new ConcurrentDictionary<string, IComp>();
        internal ConcurrentDictionary<string, Object> Values = new ConcurrentDictionary<string, object>();

        public Object this[string Name] {
            get {
                return Values[Name];
            }
            set {              
                if (Contacts.ContainsKey(Name))
                {              
                    Contacts[Name].OnVibrate(this, Name, value);
                }
            }
        }

        public override void Connect(IComp Comp, string Contact)
        {
        }

        public override void DisconnectWith(IComp Comp, string Contact)
        {
        }

        public override void OnVibrate(IComp Comp, string Contact, object Val)
        {
            if (Contacts[Contact] == Comp)
                Values[Contact] = Val;
        }
    }
}
