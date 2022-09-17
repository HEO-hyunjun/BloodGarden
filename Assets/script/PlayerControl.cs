using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D playerRigid;
    public GameObject skillPrefab;

    public float walkSpeed;

    public float dashTimer = 0;
    public int fullDashCnt = 200;
    public int dashCnt = 0;
    public float dashSpeed = 30;
    public float dashStep = 450;

    public int fullHP = 5;
    public int hp;

    //test
    // Start is called before the first frame update
    void Start()
    {
        hp = 5;
        walkSpeed = 15;
        playerRigid = GetComponent<Rigidbody2D>();
        dashTimer = 0;
        dashCnt = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.tag == "arrow")
        {
            this.hp -= 1;
            gameManager mam = GameObject.Find("GameDirector").GetComponent<gameManager>();
            mam.DecreaseHP(fullHP, this.hp);
        }
        */
    }

    void dash(Vector2 moveDir)
    {
        if (dashCnt != fullDashCnt)
        {
            dashCnt++;
            for(int i=0; i<dashStep; i++)
            {
                playerRigid.AddForce(moveDir*dashSpeed);
            }
        }
        else
            return;
    }

    void skill(int skillID)
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePos = Input.mousePosition;
        mousePos -= screenCenter;
        mousePos = mousePos.normalized;

        GameObject skill = Instantiate(skillPrefab);
        skillControl skillComp = skill.GetComponent<skillControl>();
        skillInfo skillInfo = new skillInfo(skillID);
        skillComp.setSkillID(skillID);
        skill.transform.position = mousePos * skillInfo.getSkillDistance() + playerRigid.position;
        // update끝나면 인스턴스 생성이라 보면 될듯 그래서 여기에서 변수 넘겨주면됨
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer += Time.deltaTime;
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(xAxis* walkSpeed, yAxis* walkSpeed);
        playerRigid.velocity = move;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            dash(move.normalized);
        }

        if (Input.GetMouseButtonDown(0))
        {
            skill(1);
        }

        if (dashTimer >= 8f)
        {
            dashCnt = 0;
            dashTimer = 0;
        }
    }

}
