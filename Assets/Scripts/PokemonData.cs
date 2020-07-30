using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PokemonData
{
    public int id;
    public string N;

    public List<AbilityData> Ab;

    public List<StatsData> St;

    public List<TypeData> T;

    public PokeSpecieData info;
}

[System.Serializable]
public class GalarDex{
    public List<GalarPokemon> pokedex;
}
[System.Serializable]
public class GalarPokemon{
    public int id;
    public string name;
    public List<int> base_stats;
    public List<string> abilities;
    public List<string> types;
    public string description;

    
}

[System.Serializable]
public class AbilityData
{
    public string n;
    public int id;
    public bool isH;
}

[System.Serializable]
public class StatsData
{
    public int bs;
}

[System.Serializable]
public class TypeData
{
    public string n;
}

[System.Serializable]
public class PokeSpecieData{
    public List<FlavorText> FTE;
    public List<Forms> varieties;

    public string Gen;

    public string G;
}

[System.Serializable]
public class FlavorText{
    public string v;
    public string e;
}

[System.Serializable]
public class Forms{
    public int id;
    public string n;
}
public enum TypeName{
    bug=0, dark=1,dragon=2, electric=3,
    steel=4, fairy=5,fighting=6, fire=7,
    flying=8, water=9, ghost=10, grass=11,
    ground=12, ice=13, normal=14,poison=15,psychic=16,
    rock=17

}