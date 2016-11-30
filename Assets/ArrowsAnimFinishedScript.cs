using UnityEngine;
using System.Collections;

public class ArrowsAnimFinishedScript : MonoBehaviour {
    Animator myAnimator;
    void Start(){
        myAnimator = GetComponent<Animator>();
    }
    public void ArrowsAnimFinished(){
        gameObject.SetActive(false);
    }
}
