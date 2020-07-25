using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public bool fadedIn = false, fadedOut = false, startIn = false, StartOut = false;

    public float timeIn, timeOut;
    float currentTime = 0;
    
    private CanvasGroup ui;

    private void Awake()
    {
        ui = GetComponent<CanvasGroup>();
    }

    public void FadeIn(){
        startIn = true;
    }

    public void FadeOut(){
        StartOut = true;
    }

    private void Update()
    {
        if(startIn){
            currentTime += Time.deltaTime;
            ui.alpha += currentTime/timeIn;
            if(ui.alpha >=1){
                fadedIn = true;
                startIn = false;
                currentTime = 0;
            }
        }

        if(StartOut){
            currentTime += Time.deltaTime;
            ui.alpha -= currentTime/timeOut;
            if(ui.alpha <=0){
                fadedOut = true;
                StartOut = false;
                currentTime = 0;
            }
        }
    }
}
