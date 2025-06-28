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
    void Start()
    {
        StartCoroutine(fakeLoad());
    }

    private IEnumerator fakeLoad()
    {
        float start = Time.time;
        while(Time.time < start + timeLimit)
        {
            slider.value = Time.time/(start+timeLimit);
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
