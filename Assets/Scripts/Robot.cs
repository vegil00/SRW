using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CreativeSpore.SuperTilemapEditor;
using DG.Tweening;


public class Robot: MonoBehaviour {
  
    Camera guiCamera;
    public int moveLimit;
    Tilemap moveui;
   public Tilemap background;
    public gameManager GM;
    public Transform animations;
    Vector3 prePos;
    public bool ableToMove;
    public bool ableToAttack;
    public enum RACE { PLAYER=1,ENEMY};
    public enum POSITIONSTATUS { AIR=0,GROUND,WATER};
    public enum ACTSTATUS { STAY=0,MOVE,ATTACK,WAIT};
    public enum SCANTYPE { MOVE=0,WEAPON};
    public ACTSTATUS actStatus;
    POSITIONSTATUS posStatus;
   public Dictionary<Vector2,int> rangeList;
   public Dictionary<string, Weapon> weaponList;
    public Dictionary<string, int> weaponID;
    public string mechName;
    public pilot Pilot;
    public int maxHP;
    public int maxEN;
    public int curHP;
    public int curEN;
    public RACE race;
    public int enemyCount;
    public Vector3 NEXTPOS { get; set; }
    public Weapon CUR_WEAPON { get; set; }
    public int CUR_WEAPONID { get; set; }
    /// <summary>
    /// //////////////////////////
    public int armor;
    public int motility;
    /// </summary>
    // Use this for initialization
    //public int MAXHP { get { return maxHP;  } }
    //public int MAXEN { get { return maxEn; } }
    //public int CURHP { get { return curHP; } }
    //public int CUREN { get { return curEN; } }
    void Start () {
        // battlemenu =GameObject.Find("battleMenu");
        guiCamera = GameObject.Find("GuiCamera").GetComponent<Camera>();
       
        moveui = GameObject.Find("moveui").GetComponent<Tilemap>();
        background = GameObject.Find("background").GetComponent<Tilemap>();
        
        posStatus = POSITIONSTATUS.GROUND;
        ableToMove = true;
        actStatus = ACTSTATUS.STAY;
        rangeList = new Dictionary<Vector2, int>();
        weaponList = new Dictionary<string, Weapon>();
        weaponID = new Dictionary<string, int>();
      
        if (weaponList.Count == 0)
        {
            for (int i = 0; i < transform.Find("Weapons").childCount; i++)
            {
                Weapon w = transform.Find("Weapons").GetChild(i).GetComponent<Weapon>();
                weaponList.Add(w.NAME, w);
                weaponID.Add(w.NAME, i);
            }
        }
        Pilot = transform.Find("Pilot").GetComponent<pilot>();
        GM.addRobot(this);
        if(race==RACE.ENEMY)
        {
            enemyCount = GM.enemyList.Count;
        }
        //Pilot.name = "皆川斗马";
        int GridX = TilemapUtils.GetGridX(background, transform.position);
        int GridY = TilemapUtils.GetGridY(background, transform.position);
        transform.position = TilemapUtils.GetGridWorldPos(background, GridX, GridY);
        NEXTPOS = transform.position;
        background.GetTile(GridX, GridY).paramContainer.SetParam("passable", false);
        background.GetTile(GridX, GridY).paramContainer.SetParam("race", (int)race);
    }
	
