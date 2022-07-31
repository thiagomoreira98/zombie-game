using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossControl : MonoBehaviour, IKillable
{
    public GameObject MedicKit;
    public Slider SliderBossLife;
    public Image SliderImage;
    public Color MaxLifeColor, MinLifeColor;
    public GameObject BloodParticle;
    public AudioClip deathSound;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Status status;
    private Animation animationComponent;
    private MovePosition movePosition;
    private Interface ScriptInterface;

    private void Start()
    {
        this.player = GameObject.FindWithTag("Player").transform;
        this.ScriptInterface = GameObject.FindWithTag("Interface").GetComponent<Interface>();
        this.navMeshAgent = GetComponent<NavMeshAgent>();
        this.status = GetComponent<Status>();
        this.navMeshAgent.speed = this.status.Speed;
        this.animationComponent = GetComponent<Animation>();
        this.movePosition = GetComponent<MovePosition>();
        this.SliderBossLife.maxValue = this.status.InitialLife;
        this.UpdateInterface();
    }

    private void Update()
    {
        this.navMeshAgent.SetDestination(this.player.position);
        this.animationComponent.Walk(this.navMeshAgent.velocity.magnitude);

        bool isPlayerNext = this.navMeshAgent.remainingDistance <= this.navMeshAgent.stoppingDistance;

        if (this.navMeshAgent.hasPath == true)
        {
            if (isPlayerNext)
            {
                this.animationComponent.Attack(true);
                Vector3 direction = this.player.position - this.transform.position;
                this.movePosition.Rotation(direction);
            }
            else
            {
                this.animationComponent.Attack(false);
            }
        }
    }

    void AttackPlayer()
    {
        int damage = Random.Range(30, 40);
        this.player.GetComponent<PlayerControl>().ReceiveDamage(damage);
    }

    public void ReceiveDamage(int damage)
    {
        this.status.Life -= damage;
        this.UpdateInterface();

        if (this.isDead())
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
        this.navMeshAgent.enabled = false;
        this.GenerateMedicKit();
        AudioControl.instance.PlayOneShot(this.deathSound);
        this.ScriptInterface.UpdateDeathZombiesAmount();
    }

    public bool isDead()
    {
        return this.status.Life <= 0;
    }

    void GenerateMedicKit()
    {
        Instantiate(this.MedicKit, this.transform.position, Quaternion.identity);
    }

    void UpdateInterface()
    {
        this.SliderBossLife.value = this.status.Life;

        float lifePercent = (float) this.status.Life / this.status.InitialLife;
        Color lifeColor = Color.Lerp(this.MinLifeColor, this.MaxLifeColor, lifePercent);
        this.SliderImage.color = lifeColor;
    }
}
