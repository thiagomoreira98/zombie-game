using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour {

    public GameObject Shot;
    public GameObject GunBarrel;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Instantiate(Shot, GunBarrel.transform.position, GunBarrel.transform.rotation);
        }
    }
}
