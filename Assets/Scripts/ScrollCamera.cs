using UnityEngine;
using System;

public class ScrollCamera : MonoBehaviour {
    private Camera camera;
    
    public float MinDistance = 50;
    public float MaxDistance = 125;
    public float ScrollSpeed = 200f;
    
    public void Start(){
        camera = GetComponent<Camera>();
    }
    
    private string scrollAction = "Mouse ScrollWheel";
    
    void LateUpdate()
    {
        float scrollAxis = Input.GetAxis(scrollAction) * ScrollSpeed  * Time.deltaTime;
        if (Math.Abs (scrollAxis) > 0) {
            camera.fieldOfView -= scrollAxis;
            if (camera.fieldOfView < MinDistance) {
                camera.fieldOfView = MinDistance;
            } else if (camera.fieldOfView > MaxDistance) {
                camera.fieldOfView = MaxDistance;
            }
        }
    }
}
