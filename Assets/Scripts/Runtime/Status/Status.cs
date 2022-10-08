using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Status
{
	public class Status : MonoBehaviour, IStatus, IDamageable
	{
		// 기본 상태
		[SerializeField]
		private float _maxHp;
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _defence;
		[SerializeField]
		private float _speed;

		[SerializeField]
		private float _nowHp;
		[SerializeField]
		private float _nowDamage;
		[SerializeField]
		private float _nowDefence;
		[SerializeField]
		private float _nowSpeed;

		// 최소 상태
		public float minSpeed;
		public float minDamage;
		public float minDefence;

		// 상태 변화
		private int _ongoingBuffNum;

		public bool isOngoingBuff = false;

		private void Start()
		{
			_nowDamage = _damage;
			_nowDefence = _defence;
			_nowHp = _maxHp;
			_nowSpeed = _speed;
		}

		public void Damaged(float damage)
		{
			_nowHp -= damage;
			if (_nowHp < 0)
				Dead();
		}

		public void Healed(float heal)
		{
			_nowHp += heal;
		}

		IEnumerator OngoingBuff(float speed, float damage, float defence, float time)
		{
			// 상태 변화 기록 추가
			_ongoingBuffNum += 1;
			isOngoingBuff = true;

			// 상태 변화 조건 확인
			float tmpDamage = _nowDamage;
			float tmpDefence = _nowDefence;
			float tmpSpeed = _nowSpeed;

			tmpDamage += damage;
			tmpDefence += defence;
			tmpSpeed += speed;

			if (tmpSpeed < minSpeed)
				tmpSpeed = minSpeed;
			if (tmpDamage < minDamage)
				tmpDamage = minDamage;
			if (tmpDefence < minDefence)
				tmpDefence = minDefence;

			// 상태 변화량 저장
			float returnDamage = _nowDamage - tmpDamage;
			float returnDefence = _nowDefence - tmpDefence;
			float returnSpeed = _nowSpeed - tmpSpeed;

			// 상태 변화
			_nowDamage = tmpDamage;
			_nowDefence = tmpDefence;
			_nowSpeed = tmpSpeed;

			yield return new WaitForSeconds(time);

			// 상태 복구
			_nowSpeed += returnSpeed;
			_nowDamage += returnDamage;
			_nowDefence += returnDefence;

			// 상태 변화 기록 복구
			_ongoingBuffNum -= 1;
			if (_ongoingBuffNum <= 0)
			{
				_ongoingBuffNum = 0;
				isOngoingBuff = false;
			}
			else
			{
				isOngoingBuff = true;
			}
		}

		public void Buff(float speed, float damage, float defence, float time)
		{
			StartCoroutine(OngoingBuff(speed, damage, defence, time));
		}

		public void Dead()
		{
			Debug.Log("[" + gameObject.name + " is Dead!]");
			GameObject.Destroy(this.gameObject);
		}
	}
}


