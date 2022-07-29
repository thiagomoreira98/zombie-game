using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int InitialLife = 100;
    [HideInInspector]
    public int Life;
    public float Speed = 5;

    private void Awake()
    {
        this.Life = InitialLife;
    }

}
