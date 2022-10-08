namespace Status
{
	public interface IStatus
	{
		public void Healed(float heal);
		public void Buff(float speed, float damage, float defence, float time);
		public void Dead();
	}
}
