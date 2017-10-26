using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;
public class battleMenu : MonoBehaviour {
   
    Robot robot;
    public gameManager GM;
	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void activate(Robot robot)
    {
        this.robot = robot;
        gameObject.SetActive(true);
      
            transform.GetChild(0).gameObject.SetActive(robot.ableToMove);
        
   
            transform.GetChild(1).gameObject.SetActive(robot.ableToAttack);
        
    }
    public void move()
    {
        //int range = robot.MOVELIMIT;
        //uint data= moveui.GetTileData(robot.transform.position);
        // int tileid = Tileset.GetTileIdFromTileData(data);
        // Tile tile = moveui.Tileset.GetTile(tileid);
       
        //Vector2 pos = TilemapUtils.GetGridWorldPos(BrushUtil.GetGridX(robot.transform.position, moveui.CellSize), BrushUtil.GetGridY(robot.transform.position, moveui.CellSize), moveui.CellSize);
        robot.showMoveRange();
        GM.playerRound = gameManager.PLAYERROUNDPROC.CHOOSEMOVE;
        
        gameObject.SetActive(false);
    }
    public void wait()
    {
        robot.wait();
        gameObject.SetActive(false);
    }
    public void attack()
    {
        robot.chooseWeapon();
       
    }
   
}
