using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MyMobaStrategy : IUIHandler {
    UDPClient udpClient;
    public InputManager inputManager;

    public static GameObject[] playersPool;
    void Start(){
        udpClient = GetComponent<UDPClient> ();
        StartCoroutine(sendLoginMessage());
        inputManager = GetComponent<InputManager> ();
        playersPool = GameLogic.playersPool;
    }
    private float waitTime = 3f;
    IEnumerator sendLoginMessage()
    {
        while (true) {
            yield return new WaitForSeconds(waitTime);
            if (udpClient.getServer () != null) {
                udpClient.sendString (NetworkConstants.ACTION_SERVER_LOGIN + " ");
                break;
            }
            print("WaitAndPrint " + Time.time);
        }
    }


    public override void  processMessage(string message) {
        Debug.Log (message);
        if(message.StartsWith(MobaConstants.ACTION_UPDATE_POSITION + " ")){
            message = message.Substring (MobaConstants.ACTION_UPDATE_POSITION.ToString().Length + 1);
            this.LoadPositions (message);
        } else if(message.StartsWith(MobaConstants.INPUT_POSITION)){
            message = message.Substring (MobaConstants.INPUT_POSITION.Length);
            this.MoveToLocation (message);
        } else if(message.StartsWith(MobaConstants.ACTION_SERVER_LOGIN)){
            message = message.Substring (MobaConstants.ACTION_SERVER_LOGIN.ToString().Length);
            this.IdentifyUser (message);
        }
    }
    private void MoveToLocation(string locationString){
        string[] words = locationString.Split(' ');
        int idx = 0;
        //uid x y z
        int userId = int.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        GameObject player = getPlayer (userId);
        if (player == null) {
            return;
        }
        float x = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        float y = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        float z = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        Vector3 location = new Vector3 (x, y, z);

        PlayerController controller = player.GetComponent<PlayerController> ();
        controller.moveToLocation (location);
    }
    //Tells u which player you are
    private void IdentifyUser(string userIdStr){
        int userId = int.Parse(userIdStr, System.Globalization.CultureInfo.InvariantCulture);
        GameObject player = getPlayer (userId);
        PlayerController controller = player.GetComponent<PlayerController> ();

        MobaCamera camera = GameObject.Find ("Camera").GetComponent<MobaCamera>();

        controller.isAI = false;
        camera.Target = controller.transform;
        PlayerController.playerIdx = userId;
        PlayerController.self = controller;
        inputManager.setController (controller);
    }

    private GameObject getPlayer(int idx){
        if(idx >= playersPool.Length || idx < 0){
            throw new IndexOutOfRangeException ("There are only " + playersPool.Length + " players");
        }
        return playersPool [idx];
    }

//    private GameObject getMinion(int type, int idx){
//        GameObject[] minionsPool;
//        //minionsPool = type == 0 ? blueMinionsPool:redMinionsPool;
//
//        if(minionsPool.Length >= idx){
//            throw new IndexOutOfRangeException ("There are only " + minionsPool.Length + " minions("+type+")");
//        }
//        return minionsPool [idx];
//    }


    //Types
    //0: Update position
    // 0 WHO  X  Y  Z  RX RY RZ
    // 0   1 20 30  0   0  0  0
    public void LoadPositions(string positions){
        // Split string on spaces. This will separate all the words.
        string[] words = positions.Split(' ');

        float x, y, z, rx, ry, rz, rw;
        int typeIdx, objId, teamId, idx = 0;
        Vector3 updatePos;
        Quaternion updateRot;
        GameObject updatedObject = null;
        //TYPE ID X Y Z RX RY RZ
        // This should move the player 1 to 92.58 -0.629693, no rotation affected.
        //   0 0 1 -92.58 -0.629693 0

        // This should move the red minion 50 to 0 0 0
        //   2 2 50 0 0 0 20 20 0 0

        typeIdx = int.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        objId = int.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);

        switch(typeIdx){
            case MobaConstants.TYPE_PLAYER:
                updatedObject = getPlayer (objId);
                break;
//            case MobaConstants.TYPE_MINION:
//                //teamId must be 0 or 1
//                teamId = int.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
//                updatedObject = getMinion (teamId, objId);
//                break;
        }

        if (updatedObject == null) {
            Debug.Log ("Object not found");
            return;
        }

        //Position
        x = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        y = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        z = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);


        //rot = float.Parse(words[2], System.Globalization.CultureInfo.InvariantCulture);
        Debug.Log("Reading position: " + "x: " + x + " y: " + y + " z: " + z);
        updatePos = new Vector3(x, y, z);
        updatedObject.transform.position = updatePos;

        /*
        //TODO: Update rotation

        //Rotation
        rx = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        ry = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        rz = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        rw = float.Parse(words[idx++], System.Globalization.CultureInfo.InvariantCulture);
        updateRot = new Quaternion(rx, ry, rz, rw);
        this.transform.rotation = updateRot;

        */
    }
}
