using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillControl : MonoBehaviour
{
    skillInfo info;
    // Start is called before the first frame update

    public void setSkillID(int id)
    {
        info = new skillInfo(id);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(info.delta);
        info.delta += Time.deltaTime;
        if(info.delta>1)
        {
            Destroy(gameObject);
        }
    }
}
