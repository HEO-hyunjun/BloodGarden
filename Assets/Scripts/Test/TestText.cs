using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BeatEvent;

public class TestText : MonoBehaviour
{
    BeatEventManager beatEventManager;
    TextMeshPro text;

    public void Start()
    {
        beatEventManager = BeatEventManager.Instance;
        text = gameObject.GetComponent<TextMeshPro>();
    }

    public void Update()
    {
        if (beatEventManager.beatOn)
            text.enabled = true;
        else
            text.enabled = false;
    }
}
