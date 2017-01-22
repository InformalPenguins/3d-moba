using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: This class must be shared along with the server
public class MobaConstants  {
    //TYPE OF UPDATABLES
    public const int TYPE_PLAYER = 0;
    public const int TYPE_MINION = 1;

    //IN GAME ACTIONS POSITION/ROTATION
    public const int ACTION_UPDATE_POSITION = 0;
//    public const int ACTION_ATTACK = 1;
//    public const int ACTION_STATUS = 2;
//    public const int ACTION_DESTROY = 3;

    // INPUT ACTIONS
    public const string INPUT_POSITION = "10 "; // Requires 4 arguments: uid x y z

    //NETWORK SERVER FUNCTIONS
    public const string ACTION_SERVER_LOGIN = "100 "; // Requires 0 arguments, receives 1: 100 | 100 uid
}
