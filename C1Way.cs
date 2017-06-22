using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cir8NET
{
    public class C1Way : CComp
    {
        Dictionary<string, IComp> Contacts = new Dictionary<string, IComp>();
        public override void Connect(IComp Comp, string Contact)
        {
            if (Contact != "IN" && Contact != "OUT")
                throw new Exception("Only contacts available are IN and OUT");
            var C = Contacts[Contact];
            if (C != null)
            {
                if (C != Comp) C.Connect(Comp, Contact);
            }
            else
            {
                Contacts[Contact] = Comp;
            }
        }

        public override void DisconnectWith(IComp Comp, string Contact)
        {
            if (Contacts.ContainsKey(Contact))
            {
                Contacts.Remove(Contact);
                Comp.DisconnectWith(this, Contact);
            }
        }

        public override void OnVibrate(IComp Comp, string Contact, object Val)
        {
            if (Contact == "IN" && Contacts["IN"] == Comp)
            {
                var C = Contacts["OUT"];
                if (C != null)
                    C.OnVibrate(this, Contact, Val);
            }
        }
    }
}
