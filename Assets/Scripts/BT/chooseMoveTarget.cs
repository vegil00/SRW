using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class chooseMoveTarget : Action {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public override TaskStatus OnUpdate()
    {
        Robot robot = GetComponent<Robot>();
        robot.showMoveRange();
        robot.chooseTarget();
        return TaskStatus.Success;
    }
}
