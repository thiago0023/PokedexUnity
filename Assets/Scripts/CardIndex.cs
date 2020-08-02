using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardIndex : MonoBehaviour
{
    public int id = 0;

    public int form =0;
    public bool InUse = true;

    public new string name;

    public void formChanger(int form){
        PokemonData poke = PokeDataFromJSON.dex.pokemon.Find(p => p.id == id);
        PokemonData formData = PokeDataFromJSON.dex.formPokemon.Find(p => p.id == poke.info.varieties[form].id);
        PokeDataFromJSON.dex.Name.text = formData.N;
        PokeDataFromJSON.dex.sprite.sprite = Resources.Load<Sprite>("Sprites/" + formData.N );
        PokeDataFromJSON.dex.DrawAbilities(formData);
        PokeDataFromJSON.dex.ShowType(formData);
        for(int i = 0; i < 6; i++){
            float stat = formData.St[i].bs;
            PokeDataFromJSON.dex.DrawStats(stat, 200, i);
        }
    }
}
