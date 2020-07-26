using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingImage;
    public TMP_Text loadNumber;

    public Image loadBackGround;

    public CanvasGroup canvas;

    public float speed = 120;

    public float timeToLoad = 2f;
    float time = 0;

    bool faded = false;

    bool startFade = false;

    private void Start()
    {
        StartCoroutine(Loading());
    }
    private void Update()
    {
        loadingImage.transform.Rotate(new Vector3(0,0,-1) * speed * Time.deltaTime);
        time += Time.deltaTime;
        float progress = (time/timeToLoad)* 100;
        loadBackGround.fillAmount = time/timeToLoad;
        if(progress>=100){
            progress = 100;
        }
        if(progress<10){
            loadNumber.text = "0" + Mathf.Floor(progress).ToString() +"%";
        }
        else{
            loadNumber.text = Mathf.Floor(progress).ToString()+"%";
        }

        if(startFade){
            canvas.alpha -= Time.deltaTime;
        }
        if(canvas.alpha <= 0){
            faded = true;
        }

        
        
    }

    IEnumerator Loading(){
        //AsyncOperation load = SceneManager.LoadSceneAsync("DexList",Load);
        //load.allowSceneActivation=false;
        yield return new WaitForSeconds(timeToLoad);
        Fade();
        yield return new WaitUntil(()=>faded && PokeDataFromJSON.dex.fullyLoaded);
        SceneManager.UnloadSceneAsync("Loading");
        //load.allowSceneActivation=true;

        

    }
    
    

    void Fade(){
        startFade = true;
    }
}
