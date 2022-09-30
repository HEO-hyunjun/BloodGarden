using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    public GameObject trg;
    public float speed = 15f;
    // Start is called before the first frame update
    void Start()
    {
        trg = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (trg != null)
        {
            Vector3 trgPos = trg.transform.position;
            trgPos.z = -10f;
            this.transform.position = Vector3.Lerp(this.transform.position, trgPos, speed * Time.deltaTime); ;
        }
    }
}
