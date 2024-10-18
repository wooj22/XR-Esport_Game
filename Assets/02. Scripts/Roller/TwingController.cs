using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwingController : MonoBehaviour
{
    [SerializeField] ParticleSystem item;
    [SerializeField] ParticleSystem effect;
    [SerializeField] float itemMoveSpeed;

    private void Update()
    {
        this.transform.Translate(Vector3.back * itemMoveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            item.Stop();
            effect.Play();
            Invoke(nameof(SetOff), 3f);
        }
        else if (other.gameObject.name == "DeadLine")
        {
            this.gameObject.SetActive(false);
        }
    }

    private void SetOff()
    {
        this.gameObject.SetActive(false);
    }
}
