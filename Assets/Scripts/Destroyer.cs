using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Bird" ||  tag == "Obstacle") {            
            Destroy(collision.gameObject);
        } else if (tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.SetHit(true);
            Destroy(collision.gameObject);
        }
    }
}
