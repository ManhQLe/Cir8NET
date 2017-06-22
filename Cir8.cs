using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cir8NET
{
    public class Cir8
    {
        public static CConduit Link(IComp Comp1, String C1, String C2, IComp Comp2)
        {
            var Con = new CConduit();
            Con.Connect(Comp1, C1);
            Con.Connect(Comp2, C2);
            return Con;
        }

        public static CConduit MultiLink(params object[] M) {
            var Con = new CConduit();
            if ((M.Length % 2)  > 0) {
                throw new Exception("Component and Port are required");
            }
            for (int i = 0; i < M.Length; i+=2) {
                Con.Connect(M[i] as IComp, M[i + 1] as String);
            }
            return Con;
        }
        public static CConduit Wire(string name) {
            return new CConduit { Name = name };
        }

        public static CConduit Connect(IComp A, IComp B, string Contact) {
            var C = new CConduit();
            C.Connect(A, Contact);
            C.Connect(B, Contact);
            return C;
        }

        public static void Disconnect(IComp A, IComp B, string Contact)
        {
            A.DisconnectWith(B, Contact);
            B.DisconnectWith(A, Contact);
        }
    }
}
