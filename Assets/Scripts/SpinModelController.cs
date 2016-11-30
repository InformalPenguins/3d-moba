using UnityEngine;
using System.Collections;

public class SpinModelController : MonoBehaviour {
    public float delay = 0.1f;
    private float currentTime = 0f;
    public Vector3 rotation = new Vector3(0, 1f, 0);
    // Update is called once per frame
    void Update () {
        currentTime += Time.deltaTime;
        if(currentTime >= delay){
            currentTime = 0;
            transform.Rotate(rotation);
        }
    }
}
