using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float laserSpeed = 30f;

    private void Update()
    {
        float moveDir = laserSpeed * Time.deltaTime;
        transform.Translate(0, 0, moveDir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "LaserDownBorder")
        {
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Player")
        {
            // ª°∞≤∞‘ ∫Ø«œ∞‘, ¿Ã∆Â∆Æµµ «“±Ó?
            Destroy(this.gameObject, 3f);
        }
    }

    public void laserSpeedSetting(float laserSpeed)
    {
        this.laserSpeed = laserSpeed;
    }
}
