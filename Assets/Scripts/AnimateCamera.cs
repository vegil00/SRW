using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateCamera : MonoBehaviour {
  public  Transform AttackerAnimation;
   public Transform DefencerAnimation;
   public Transform Attacker;
   public Transform Defencer;
    public Transform AnimationUI;
    public gameManager GM;
	// Use this for initialization
	void Start () {
        DefencerAnimation = transform.GetChild(1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void WakeUp(Transform a, Transform d)
    {
        Attacker = a;
        AttackerAnimation = a.GetComponent<Robot>().animations;


        Defencer = d;
        DefencerAnimation = d.GetComponent<Robot>().animations;
        AttackerAnimation.gameObject.SetActive(true);
        if (Attacker.GetComponent<Robot>().race==Robot.RACE.PLAYER)
        {
            AnimationUI.GetComponent<AnimationUI>().WakeUp(Attacker.GetComponent<Robot>(), Defencer.GetComponent<Robot>());
        }
        else
        {
            AnimationUI.GetComponent<AnimationUI>().WakeUp( Defencer.GetComponent<Robot>(), Attacker.GetComponent<Robot>());
        }
       
        //attack();
        if (Attacker.GetComponent<Robot>().CUR_WEAPON.costTYPE == Weapon.COSTTYPE.EN)
        {
            if (Attacker.GetComponent<Robot>().race == Robot.RACE.PLAYER)
            {

                AnimationUI.GetComponent<AnimationUI>().deductPlayerEN(Attacker.GetComponent<Robot>().CUR_WEAPON.enCost);
            }
            else if (Attacker.GetComponent<Robot>().race == Robot.RACE.ENEMY)
            {

                AnimationUI.GetComponent<AnimationUI>().deductEnemyEN(Attacker.GetComponent<Robot>().CUR_WEAPON.enCost);
            }

        }
        else
        {
            if (Attacker.GetComponent<Robot>().race == Robot.RACE.PLAYER)
            {
                playerENCosted();
            }
            if (Attacker.GetComponent<Robot>().race == Robot.RACE.ENEMY)
            {
                enemyENCosted();
            }

        }
    }

    public void attackAnimationComplete(Transform r)
    {
       if(r==AttackerAnimation)
        {
            DefencerAnimation.GetComponent<Animator>().Play("beHit");
        }
       else if(r==DefencerAnimation)
        {
            AttackerAnimation.GetComponent<Animator>().Play("beHit");
        }
    }
    public void beHitAnimationComplete(Transform r)
    {
        if(r==AttackerAnimation)
        {
           
                CaculateDamge(Attacker);
           
        }
        else if(r==DefencerAnimation)
        {
            CaculateDamge(Defencer);
        }
    }
    void attack()
    {
        if(Attacker.GetComponent<Robot>().race==Robot.RACE.PLAYER)
        {
         transform.Find("battleBackground").GetComponent<Animator>().SetInteger("dir", 2);
        }
        else
        {
            transform.Find("battleBackground").GetComponent<Animator>().SetInteger("dir", 1);
        }
        AttackerAnimation.gameObject.SetActive(true);
        if(Attacker.GetComponent<Robot>().CUR_WEAPON!=null)
        {
            AnimationUI.Find("Dialog").GetChild(0).Find("Image").GetComponent<Image>().sprite = Attacker.GetComponent<Robot>().CUR_WEAPON.getAvatar();
            AnimationUI.Find("Dialog").GetChild(0).Find("Text").GetComponent<Text>().text = Attacker.GetComponent<Robot>().CUR_WEAPON.getLine();
        }
        else
        {
            AnimationUI.Find("Dialog").GetChild(0).Find("Image").GetComponent<Image>().sprite = Attacker.GetComponent<Robot>().getAttackAvatar();
            AnimationUI.Find("Dialog").GetChild(0).Find("Text").GetComponent<Text>().text = Attacker.GetComponent<Robot>().getAttackLine();
        }
      
        
       
    }
    public void DefencerAppear()
    {
        //if(Defencer.GetComponent<Robot>().race==Robot.RACE.PLAYER)
        {
            Defencer.GetComponent<Robot>().animations.gameObject.SetActive(true);
            Defencer.GetComponent<Robot>().animations.transform.GetComponent<RobotAnimations>().Appear();
        }
        //DefencerAnimation.gameObject.SetActive(true);
        //DefencerAnimation.GetComponent<RobotAnimations>().Appear();
    }
    public void AttackerAppear()
    {
        AttackerAnimation.gameObject.SetActive(true);
        AttackerAnimation.GetComponent<RobotAnimations>().Appear();
    }
    public void EnemyBeHit()
    {
        if(Defencer.GetComponent<Robot>().race==Robot.RACE.ENEMY)
        {
           
            //CaculateDamge(Defencer);
            if (Defencer.GetComponent<Robot>().curHP <= 0)
            {
                DefencerAnimation.GetComponent<Animator>().Play("die");
            }
            else
            DefencerAnimation.GetComponent<Animator>().Play("beHit");
        }
        if(Attacker.GetComponent<Robot>().race==Robot.RACE.ENEMY)
        {
          
            //CaculateDamge(Attacker);
            if (Attacker.GetComponent<Robot>().curHP <= 0)
            {
                AttackerAnimation.GetComponent<Animator>().Play("die");
            }
            else
                AttackerAnimation.GetComponent<Animator>().Play("beHit");
        }
        
    }
    public void PlayerBeHit()
    {
        if (Defencer.GetComponent<Robot>().race == Robot.RACE.PLAYER)
        {
           
            CaculateDamge(DefencerAnimation);
            if(Defencer.GetComponent<Robot>().curHP<=0)
            {
                DefencerAnimation.GetComponent<Animator>().Play("die");
            }
            else
            {
                DefencerAnimation.GetComponent<Animator>().Play("beHit");
            }
        }
        if (Attacker.GetComponent<Robot>().race == Robot.RACE.PLAYER)
        {
           
            CaculateDamge(AttackerAnimation);
            if(Attacker.GetComponent<Robot>().curHP<=0)
            {
                AttackerAnimation.GetComponent<Animator>().Play("die");
            }
            else
            AttackerAnimation.GetComponent<Animator>().Play("beHit");
        }
    }
    public void CaculateDamge(Transform r)

    {
        if(r==AttackerAnimation)
        {
           int value= GameObject.Find("GameManager").GetComponent<gameManager>().CalculateDamage(Defencer.GetComponent<Robot>(),Attacker.GetComponent<Robot>());
            if (Attacker.GetComponent<Robot>().race == Robot.RACE.PLAYER)
            {
                AnimationUI.GetComponent<AnimationUI>().deductPlayerHP(value);
               
            }
            else if (Attacker.GetComponent<Robot>().race == Robot.RACE.ENEMY)
            {
                AnimationUI.GetComponent<AnimationUI>().deductEnemyHP(value);
               
            }
            Attacker.GetComponent<Robot>().beHit(value);

        }
        else if(r==DefencerAnimation)
        {
             int value=GameObject.Find("GameManager").GetComponent<gameManager>().CalculateDamage(Attacker.GetComponent<Robot>(), Defencer.GetComponent<Robot>());
           if(Defencer.GetComponent<Robot>().race==Robot.RACE.PLAYER)
            {
                AnimationUI.GetComponent<AnimationUI>().deductPlayerHP(value);

            }
           else if(Defencer.GetComponent<Robot>().race==Robot.RACE.ENEMY)
            {
                AnimationUI.GetComponent<AnimationUI>().deductEnemyHP(value);
            }
            Defencer.GetComponent<Robot>().beHit(value);
        }
    }
        
    public void backgroundStop()
    {
        transform.Find("battleBackground").GetComponent<Animator>().SetInteger("dir", 0);
    }
    public void enemyDamageShowed()
    {
        if(Attacker.GetComponent<Robot>().race==Robot.RACE.ENEMY)
        {
            AnimationUI.gameObject.SetActive(false);
            AttackerAnimation.gameObject.SetActive(false);
            AttackerAnimation.gameObject.SetActive(false);
            GM.AnimationFishished();
            GM.enemyAI = true;
            if(Attacker.GetComponent<Robot>().enemyCount==GM.enemyList.Count)
            {
                GM.BeginRound();
            }
            gameObject.SetActive(false);
        }
        if(Defencer.GetComponent<Robot>().race==Robot.RACE.ENEMY)
        {
            if (Defencer.GetComponent<Robot>().CUR_WEAPON != null)
            {
                DefencerAnimation.GetComponent<Animator>().Play(Defencer.GetComponent<Robot>().CUR_WEAPON.animationName);
                transform.Find("battleBackground").GetComponent<Animator>().SetInteger("dir", 1);
            }
                
            else
            {
                AnimationUI.gameObject.SetActive(false);
                AttackerAnimation.gameObject.SetActive(false);
                AttackerAnimation.gameObject.SetActive(false);
                GM.AnimationFishished();
                gameObject.SetActive(false);
            }
        }
    }
    public void playerDamageShowed()
    {
        if (Attacker.GetComponent<Robot>().race == Robot.RACE.PLAYER)
        {
            AnimationUI.gameObject.SetActive(false);
            AttackerAnimation.gameObject.SetActive(false);
            AttackerAnimation.gameObject.SetActive(false);
            GM.AnimationFishished();
            gameObject.SetActive(false);
            return;
        }
        else if(Defencer.GetComponent<Robot>().race==Robot.RACE.PLAYER)
        {
            if (Defencer.GetComponent<Robot>().CUR_WEAPON != null)
            {
                DefencerAnimation.GetComponent<Animator>().Play(Defencer.GetComponent<Robot>().CUR_WEAPON.animationName);
                transform.Find("battleBackground").GetComponent<Animator>().SetInteger("dir", 2);
            }
            else
            {
                AnimationUI.gameObject.SetActive(false);
                AttackerAnimation.gameObject.SetActive(false);
                AttackerAnimation.gameObject.SetActive(false);
                GM.AnimationFishished();
                gameObject.SetActive(false);
            }
        }
    }
    public void playerENCosted()
    {
        if (Attacker.GetComponent<Robot>().race == Robot.RACE.PLAYER)
        {
            if (Attacker.GetComponent<Robot>().CUR_WEAPON != null)
            {
                AttackerAnimation.GetComponent<Animator>().Play(Attacker.GetComponent<Robot>().CUR_WEAPON.animationName);
                transform.Find("battleBackground").GetComponent<Animator>().SetInteger("dir", 2);
            }

        }
        if (Defencer.GetComponent<Robot>().race == Robot.RACE.PLAYER)
        {
            if (Defencer.GetComponent<Robot>().CUR_WEAPON != null)
            {
                DefencerAnimation.GetComponent<Animator>().Play(Defencer.GetComponent<Robot>().CUR_WEAPON.animationName);
                transform.Find("battleBackground").GetComponent<Animator>().SetInteger("dir", 2);
            }
        }
        if (Attacker.GetComponent<Robot>().CUR_WEAPON != null)
        {
            AnimationUI.Find("Dialog").GetChild(0).Find("Image").GetComponent<Image>().sprite = Attacker.GetComponent<Robot>().CUR_WEAPON.getAvatar();
            AnimationUI.Find("Dialog").GetChild(0).Find("Text").GetComponent<Text>().text = Attacker.GetComponent<Robot>().CUR_WEAPON.getLine();
        }
        else
        {
            AnimationUI.Find("Dialog").GetChild(0).Find("Image").GetComponent<Image>().sprite = Attacker.GetComponent<Robot>().getAttackAvatar();
            AnimationUI.Find("Dialog").GetChild(0).Find("Text").GetComponent<Text>().text = Attacker.GetComponent<Robot>().getAttackLine();
        }
    }
    public void enemyENCosted()
    {
        if(Attacker.GetComponent<Robot>().race==Robot.RACE.ENEMY)
        {
            if(Attacker.GetComponent<Robot>().CUR_WEAPON!=null)
            {
                string test = Attacker.GetComponent<Robot>().CUR_WEAPON.animationName;
                AttackerAnimation.GetComponent<Animator>().Play(test);
                
                transform.Find("battleBackground").GetComponent<Animator>().SetInteger("dir", 1);

            }
            
        }
        if (Defencer.GetComponent<Robot>().race == Robot.RACE.ENEMY)
        {
            if (Defencer.GetComponent<Robot>().CUR_WEAPON != null)
            {
                DefencerAnimation.GetComponent<Animator>().Play(Defencer.GetComponent<Robot>().CUR_WEAPON.animationName);
                transform.Find("battleBackground").GetComponent<Animator>().SetInteger("dir", 1);
            }
        }
        if (Attacker.GetComponent<Robot>().CUR_WEAPON != null)
        {
            AnimationUI.Find("Dialog").GetChild(0).Find("Image").GetComponent<Image>().sprite = Attacker.GetComponent<Robot>().CUR_WEAPON.getAvatar();
            AnimationUI.Find("Dialog").GetChild(0).Find("Text").GetComponent<Text>().text = Attacker.GetComponent<Robot>().CUR_WEAPON.getLine();
        }
        else
        {
            AnimationUI.Find("Dialog").GetChild(0).Find("Image").GetComponent<Image>().sprite = Attacker.GetComponent<Robot>().getAttackAvatar();
            AnimationUI.Find("Dialog").GetChild(0).Find("Text").GetComponent<Text>().text = Attacker.GetComponent<Robot>().getAttackLine();
        }

    }
}
