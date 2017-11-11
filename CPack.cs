using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Cir8NET
{
    public delegate void CPackFX(CPack Pack, string Contact = null);
	public class CPack : CComp
	{
		CPort _Ports = new CPort();
		String[] _Ins = null;
		Dictionary<string, bool> _HasInput = new Dictionary<string, bool>();
		int ICount = 0;
		bool _Stage = false;
		CPackFX _FX = RelaxFX;
		Semaphore Sem = new Semaphore(1, 1);
		CPackFX _InitFX = RelaxFX;
		int Initialized = 0;

		public CPack() {			
		}

		object _Props;
		public CPort Ports
		{
			get
			{
				return _Ports;
			}
		}

		public Object Props {
			get {
				return _Props;
			}
			set {
				_Props = value;
			}
		}

		public String[] Ins
		{
			get
			{
				return _Ins;
			}

			set
			{
				_Ins = value;
			}
		}

		public bool Stage
		{
			get
			{
				return _Stage;
			}

			set
			{
				_Stage = value;
				_HasInput.Clear();
				ICount = 0;
			}
		}

		public CPackFX FX
		{
			get
			{
				return _FX;
			}

			set
			{
				_FX = value;
			}
		}

		public CPackFX InitFX
		{
			get
			{
				return _InitFX;
			}

			set
			{
				_InitFX = value;
			}
		}


		public T PortValue<T>(string N)
		{
			return (T)Ports[N];
		}		
		

		public override void Connect(IComp Comp, string Contact)
        {
			if (++Initialized == 1) {
				this.InitFX(this);
			}

            if (Ports.Contacts.ContainsKey(Contact))
            {
                var EComp = Ports.Contacts[Contact];
                if (EComp != Comp)
                    EComp.Connect(Comp, Contact);
            }
            else {                
                Ports.Contacts[Contact] = Comp;
                Comp.Connect(this, Contact);
            }
        }

        public override void DisconnectWith(IComp Comp, string Contact)
        {
            IComp C= Ports.Contacts[Contact];
            if (C != null) {
                IComp V;
                Ports.Contacts.TryRemove(Contact, out V);
                Comp.DisconnectWith(this, Contact);
            }
        }

        public override void OnVibrate(IComp Comp, string Contact, object Val)
        {
            if (Ins==null || Ins.Length == 0)
                this.FX(this);

            var IsInput = Ins.Contains(Contact);
            if (IsInput) {
                bool Already = false;
                if (Stage)
                {
                    Sem.WaitOne();
                    Already = _HasInput.ContainsKey(Contact);
                    _HasInput[Contact] = true;

                    Ports.Values[Contact] = Val;
                    if (!Already && ++ICount == Ins.Length)
                    {
                        this.FX(this,Contact);
                        _HasInput.Clear();
                        ICount = 0;
                    }
                    Sem.Release();
                }
                else
                {
                    Already = _HasInput.ContainsKey(Contact);
                    _HasInput[Contact] = true;
                    Ports.Values[Contact] = Val;
                    if (!Already && ++ICount >= Ins.Length
                        || ICount >= Ins.Length
                    )
                        this.FX(this, Contact);
                }
            }
        }

		public static void RelaxFX(CPack me, String Name) {
			//I'm meditating
		}
    }
}
