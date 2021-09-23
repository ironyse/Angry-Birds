﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    public GameObject Parent;
    public Rigidbody2D RigidBody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    public BirdState State { get { return _state; } }

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    void Start() {
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    void FixedUpdate() {

        // mengubah state bird ketika rigidbody dari bird memiliki velocity lebih dari velocity minimal
        if (_state == BirdState.Idle && RigidBody.velocity.sqrMagnitude >= _minVelocity) {
            _state = BirdState.Thrown;
        }

        // menandai bird untuk dihancurkan ketika bird telah dilempar/mengenai object lain dan memiliki velocity lebih rendah dari velocity minimal
        if (
            (_state == BirdState.Thrown || _state == BirdState.HitSomething)
            && RigidBody.velocity.sqrMagnitude < _minVelocity 
            && !_flagDestroy
           ) 
            {
                _flagDestroy = true;
                StartCoroutine(DetroyAfter(2));
            }
    }

    private void OnDestroy() {
        if (_state == BirdState.Thrown || _state == BirdState.HitSomething) 
            OnBirdDestroyed();    
    }

    private IEnumerator DetroyAfter(float second) {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent) {
        if (!parent) return; // to prevent error
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed) {
        Collider.enabled = true;
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = velocity * speed * distance;
        OnBirdShot(this);
    }

    public virtual void OnTap() { }

    public virtual void OnHit() { }

    private void OnCollisionEnter2D(Collision2D collision) {
        _state = BirdState.HitSomething;
        OnHit();
    }


}
