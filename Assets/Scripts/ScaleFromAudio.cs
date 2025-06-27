using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromAudio : MonoBehaviour
{
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private float threshold;
    [SerializeField] private float sensitivity;

    // Update is called once per frame
    void Update()
    {
        float loudness = AudioLoudnessDetector.Instance.GetLoudnessFromMic()*sensitivity;
        Debug.Log(loudness);
        if (threshold > loudness)
            loudness = 0f;
            
        this.transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);   
    }
}
