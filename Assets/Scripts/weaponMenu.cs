using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponMenu : MonoBehaviour {

    // Use this for initialization
   public Robot robot;
    public GameObject[] weaponButtons;
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   public void activate(Robot r)
    {
        Debug.Log("WeaponMenu");
        robot = r;
        int count = 0;
        foreach(KeyValuePair<string,Weapon> pair in robot.WEAPONLIST)
        {
            
            weaponButtons[count].SetActive(true);
           
            weaponButtons[count].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = pair.Key;
            weaponButtons[count].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = pair.Value.ATTACK.ToString();
            weaponButtons[count].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = pair.Value.MINRANGE.ToString() + "~" + pair.Value.MAXRANGE.ToString();
            weaponButtons[count].transform.GetChild(3).GetChild(0).GetComponent<Text>().text ="+"+ pair.Value.MAXRANGE.ToString();
            weaponButtons[count].transform.GetChild(0).GetComponent<weapomButton>().weaponname = pair.Key;
            count++;
        }
    }
    public void diactivate()
    {
        Debug.Log("diactivate");
       foreach(GameObject child in weaponButtons)
        {
            if(child.activeSelf)
            {
                child.SetActive(false);
            }
        }
        gameObject.SetActive(false);
    }
}
