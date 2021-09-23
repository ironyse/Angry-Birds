using UnityEngine;

public class YellowBird : Bird
{
    [SerializeField] public float _boostForce = 100f;

    public bool _hasBoost = false;

    public void Boost() { 
        // menambahkan gaya pada bird ketika method boost dipanggil
        if (State == BirdState.Thrown && !_hasBoost) {
            RigidBody.AddForce(RigidBody.velocity * _boostForce);
            _hasBoost = true;
        }
    }

    public override void OnTap() {
        Boost();
    }
}
