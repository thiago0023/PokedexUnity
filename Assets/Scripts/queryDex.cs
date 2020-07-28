using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;
using System;


public class queryDex : MonoBehaviour
{
    

    public GameObject ClickMask, sortbox;

    public Fade ListFade;

    public TMP_Dropdown filterDrop, sortDrop;
    public void OrderBy(){
        PokeDataFromJSON.dex.ClickMask.SetActive(true);
        int i = sortDrop.value;
        StartCoroutine(SortWithFade(i));
    }


    void Sorting(int i){     
        
        List<CardIndex> query = new List<CardIndex>();
        foreach(Transform child in PokeDataFromJSON.dex.DexList.transform){
            query.Add(child.GetComponent<CardIndex>());
        }
        
        switch (i)
        {   
            case (int)Filter.AtoZ:
                
                query = query.OrderBy(poke => poke.name).ToList();
                FilterReorder(query);
                
                
                    
                break;

            case (int)Filter.ZtoA:
            
                query = query.OrderByDescending(poke => poke.name).ToList();
                FilterReorder(query);
                
               
                    
                break;

            case (int)Filter.NumberAsc:
                
                query = query.OrderBy(poke => poke.id).ToList();
                FilterReorder(query);
                
                
                    
                break;
            
            case (int)Filter.NumberDesc:
                
                query = query.OrderByDescending(poke => poke.id).ToList();
                FilterReorder(query);
                
                
                break;
              
            
                
        }
    }
   
    IEnumerator SortWithFade(int i){
        sortbox.SetActive(false);
        yield return null;
        ListFade.FadeOut();
        yield return new WaitUntil(()=>ListFade.fadedOut);
        ListFade.fadedOut = false;
        yield return null;
        

        Sorting(i);
        
        ListFade.FadeIn();
        yield return new WaitUntil(()=>ListFade.fadedIn);
        ListFade.fadedIn = false;
        PokeDataFromJSON.dex.ClickMask.SetActive(false);
        
    }
    public void FilterByType(int i){
        PokeDataFromJSON.dex.ClickMask.SetActive(true);
        StartCoroutine(FilterType());
    }

    void FilterReorder(List<CardIndex> query){
        int count = 0;
        List<PokemonData> pokemons = new List<PokemonData>();
            foreach(CardIndex poke in query){
                PokemonData pokemon = PokeDataFromJSON.dex.pokemon[poke.id-1];
                pokemons.Add(pokemon);
            }
            foreach(Transform child in PokeDataFromJSON.dex.DexList.transform){
                PokeDataFromJSON.dex.DrawCardInfo(child.gameObject, pokemons[count]);
                count ++;
            }

    }

    public void FixList(){
        var list = PokeDataFromJSON.dex.DexList;
        var area = PokeDataFromJSON.dex.DexArea;
        Destroy(area.GetComponent<UI_ScrollRectOcclusion>());
        list.GetComponent<VerticalLayoutGroup>().enabled = true;
        list.GetComponent<ContentSizeFitter>().enabled = true;
        //Canvas.ForceUpdateCanvases();
        area.AddComponent<UI_ScrollRectOcclusion>();
        var content = PokeDataFromJSON.dex.content;
        content.position = new Vector3(content.position.x,0, content.position.z);
        Canvas.ForceUpdateCanvases();

    }

    IEnumerator FilterType(){
        ListFade.FadeOut();
        yield return new WaitUntil(()=>ListFade.fadedOut);
        yield return null;
        if(filterDrop.value !=0){
            var query = from poke in PokeDataFromJSON.dex.pokemon.GetRange(0,807)
                        where poke.T.Exists(t => t.n==Enum.GetNames(typeof(TypeName))[filterDrop.value -1])
                        select poke;
        
            PokeDataFromJSON.dex.DrawReorderedDex(query);
        }
        else{
            var query = PokeDataFromJSON.dex.pokemon.GetRange(0,807);
            PokeDataFromJSON.dex.DrawReorderedDex(query);
        }
        Sorting(sortDrop.value);
        FixList();
        yield return null;
        ListFade.fadedOut=false;
        ListFade.FadeIn();
        yield return new WaitUntil(()=>ListFade.fadedIn);
        ListFade.fadedIn = false;
        PokeDataFromJSON.dex.ClickMask.SetActive(false);

    }

    public void MountFilterDrop(){
        
        var options = new List<string>();
        for(int i =0; i< Enum.GetValues(typeof(TypeName)).Length; i++){
            options.Add(Enum.GetNames(typeof(TypeName))[i]);
        }
        filterDrop.AddOptions(options);
    }

    
}

public enum Filter{
    NumberAsc=0, NumberDesc=1,AtoZ=2, ZtoA=3
}



