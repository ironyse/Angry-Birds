using UnityEngine;

public class BlackBird : Bird
{
    public LayerMask LayerToExplode;

    public float explodeRadius;
    public float explodeForce;   
    public bool _hasExploded = false;

    public GameObject explosionObj;

    public void Explode() {
        if (!_hasExploded) {
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, explodeRadius, LayerToExplode);

            foreach (Collider2D obj in objects)
            {
                Vector2 direction = obj.transform.position - transform.position;
                obj.GetComponent<Rigidbody2D>().AddForce(direction * explodeForce);
            }
            
            _hasExploded = true;
            Destroy(gameObject);

            Instantiate(explosionObj, transform.position, Quaternion.identity);            
        }        
    }

    public override void OnHit()
    {
        Explode();
    }

    public override void OnTap()
    {
        Explode();
    }

    // debugging purpose
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
