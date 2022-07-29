using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePosition : MonoBehaviour
{

    private Rigidbody rigidbodyCharacter;

    private void Awake()
    {
        this.rigidbodyCharacter = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction, float speed)
    {
        Vector3 moveTo = direction.normalized * speed * Time.deltaTime;
        this.rigidbodyCharacter.MovePosition(this.rigidbodyCharacter.position + moveTo);
    }

    public void Rotation(Vector3 direction)
    {
        Quaternion newRotation = Quaternion.LookRotation(direction);
        this.rigidbodyCharacter.MoveRotation(newRotation);
    }
}
