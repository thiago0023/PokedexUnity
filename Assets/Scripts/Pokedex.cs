using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Pokedex", menuName = "Pokedex/new Pokedex")]
public class Pokedex : ScriptableObject {

    public List<Pokemon> pokemons;
    
}