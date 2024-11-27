using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwingController : MonoBehaviour
{
    [SerializeField] ParticleSystem item;
    [SerializeField] ParticleSystem effect;
    [SerializeField] public float itemMoveSpeed;
    private bool isMoving = true;
    private RollerGameManager _rollerGameManager;
    

    private void Start()
    {
        _rollerGameManager = GameObject.Find("GameManager").GetComponent<RollerGameManager>();
    }

    private void Update()
    {
        if (isMoving)
        {
            this.transform.Translate(Vector3.back * itemMoveSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
            isMoving = false;
            item.Stop();
            effect.Play();
            _rollerGameManager.HitItem();
            Invoke(nameof(SetOff), 3f);
        }
        else if (other.gameObject.name == "DeadLine")
        {
            _rollerGameManager.LoseItem();
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<SphereCollider>().enabled = true;
        }
    }

    private void SetOff()
    {
        isMoving = true;
        this.gameObject.SetActive(false);
        this.gameObject.GetComponent<SphereCollider>().enabled = true;
    }
}
