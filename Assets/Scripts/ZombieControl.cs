using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControl : MonoBehaviour, IKillable
{

    public GameObject Player;
    public AudioClip deathSound;
    public GameObject MedicKit;
    [HideInInspector]
    public ZombieGenerator ZGenerator;
    public GameObject BloodParticle;

    private Interface ScriptInterface;
    private PlayerControl playerControl;
    private MovePosition movePosition;
    private Animation animationComponent;
    private Status status;
    private Vector3 randomPosition;
    private Vector3 direction;
    private float walkTimeCount;
    private float timeBetweenRandomPositions = 4;
    private float generateMedicKitPercent = 0.1f;

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
            this.walkTimeCount += this.timeBetweenRandomPositions + Random.Range(-1f, 1f);
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
        int zombieType = Random.Range(1, this.transform.childCount);
        this.transform.GetChild(zombieType).gameObject.SetActive(true);
    }

    public void ReceiveDamage(int damage)
    {
        this.status.Life -= damage;

        if(this.isDead())
        {
            this.Die();
        }
    }

    public void CreateParticleBlood(Vector3 position, Quaternion rotation)
    {
        Instantiate(this.BloodParticle, position, rotation);
    }

    public void Die()
    {
        Destroy(this.gameObject, 2);
        this.animationComponent.Die();
        this.movePosition.Die();
        this.enabled = false;
        AudioControl.instance.PlayOneShot(this.deathSound);
        this.GenerateMedicKit(this.generateMedicKitPercent);
        this.ScriptInterface.UpdateDeathZombiesAmount();
        this.ZGenerator.DecrementActiveZombies();
    }

    public bool isDead()
    {
        return this.status.Life <= 0;
    }

    void GenerateMedicKit(float percent)
    {
        // Random.value generates a value from 0 to 1
        if(Random.value <= percent)
        {
            Instantiate(this.MedicKit, this.transform.position, Quaternion.identity);
        }
    }
}
