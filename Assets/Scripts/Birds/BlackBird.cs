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
            // mengecek setiap object yang berada dalam radius ledakan
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, explodeRadius, LayerToExplode);

            // mengiterasi setiap object yang berada dalam radius ledakan dan memberikan gaya dan mengurangi health jika itu musuh
            foreach (Collider2D obj in objects)
            {
                Vector2 direction = obj.transform.position - transform.position;
                obj.GetComponent<Rigidbody2D>().AddForce(direction * explodeForce);
                if (obj.tag == "Enemy")
                {                    
                    Enemy enemy = obj.GetComponent<Enemy>();
                    enemy.Health -= explodeForce;
                    if (enemy.Health <= 0)
                    {
                        enemy.SetHit(true);
                        Destroy(enemy.gameObject);

                    }
                }
            }
            
            _hasExploded = true;
            Destroy(gameObject);

            // menginstantiate efek ledakan
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
