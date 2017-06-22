using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cir8NET
{
    public interface IComp
    {
        string Name { get; set; }

        void Connect(IComp Comp, String Contact);
        void DisconnectWith(IComp Comp, String Contact);
        void OnVibrate(IComp Comp, String Contact, object Val);
    }
}
