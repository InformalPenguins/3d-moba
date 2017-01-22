
/*
 
    -----------------------
    UDP-Send
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
    // > gesendetes unter
    // 127.0.0.1 : 8050 empfangen
   
    // nc -lu 127.0.0.1 8050
 
        // todo: shutdown thread at the end
*/
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSender : MonoBehaviour
{
    private static int localPort;

    private string serverIP;
    private int serverPort;

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient server;

    // gui
    string strMessage="";
    GameLogic gameLogic;

//    // call it from shell (as program)
//    private static void Main()
//    {
//        UDPSend sendObj=new UDPSend();
//        sendObj.init();
//
//        // testing via console
//        // sendObj.inputFromConsole();
//
//        // as server sending endless
//        sendObj.sendEndless(" endless infos \n");
    //}

    // start from unity3d
    public void Start()
    {
        Application.runInBackground = true;
        gameLogic = GameObject.Find ("GameLogic").GetComponent<GameLogic>();
        init();
    }

    // OnGUI
//    void OnGUI()
//    {
//        Rect rectObj=new Rect(40,380,200,400);
//        GUIStyle style = new GUIStyle();
//        style.alignment = TextAnchor.UpperLeft;
//        GUI.Box(rectObj,"# UDPSend-Data\n127.0.0.1 "+serverPort+" #\n"
//            + "shell> nc -lu 127.0.0.1  "+serverPort+" \n"
//            ,style);
//
//        // ------------------------
//        // send it
//        // ------------------------
//        strMessage=GUI.TextField(new Rect(40,420,140,20),strMessage);
//        if (GUI.Button(new Rect(190,420,40,20), "send"))
//        {
//            sendString(strMessage+"\n");
//        }      
//
//
//        rectObj = new Rect(40,10,200,400);
//
//        style = new GUIStyle();
//        style.alignment = TextAnchor.UpperLeft;
//        GUI.Box(rectObj,"# UDPReceive\n127.0.0.1 "+serverPort+" #\n"
//            + "shell> nc -u 127.0.0.1 : "+serverPort+" \n"
//            + "\nLast Packet: \n"+ lastReceivedUDPPacket
//            + "\n\nAll Messages: \n"+allReceivedUDPPackets
//            ,style);
//    }
    Thread receiveThread;
    // init
    public void init()
    {
        serverIP = PlayerPrefs.GetString("serverIp");
        if (serverIP.Trim().Equals ("")) {
            serverIP = "127.0.0.1";
        }
        serverPort=8051;

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
        server = new UdpClient();
        server.EnableBroadcast = true;

        print("Sending to "+serverIP+" : "+serverPort);

        receiveThread = new Thread(new ThreadStart(handleServer));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        sendString (MobaConstants.ACTION_SERVER_LOGIN);
    }
    // infos
    public string lastReceivedUDPPacket="";
    public string allReceivedUDPPackets=""; // clean up this from time to time!
    private void handleServer(){
        while (true)
        {

            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = server.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                processText(text);
                print(">> " + text);
                lastReceivedUDPPacket=text;
                allReceivedUDPPackets=allReceivedUDPPackets+text;
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    // inputFromConsole
    private void inputFromConsole()
    {
        try
        {
            string text;
            do
            {
                text = Console.ReadLine();

                // Den Text zum Remote-Client senden.
                if (text != "")
                {

                    // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
                    byte[] data = Encoding.UTF8.GetBytes(text);

                    // Den Text zum Remote-Client senden.
                    server.Send(data, data.Length, remoteEndPoint);
                }
            } while (text != "");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }

    }

    // sendData
    public void sendString(string message)
    {
        try
        {
            //if (message != "")
            //{

            // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Den message zum Remote-Client senden.
            server.Send(data, data.Length, remoteEndPoint);
            //}
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }


    // endless test
    private void sendEndless(string testStr)
    {
        do
        {
            sendString(testStr);


        }
        while(true);

    }
    private void processText(string text){
        if(text == null || text.Length == 0){
            return;
        }

        GameLogic.Instance().Enqueue(gameLogic.processText(text));
    }

}