using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {
    public GameObject creep;
    public GameObject target;
    private float waveDelay = 3f, lastWave = 1;
    public Transform spawner;
    private Vector3 spawnLocation;
    private Quaternion spawnRotation;
    void Awake(){
        spawnLocation = spawner.transform.position;
        spawnRotation = spawner.transform.localRotation;
    }
    void FixedUpdate () {
        lastWave -= Time.deltaTime;
        if(lastWave <= 0){
            lastWave = waveDelay;
            spawnWave ();
        }
    }
    private void spawnDelayed(int amount){
        int delay = 1;
        for(int i = 0; i< amount; i++){
            //One behind the other by delay seconds
            StartCoroutine(spawnDelayedRoutine(amount * delay));
        }
    }
    private IEnumerator spawnDelayedRoutine(float delay){
        yield return new WaitForSeconds(delay);
        GameObject creep = getCreep ();
        CreepController creepController = creep.GetComponent<CreepController>();
        //move to enemy nexus
        creepController.setTarget (target);
    }

    private void spawnWave(){
        spawnDelayed (2);
    }


    GameObject getCreep(){
        GameObject clonedCreep = Instantiate (creep, spawnLocation, spawnRotation) as GameObject;
        return clonedCreep;
    }
}
