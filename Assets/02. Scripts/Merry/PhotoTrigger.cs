using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoTrigger : MonoBehaviour
{
    [SerializeField] TextMesh photoCount;
    [SerializeField] BoxCollider bc;
    [SerializeField] List<ParticleSystem> vfxLists;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PhotoCounting());
        }
    }

    // ���� ī����
    IEnumerator PhotoCounting()
    {
        bc.enabled = false;
        for (int i = 3; i >=1; i--)
        {
            photoCount.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        photoCount.text = "";

        PlayPhotoParticle();
        yield return new WaitForSeconds(10f);
        photoCount.text = "Photo";
        bc.enabled = true;
    }

    // ���� ��ƼŬ ���
    private void PlayPhotoParticle()
    {
        for(int i=0; i< vfxLists.Count; i++)
        {
            vfxLists[i].Play();
        }
    }
}
