using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 offset;

    private void Awake()
     {
        if (!player)
        {
            player = FindObjectOfType<NewBehaviourScript>().transform; 
        }
        
        offset = transform.position - player.position;
    }


    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
        }
    }
}