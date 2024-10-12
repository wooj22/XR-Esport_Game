using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusDirector : MonoBehaviour
{
    [SerializeField] private GameObject firecrackers;

    public void PlayFirecracker()
    {
        firecrackers.gameObject.SetActive(true);
    }
}
