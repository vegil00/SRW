using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battleConfirm : MonoBehaviour {

    // Use this for initialization
    public Robot player;
    public Robot enemy;
    public gameManager GM;
	void Start () {
		
	}

    // Update is called once per frame
    void Update () {
		
	}
    public void wakeUp( Robot p,Robot e)
    {
       
        player = p;
        enemy = e;
        e.chooseWeapon(p);
        //player = e;
        ///////////////////////
        Transform playerBar = transform.GetChild(2);
        //玩家HP、EN
        playerBar.GetChild(0).GetComponent<Slider>().value =(float) player.curHP / player.maxHP;
        playerBar.GetChild(1).GetComponent<Slider>().value = (float)player.curEN / player.maxEN;
        playerBar.GetChild(2).GetComponent<Text>().text = player.curHP.ToString() + "/" + player.maxHP.ToString();
        playerBar.GetChild(3).GetComponent<Text>().text = player.curEN.ToString() + "/" + player.maxEN.ToString();
        //玩家机体名
        transform.Find("playerName").GetChild(0).GetComponent<Text>().text = player.mechName;
        //玩家武器名
        if(player.CUR_WEAPON!=null)
        transform.Find("PlayerWeaponName").GetChild(0).GetComponent<Text>().text = player.CUR_WEAPON.NAME;
        else
        {
            transform.Find("PlayerWeaponName").GetChild(0).GetComponent<Text>().text = "--";
        }
        //玩家机师名
        transform.Find("PlayerPilotName").GetChild(0).GetComponent<Text>().text = player.Pilot.pilotName;
        //玩家等级
        transform.Find("PlayerLevel").GetChild(0).GetComponent<Text>().text = player.Pilot.level.ToString();
        //玩家气力
        transform.Find("PlayerConfident").GetChild(0).GetComponent<Text>().text = player.Pilot.cur_Confident.ToString();
        Transform enemyBar = transform.Find("enemyHPbar");
        enemyBar.Find("HP").GetComponent<Slider>().value = (float)enemy.curHP / enemy.maxHP;
        enemyBar.Find("EN").GetComponent<Slider>().value = (float)enemy.curEN / enemy.maxEN;
        enemyBar.Find("HPValue").GetComponent<Text>().text = enemy.curHP.ToString() + "/" + enemy.maxHP.ToString();
        enemyBar.Find("ENValue").GetComponent<Text>().text = enemy.curEN.ToString() + "/" + enemy.maxEN.ToString();
        transform.Find("enemyName").GetChild(0).GetComponent<Text>().text = enemy.mechName;
        if(enemy.CUR_WEAPON!=null)
        transform.Find("enemyWeaponName").GetChild(0).GetComponent<Text>().text = enemy.CUR_WEAPON.weaponName;
        else
        {
            transform.Find("enemyWeaponName").GetChild(0).GetComponent<Text>().text = "--";
        }
        transform.Find("EnemyPilotName").GetChild(0).GetComponent<Text>().text = enemy.Pilot.pilotName;
        transform.Find("EnemyLevel").GetChild(0).GetComponent<Text>().text = enemy.Pilot.level.ToString();
        transform.Find("EnemyConfident").GetChild(0).GetComponent<Text>().text = enemy.Pilot.cur_Confident.ToString();
        if(p.CUR_WEAPON!=null)
        {
            Transform t = transform.Find("PlayerAccuracy");
            transform.Find("PlayerAccuracy").GetChild(0).GetComponent<Text>().text = GM.getAccuracy(p, e).ToString() + "%";
        }
        else
        {
            transform.Find("PlayerAccuracy").GetChild(0).GetComponent<Text>().text = "---";
        }
        if (e.CUR_WEAPON != null)
        {
            transform.Find("EnemyAccuracy").GetChild(0).GetComponent<Text>().text = GM.getAccuracy(e, p).ToString() + "%";
        }
        else
        {
            transform.Find("EnemyAccuracy").GetChild(0).GetComponent<Text>().text = "---";
        }


    }
    public void battle()
    {
        if(GM.phase==gameManager.PHASE.PLAYERPHASE)
        {
            player.ableToAttack = false;
            player.ableToMove = false;
            player.actStatus = Robot.ACTSTATUS.WAIT;
            GM.playerRound = gameManager.PLAYERROUNDPROC.IDLE;
            player.transform.GetComponent<SpriteRenderer>().color = Color.gray;
            GM.awakeAnimation(player.transform, enemy.transform);
            gameObject.SetActive(false);
        }
        if(GM.phase==gameManager.PHASE.ENEMYPHASE)
        {
            enemy.ableToAttack = false;
            enemy.ableToMove = false;
            enemy.actStatus = Robot.ACTSTATUS.WAIT;
            GM.enemyRound = gameManager.ENEMYROUNDPROC.IDLE;
            enemy.transform.GetComponent<SpriteRenderer>().color = Color.gray;
            GM.awakeAnimation(enemy.transform, player.transform);
            gameObject.SetActive(false);
        }
        
    }
}
