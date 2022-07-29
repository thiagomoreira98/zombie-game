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
        if(other.tag.Equals("Zombie")) {
            other.GetComponent<ZombieControl>().ReceiveDamage(this.damage);
        }

        Destroy(this.gameObject);
    }
}
