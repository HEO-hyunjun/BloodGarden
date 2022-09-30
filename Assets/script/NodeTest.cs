using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTest : MonoBehaviour
{
    skillInfo info;
    float destroyTime = 0;

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
        destroyTime += Time.deltaTime;
        if(info.maintainTime < destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
