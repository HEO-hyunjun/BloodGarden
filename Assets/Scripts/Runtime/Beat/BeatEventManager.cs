using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Beat
{
    public class BeatEvent
	{
        Action beatEventAction;
        float offset;

        
		public float Offset { get => offset; set => offset = value; }
		public Action BeatEventAction { get => beatEventAction; set => beatEventAction = value; }
	}

    public class BeatEventManager: MonoBehaviour
    {
        // �̺�Ʈ ����
        private static BeatEventManager _instance;

        // ���� ����
        public float bpm = 60;
        public float hitLength = 0.3f;
        public bool beatOn = false;
        public bool terminateBeat = false;


        // ���� ����
        List<BeatEvent> beatEvents = new List<BeatEvent> ();
        float hitLengthHalf;
        float interval;

        // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
        public static BeatEventManager Instance
        {
            get
            {
                // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(BeatEventManager)) as BeatEventManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
            DontDestroyOnLoad(gameObject);

            interval = 60f / bpm - hitLength;
            hitLengthHalf = hitLength / 2f;
        }

        private void Start()
        {
            StartCoroutine(CallBeat());
        }


        public void RegisterBeatEvent(BeatEvent beatEvent)
		{
            beatEvents.Add(beatEvent);
		}

        IEnumerator CallEvent(float time, Action beatEventAction)
		{
            yield return new WaitForSeconds (time);
            beatEventAction();
		}
        
        IEnumerator CallBeat()
        {
            while(!terminateBeat)
            {
                // Hit Start
                beatOn = true;
                yield return new WaitForSeconds(hitLengthHalf);

                // Hit Beat
                foreach(BeatEvent beatEvent in beatEvents)
				{
                    if (beatEvent.Offset < 0)
					{
                        StartCoroutine(CallEvent(beatEvent.Offset + interval, beatEvent.BeatEventAction));
                    }
                    else if (beatEvent.Offset == 0)
					{
                        beatEvent.BeatEventAction();
					}
                    else
					{
                        StartCoroutine(CallEvent(beatEvent.Offset, beatEvent.BeatEventAction));
					}
				}
                yield return new WaitForSeconds(hitLengthHalf);

                // Hit End
                beatOn = false;
                yield return new WaitForSeconds(interval);
            }
        }
    }
}

