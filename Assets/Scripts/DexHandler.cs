using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.SceneManagement;

public class DexHandler : MonoBehaviour
{
    [Header("Abilities")]
    public TMP_Text AbName, AbShort, AbDetail;
    public CanvasGroup abPanel;


    [Header("Handler")]
    public TMP_Text Number, Name, FlavorText, Group;
    public GameObject Abilities, StatsBars, TypeBar;
    public Image sprite;
    public GameObject imgPrefab;
    public HorizontalScrollSnap imgSlider;

    public TMP_Dropdown flavorDrop;
    public Button Next, Back, Base, Min, Max, ChangeForm;

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
        StartCoroutine(Load());
    }

    IEnumerator Load(){
        PokeDataFromJSON.dex.OnDexPageLoaded();
        fade.FadeIn();
        yield return new WaitUntil(()=>fade.fadedIn);
        fade.fadedIn = false;
        ChangeForm.onClick.AddListener(() =>PokeDataFromJSON.dex.GetCurrentList()[PokeDataFromJSON.dex.currentPoke].formChanger(1));
        int id = PokeDataFromJSON.dex.GetCurrentList()[PokeDataFromJSON.dex.currentPoke].id;
        /*foreach(Forms form in PokeDataFromJSON.dex.pokemon.Find(p => p.id == id).info.varieties){
            GameObject img = Instantiate(imgPrefab);
            imgSlider.AddChild(img);
        }*/
       
        
    }

    public void AbPanelExit(){
        abPanel.gameObject.SetActive(false);
    }

}
