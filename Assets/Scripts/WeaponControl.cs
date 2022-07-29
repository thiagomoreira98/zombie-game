using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour {

    public GameObject Shot;
    public GameObject GunBarrel;
    public AudioClip shotSound;

    // Update is called once per frame
    void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Instantiate(Shot, GunBarrel.transform.position, GunBarrel.transform.rotation);
            AudioControl.instance.PlayOneShot(this.shotSound);
        }
    }
}
