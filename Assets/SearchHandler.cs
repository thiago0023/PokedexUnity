using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SearchHandler : MonoBehaviour
{

    public TMP_InputField search;
    public queryDex helper;


    public void Search(){
        PokeDataFromJSON.dex.currentList.Clear();
        foreach(Transform child in PokeDataFromJSON.dex.DexList.transform){
            if(!child.GetComponent<CardIndex>().name.Contains(search.text.ToLower())){
                child.GetComponent<CardIndex>().InUse = false;
                child.gameObject.SetActive(false);
            }
            else{
                child.GetComponent<CardIndex>().InUse = true;
                child.gameObject.SetActive(true);
                PokeDataFromJSON.dex.currentList.Add(child.GetComponent<CardIndex>());
            }
        }
        

        helper.FixList();
        
        
    }

    public void SearchOn(){
        search.GetComponent<CanvasGroup>().alpha = 1;
        search.GetComponent<CanvasGroup>().interactable = true;
        search.ActivateInputField();
    }
    
    public void SearchOff(){
        search.GetComponent<CanvasGroup>().alpha = 0;
        search.GetComponent<CanvasGroup>().interactable = false;
    }
    
    
}
