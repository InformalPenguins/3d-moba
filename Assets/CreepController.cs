using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepController : MonoBehaviour {
    private UnityEngine.AI.NavMeshAgent agent;
    private GameObject target;

    // Use this for initialization
    void Awake () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void setTarget(GameObject target){
        this.target = target;
        moveToLocation (target.transform.position);
    }

    public void moveToLocation(Vector3 location){
        //transform.LookAt(location);
        agent.destination = location;
        agent.updateRotation = false;
        //stopAttacks ();
    }
    private void checkNavMesh(){
        //For walking to terrain distance can be 0
        // If enemy selected, then distance can be the min basic attack range
        if(agent.pathPending){
            return;
        }

        if(agent.hasPath && agent.remainingDistance > 0f){
            //Moving to target
        } else {
            stopMoving();
        }
    }
    private void stopMoving(){
        agent.velocity = Vector3.zero;
    }
}
