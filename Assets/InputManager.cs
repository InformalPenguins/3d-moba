using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour {
    public string Q = "q", W = "w", E = "e", R = "r";
    public string STOP = "s";
    PlayerController playerController;
    [Tooltip("0|1 For Left|Right Click")]
    public int MoveClickButton = 1;
    public void setController(PlayerController playerController){
        this.playerController = playerController;
    }
    float eDelay = 10f, lastE = 0;

    // Update is called once per frame
    void Update () {
        lastE -= Time.deltaTime;

        if(Input.GetKey(Q)){
            playerController.attackQ ();
        }
        print (lastE);
        if(Input.GetKey(E) && lastE <= 0){
            lastE = eDelay;
            Vector3 mousePosition = getMousePosition (); //            mouseWaiting = mouseDelay;
            float distance = Vector3.Distance(playerController.transform.position, mousePosition);
            if(distance <= 5f && mousePosition != null)
            {
                playerController.attackE (mousePosition);
            }
        }
        if(Input.GetKey(W)){
            playerController.attackW ();
        }
        if(Input.GetKey(R)){
            playerController.attackR ();
        }

        if(Input.GetKey(STOP)){
            playerController.stopActions();
        }
        checkMouseClicked();
    }
    private Vector3 getMousePosition(){
        //            mouseWaiting = mouseDelay;
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100))
        {
            return hit.point;
        }
        throw new NullReferenceException ("Mouse out of bounds");
    }

    //float mouseWaiting = 0f;
//    private float mouseDelay = 0.5f; //detect every 2 seconds.
    private void checkMouseClicked(){
        //        mouseWaiting -= Time.deltaTime;
        //if(mouseWaiting <= 0 && Input.GetMouseButton (MoveClickButton))
        if(Input.GetMouseButton (MoveClickButton))
        {
            try {
                Vector3 mousePosition = getMousePosition (); //            mouseWaiting = mouseDelay;
                if(mousePosition != null) {
                    playerController.localMoveToLocation (mousePosition);
                }
            } catch(NullReferenceException e){
                print ("expected mouse out of bounds click");
            }
        }
    }
}
