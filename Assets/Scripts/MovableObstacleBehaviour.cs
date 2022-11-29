using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacleBehaviour : MonoBehaviour
{
    [SerializeField] private Transform initPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private List<GameObject> obstacles = new List<GameObject>();
    [SerializeField] private GameObject currentObstacle;
    [SerializeField] private float minDuration = 10;
    [SerializeField] private float maxDuration = 20;
    [SerializeField] private float duration;
    private float lerpTime = 0;

    private void Start()
    {
        currentObstacle = obstacles[Random.Range(0, 2)];
        duration = Random.Range(minDuration, maxDuration);
    }

    private void Update()
    {
        lerpTime += Time.deltaTime;
        currentObstacle.transform.position = Vector3.Lerp(initPos.position, endPos.position, lerpTime / duration);
        if(lerpTime >= duration)
        {
            currentObstacle.SetActive(false);
            lerpTime = 0;
            currentObstacle.transform.position = Vector3.Lerp(initPos.position, endPos.position, lerpTime / duration);
            currentObstacle = obstacles[Random.Range(0, 2)];
            currentObstacle.SetActive(true);
        }
    }

}
