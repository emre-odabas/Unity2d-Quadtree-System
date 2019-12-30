using System.Collections;
using System.Collections.Generic;
using Peril.Physics;
using TMPro;
using UnityEngine;

public class CollisionSystem : MonoBehaviour {
    public enum CollisionSystemType {
        Brute,
        QuadTree
    }

    public TextMeshProUGUI txtTotalObject;

    public PhysicsBody PhysicsBody;

    public int MaxScaleSize = 6;

    public GameObject refDivisionParticle;

    private int rndBody;

    [Header ("CollisionSystem Settings")]
    public CollisionSystemType CSType;
    public int MaxBodies = 500;

    [Header ("QuadTree Settings")]
    public Vector2 WorldSize = new Vector2 (200, 200);
    public int BodiesPerNode = 6;
    public int MaxSplits = 6;

    public QuadTree _quadTree;
    public List<IQuadTreeBody> _quadTreeBodies = new List<IQuadTreeBody> ();
    private CollisionSystemQuadTree _csQuad;
    private CollisionSystemBrute _csBrute = new CollisionSystemBrute ();

    private void Start () {

        _quadTree = new QuadTree (new Rect (0, 0, WorldSize.x, WorldSize.y), BodiesPerNode, MaxSplits);
        _csQuad = new CollisionSystemQuadTree (_quadTree);

        //Başlangıçta belirttiğimiz kadar circleyi rasgele pozisyonlarla yerleştirdik
        for (int i = 0; i < MaxBodies; i++) {
            var body = GameObject.Instantiate<PhysicsBody> (PhysicsBody);
            body.transform.parent = GameObject.FindGameObjectWithTag ("objects").transform;

            var posX = Random.Range (0, WorldSize.x);
            var posY = Random.Range (0, WorldSize.y);
            body.transform.position = new Vector2 (posX, posY);

            var lScale = Random.Range (1, MaxScaleSize);
            body.transform.localScale = new Vector2 (lScale, lScale);
        }

    }

    private void Update () {
        switch (CSType) {
            case CollisionSystemType.Brute:
                _csBrute.Step ();
                break;
            case CollisionSystemType.QuadTree:
                _csQuad.Step ();
                break;
        }

        _quadTree.Clear ();
        foreach (var b in _quadTreeBodies) {
            _quadTree.AddBody (b);
        }

    }

    private void FixedUpdate () {
        setTotalObject (); // Toplam obje sayısını güncel tuttuk
    }

    private void OnDrawGizmos () {
        if (_quadTree == null) return;
        _quadTree.DrawGizmos ();
    }

    public void setTotalObject () {
        txtTotalObject.text = "TOTAL\n" + GameObject.FindGameObjectWithTag ("objects").transform.childCount.ToString ();
    }

    //yeni obje oluşturmak için
    public void divisionBody () {
        StartCoroutine ("rndSpawnBody");
    }

    IEnumerator rndSpawnBody () {

        float bodyScale = UnityEngine.Random.Range (1f, MaxScaleSize);
        Vector3 vcRndBody = new Vector3 (UnityEngine.Random.Range (0, WorldSize.x), UnityEngine.Random.Range (0, WorldSize.y));
        GameObject divisionParticle = Instantiate (refDivisionParticle, vcRndBody, Quaternion.identity);
        ParticleSystem.MainModule pMain = divisionParticle.GetComponent<ParticleSystem> ().main;
        pMain.startSize = bodyScale;

        yield return new WaitForSeconds (2);

        var body = GameObject.Instantiate<PhysicsBody> (PhysicsBody);
        body.transform.position = vcRndBody;
        body.transform.localScale = new Vector2 (bodyScale, bodyScale);
        body.transform.parent = GameObject.FindGameObjectWithTag ("objects").transform;
    }

}