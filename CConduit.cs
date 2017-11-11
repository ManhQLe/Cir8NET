using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Cir8NET
{
    public class CConduit : CComp
    {
        bool _ParallelTrx = true;


        List<KeyValuePair<string, IComp>> Contacts = new List<KeyValuePair<string, IComp>>();

        public bool ParallelTrx
        {
            get
            {
                return _ParallelTrx;
            }

            set
            {
                _ParallelTrx = value;
            }
        }

        public object Signal {            
            set {
                this.OnVibrate(null, null, value);
            }
        }

        public override void Connect(IComp Comp, string Contact)
        {
            var idx = Contacts.FindIndex((pair) =>
            {
                return pair.Key == Contact && pair.Value == Comp;
            });
            if (idx < 0)
            {				
                Contacts.Add(new KeyValuePair<string, IComp>(Contact, Comp));
                Comp.Connect(this, Contact);
            }
        }

        public override void DisconnectWith(IComp Comp, string Contact)
        {
            var idx = Contacts.FindIndex((pair) =>
            {
                return pair.Key == Contact && pair.Value == Comp;
            });
            if (idx >= 0)
                Contacts.RemoveAt(idx);
        }

        public override void OnVibrate(IComp Comp, string Contact, object Val)
        {
            foreach (var pair in Contacts) {
                if (pair.Value != Comp && pair.Key != Contact) {
                    if (ParallelTrx)
                    {
						new Thread(() => AsyncVirbrate(this, pair, Val)).Start();						
                    }
                    else
                        pair.Value.OnVibrate(this, pair.Key, Val);
                }
            }
        }

        public static void AsyncVirbrate(IComp From, KeyValuePair<String, IComp> Pair, Object Val) {            
            Pair.Value.OnVibrate(From, Pair.Key, Val);
        }
    }
}
