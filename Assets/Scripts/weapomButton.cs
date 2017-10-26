using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weapomButton : MonoBehaviour {
    bool choosen;
    public GameObject menu;
    public string weaponname;
    gameManager GM;
	// Use this for initialization
	void Start () {
        choosen = false;
        GM = GameObject.Find("GameManager").GetComponent<gameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Click()
    {
        Robot r = menu.GetComponent<weaponMenu>().robot;
        if (!choosen)
        {
            choosen = true;
            transform.GetComponent<Animator>().SetBool("highLighted", true);
            r.CUR_WEAPON = r.weaponList[weaponname];
           
            
        }
        else
        {
            transform.GetComponent<Animator>().SetBool("highLighted",false);
            transform.GetComponent<Animator>().SetBool("Pressed", true);
            Debug.Log("Pressed");
          
            //GameObject.Find("GameManager").GetComponent<gameManager>().awakeBattleCorfirm(r);

            r.showWeaponRange(r.CUR_WEAPON.NAME);
            GM.playerRound = gameManager.PLAYERROUNDPROC.CHOOSEENE;
            
            menu.GetComponent<weaponMenu>().diactivate();
            
        }
    }
}
