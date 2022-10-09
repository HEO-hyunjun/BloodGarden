using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEvent
{
    public class BeatEventManager: MonoBehaviour
    {
        private static BeatEventManager _instance;
        public float bpm;
        public float length;
        public bool beatOn = false;
        public bool terminateBeat = false;

        float Interval;

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

            Interval = 60f / bpm - length;
        }

        private void Start()
        {
            StartCoroutine(CallBeat());
        }
        
        IEnumerator CallBeat()
        {
            while(!terminateBeat)
            {
                beatOn = true;
                yield return new WaitForSeconds(length);
                beatOn = false;
                yield return new WaitForSeconds(Interval);
            }
        }
    }
}

