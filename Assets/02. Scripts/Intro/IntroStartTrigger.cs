using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroStartTrigger : MonoBehaviour
{
    [SerializeField] IntroManager _introManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _introManager.StartMidnightCarnival();
            Destroy(this.gameObject, 3f);
        }
    }
}