	// Update is called once per frame
    public void Init(string mechname,int hp,int en)
    {
        mechName = mechname;
       
        curHP = maxHP = hp;
        curEN = maxEN = en;
    }
    public void setPilot(pilot p)
    {
        Pilot = p;
    }
	void Update () {
        int test = background.GetTile(transform.position).paramContainer.GetIntParam("race");
        int test2 = (int)race;
        //Debug.Log(test);
        if (race == RACE.ENEMY && curHP <= 0)
        {
            background.GetTile(NEXTPOS).paramContainer.SetParam("passable", true);
            background.GetTile(NEXTPOS).paramContainer.SetParam("race", 0);
            background.UpdateMesh();
            GM.enemyList.Remove(this);
            gameObject.SetActive(false);

        }
        if (race == RACE.PLAYER && curHP <= 0)
        {
            GM.gameOver();
        }
        if (background.GetTile(transform.position).paramContainer.GetIntParam("race") != (int)race&&(Vector2)transform.position==(Vector2)NEXTPOS)
        {
            background.GetTile(transform.position).paramContainer.SetParam("race", (int)race);
            background.UpdateMesh();
        }
        if (Input.GetMouseButtonDown(0)&&race==RACE.PLAYER)
        {
            int girdX = TilemapUtils.GetMouseGridX(moveui, GameObject.Find("Main Camera").GetComponent<Camera>());
            int girdY= TilemapUtils.GetMouseGridY(moveui, GameObject.Find("Main Camera").GetComponent<Camera>());
            uint data = moveui.GetTileData(TilemapUtils.GetGridWorldPos(moveui, girdX, girdY));
            TileData tiledata = new TileData(data);
            if(GM.playerRound== gameManager.PLAYERROUNDPROC.CHOOSEMOVE&& actStatus == ACTSTATUS.MOVE)
            {
                if (tiledata.tileId == 0)
                {
                    background.GetTile(transform.position).paramContainer.SetParam("passable", true);
                    background.GetTile(transform.position).paramContainer.SetParam("race", 0);
                    prePos = transform.position;
                    Vector3 pos = new Vector3(TilemapUtils.GetGridWorldPos(moveui, girdX, girdY).x, TilemapUtils.GetGridWorldPos(moveui, girdX, girdY).y);
                    background.GetTile(pos).paramContainer.SetParam("passable", false);
                    background.GetTile(pos).paramContainer.SetParam("race", 1);
                    transform.DOMove(pos, 0.5f, false);
                    Camera.main.transform.DOMove(pos + new Vector3(0, 0, -10), 0.5f, false);
                    NEXTPOS = pos;


                    // Vector3 pos1 = Camera.main.WorldToScreenPoint(pos);
                    // Vector3 pos2 = guiCamera.ScreenToWorldPoint(pos1);
                    // battlemenu.transform.position = pos2;
                    //// battlemenu.SetActive(true);
                    // battlemenu.GetComponent<battleMenu>().activate(this);
                    foreach (KeyValuePair<Vector2, int> pair in rangeList)
                    {
                        Vector2 grid = pair.Key;
                        grid = TilemapUtils.GetGridWorldPos(moveui, (int)grid.x, (int)grid.y);
                        moveui.Erase(grid);
                    }
                    moveui.UpdateMesh();
                    scanAbleToAttack();
                    if (ableToAttack)
                        actStatus = ACTSTATUS.ATTACK;

                    ableToMove = false;

                    GM.awakeBattleMenu(transform);
                    background.UpdateMesh();
                }
                else
                {
                    GM.mapMenu.SetActive(true);
                    foreach (KeyValuePair<Vector2, int> pair in rangeList)
                    {
                        Vector2 grid = pair.Key;
                        grid = TilemapUtils.GetGridWorldPos(moveui, (int)grid.x, (int)grid.y);
                        moveui.Erase(grid);
                    }
                    moveui.UpdateMesh();
                }
            }
            if(GM.playerRound==gameManager.PLAYERROUNDPROC.CHOOSEENE&&actStatus==ACTSTATUS.ATTACK)
            {
                if(tiledata.tileId==1)
                {
                    if(background.GetTile(girdX,girdY).paramContainer.GetIntParam("race")==(int)RACE.ENEMY)
                    {
                        Vector2 pos = new Vector3(TilemapUtils.GetGridWorldPos(moveui, girdX, girdY).x, TilemapUtils.GetGridWorldPos(moveui, girdX, girdY).y);
                        foreach(Robot r in GM.enemyList)
                        {
                            int testX = TilemapUtils.GetGridX(background, r.transform.position);
                            int testY = TilemapUtils.GetGridY(background, r.transform.position);
                            if (TilemapUtils.GetGridX(background,r.transform.position)==girdX&&TilemapUtils.GetGridY(background,r.transform.position)==girdY)
                            {
                                foreach (KeyValuePair<Vector2, int> pair in rangeList)
                                {
                                    Vector2 grid = pair.Key;
                                    grid = TilemapUtils.GetGridWorldPos(moveui, (int)grid.x, (int)grid.y);
                                    moveui.Erase(grid);
                                }
                                moveui.UpdateMesh();
                                GM.awakeBattleCorfirm(this, r);
                                break;
                            }
                        }
                    }
                  
                    

                }
                else
                {
                    GM.awakeWeaponMenu(transform);
                }
               
            }
           
          
        }
        if(actStatus==ACTSTATUS.ATTACK&&race==RACE.ENEMY&&transform.position==NEXTPOS)
        {
           
            scanAbleToAttack();
            if (ableToAttack)
            {
                Robot target = chooseTarget();
                chooseWeapon(target);
                target.chooseWeapon(this);
                GM.awakeBattleCorfirm(target, this);
            }
            else
            {
                actStatus = ACTSTATUS.WAIT;
                GM.enemyRound = gameManager.ENEMYROUNDPROC.IDLE;
                if(enemyCount==GM.enemyList.Count)
                {
                    GM.BeginRound();
                }
                GM.enemyAI = true;

            }
        }
       
        
		
	}
    public int MOVELIMIT
    {
        get { return moveLimit; }
    }
    public void OnPointerClick(BaseEventData data)
    {
        Debug.Log("click");
      
        
        if(GM.mapMenu.activeSelf)
        {
            if(GM.playerRound == gameManager.PLAYERROUNDPROC.ACTMENU)
            {
                GM.mapMenu.SetActive(false);
                GM.playerRound = gameManager.PLAYERROUNDPROC.IDLE;
                actStatus = ACTSTATUS.STAY;
            }
            if(GM.playerRound==gameManager.PLAYERROUNDPROC.AFTERMOVE)
            {
                GM.mapMenu.SetActive(true);
                GM.playerRound = gameManager.PLAYERROUNDPROC.ACTMENU;
                actStatus = ACTSTATUS.MOVE;
            }
         
        }
        else
        {
            if(GM.playerRound == gameManager.PLAYERROUNDPROC.IDLE&&actStatus==ACTSTATUS.STAY)
            {
               scanAbleToAttack();
                GM.awakeBattleMenu(transform);
               
            }
           
        }
    }
    public Dictionary<string,Weapon> WEAPONLIST { get { return weaponList; } }
    public void showMoveRange()
    {
        TileData data = new TileData();
        data.tileId = 0;
        moveui.SetTileData(transform.position, data.BuildData());
        rangeList.Clear();
        Dictionary<Vector2, int> tempList = new Dictionary<Vector2, int>();
        Vector2 center = new Vector2(BrushUtil.GetGridX(transform.position, background.CellSize), BrushUtil.GetGridY(transform.position, background.CellSize));
        rangeList.Add(center, 0);
        tempList.Add(center, 0);
        int countPoint = 0;
        while(countPoint<moveLimit)
        {
            tempList = scanMoveRange(tempList, rangeList,0, moveLimit);
            countPoint++;
        }
       
            foreach(KeyValuePair<Vector2,int> pair in rangeList)
        {
            Vector2 pos = pair.Key;
            pos = TilemapUtils.GetGridWorldPos(background,(int)pos.x,(int)pos.y);
           
            moveui.SetTileData(pos, data.BuildData());


        }

        actStatus = ACTSTATUS.MOVE;
        moveui.UpdateMesh();
    }
  public  void showWeaponRange(string weaponname)
    {
        TileData data = new TileData();
        data.tileId = 1;
        Dictionary<Vector2, int> tempList = new Dictionary<Vector2, int>();
        rangeList.Clear();
        Vector2 center = new Vector2(BrushUtil.GetGridX(transform.position, background.CellSize), BrushUtil.GetGridY(transform.position, background.CellSize));
        rangeList.Add(center, 0);
        tempList.Add(center, 0);
        int countPoint = 0;
        while (countPoint < weaponList[weaponname].MAXRANGE)
        {
            tempList = scanWeaponRange(CUR_WEAPON,tempList, rangeList);
            countPoint++;
        }
        
        rangeList.Remove(center);
        foreach (KeyValuePair<Vector2, int> pair in rangeList)
        {
            Vector2 pos = pair.Key;
            pos = TilemapUtils.GetGridWorldPos(background, (int)pos.x, (int)pos.y);

            moveui.SetTileData(pos, data.BuildData());


        }
        moveui.UpdateMesh();
    }
   public bool scanAbleToAttack()
    {

        Dictionary<Vector2, int> tempList = new Dictionary<Vector2, int>();
        foreach (KeyValuePair<string,Weapon> pair in weaponList)
        {
            rangeList.Clear();
            
            tempList.Clear();
            Vector2 center = new Vector2(BrushUtil.GetGridX(NEXTPOS, background.CellSize), BrushUtil.GetGridY(NEXTPOS, background.CellSize));
            rangeList.Add(center, 0);
            tempList.Add(center, 0);
            int countPoint = 0;
            while(countPoint<pair.Value.MAXRANGE)
            {
                tempList= scanWeaponRange(pair.Value, tempList, rangeList);
                countPoint++;
            }
           
            rangeList.Remove(center);
            int i = 0;
            foreach (KeyValuePair<Vector2,int> kvp in rangeList)
            {
                i++;
                if(i==32)
                {
                    Debug.Log("final");
                }
                int r = background.GetTile((int)kvp.Key.x,(int)kvp.Key.y).paramContainer.GetIntParam("race");
                
                if (r!=0&&r!=(int)race)
                {
                    ableToAttack = true;
                    return true;
                }
            }
            return false;

        }
        ableToAttack = false;
    }
    Dictionary<Vector2, int> scanWeaponRange(Weapon weapon, Dictionary<Vector2, int> tempList, Dictionary<Vector2, int> rangeList)
    {
        int min = weapon.minRange;
        int max = weapon.maxRange;
        Dictionary<Vector2, int> result = new Dictionary<Vector2, int>();
        for(int i=0;i<tempList.Count;i++)
        {
            KeyValuePair<Vector2, int> kvp = new KeyValuePair<Vector2, int>();
            foreach (KeyValuePair<Vector2, int> item in tempList)
            {
                if (item.Value == i)
                {
                    kvp = item;
                    directionScan(new Vector2(kvp.Key.x, kvp.Key.y + 1), kvp.Value, result, rangeList, min, max,SCANTYPE.WEAPON);
                    directionScan(new Vector2(kvp.Key.x, kvp.Key.y - 1), kvp.Value, result, rangeList, min, max, SCANTYPE.WEAPON);
                    directionScan(new Vector2(kvp.Key.x + 1, kvp.Key.y), kvp.Value, result, rangeList, min, max, SCANTYPE.WEAPON);
                    directionScan(new Vector2(kvp.Key.x - 1, kvp.Key.y), kvp.Value, result, rangeList, min, max, SCANTYPE.WEAPON);

                }
            }
        }
        return result;
    }
    Dictionary<Vector2,int> scanMoveRange(Dictionary<Vector2, int> tempList, Dictionary<Vector2, int> rangeList,int minLimit,int maxLimit)
    {
        Dictionary<Vector2, int> result = new Dictionary<Vector2, int>();
        for(int i=0;i<tempList.Count;i++)
        {
            KeyValuePair<Vector2, int> kvp=new KeyValuePair<Vector2, int>();
            foreach(KeyValuePair<Vector2,int> item in tempList)
            {
                if(item.Value==i)
                {
                    kvp = item;
                    directionScan(new Vector2(kvp.Key.x, kvp.Key.y + 1), kvp.Value, result, rangeList, minLimit,maxLimit,SCANTYPE.MOVE);
                    directionScan(new Vector2(kvp.Key.x, kvp.Key.y - 1), kvp.Value, result, rangeList, minLimit, maxLimit, SCANTYPE.MOVE);
                    directionScan(new Vector2(kvp.Key.x + 1, kvp.Key.y), kvp.Value, result, rangeList, minLimit, maxLimit, SCANTYPE.MOVE);
                    directionScan(new Vector2(kvp.Key.x - 1, kvp.Key.y), kvp.Value, result, rangeList, minLimit, maxLimit, SCANTYPE.MOVE);
                   
                }
            }
           
        }
        return result;

    }
    
