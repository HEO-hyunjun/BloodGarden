using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Beat;

public class TestText : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float offset = 0f;

    public void Start()
    {
        BeatEvent beatEvent = new BeatEvent();
        beatEvent.BeatEventAction = Action;
        beatEvent.Offset = offset;
        BeatEventManager.Instance.RegisterBeatEvent(beatEvent);
    }

    public void Action()
	{
        textUI.text = Time.time.ToString();
	}

    public void Update()
    {
        if (BeatEventManager.Instance.beatOn)
            textUI.enabled = true;
        else
            textUI.enabled = false;
    }
}
