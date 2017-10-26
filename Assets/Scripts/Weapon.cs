using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
   public int attack;
   public int minRange;
   public int maxRange;
   public string weaponName;
   public int Accuracy;
   public int CriticalRate;
   public int ConfidentDemand;
   public int curNum;
    public int[] attackAvatars;
    public int[] attackLines;

    public string animationName;
    public enum WEAPONTYPE{BEAM=0,BULLET,CRITICAL,NORMAL};
    public enum CASTTYPE { MELEE=0,SHOOT};
    public enum COSTTYPE { EN=0,NUM};
   public WEAPONTYPE weaponType;
   public CASTTYPE castType;
   public COSTTYPE costType;

   public int numLimit;
   public int enCost;
    public int upGradeRate;
	// Use this for initialization
	void Start () {
        upGradeRate = 0;
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //public void Init(int atk,int min,int max,string n,int acc,int ct, int cdemand,int wtype,int costtype,int casttype,int encost,int numlimit)
    //{
    //    attack = atk;
    //    minRange = min;
    //    maxRange = max;
    //    weaponName = n;
    //    Accuracy = acc;
    //    CriticalRate = ct;
    //    ConfidentDemand = cdemand;
    //    weaponType =(WEAPONTYPE) wtype;
    //    castType = (CASTTYPE)casttype;
    //    costType = (COSTTYPE)costtype;
    //    enCost = encost;
    //    curNum=numLimit = numlimit;
    //}
    public void Upgrade (int rate)
    {
        upGradeRate += rate;
    }
   
    public int ATTACK { get { return attack+200*upGradeRate; } }
    public int MINRANGE { get { return minRange; } }
    public int MAXRANGE { get { return maxRange; } }
    public string NAME { get { return weaponName; } }
    public int ACCURACY { get { return Accuracy; } }
    public int CT { get { return CriticalRate; } }
    public int CONFIDENTDEMAND { get { return ConfidentDemand; } }
    public WEAPONTYPE weaponTYPE { get { return weaponType; } }
    public CASTTYPE castTYPE { get { return castTYPE; } }
    public COSTTYPE costTYPE { get { return costType; } }
    public int ENCOST { get { return enCost; } }
    public int NUMLIMIT
    {
        get { return numLimit; }
    }
    public int CURNUM { get { return curNum; } }
    public string getLine()
    {
        //if(attackLines.Length==1)
        //{
        //    return  transform.parent.Find("Pilot").GetComponent<pilot>().attackedLines[0];
        //}
        //else
        //{
            int randomvalue = Random.Range(attackLines[0], attackLines[attackLines.Length-1]);
            return transform.parent.parent.Find("Pilot").GetComponent<pilot>().attackLines[randomvalue];
       // }
        
    }
    public Sprite getAvatar()
    {
        //if(attackAvatars.Length==1)
        //{
        //    return transform.parent.Find("Pilot").GetComponent<pilot>().attackedAvatars[0];
        //}
        //else
        //{
        Debug.Log(NAME);
            int randomvalue = Random.Range(attackAvatars[0], attackAvatars[attackAvatars.Length-1]);
            return transform.parent.parent.Find("Pilot").GetComponent<pilot>().attackAvatars[randomvalue];
        //}
        
    }
}
