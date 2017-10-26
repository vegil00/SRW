using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimations : MonoBehaviour {
    public Transform animationCamera;
    public AudioSource audio;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   public void EnemyAppear()
    {
        if(transform == animationCamera.GetComponent<AnimateCamera>().AttackerAnimation)
        animationCamera.GetComponent<AnimateCamera>().DefencerAppear();
        else if(transform == animationCamera.GetComponent<AnimateCamera>().DefencerAnimation)
        {
            animationCamera.GetComponent<AnimateCamera>().AttackerAppear();
        }
    }
    public void turnToIdle()
    {
        transform.GetComponent<Animator>().Play("idle");

    }
    public void Appear()
    {
        transform.GetComponent<Animator>().Play("Appear");
    }
    public void Disappear()
    {
        gameObject.SetActive(false);
       
    }
    public void EnemyBeHit()
    {
        transform.GetComponentInParent<AnimateCamera>().EnemyBeHit();
    }
    public void PlayerBeHit()
    {
        transform.GetComponentInParent<AnimateCamera>().PlayerBeHit();
    }
    public void showDamage()
    {
        animationCamera.GetComponent<AnimateCamera>().CaculateDamge(transform);
    }
    public void AnimationEnd()
    {
        animationCamera.GetComponent<AnimateCamera>().attackAnimationComplete(transform);
    }
    public void playAudio()
    {
        //if(animationCamera.GetComponent<AnimateCamera>().AttackerAnimation==this)
        {
            animationCamera.GetComponent<AnimateCamera>().GM.cur_source.Stop();
            animationCamera.GetComponent<AnimateCamera>().GM.cur_source = audio;
            animationCamera.GetComponent<AnimateCamera>().GM.cur_source.Play();

        }
    }
    public void Die()
    {
        animationCamera.GetComponent<AnimateCamera>().GM.enemyRound = gameManager.ENEMYROUNDPROC.IDLE;
        animationCamera.GetComponent<AnimateCamera>().AnimationUI.gameObject.SetActive(false);
        animationCamera.GetComponent<AnimateCamera>().GM.AnimationFishished();
        animationCamera.gameObject.SetActive(false);
        
    }
    public void getTarget()
    {

    }
}
