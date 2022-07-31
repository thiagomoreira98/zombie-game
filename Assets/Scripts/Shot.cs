using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public float Speed = 30;
    public int damage = 1;
    private Rigidbody rigidbodyShot;

    private void Start() {
        this.rigidbodyShot = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        this.rigidbodyShot.MovePosition(this.rigidbodyShot.position + this.transform.forward * this.Speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        switch (other.tag)
        {
            case "Zombie":
                this.CauseDamage(other.GetComponent<ZombieControl>());
                break;
            case "Boss":
                this.CauseDamage(other.GetComponent<BossControl>());
                break;
        }

        Destroy(this.gameObject);
    }

    void CauseDamage(IKillable killable)
    {
        Quaternion rotation = Quaternion.LookRotation(-this.transform.forward);

        killable.ReceiveDamage(this.damage);
        killable.CreateParticleBlood(this.transform.position, rotation);
    }
}
