using UnityEngine;

public class HideMeshOnPlay : MonoBehaviour {
    public Renderer Mesh;
    void Start () {
        Mesh.enabled = false;
        this.enabled = false;
    }
}
