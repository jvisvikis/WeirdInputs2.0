using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoading : Window
{
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private Slider slider;
    [SerializeField] private float timeLimit;
    // Start is called before the first frame update
    void OnEnable ()
    {
        StartCoroutine(fakeLoad());
    }

    private IEnumerator fakeLoad()
    {
        float start = 0;
        while(start<=timeLimit)
        {
            slider.value = start/timeLimit;
            start += Time.deltaTime;
            yield return null;
        }
        //finish
        screenManager.NextWindow();
    }

    public override bool CheckAnswer()
    {
        throw new System.NotImplementedException();
    }
}
