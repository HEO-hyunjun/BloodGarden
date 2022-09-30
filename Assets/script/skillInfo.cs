using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class skillInfo
{

    public float damage = 1;
    public float maintainTime;
    public float skillWidth;
    public float skillHeight;
    public int skillID;


    public skillInfo(int id)
    {
        //db에서 스킬 정보 긁어오기
        string idString = string.Format("{0:D3}", id);
        string fileName = "skillInfo";
        string path = Application.dataPath + "/jsons/" + fileName + idString +".json";

        FileStream fileStream = new FileStream(path, FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string json = Encoding.UTF8.GetString(data);
        skillInfo skill = JsonUtility.FromJson<skillInfo>(json);

        skillID = id;
        damage = skill.damage;
        skillWidth = skill.skillWidth;
        skillHeight = skill.skillHeight;
        maintainTime = skill.maintainTime;

    }
    public float getSkillDistance()
    {
        return skillWidth / 2f;
    }
}
