using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public float Speed = 30;
    private Rigidbody rigidbodyShot;

    private void Start() {
        rigidbodyShot = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        rigidbodyShot.MovePosition(rigidbodyShot.position + transform.forward * Speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag.Equals("Zombie")) {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}
