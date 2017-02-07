using UnityEngine;

public abstract class IUIHandler : MonoBehaviour
{
    public abstract void processMessage(string message);
}