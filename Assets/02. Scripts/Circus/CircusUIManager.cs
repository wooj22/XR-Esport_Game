using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircusUIManager : MonoBehaviour
{
    [SerializeField] Text timerLable;
    [SerializeField] Slider gaugeBar;
    [SerializeField] Text adviceLable;
    [SerializeField] Image adviceBackImage;

    private int startCount;

    public void StartCountDown()
    {
        startCount = 5;
        StartCoroutine(StartCountDown2());
    }

    IEnumerator StartCountDown2()
    {
        yield return new WaitForSeconds(1f);
        adviceLable.text = startCount.ToString();
    }
}
