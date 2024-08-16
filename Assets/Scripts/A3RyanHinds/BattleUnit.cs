using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] KaijuBase _kaijuBase;
    [SerializeField] int _level;
    [SerializeField] bool isPlayerKaiju;

    public KaijuA3 Kaiju { get; set; }

    public void SetupBattle()
    {
        Kaiju = new KaijuA3(_kaijuBase, _level);

        if (isPlayerKaiju)
        {
            GetComponent<Image>().sprite = Kaiju.KaijuBase.BackImage;
        }
    }
}
