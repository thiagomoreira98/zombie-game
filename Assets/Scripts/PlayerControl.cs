using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    public float Speed = 10;
    public LayerMask FloorMask;
    public GameObject GameOverText;
    public bool IsDead = false;

    private Vector3 direction;
    private Rigidbody rigidbodyPlayer;
    private Animator animator;

    void Start() {
        Time.timeScale = 1;
        rigidbodyPlayer = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");
        direction = new Vector3(eixoX, 0, eixoZ);
        SetIsRunning();

        if(IsDead) {
            if(Input.GetButtonDown("Fire1")) {
                SceneManager.LoadScene("game");
            }
        }
    }

    void FixedUpdate() {
        Vector3 moveTo = direction * Time.deltaTime * Speed;
        rigidbodyPlayer.MovePosition(rigidbodyPlayer.position + moveTo);
        mousePosition();
    }

    void SetIsRunning() {
        if (direction != Vector3.zero) {
            animator.SetBool("running", true);
        }
        else {
            animator.SetBool("running", false);
        }
    }

    void mousePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        RaycastHit impact;

        if(Physics.Raycast(ray, out impact, 100, FloorMask)) {
            Vector3 positionPlayerAim = impact.point - transform.position;
            positionPlayerAim.y = transform.position.y;
            Quaternion newRotation = Quaternion.LookRotation(positionPlayerAim);
            rigidbodyPlayer.MoveRotation(newRotation);
        }
    }
}
