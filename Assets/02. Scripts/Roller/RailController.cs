using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailController : MonoBehaviour
{
    [SerializeField] private Transform railStartPos;
    [SerializeField] public float railMoveSpeed;
    public bool isGaming;

    private void Start()
    {
        isGaming = false;
    }

    private void Update()
    {
        if (isGaming)
        {
            this.transform.Translate(Vector3.back * railMoveSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "DeadLine")
        {
            this.transform.position = railStartPos.position;
        }
    }
}
