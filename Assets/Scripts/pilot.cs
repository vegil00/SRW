using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pilot : MonoBehaviour {
    public int level;
    public string pilotName;
    //public int LEVEL { get { return level; } }
    //public string NAME { get { return pilotName; } set; }
    public int curExp;
    public int nextLevelExp;
    public int crashExp;
    public int meele;
    public int shoot;
    public int defence;
    public int tech;
    public int avoid;
    public int accuracy;
    public int maxSprite;
    public int curSprite;
    public int cur_Confident;
    public int max_Confideng;
    public Sprite[] attackAvatars;
    public string[] attackLines;
    public Sprite[] attackedAvatars;
    public string[] attackedLines;
	// Use this for initialization
	void Start () {
        level = 1;
	}
    public pilot()
    {  level=1;
     pilotName=" ";
   
     curExp=0;
    nextLevelExp=100;
     crashExp=0;
   meele=0;
   shoot=0;
     defence=0;
    tech=0;
     accuracy=0;
     maxSprite=50;
     curSprite=50;

}
    public void Init(string name,int level,int crashExp,int meele,int shoot,int def,int tech,int acc,int sprite)
    {
        pilotName = name;
        this.level = level;
        this.crashExp = crashExp;
        this.meele = meele;
        this.shoot = shoot;
        this.defence = def;
        this.tech = tech;
        this.accuracy = acc;
        curSprite = maxSprite = sprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
