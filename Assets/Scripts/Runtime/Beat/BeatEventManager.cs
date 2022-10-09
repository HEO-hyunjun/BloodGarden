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
        // 이벤트 관련
        private static BeatEventManager _instance;

        // 리듬 관련
        public float bpm = 60;
        public float hitLength = 0.3f;
        public bool beatOn = false;
        public bool terminateBeat = false;


        // 내부 변수
        List<BeatEvent> beatEvents = new List<BeatEvent> ();
        float hitLengthHalf;
        float interval;

        // 인스턴스에 접근하기 위한 프로퍼티
        public static BeatEventManager Instance
        {
            get
            {
                // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
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
            // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
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

