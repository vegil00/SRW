using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class chooseWeapon : Action {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override TaskStatus OnUpdate()
    {

        Robot robot = transform.GetComponent<Robot>();
        robot.chooseWeapon();
            return TaskStatus.Success;
       
    }
}
