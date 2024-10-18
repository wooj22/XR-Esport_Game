using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailController : MonoBehaviour
{
    [SerializeField] private Transform railStartPos;
    [SerializeField] float railMoveSpeed;
    public bool isMoving;

    private void Start()
    {
        isMoving = false;
    }

    private void Update()
    {
        this.transform.Translate(Vector3.back * railMoveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "DeadLine")
        {
            this.transform.position = railStartPos.position;
        }
    }
}
