using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float Health = 50f;
    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

    private bool _isHit = false;

    private void OnDestroy()
    {
        if (_isHit)
        {
            OnEnemyDestroyed(gameObject);
        }
    }

    // mendeteksi tubrukan antara enemy dengan objek lain, jika bertubrukan dengan bird/border maka enemy akan dihancurkan
    // bila menyentuh dengan objek wall/obstacle maka darah enemy akan berkurang
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() == null) return;

        if (collision.gameObject.tag == "Bird" || collision.gameObject.tag == "Border") {

            _isHit = true;
            Destroy(gameObject);

        } else if (collision.gameObject.tag == "Obstacle") {

            float damage = collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            Health -= damage;
            if (Health <= 0) {
                _isHit = true;
                Destroy(gameObject);

            }
        }
    }

    public void SetHit(bool isHit)
    {
        _isHit = isHit;
    }
}
