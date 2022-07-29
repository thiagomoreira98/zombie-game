using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IKillable
{

    public LayerMask FloorMask;
    public GameObject GameOverText;
    public Interface ScriptInterface;
    public AudioClip damageSound;
    public Status status;

    private Vector3 direction;
    private MovePosition movePosition;
    private Animation animationComponent;
    
    void Start()
    {
        this.movePosition = GetComponent<MovePosition>();
        this.animationComponent = GetComponent<Animation>();
        this.status = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");
        this.direction = new Vector3(eixoX, 0, eixoZ);
        this.animationComponent.Walk(this.direction.magnitude);
    }

    void FixedUpdate()
    {
        this.movePosition.Move(this.direction, this.status.Speed);
        this.Rotation();
    }

    void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        RaycastHit impact;

        if(Physics.Raycast(ray, out impact, 100, FloorMask)) 
        {
            Vector3 positionPlayerAim = impact.point - this.transform.position;
            positionPlayerAim.y = this.transform.position.y;
            this.movePosition.Rotation(positionPlayerAim);
        }
    }

    public void ReceiveDamage(int damage)
    {
        this.status.Life -= damage;
        this.ScriptInterface.UpdateSliderPlayerLife();
        AudioControl.instance.PlayOneShot(this.damageSound);

        if(this.isDead())
        {
            this.Die();
        }
        
    }

    public void Die()
    {
        this.ScriptInterface.GameOver();
    }

    public bool isDead()
    {
        return this.status.Life <= 0;
    }
}
