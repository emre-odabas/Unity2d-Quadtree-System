 using Peril.Physics;
 using UnityEngine;

 public class PhysicsBody : MonoBehaviour, ICollisionBody, IQuadTreeBody {

     private ICollisionShape _shape;
     private Color _gizmoColor = Color.red;

     private void Awake () {
         var collider = GetComponent<CircleCollider2D>();
         _shape = new BoxShape (collider.bounds, false);
         _shape.Center = transform.position;
     }

     private void Start () {
         int randX = UnityEngine.Random.Range (-15, 15);
         int randY = UnityEngine.Random.Range (-15, 15);

         gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (randX, randY) * 2, ForceMode2D.Impulse);
     }

     private void Update () {
         _shape.Center = transform.position;
     }

     private void OnDrawGizmos () {
         Gizmos.color = _gizmoColor;
         Gizmos.DrawWireCube (transform.position, Vector2.one * gameObject.transform.localScale); // circle etrafındaki gizmos çizgisinin boyutunu belirledik
     }

     #region ICollisionBody

     public int RefId { get; set; }

     public bool Sleeping { get { return false; } }

     public ICollisionShape CollisionShape { get { return _shape; } }

     public void OnCollision (CollisionResult result, ICollisionBody other) {
         switch (result.Type) {
             case CollisionType.Enter:
                 _gizmoColor = Color.blue;
                    
                 break;
             case CollisionType.Exit:
                 _gizmoColor = Color.red;               
                 break;
         }
     }

     #endregion

     #region IQuadTreeBody

     public Vector2 Position { get { return new Vector2 (transform.position.x, transform.position.z); } }
     public bool QuadTreeIgnore { get { return false; } }

     #endregion
 }