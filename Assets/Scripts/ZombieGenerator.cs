using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour {

    public GameObject Zombie;
    public float TimeGenerateZombie = 1;
    private float timeCount = 0;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        timeCount += Time.deltaTime;

        if(timeCount >= TimeGenerateZombie) {
            Instantiate(Zombie, transform.position, transform.rotation);
            timeCount = 0;
        }
    }
}
