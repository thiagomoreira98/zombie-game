using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicKit : MonoBehaviour
{
    public AudioClip getMedicKitSound;

    private int healAmount = 15;
    private int destroyTime = 5;

    private void Start()
    {
        Destroy(this.gameObject, this.destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerControl>().Heal(this.healAmount);
            AudioControl.instance.PlayOneShot(this.getMedicKitSound);
            Destroy(this.gameObject);
        }
    }
}
