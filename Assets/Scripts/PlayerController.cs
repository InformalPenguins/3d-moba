﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    private Animator anim;
    private UnityEngine.AI.NavMeshAgent agent;

    public int id = 1;
    private GameObject cone;
    public GameObject conePrefab;
    public GameObject swamp;
    public static int playerIdx = -1;
    public static PlayerController self;

    public Transform arrows;

    public bool isAI = true;
    private float MovementSpeed = 6.5f; //3.5f; //Usually 2 (kind of slow or map too big)
    
    LineRenderer line;
    UDPClient udpClient;
    //private UnityEngine.AI.NavMeshPath path;
    // Use this for initialization
    void Start () {
        Transform model = transform.Find("Model");
        anim = model.GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = MovementSpeed;
        line = GetComponent<LineRenderer>();
        //path = new UnityEngine.AI.NavMeshPath();

        GameLogic.playersPool [id] = gameObject;
        udpClient = MainThreadProcessor.Instance ().GetComponent<UDPClient> ();
        cone = GameObject.Instantiate (conePrefab, transform.position, conePrefab.transform.rotation);
    }

    void Update () {
    }

    public void attackQ(){
        anim.SetBool("isQ", true);
        turnOffDelayed (0.15f, "isQ");
        stopMoving (true);
    }
    public void attackW(){
        anim.SetBool("isW", true);
        turnOffDelayed (0.33f, "isW");
        Swamp ();
        stopMoving (true);
    }
    public void attackR(){
        anim.SetBool("isR", true);
        turnOffDelayed (0.8f, "isR");
        stopMoving (true);
    }
    public void attackE(Vector3 position){
        anim.SetBool("isE", true);
        turnOffDelayed (0.4f, "isE");
        Cone (position);
        stopMoving (true);
    }
    private void Cone(Vector3 position){
        if(cone.activeInHierarchy){
            print("Cone already active.");
            return;
        }
        cone.transform.position = position;
        cone.SetActive(true);
        StartCoroutine("FadeCone");
    }


    private float syncDelay = 2f, 
                  lastDelay = 0f;
    public void FixedUpdate(){
        checkNavMesh();
        preventRotation();

        lastDelay -= Time.deltaTime;
        if(lastDelay <=0){
            resyncPositionRotation ();
            lastDelay = syncDelay;
        }
    }

    private void resyncPositionRotation(){
        if (PlayerController.playerIdx < 0) {
            return;
        }
        //Position string
        string positionString =  MobaConstants.ACTION_UPDATE_POSITION + " " + MobaConstants.TYPE_PLAYER + " " + PlayerController.playerIdx +
            " " + transform.position.x + " " + transform.position.y + " " + transform.position.z + 
            " " + transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.z + " " + transform.rotation.w;

        udpClient.sendString (positionString);
    }

    private void preventRotation(){
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }
    //    void LateUpdate(){
    ////        checkNavMesh();
    //    }
    private void checkNavMesh(){
        //For walking to terrain distance can be 0
        // If enemy selected, then distance can be the min basic attack range
        if(agent.pathPending){
            return;
        }

        if(agent.hasPath && agent.remainingDistance > 0f){
            //print("agent.remainingDistance: " + agent.remainingDistance);
        } else {
            stopMoving();
//            DrawPath(agent.path);
//            DrawPath(this.path);

//            for (int i = 0; i < this.path.corners.Length-1; i++)
//                Debug.DrawLine(this.path.corners[i], this.path.corners[i+1], Color.red, 1, true);  
        }
    }
    private void DrawPath(UnityEngine.AI.NavMeshPath path){
        //TODO: Fix, not working - always renders from 0 0 0 --- ):
        if(path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;
        
        line.SetVertexCount(path.corners.Length); //set the array of positions to the amount of corners
        
        for(var i = 1; i < path.corners.Length; i++){
            line.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }
    public void localMoveToLocation(Vector3 location){
        if (PlayerController.playerIdx < 0) {
            return;
        }
        this.moveToLocation (location);
        string locationMessage = MobaConstants.INPUT_POSITION + PlayerController.playerIdx + " " + location.x + " " + location.y + " " + location.z ;
        udpClient.sendString (locationMessage);
    }
    public void moveToLocation(Vector3 location){
//        NavMesh.CalculatePath(transform.position, location, NavMesh.AllAreas, path);
//        agent.Resume();
//        agent.velocity = Vector3.zero;
        arrows.position = location + (Vector3.up * 0.5f);
        arrows.GetComponent<Animator>().Play("idle", -1, 0f);
        arrows.gameObject.SetActive(true);

        transform.LookAt(location);
        anim.SetBool("moving", true);

        agent.destination = location;
        agent.updateRotation = false;
        stopAttacks ();
    }

    private void stopMoving(bool force){
        if (force) {
//            agent.Stop ();
            agent.destination = transform.position;
        }
        this.stopMoving ();
    }
        
    private void stopMoving(){
        anim.SetBool("moving", false);
        agent.velocity = Vector3.zero;
    }

    public void stopActions(){
        stopMoving();
        stopAttacks ();
    }
    private void turnOff(string state){
        anim.SetBool(state, false);
    }
    private void turnOffDelayed(float delay, string state){
        StartCoroutine(turnOffDelayedRoutine(delay, state));
    }
    private IEnumerator turnOffDelayedRoutine(float delay, string state){
        yield return new WaitForSeconds(delay);
        turnOff (state);
    }
    private void stopAttacks(){
        turnOff ("isE");
        turnOff ("isR");
        turnOff ("isW");
        turnOff ("isQ");
        turnOff ("isAttacking");
    }
    private void attack(){
        anim.SetBool("isAttacking", true);
    }
    private IEnumerator FadeCone(){
        yield return new WaitForSeconds(5f);
        cone.SetActive(false);
    }
    
    private void Swamp(){
        if(swamp.activeInHierarchy){
            print("Cone already active.");
            return;
        }
        swamp.SetActive(true);
        StartCoroutine("FadeSwamp");
    }
    private IEnumerator FadeSwamp(){
        yield return new WaitForSeconds(3f);
        swamp.SetActive(false);
    }

    public void LoadPositions(string positions){
        // Split string on spaces. This will separate all the words.
        string[] words = positions.Split(' ');
        int wordsNum = words.Length;

        float x, z, rot;
        Vector3 updatePos, updateRot;

        for (int i = 0; i <= wordsNum; i++) {
            x = float.Parse(words[0], System.Globalization.CultureInfo.InvariantCulture); 
            z = float.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture); 
            rot = float.Parse(words[2], System.Globalization.CultureInfo.InvariantCulture);
            Debug.Log("Reading position: " + "x: " + x + " z: " + z + " yRot: " + rot);

            updatePos = new Vector3(x, this.gameObject.transform.position.y, z);
            this.gameObject.transform.position = updatePos;

            updateRot = new Vector3(this.gameObject.transform.rotation.x, rot / 4, this.gameObject.transform.rotation.z);
            this.transform.localEulerAngles = updateRot;

            //UpdateCameraMatrix();
            //StartCoroutine(UpdateSurfs());
        }
    }

}
