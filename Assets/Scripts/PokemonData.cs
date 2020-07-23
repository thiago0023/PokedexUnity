using System.Collections.Generic;

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
}

[System.Serializable]
public class FlavorText{
    public string v;
    public string e;
}