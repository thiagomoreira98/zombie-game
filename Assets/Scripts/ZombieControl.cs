using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControl : MonoBehaviour {

    public GameObject Player;
    public float Speed = 5;

    private Rigidbody rigidbodyZombie;
    private Animator animator;
    private PlayerControl playerControl;

    // Start is called before the first frame update
    void Start() {
        Player = GameObject.FindWithTag("Player");
        randomGenerateZombieType();

        rigidbodyZombie = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerControl = Player.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        Vector3 direction = Player.transform.position - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(direction);
        rigidbodyZombie.MoveRotation(newRotation);

        if (distance > 2.5) {
            Vector3 directionPlayer = direction.normalized * Speed * Time.deltaTime;
            rigidbodyZombie.MovePosition(rigidbodyZombie.position + directionPlayer);
            animator.SetBool("attacking", false);
        } else {
            animator.SetBool("attacking", true);
        }
    }

    void AttackPlayer() {
        Time.timeScale = 0;
        playerControl.GameOverText.SetActive(true);
        playerControl.IsDead = true;
    }

    void randomGenerateZombieType() {
        int zombieType = Random.Range(1, 28);
        transform.GetChild(zombieType).gameObject.SetActive(true);
    }
}
