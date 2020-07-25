using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DexHandler : MonoBehaviour
{

    public TMP_Text Number, Name, FlavorText;
    public GameObject Abilities, StatsBars, TypeBar;
    public Image sprite;

    public TMP_Dropdown flavorDrop;
    public Button Next, Back, Base, Min, Max;

    public Fade fade;

    public void QuitApp(){
        Application.Quit();
    }
    public void ReturnToDexList(){
        StartCoroutine(ExitingScreen());
    }

    IEnumerator ExitingScreen(){
        
        fade.FadeOut();
        yield return new WaitUntil(()=> fade.fadedOut);
        PokeDataFromJSON.dex.ClickMask.SetActive(false);
        SceneManager.UnloadSceneAsync("DexJSON");
        
    }

    private void Awake()
    {
        fade.FadeIn();
    }

}
