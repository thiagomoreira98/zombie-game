using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour {

    public GameObject Zombie;
    public float TimeGenerateZombie = 1;
    public LayerMask LayerZombie;

    private float timeCount = 0;
    private float generateDistance = 3;
    private float playerDistanceGeneration = 25;
    private GameObject player;

    private void Start()
    {
        this.player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        if(Vector3.Distance(this.transform.position, this.player.transform.position) > this.playerDistanceGeneration)
        {
            this.timeCount += Time.deltaTime;

            if (this.timeCount >= this.TimeGenerateZombie)
            {
                StartCoroutine(this.GenerateZombie());
                this.timeCount = 0;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this.generateDistance);
    }

    IEnumerator GenerateZombie()
    {
        Vector3 position = this.CreateRandomPosition();
        Collider[] colliders = Physics.OverlapSphere(position, 1, LayerZombie);

        while (colliders.Length > 0)
        {
            position = this.CreateRandomPosition();
            colliders = Physics.OverlapSphere(position, 1, LayerZombie);
            yield return null;
        }

        Instantiate(this.Zombie, position, this.transform.rotation);
    }

    Vector3 CreateRandomPosition()
    {
        Vector3 position = Random.insideUnitSphere * this.generateDistance;
        position += this.transform.position;
        position.y = 0;
        return position;
    }
}
