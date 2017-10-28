using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using CreativeSpore.SuperTilemapEditor;

public class playerInRange : Conditional {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override TaskStatus OnUpdate()
    {

        Robot robot = transform.GetComponent<Robot>();
       bool result =robot.scanAbleToAttack();

 
       if(result)
        {
            return TaskStatus.Success;
        }
       else
        {
            return TaskStatus.Failure;
        }
    }
}
