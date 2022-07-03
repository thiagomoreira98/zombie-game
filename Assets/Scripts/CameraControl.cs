using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public GameObject Player;
    private Vector3 cameraDistance;

    // Start is called before the first frame update
    void Start() {
        cameraDistance = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void Update() {
        transform.position = Player.transform.position + cameraDistance;
    }
}
