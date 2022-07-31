using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour {

    public GameObject Zombie;
    public float TimeGenerateZombie = 1;
    public LayerMask LayerZombie;
    public Interface ScriptInterface;

    private float timeCount = 0;
    private float generateDistance = 3;
    private float playerDistanceGeneration = 25;
    private GameObject player;
    private int maxZombies = 2;
    private int activeZombies = 0;

    private void Start()
    {
        this.player = GameObject.FindWithTag("Player");

        for (int i = 0; i < this.maxZombies; i++)
        {
            this.GenerateZombie();
        }
    }

    // Update is called once per frame
    void Update() {
        if(this.isToGenerateZombie())
        {
            this.timeCount += Time.deltaTime;

            if (this.timeCount >= this.TimeGenerateZombie)
            {
                StartCoroutine(this.GenerateZombie());
                this.timeCount = 0;
            }
        }

        if(this.ScriptInterface.IsToIncreaseDificult())
        {
            this.IncreaseDificult();
        }
    }

    private void IncreaseDificult()
    {
        this.maxZombies++;
    }

    private bool isToGenerateZombie()
    {
        bool canGenerateZombieByDistance = Vector3.Distance(this.transform.position, this.player.transform.position) > this.playerDistanceGeneration;

        if (canGenerateZombieByDistance && this.activeZombies < this.maxZombies)
        {
            return true;
        } else
        {
            return false;
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

        ZombieControl zc = Instantiate(this.Zombie, position, this.transform.rotation).GetComponent<ZombieControl>();
        zc.ZGenerator = this;
        this.activeZombies += 1;
    }

    Vector3 CreateRandomPosition()
    {
        Vector3 position = Random.insideUnitSphere * this.generateDistance;
        position += this.transform.position;
        position.y = 0;
        return position;
    }

    public void DecrementActiveZombies()
    {
        this.activeZombies -= 1;
    }
}
