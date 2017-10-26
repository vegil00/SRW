using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {
    public GameObject AnimationCamera;
    public GameObject AnimationUI;
    public GameObject battlecorfirm;
    public GameObject mapCamera;
    public GameObject mapMenu;
    public GameObject weaponmenu;
    public GameObject gameMenu;
    public Camera guiCamera;
    public AudioSource cur_source;
    
    public Tilemap background;
    public bool enemyAI;
    int enemyCount;
    public int RoundCount;
    public enum PHASE { PLAYERPHASE=0,ENEMYPHASE};
    public enum PLAYERROUNDPROC { IDLE=0,ACTMENU,CHOOSEMOVE,AFTERMOVE,CHOOSEWEAPON,CHOOSEENE,BATTLECORFIRM,BATTLEANIMATION};
    public enum ENEMYROUNDPROC { IDLE = 0, MOVE, BATTLECORFIRM, BATTLEANIMATION };
    public PLAYERROUNDPROC playerRound;
    public ENEMYROUNDPROC enemyRound;
    public PHASE phase;
    public List<Robot> playerList;
    public List<Robot> enemyList;
	// Use this for initialization
	void Start () {
        playerList = new List<Robot>();
        playerList = new List<Robot>();
        cur_source = transform.GetComponent<AudioSource>();
        cur_source.Play();
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(phase==PHASE.ENEMYPHASE&&enemyRound==ENEMYROUNDPROC.IDLE&&enemyAI)
        {
            foreach(Robot r in enemyList)
            {
                
                if (r.actStatus==Robot.ACTSTATUS.STAY)
                {
                    enemyRound = ENEMYROUNDPROC.MOVE;
                    r.procBehavior();

                   
                  
                    break;
                }
              
            }
           
        }
        if(phase==PHASE.PLAYERPHASE&&playerRound==PLAYERROUNDPROC.IDLE&&Input.GetMouseButtonDown(0))
        {
            int girdX = TilemapUtils.GetMouseGridX(background, GameObject.Find("Main Camera").GetComponent<Camera>());
            int girdY = TilemapUtils.GetMouseGridY(background, GameObject.Find("Main Camera").GetComponent<Camera>());
            bool test = background.GetTile(girdX, girdY).paramContainer.GetBoolParam("passable");
            //if (background.GetTile(girdX,girdY).paramContainer.GetBoolParam("passable"))
            if (background.GetTile(girdX, girdY).paramContainer.GetIntParam("race")==0)
            {
                if(!gameMenu.activeSelf)
               
                {
                    gameMenu.SetActive(true);
                }
            }
        }
        if(enemyList.Count==0)
        {
            gameOver();
        }
	}
    public void awakeAnimation(Transform a,Transform d)
    {
        mapCamera.SetActive(false);
        AnimationCamera.SetActive(true);
        AnimationCamera.GetComponent<AnimateCamera>().WakeUp(a,d);
        AnimationUI.SetActive(true);
        //if (a.GetComponent<Robot>().race == Robot.RACE.PLAYER)
        //    AnimationUI.GetComponent<AnimationUI>().WakeUp(a.GetComponent<Robot>(), d.GetComponent<Robot>());
        //else
        //{
        //    AnimationUI.GetComponent<AnimationUI>().WakeUp (d.GetComponent<Robot>(),a.GetComponent<Robot>());
        //}
        
    }
    public void awakeBattleMenu(Transform p)
    {
        Vector3 pos1 = Camera.main.WorldToScreenPoint(p.GetComponent<Robot>().NEXTPOS);
        Vector3 pos2 = guiCamera.ScreenToWorldPoint(pos1);
        mapMenu.transform.position = pos2;
        // battlemenu.SetActive(true);
       mapMenu.GetComponent<battleMenu>().activate(p.GetComponent<Robot>());
        playerRound = PLAYERROUNDPROC.ACTMENU;
    }
    public void awakeBattleCorfirm(Robot p,Robot e)
    {
        battlecorfirm.SetActive(true);
        battlecorfirm.GetComponent<battleConfirm>().wakeUp(p,e);
        if(phase==PHASE.PLAYERPHASE)
        playerRound = PLAYERROUNDPROC.BATTLECORFIRM; 
        else
        {
            enemyRound = ENEMYROUNDPROC.BATTLECORFIRM;
        }
    }
    public void awakeWeaponMenu(Transform p)
    {
        weaponmenu.SetActive(true);
        weaponmenu.GetComponent<weaponMenu>().activate(p.GetComponent<Robot>());
        playerRound = PLAYERROUNDPROC.CHOOSEWEAPON;
    }
    public int CalculateDamage(Robot a,Robot d)
    {
        int value= a.getBasicDamage() - d.getBasicDefence();
        if (value > 10)
            return value;
        else
            return 10;
    }
    public int getAccuracy(Robot a,Robot d)
    {
        return a.getAccuracy() - d.getAvoidRate();
    }
    public void addRobot(Robot r)
    {
        if (r.race == Robot.RACE.PLAYER)
        {
            playerList.Add( r);
        }
        if(r.race==Robot.RACE.ENEMY)
        {
            enemyList.Add(r);
        }
    }
    public void AnimationFishished()
    {
        mapCamera.SetActive(true);
    }
    public void EndTurn()
    {
        enemyAI = true;
        enemyCount = 0;
        phase = PHASE.ENEMYPHASE;
        foreach(Robot r in enemyList)
        {
            r.actStatus = Robot.ACTSTATUS.STAY;
            r.ableToMove = true;
            r.ableToAttack = false;
        }
    }
    public void BeginRound()
    {
        RoundCount++;
        playerRound = PLAYERROUNDPROC.IDLE;
        phase = PHASE.PLAYERPHASE;
        foreach(Robot r in playerList)
        {
            r.actStatus = Robot.ACTSTATUS.STAY;
            r.ableToMove = true;
            r.ableToAttack = false;
            r.transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
        foreach(Robot r in enemyList)
        {
            r.transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public void gameOver()
    {
        SceneManager.LoadScene(0);
    }
}
