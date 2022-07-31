using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    public float timeBetweenGenerations = 60;
    public GameObject Boss;
    public Transform[] generationPositions;

    private float nextGenerationTime = 0;
    private Interface ScriptInterface;
    private Transform player;

    private void Start()
    {
        this.ScriptInterface = GameObject.FindWithTag("Interface").GetComponent<Interface>();
        this.player = GameObject.FindWithTag("Player").transform;
        this.nextGenerationTime = this.timeBetweenGenerations;
    }

    private void Update()
    {
        if(Time.timeSinceLevelLoad > this.nextGenerationTime)
        {
            Vector3 generationPosition = this.GetGeneratePosition();
            Instantiate(this.Boss, generationPosition, Quaternion.identity);
            this.nextGenerationTime = Time.timeSinceLevelLoad + this.timeBetweenGenerations;
            this.ScriptInterface.ShowBossWarnText();
        }
    }

    Vector3 GetGeneratePosition()
    {
        Vector3 maxDistancePosition = Vector3.zero;
        float maxDistance = 0;

        foreach (Transform position in this.generationPositions) {
            float distanceBetweenPlayer = Vector3.Distance(position.position, player.position);

            if(distanceBetweenPlayer > maxDistance)
            {
                maxDistance = distanceBetweenPlayer;
                maxDistancePosition = position.position;
            }
        }

        return maxDistancePosition;
    }
}
