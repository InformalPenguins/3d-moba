using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public string Q = "q", W = "w", E = "e", R = "r";
    public string STOP = "s";
    PlayerController playerController;
    [Tooltip("0|1 For Left|Right Click")]
    public int MoveClickButton = 1;
    public void setController(PlayerController playerController){
        this.playerController = playerController;
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKey(Q)){
            playerController.attackQ ();
        }
        if(Input.GetKey(E)){
            playerController.attackE ();
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

    //float mouseWaiting = 0f;
//    private float mouseDelay = 0.5f; //detect every 2 seconds.
    private void checkMouseClicked(){
        //        mouseWaiting -= Time.deltaTime;
        //if(mouseWaiting <= 0 && Input.GetMouseButton (MoveClickButton))
        if(Input.GetMouseButton (MoveClickButton))
        {
            //            mouseWaiting = mouseDelay;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100))
            {
                playerController.localMoveToLocation (hit.point);
            }
        }
    }
}
