using UnityEngine;

public class DeleteParticle : MonoBehaviour {

    //Particle oluştuktan sonra belirttiğimiz sürede yok olmasını sağladık
    public float deleteTime = 1f;
    void Start () {
        Destroy (gameObject, deleteTime);
    }

}