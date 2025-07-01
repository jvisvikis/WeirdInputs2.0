using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject calibrationPanel;
    [SerializeField] private AudioSource voiceMessage;
    public bool gameStarted {  get; private set; }
    public bool gameWon { get; set; }
    public bool gameOver { get; set; }

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void PlayVoiceMessage()
    {
        calibrationPanel.SetActive(false);
        voiceMessage.Play();
        StartCoroutine(StartGameAfter(voiceMessage.clip.length));
    }

    private IEnumerator StartGameAfter(float time)
    {
        yield return new WaitForSeconds(time);
        StartGame();
    }
    
    public void StartGame()
    {
        Debug.Log("GameStarted");
        gameStarted = true;
        ScreenManager sm = FindObjectOfType<ScreenManager>();
        if(sm != null )
            sm.LoadFirstWindow();
    }
    

}
