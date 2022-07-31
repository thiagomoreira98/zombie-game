using UnityEngine;

public interface IKillable
{
    void CreateParticleBlood(Vector3 position, Quaternion rotation);
    void ReceiveDamage(int damage);
    void Die();
    bool isDead();
}
