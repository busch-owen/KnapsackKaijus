using UnityEngine;
using System.Collections.Generic;

public class KaijuParty : Singleton<KaijuParty>
{
    [field: SerializeField] public List<Kaiju> KaijuInParty { get; private set; } = new(6);

    public void AddKaijuToParty(Kaiju kaijuToAdd)
    {
        KaijuInParty.Add(kaijuToAdd);
    }
}
