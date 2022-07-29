using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControl : MonoBehaviour, IKillable
{

    public GameObject Player;
    public AudioClip deathSound;

    private PlayerControl playerControl;
    private MovePosition movePosition;
    private Animation animationComponent;
    private Interface ScriptInterface;
    private Status status;
    private Vector3 randomPosition;
    private Vector3 direction;
    private float walkTimeCount;
    private float timeBetweenRandomPositions = 4;

    // Start is called before the first frame update
    void Start()
    {
        this.Player = GameObject.FindWithTag("Player");
        randomGenerateZombie();

        this.playerControl = Player.GetComponent<PlayerControl>();
        this.movePosition = GetComponent<MovePosition>();
        this.animationComponent = GetComponent<Animation>();
        this.status = GetComponent<Status>();
        this.ScriptInterface = GameObject.FindWithTag("Interface").GetComponent<Interface>();
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(this.transform.position, this.Player.transform.position);

        this.movePosition.Rotation(this.direction);
        this.animationComponent.Walk(this.direction.magnitude);

        if(distance > 15)
        {
            this.Walk();
        }
        else if (distance > 2.5)
        {
            this.direction = this.Player.transform.position - this.transform.position;
            this.movePosition.Move(this.direction, this.status.Speed);
            this.animationComponent.Attack(false);
        }
        else
        {
            this.direction = this.Player.transform.position - this.transform.position;
            this.animationComponent.Attack(true);
        }
    }

    void Walk()
    {
        walkTimeCount -= Time.deltaTime;

        if(walkTimeCount <= 0)
        {
            this.randomPosition = this.CreateRandomPosition();
            walkTimeCount += timeBetweenRandomPositions;
        }

        bool isNear = Vector3.Distance(this.transform.position, this.randomPosition) <= 0.05;
        if (!isNear)
        {
            this.direction = this.randomPosition - this.transform.position;
            this.movePosition.Move(this.direction, this.status.Speed);
        }
    }

    Vector3 CreateRandomPosition()
    {
        Vector3 position = Random.insideUnitSphere * 10;
        position += this.transform.position;
        position.y = this.transform.position.y;
        return position;
    }

    void AttackPlayer()
    {
        int damage = Random.Range(20, 30);
        this.playerControl.ReceiveDamage(damage);
    }

    void randomGenerateZombie()
    {
        int zombieType = Random.Range(1, 28);
        this.transform.GetChild(zombieType).gameObject.SetActive(true);
    }

    public void ReceiveDamage(int damage)
    {
        this.status.Life -= damage;

        if(this.isDead())
        {
            this.Die();
            this.ScriptInterface.IncrementKills();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
        AudioControl.instance.PlayOneShot(this.deathSound);
    }

    public bool isDead()
    {
        return this.status.Life <= 0;
    }
}
