using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHandler : MonoBehaviour, INetworkHandler
{
    void INetworkHandler.processMessage(string text)
    {
        if (text == null || text.Length == 0)
        {
            return;
        }
        Debug.Log(">> " + text);

        MainThreadProcessor.Instance().Enqueue(MainThreadProcessor.Instance().processMessage(text));
    }

}
