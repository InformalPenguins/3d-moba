using UnityEngine;
using System.Collections;
using System;

public class MobaCamera : MonoBehaviour {
    public const float HYPH = 0.70710666564f;

    [Tooltip("A transform that this gameobject will follow.")]
    public Transform Target;
    [Tooltip("KeyCode to lock to the Target")]
    [Space(1), Header("Keys")]
    public string CAMERA_LOCK = "space";

    private Camera camera;
//    public float MinDistance = 50;
//    public float MaxDistance = 125;
    [Space(1), Header("Zoom Settings")]
    public float ZoomSpeed = 100f;
    //    private float MinY = 3f, MaxY = 8.4f;
    public float maxFieldOfView = 50f;
    public float minFieldOfView = 11f;
    
    
    [Space(1), Header("Screen Movement Settings")]
    public float MovementFactor = 0.7f;
    public float MaxMovementSpeed = 40f;
    
    [Space(1), Header("DEV MODE (Not working)")]
    private float MapBorderX1 = 100f, MapBorderZ1 = 100f,
    MapBorderX2 = 160f, MapBorderZ2 = 160f;

    
    
    private float lesserMargin = 0.03f;
    private float upperMargin = 0.97f;
//    private float initialZ = 0;

    private string scrollAction = "Mouse ScrollWheel";

    private float bottomBorder, topBorder, leftBorder, rightBorder;

    private Vector3 resetPosition;

    private Vector3 vectorUp = new Vector3(-1, 1, -1),
                    vectorForward = new Vector3(HYPH, 0, HYPH),
                    vectorRight = new Vector3(HYPH, 0, -HYPH);

    public void Start(){
        camera = GetComponent<Camera>();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        bottomBorder = screenHeight * lesserMargin;
        leftBorder = screenWidth * lesserMargin;
        rightBorder = screenWidth * upperMargin;
        topBorder = screenHeight * upperMargin;

        resetPosition = transform.position - Target.position;
        print ("reset position : " + resetPosition);
    }

    void LateUpdate()
    {
        Vector3 translation = Vector3.zero;
        if(Input.GetKey(CAMERA_LOCK)){
            //You can only lock the camera or move, otherwise it jumps while doing both.
            lockCamera ();
        } else {
            translation = checkMousePosition(translation, Input.mousePosition.x, Input.mousePosition.y);
            
            Vector3 newTranslation = camera.transform.position + translation;

            //TODO: Keep cam inside the map boundaries.
//            bool inMapRange = newTranslation.x < MapBorderX2 && newTranslation.x > MapBorderX1 && newTranslation.z > MapBorderZ1 && newTranslation.z < MapBorderZ2;
//            inMapRange = true;
//            if(inMapRange){
                camera.transform.position = newTranslation;
//            }
        }

        //Handling zoom in out.
        float zoomDelta = Input.GetAxis(scrollAction) * Time.deltaTime;
        if (Math.Abs (zoomDelta) > 0) {
            camera.fieldOfView -= zoomDelta * ZoomSpeed ;

            if(camera.fieldOfView > maxFieldOfView){
                camera.fieldOfView = maxFieldOfView;
            }
            if(camera.fieldOfView < minFieldOfView){
                camera.fieldOfView = minFieldOfView;
            }
        }
    }

    private Vector3 checkMousePosition(Vector3 translation, float x, float y){
        float horizontalValue = 0,
        verticalValue = 0;
       

        if( x <= leftBorder ){
//            horizontalValue = leftBorder - x;
            horizontalValue = x - leftBorder;
        } else if(x >= rightBorder){
            horizontalValue = x - rightBorder;
        }
        
        if( y <= bottomBorder ){
//            verticalValue = bottomBorder - y;
            verticalValue = y - bottomBorder;
        } else if(y >= topBorder){
            verticalValue = y - topBorder;
        }

        if(horizontalValue > MaxMovementSpeed){
            horizontalValue = MaxMovementSpeed;
        } else if(horizontalValue < -MaxMovementSpeed){
            horizontalValue = -MaxMovementSpeed;
        }

        if(verticalValue > MaxMovementSpeed){
            verticalValue = MaxMovementSpeed;
        } else if(verticalValue < -MaxMovementSpeed){
            verticalValue = -MaxMovementSpeed;
        }

        
        float localMovementFactor = MovementFactor * Time.deltaTime;
        if( Math.Abs(horizontalValue) > 0){
//            Vector3 hz = horizontalValue > 0 ? vectorRight : -vectorRight;
            
            translation += vectorRight * localMovementFactor * horizontalValue;
            //            translation += new Vector3(horizontalValue * localMovementFactor, 0, verticalValue * localMovementFactor);
        }
        if( Math.Abs(verticalValue) > 0){
//            Vector3 vrt = verticalValue > 0 ? vectorForward : -vectorForward ;
            
            translation += vectorForward * localMovementFactor * verticalValue;
        }
        return translation;
    }
//    void OnDrawGizmos() {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireCube(transform.position, new Vector3(2, 2, 2));
//    }
    private void lockCamera(){
//        print ("Input.mousePosition: " + Input.mousePosition); 
//        print ("camera.transform.position: " + camera.transform.position);

        if (Target != null)
        {
            transform.position = new Vector3(Target.position.x + resetPosition.x, transform.position.y, Target.position.z + resetPosition.z);
        }
    }
}
