using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Status
{
	public class HitBox : MonoBehaviour, IAttack
	{
		[SerializeField]
		private string _targetTag;
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _attackInterval;

		private bool _isTargetInside = false;
		private GameObject _target;
		
		IEnumerator Attacking(IStatus target)
		{
			while(_isTargetInside && _target != null)
			{
				target.Attacked(_damage);
				yield return new WaitForSeconds(_attackInterval);
			}
		}

		public void Attack(IStatus target)
		{
			StartCoroutine(Attacking(target));
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag(_targetTag) && _isTargetInside == false)
			{
				_isTargetInside = true;
				_target = collision.gameObject;
				Attack(_target.GetComponent<IStatus>());
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.CompareTag(_targetTag))
			{
				if (collision.gameObject == _target)
				{
					_isTargetInside = false;
				}
				else
				{
					Debug.Log("Multiple Player Object");
				}
				_target = null;
			}
		}
	}
}

