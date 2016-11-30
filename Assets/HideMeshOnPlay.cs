using UnityEngine;

public class HideMeshOnPlay : MonoBehaviour {
    public Renderer Mesh;
    // Use this for initialization
    void Start () {
        Mesh.enabled = false;
        this.enabled = false;
    }
}
