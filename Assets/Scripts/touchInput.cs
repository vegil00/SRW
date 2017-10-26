using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class touchInput : MonoBehaviour {
    public GameObject battlemenu;
	// Use this for initialization
	void Start () {
       // battlemenu =GameObject.Find("battleMenu");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnPointerClick(BaseEventData data)
    {
        Debug.Log("click");
        
        if(battlemenu.activeSelf)
        {
            battlemenu.SetActive(false);
        }
        else
        {
            battlemenu.transform.position = transform.position;
            battlemenu.SetActive(true);
        }
    }
}
