﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.UI;



public class PokeDataFromJSON : MonoBehaviour
{
    public TextAsset pokemonData;
    public TextAsset DetailsData;

    public List<PokemonData> pokemon;

    public TMP_Text Number, Name, FlavorText;
    public GameObject Abilities, StatsBars;
    public Image sprite;

    public TMP_Dropdown flavorDrop;



    public int currentPoke = 0;
    

    public void GetAllPokemon(){
        var data = (JObject)JsonConvert.DeserializeObject(pokemonData.text);
        var data2 = (JObject)JsonConvert.DeserializeObject(DetailsData.text);
        List<PokeSpecieData> infoData = new List<PokeSpecieData>();
        for(int i = 0; i < 806; i++){
            infoData.Add(JsonUtility.FromJson<PokeSpecieData>(data2["pokemon-species"][i].ToString()));
        }
        for(int i = 1; i < 807; i++){
            pokemon.Add(JsonUtility.FromJson<PokemonData>(data["pokemon"][i.ToString()].ToString()));
            pokemon[i-1].info = infoData[i-1];
        }
        infoData.Clear();        

    }

    public void DrawDex(){
        sprite.gameObject.SetActive(false);
        PokemonData poke = pokemon[currentPoke];
        Number.text = poke.id.ToString();
        Name.text = poke.N;
        DropdownFill(poke);
        FlavorText.text = FixFlavor(poke.info.FTE[0].e);
        sprite.sprite = Resources.Load<Sprite>("Sprites/" + poke.N );
        sprite.gameObject.SetActive(true);
    }
    private void Start()
    {
        GetAllPokemon();
        DrawDex();
        
    }

    public void Next(){
        if(currentPoke < pokemon.Count -1){
            currentPoke++;
        }
        else{
            currentPoke = 0;
        }
        DrawDex();
    }

    public void Back(){
        if(currentPoke > 0){
            currentPoke--;
        }
        else{
            currentPoke = pokemon.Count - 1;
        }
        DrawDex();
    }

    public void DropdownFill(PokemonData poke){
        var options = new List<string>();
        foreach(FlavorText fte in poke.info.FTE)
        {   
            options.Add(fte.v);
        }
        flavorDrop.options.Clear();
        flavorDrop.AddOptions(options);
    }

    public void DropChange(){
        FlavorText.text = FixFlavor(pokemon[currentPoke].info.FTE[flavorDrop.value].e);
    }

    public string FixFlavor(string flavor){
        flavor = flavor.Replace("\n", " ");
        flavor = flavor.Replace("\u000c", " ");
        
        return flavor;
    }
}
