using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    private Animator anim;
    private NavMeshAgent agent;

    public GameObject cone;
    public GameObject swamp;
    public string Q = "q", W = "w", E = "e", R = "r";
    public string STOP = "s";

    public Transform arrows;
    
    [Tooltip("0|1 For Left|Right Click")]
    public int MoveClickButton = 1;
    public bool isAI = false;
    public float MovementSpeed = 100f;
    
    LineRenderer line;
    private NavMeshPath path;
    // Use this for initialization
    void Start () {
        Transform model = transform.Find("Model");
        anim = model.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = MovementSpeed;
        line = GetComponent<LineRenderer>();
        path = new NavMeshPath();

//        agent.updateRotation = false;

        if(isAI){
//            moveToLocation( new Vector3(125.2f, 0.0f, 123.9f));
        }
    }

    void Update () {
        if(!isAI){
            anim.SetBool("isQ", Input.GetKey(Q));
            anim.SetBool("isE", Input.GetKey(E));
            if(Input.GetKey(E)){
                Cone ();
            }
            if(Input.GetKey(W)){
                Swamp ();
            }
            anim.SetBool("isW", Input.GetKey(W));
            anim.SetBool("isR", Input.GetKey(R));
            
            if(Input.GetKey(STOP)){
                this.stopActions();
            }
            checkMouseClicked();
        }
    }
        void FixedUpdate(){
            checkNavMesh();
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
            print("agent.remainingDistance: " + agent.remainingDistance);
        } else {
            stopMoving();
//            DrawPath(agent.path);
//            DrawPath(this.path);

//            for (int i = 0; i < this.path.corners.Length-1; i++)
//                Debug.DrawLine(this.path.corners[i], this.path.corners[i+1], Color.red, 1, true);  
        }
    }
    float mouseWaiting = 0f;
    private float mouseDelay = 0.5f; //detect every 2 seconds.
    private void checkMouseClicked(){
//        mouseWaiting -= Time.deltaTime;
        if(mouseWaiting <= 0 && Input.GetMouseButton (MoveClickButton))
        {
//            mouseWaiting = mouseDelay;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            
            if(Physics.Raycast(ray, out hit, 100))
            {
                arrows.position = hit.point + (Vector3.up * 0.5f);
                arrows.GetComponent<Animator>().Play("idle", -1, 0f);
                arrows.gameObject.SetActive(true);
                moveToLocation (hit.point);
            }
        }
    }
    private void DrawPath(NavMeshPath path){
        //TODO: Fix, not working - always renders from 0 0 0 --- ):
        if(path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;
        
        line.SetVertexCount(path.corners.Length); //set the array of positions to the amount of corners
        
        for(var i = 1; i < path.corners.Length; i++){
            line.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }
    private void moveToLocation(Vector3 location){
//        NavMesh.CalculatePath(transform.position, location, NavMesh.AllAreas, path);
//        agent.Resume();
//        agent.velocity = Vector3.zero;
        transform.LookAt(location);
        anim.SetBool("moving", true);

        agent.destination = location;
    }

    private void stopMoving(){
        //agent.destination = transform.position;
        anim.SetBool("moving", false);
        agent.velocity = Vector3.zero;
//        agent.Stop();
    }

    protected void stopActions(){
        stopMoving();
    }
    private void attack(){
        anim.SetBool("isAttacking", true);
    }
    private void Cone(){
        if(cone.activeInHierarchy){
            print("Cone already active.");
            return;
        }
        cone.SetActive(true);
        StartCoroutine("FadeCone");
    }
    
    private IEnumerator FadeCone(){
        yield return new WaitForSeconds(3f);
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
}
