using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationUI : MonoBehaviour {
    public int playerMaxHP;
    public int playerCurHP;
    public int playerActualHP;
    public int playerMaxEN;
    public int playerCurEN;
    public int playerActualEN;
    public int enemyMaxHP;
    public int enemyCurHP;
    public int enemyActualHP;
    public int enemyMaxEN;
    public int enemyCurEN;
    public int enemyActualEN;
    public AnimateCamera animationCamera;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(playerActualHP<playerCurHP)
        {
            playerCurHP -=(int)(Time.deltaTime * 500);
            if(playerCurHP<playerActualHP)
            {
                playerCurHP = playerActualHP;
            }
            transform.Find("playerHPBar").Find("HP").GetComponent<Slider>().value = (float)playerCurHP / playerMaxHP;
            transform.Find("playerHPBar").Find("HPValue").GetComponent<Text>().text = playerCurHP.ToString() + "/" + playerMaxHP.ToString();
            if(playerCurHP==playerActualHP)
            {
                StartCoroutine(playerDamageShowed());
              
            }
        }
        if(playerActualEN<playerCurEN)
        {
            playerCurEN -= (int)(Time.deltaTime * 500);
            if(playerCurEN<playerActualEN)
            {
                playerCurEN = playerActualEN;
            }
            transform.Find("playerHPBar").Find("EN").GetComponent<Slider>().value = (float)playerCurEN / playerMaxEN;
            transform.Find("playerHPBar").Find("ENValue").GetComponent<Text>().text = playerCurEN.ToString() + "/" + playerMaxEN.ToString();
            if(playerCurEN==playerActualEN)
            {
                StartCoroutine(playerENCosted());
               
            }
        }
        if(enemyActualHP<enemyCurHP)
        {
            enemyCurHP -= (int)(Time.deltaTime * 500);
            if(enemyCurHP<enemyActualHP)
            {
                enemyCurHP = enemyActualHP;
            }
            transform.Find("enemyHPBar").Find("HP").GetComponent<Slider>().value = (float)enemyCurHP / enemyMaxHP;
            transform.Find("enemyHPBar").Find("HPValue").GetComponent<Text>().text = enemyCurHP.ToString() + "/" + enemyMaxHP.ToString();
            if(enemyCurHP==enemyActualHP)
            {
                StartCoroutine(showEnemyDamage());
            }
        }
        if(enemyActualEN<enemyCurEN)
        {
            enemyCurEN -= (int)(Time.deltaTime * 500);
            if(enemyCurEN<enemyActualEN)
            {
                enemyCurEN = enemyActualEN;
            }
            transform.Find("enemyHPBar").Find("EN").GetComponent<Slider>().value = (float)enemyCurEN / enemyMaxEN;
            transform.Find("enemyHPBar").Find("ENValue").GetComponent<Text>().text = enemyCurEN.ToString() + "/" + enemyMaxEN.ToString();
            if(enemyCurEN==enemyActualEN)
            {
                StartCoroutine(enemyENCosted());
                
            }
        }
	}
   public void WakeUp(Robot p,Robot e)
    {
        playerActualHP = playerCurHP = p.curHP;
        playerMaxHP = p.maxHP;
        playerActualEN = playerCurEN = p.curEN;
        playerMaxEN = p.maxEN;
        enemyActualHP = enemyCurHP = e.curHP;
        enemyMaxHP = e.maxHP;
        enemyActualEN = enemyCurEN = e.curEN;
        enemyMaxEN = e.maxEN;
        transform.Find("playerHPBar").Find("HP").GetComponent<Slider>().value =(float) playerCurHP /playerMaxHP;
        transform.Find("playerHPBar").Find("EN").GetComponent<Slider>().value = (float)playerCurEN / playerMaxEN;
        transform.Find("enemyHPBar").Find("HP").GetComponent<Slider>().value = (float)enemyCurHP / enemyMaxHP;
        transform.Find("enemyHPBar").Find("EN").GetComponent<Slider>().value = (float)enemyCurEN / enemyMaxEN;
        transform.Find("playerHPBar").Find("HPValue").GetComponent<Text>().text = playerCurHP.ToString() + "/" + playerMaxHP.ToString();
        transform.Find("playerHPBar").Find("ENValue").GetComponent<Text>().text = playerCurEN.ToString() + "/" + playerMaxEN.ToString();
        transform.Find("enemyHPBar").Find("HPValue").GetComponent<Text>().text = enemyCurHP.ToString() + "/" + enemyMaxHP.ToString();
        transform.Find("enemyHPBar").Find("ENValue").GetComponent<Text>().text = enemyCurEN.ToString() + "/" + enemyMaxEN.ToString();



    }
    public void deductPlayerHP(int value)
    {
        playerActualHP -= value;
        if (playerActualHP < 0)
            playerActualHP = 0;
    }
    public void deductPlayerEN(int value)
    {
        playerActualEN -= value;
        if (playerActualEN < 0)
            playerActualEN = 0;
    }
    public void deductEnemyHP(int value)
    {
        enemyActualHP -= value;
        if (enemyActualHP < 0)
            enemyActualHP = 0;
    }
    public void deductEnemyEN(int value)
    {
        enemyActualEN -= value;
        if (enemyActualEN < 0)
            enemyActualEN = 0;
    }
    IEnumerator playerDamageShowed()
        {
        yield return new WaitForSeconds(0.5f);
        animationCamera.playerDamageShowed();

    }
    IEnumerator showEnemyDamage()
    {
        yield return new WaitForSeconds(0.5f);
        animationCamera.enemyDamageShowed();

    }
    IEnumerator enemyENCosted()
    {
        yield return new WaitForSeconds(0.5f);
        animationCamera.enemyENCosted();
    }
    IEnumerator playerENCosted()
    {
        yield return new WaitForSeconds(0.5f);
        animationCamera.playerENCosted();
    }
}
