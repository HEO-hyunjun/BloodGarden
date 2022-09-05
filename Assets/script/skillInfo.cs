using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillInfo
{
    public float damage = 1;
    public float delta;
    public float skillWidth = 6;
    public float skillHeight = 4;
    public int skillID = 0;

    public skillInfo(int id)
    {
        skillID = id;
        delta = 0;
        //db에서 스킬 정보 긁어오기
    }

    public float getSkillDistance()
    {
        return skillWidth / 2f;
    }
}
