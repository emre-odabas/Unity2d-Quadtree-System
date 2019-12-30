using System.Collections;
using UnityEngine;

public class CircleControl : MonoBehaviour {

    [SerializeField] private int circleHealth = 5;
    public bool isMove = true;
    public GameObject[] Particles;

    private CollisionSystem _CollisionSystem;

    private Rigidbody2D rb;

    private ParticleSystem.MainModule pMain;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D> ();
        _CollisionSystem = GameObject.FindGameObjectWithTag ("gamecontrol").GetComponent<CollisionSystem> ();

    }

    void Update () {

    }

    void OnCollisionEnter2D (Collision2D col) {
        if (isMove) {
            int randX = UnityEngine.Random.Range (-15, 15);
            int randY = UnityEngine.Random.Range (-15, 15);
            gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (randX, randY), ForceMode2D.Impulse); //Obje herhangi bir yere çarparsa rasgele güç uygulanır

            if (col.gameObject.tag == "circle") {

                circleHealth--; //Circle, başka bir circle objesine çarparsa canını bir azalttık

                if (circleHealth <= 0) {// Eğer can 0 olmuşsa palama efektini aktif ettik ve rasgele pozisyonda yeni bir circle oluşturmasını istedik
                    createParticle (false);
                    StartCoroutine ("DestroyAndDivision");
                    FindObjectOfType<AudioManager> ().Play ("DestroyCircleSound");
                }

                createParticle (true);
            }
        }

    }

    private void createParticle (bool isCollision) {

        if (isCollision) {
            GameObject collisionParticle = Instantiate (Particles[0], gameObject.transform.position, Quaternion.identity);
            pMain = collisionParticle.GetComponent<ParticleSystem> ().main;
            pMain.startSize = transform.localScale.x;
            collisionParticle.transform.parent = gameObject.transform;
            Debug.Log ("collsion");
        } else {
            GameObject destroyParticle = Instantiate (Particles[1], gameObject.transform.position, Quaternion.identity);
            pMain = destroyParticle.GetComponent<ParticleSystem> ().main;
            pMain.startSize = transform.localScale.x;
            destroyParticle.transform.parent = gameObject.transform;
            Debug.Log ("destroy");
        }
    }

    IEnumerator DestroyAndDivision () {
        yield return new WaitForSeconds (0.1f);

        Destroy (gameObject);

        _CollisionSystem.divisionBody ();
    }

}