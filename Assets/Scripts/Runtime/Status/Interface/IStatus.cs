using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Status
{
	public interface IStatus
	{
		public void Attacked(float damage);
		public void Healed(float heal);
		public void Buff(float speed, float damage, float defence, float time);
		public void Dead();
	}
}
