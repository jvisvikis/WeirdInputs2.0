using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetector : MonoBehaviour
{
    public static AudioLoudnessDetector Instance { get; private set; }
    public float baseLevel { get; private set; }

    [SerializeField] private int sampleWindow = 64;
    private AudioClip microphoneClip;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();
        StartCoroutine(CalibrateBase(3f));
    }

    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMic()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]),microphoneClip);
    }
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0)
            startPosition = 0;
        
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData,startPosition);

        float totalLoudness = 0;
        foreach (float wave in waveData)
        {
            totalLoudness += Mathf.Abs(wave);
        }
        return totalLoudness/sampleWindow;
    }

    public IEnumerator CalibrateBase(float timeLimit)
    {
        baseLevel = 0;
        float start = Time.time;
        List<float> loudnessTimeStamps = new List<float>();
        while (Time.time < start + timeLimit)
        {
            loudnessTimeStamps.Add(GetLoudnessFromMic());
            yield return null;
        }
        float totalLoudness = 0;
        foreach(float s in loudnessTimeStamps)
        {
            totalLoudness += s;
        }
        baseLevel = totalLoudness/loudnessTimeStamps.Count;
        Debug.Log(baseLevel);
    }
}