    void directionScan(Vector2 dir,int matrix, Dictionary<Vector2, int> tempList, Dictionary<Vector2, int> range,int minLimit,int maxLimit,SCANTYPE type)
    {
        if (!(dir.x >= background.MinGridX && dir.x <= background.MaxGridX && dir.y >= background.MinGridY && dir.y <= background.MaxGridY))
        {
            return;
        }

      
            int Matrix = 1;
            if (type==SCANTYPE.MOVE)
        {
            Matrix = background.GetTile(TilemapUtils.GetGridWorldPos(background, (int)dir.x, (int)dir.y)).paramContainer.FindParam("moveCost").GetAsInt();
            if (!range.ContainsKey(dir) && effectivePos(dir) && Matrix != 0)
            {
                int value = matrix + Matrix;
                if (value <= moveLimit && value >= minLimit)
                {
                    tempList.Add(dir, value);
                    range.Add(dir, value);
                }

            }
        }
                
            if (type==SCANTYPE.WEAPON)
            {
                Matrix = 1;
            if (!range.ContainsKey(dir) && Matrix != 0)
            {
                int value = matrix + Matrix;
                if (value <= moveLimit && value >= minLimit)
                {
                    tempList.Add(dir, value);
                    range.Add(dir, value);
                }

            }
        }
          
        
        
    }
    bool effectivePos(Vector2 dir)
    {
        bool passable= background.GetTile(TilemapUtils.GetGridWorldPos(background, (int)dir.x, (int)dir.y)).paramContainer.FindParam("passable").GetAsBool();
        return passable;
    }
    public void wait()
    {
        transform.GetComponent<SpriteRenderer>().color = Color.gray;
        actStatus = ACTSTATUS.WAIT;
        GM.playerRound = gameManager.PLAYERROUNDPROC.IDLE;
    }
    public void chooseWeapon()
    {
       GM.mapMenu.SetActive(false);
        GM.awakeWeaponMenu(transform);
      
    }
   public void setCurWeapon(string name)
    {
        CUR_WEAPON = weaponList[name];
        animations.GetComponent<Animator>().SetInteger("WeaponID", weaponID[name]);
    }
    public string getAttackLine()
    {
       
            int randomvalue = Random.Range(0, Pilot.attackLines.Length-1);
            return Pilot.attackLines[randomvalue];
       
    }
    public Sprite getAttackAvatar()
    {
      
        int randomvalue = Random.Range(0, Pilot.attackAvatars.Length-1);
        return Pilot.attackedAvatars[randomvalue];
    }
    public int getBasicDamage()
    {
        if (CUR_WEAPON.castType == Weapon.CASTTYPE.MELEE)
        {
            return CUR_WEAPON.ATTACK * (Pilot.cur_Confident + Pilot.meele) / 200;
        }
        else if (CUR_WEAPON.castType == Weapon.CASTTYPE.SHOOT)
        {
            return CUR_WEAPON.ATTACK * (Pilot.cur_Confident + Pilot.shoot) / 200;
        }
        else
            return 0;
    }
    public int getBasicDefence()
    {
        return armor * (Pilot.cur_Confident + Pilot.defence) / 200;
    }
    public int getAccuracy()
    {
        return (Pilot.accuracy / 2 + 140) + CUR_WEAPON.ACCURACY;
    }
    public int getAvoidRate()
    {
        return Pilot.avoid / 2 + motility;
    }
    public void beHit(int value)
    {
        curHP -= value;
        if(curHP<0)
        {
            curHP = 0;
        }
        if(curHP==0)
        {
            gameObject.SetActive(false);
        }
    }
    public void costEN(int value)
    {
        curEN -= value;
    }
    public void costNum()
    {
        CUR_WEAPON.curNum--;
    }
    public void chooseWeapon(Robot target)
    {
        int girdX = TilemapUtils.GetGridX(moveui, target.transform.position);
        int girdY = TilemapUtils.GetGridY(moveui, target.transform.position);
        Dictionary<Vector2, int> tempList = new Dictionary<Vector2, int>();
        foreach (KeyValuePair<string,Weapon> weapon in weaponList)
        {
            rangeList.Clear();

            tempList.Clear();
            Vector2 center = new Vector2(BrushUtil.GetGridX(NEXTPOS, background.CellSize), BrushUtil.GetGridY(NEXTPOS, background.CellSize));
            rangeList.Add(center, 0);
            tempList.Add(center, 0);
            int countPoint = 0;
            while (countPoint < weapon.Value.MAXRANGE)
            {
                tempList = scanWeaponRange(weapon.Value, tempList, rangeList);
                countPoint++;
            }

            rangeList.Remove(center);
            foreach (KeyValuePair<Vector2, int> kvp in rangeList)
            {
                if((int)kvp.Key.x==girdX&&(int)kvp.Key.y==girdY)
                {
                    ableToAttack = true;
                    if(CUR_WEAPON==null)
                    {
                        CUR_WEAPON = weapon.Value;
                    }
                    else
                    {
                        if(weapon.Value.ATTACK>CUR_WEAPON.ATTACK)
                        {
                            CUR_WEAPON = weapon.Value;
                        }
                    }
                }
              
            }
        }
    }
    public Robot chooseTarget()
    {
        Robot target=null;
        float mindis=0;
        foreach(Robot r in GM.playerList)
        {
            Vector2 temp = transform.position - r.transform.position;
            if (mindis==0.0f)
            {

                mindis = temp.sqrMagnitude;
            }
            if(mindis>=temp.sqrMagnitude)
            {
                mindis = temp.sqrMagnitude;
                target = r;

            }
        }
        return target;
    }
    public void procBehavior()
    {
        GM.enemyAI = false;
        Camera.main.transform.position = new Vector3(0, 0, -10) + transform.position;
        scanAbleToAttack();
        Robot target = chooseTarget();
        if (ableToAttack)
        {
            GM.enemyRound = gameManager.ENEMYROUNDPROC.BATTLECORFIRM;
            chooseWeapon(target);
            GM.awakeBattleCorfirm(target, this);

        }
        else
        {
            showMoveRange();
            float dis = 0;
            Vector2 pos = transform.position ;
            foreach(KeyValuePair<Vector2,int> pair in rangeList)
            {
                if(dis==0)
                {
                    dis = (target.transform.position -TilemapUtils.GetGridWorldPos(background, (int)pair.Key.x,(int)pair.Key.y)).sqrMagnitude;
                   
                }
                if (dis >= (target.transform.position - TilemapUtils.GetGridWorldPos(background, (int)pair.Key.x, (int)pair.Key.y)).sqrMagnitude)
                {
                    dis = (target.transform.position - TilemapUtils.GetGridWorldPos(background, (int)pair.Key.x, (int)pair.Key.y)).sqrMagnitude;
                    pos = pair.Key;
                }
            }
            background.GetTile((int)pos.x, (int)pos.y).paramContainer.SetParam("passable", false);
            background.GetTile(transform.position).paramContainer.SetParam("passable", true);
            background.GetTile(transform.position).paramContainer.SetParam("race", 0);
            pos =TilemapUtils.GetGridWorldPos(background,(int)pos.x,(int)pos.y);
            background.GetTile(pos).paramContainer.SetParam("race", 2);
            
            NEXTPOS = pos;
            prePos = transform.position;
            transform.DOMove(pos, 0.5f, false);
            foreach (KeyValuePair<Vector2, int> pair in rangeList)
            {
                Vector2 grid = pair.Key;
                grid = TilemapUtils.GetGridWorldPos(moveui, (int)grid.x, (int)grid.y);
                moveui.Erase(grid);
            }
            moveui.UpdateMesh();
            background.UpdateMesh();
            // StartCoroutine(eraseMoveUI());
            actStatus = ACTSTATUS.ATTACK;
            
           
           

        }
    }
    //IEnumerator  eraseMoveUI()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    {
    //        foreach (KeyValuePair<Vector2, int> pair in rangeList)
    //        {
    //            Vector2 grid = pair.Key;
    //            grid = TilemapUtils.GetGridWorldPos(moveui, (int)grid.x, (int)grid.y);
    //            moveui.Erase(grid);
    //        }
    //        moveui.UpdateMesh();
         
    //    }
    //}

}
