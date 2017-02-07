using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {
    public GameObject creep;
    public GameObject target;
    private float waveDelay = 9f, lastWave = 1f;
    private float inBetweenDelay = 1f, lastMinion = 0;
    public Transform spawner;
    private Vector3 spawnLocation;
    private Quaternion spawnRotation;
    public bool color = false;
    public Material customMaterial;
    private int spawnAmount = 0;
    public int hp = 3;

    void Awake(){
        spawnLocation = spawner.transform.position;
        spawnRotation = spawner.transform.localRotation;
    }
    void Update () {
        lastWave -= Time.deltaTime;
        if(lastWave <= 0){
            lastWave = waveDelay;
            spawnWave ();
        }

        if (spawnAmount > 0) {
            lastMinion -= Time.deltaTime;
            if(lastMinion <= 0){
                lastMinion = inBetweenDelay;
                spawnMinion ();
            }
        }
        winCondition ();
    }
    private void winCondition(){
        if(hp < 0){
            GameLogic.Instance ().EndGame (color);
        }
    }

    private void spawnWave(){
        spawnAmount = 3;
    }
    private void spawnMinion(){
        spawnAmount--;
        GameObject creep = getCreep ();
        CreepController creepController = creep.GetComponent<CreepController>();
        //move to enemy nexus
        creepController.setTarget (target);
        if (color) {
            Renderer rend = creep.GetComponent<MeshRenderer>();
            rend.material = customMaterial;
        }
    }


    GameObject getCreep(){
        GameObject clonedCreep = Instantiate (creep, spawnLocation, spawnRotation) as GameObject;
        return clonedCreep;
    }
}
