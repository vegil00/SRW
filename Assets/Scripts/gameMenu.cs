using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameMenu : MonoBehaviour {

    // Use this for initialization
    public gameManager GM;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Find("BG").Find("RoundCount").GetComponent<Text>().text = GM.RoundCount.ToString();
	}
    public void EndTurn()
    {
        transform.Find("EndTurn").gameObject.SetActive(true);
    }
    public void EndTurnYes()
    {
        GM.EndTurn();
        transform.Find("EndTurn").gameObject.SetActive(false);
        transform.gameObject.SetActive(false);
        
    }
    public void ENdTurnNo()
    {
        transform.Find("EndTurn").gameObject.SetActive(false);
    }
    public void showTarget()
    {
        if(transform.Find("Target").gameObject.activeSelf)
        {
            transform.Find("Target").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("Target").gameObject.SetActive(true);
        }
    }
    public void returnGame()
    {
        transform.gameObject.SetActive(false);
    }
    public void returnTitle()
    {
        transform.Find("MainMenu").gameObject.SetActive(true);
    }
    public void mainmenuYes()
    {
        SceneManager.LoadScene(0);
    }
    public void mainMenuNo()
    {
        transform.Find("EndTurn").gameObject.SetActive(false);
    }
}
