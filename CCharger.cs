using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Cir8NET
{
    public class CCharger : CComp
    {
        Dictionary<string, IComp> Contacts = new Dictionary<string, IComp>();
        List<object> Collections;
        int max = 1;
        Semaphore Sem = null;
        public CCharger():base() {
            MAX = 1;
        }

        public int MAX
        {
            get {
                return max;
            }
            set{
                max = value;
                Sem = new Semaphore(max, max);
            }
        }

        public override void Connect(IComp Comp, string Contact)
        {           
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
            var C = Contacts[Contact];
            if (C == Comp){
                Sem.WaitOne();
                Collections.Add(Val);
                if (Collections.Count == MAX) {
                    var Col = Collections;
                    foreach (var c in Contacts.Keys) {
                        Contacts[c].OnVibrate(this, c, Col);
                    }
                }
                Sem.Release();
            }            
        }
    }
}
