using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefHandler : MonoBehaviour {
    string strMessage="";
    string serverIP;
    void Start(){
        serverIP = PlayerPrefs.GetString("serverIp");
        if (serverIP.Trim().Equals ("")) {
            serverIP = "127.0.0.1";
        }
        strMessage = serverIP;
    }
    void OnGUI()
    {
        Rect rectObj=new Rect(40,380,200,400);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;

        // ------------------------
        // send it
        // ------------------------
        strMessage = GUI.TextField(new Rect(20,20,140,25), strMessage);

        if (GUI.Button(new Rect(180, 20,150,25), "Connect"))
        {
            saveServer(strMessage.Trim());
        }      
    }
    private void saveServer(string value){
        PlayerPrefs.SetString("serverIp", value);
        serverIP = value;
        Application.LoadLevel("Terrain");
    }
}
