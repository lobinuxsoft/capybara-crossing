using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacleBehaviour : MonoBehaviour
{
    [SerializeField] private Transform initPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private float minDuration = 10;
    [SerializeField] private float maxDuration = 20;
    [SerializeField] private float duration;
    private float lerpTime = 0;

    private void Start()
    {
        duration = Random.Range(minDuration, maxDuration);
    }

    private void Update()
    {
        lerpTime += Time.deltaTime;
        obstacle.transform.position = Vector3.Lerp(initPos.position, endPos.position, lerpTime / duration);
        if(lerpTime >= duration)
        {
            lerpTime = 0;
        }
    }

}
