using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Demo : MonoBehaviour {
    public enum T { Spell, Projectiles, Aura, Shield, Variations, Buff, Tome }

    public T demo;
    public GameObject[] SpellList;
    public GameObject[] CastingList;
    public GameObject[] AuraList;
    public GameObject[] ShieldList;
    public GameObject[] VariationsList;

    public GameObject[] BuffList;
    public GameObject[] TomeList;

    public Text Title;
    public int Selection = 0;

    public GameObject BackText;
    public GameObject NextText;
    public GameObject BackButton;
    public GameObject NextButton;

    public GameObject SpellsGroup;
    public GameObject CastingGroup;
    public GameObject AuraGroup;
    public GameObject ShieldGroup;
    public GameObject VariationsGroup;

    public GameObject BuffGroup;
    public GameObject TomeGroup;

    public GameObject SelectionSpells;
    public GameObject SelectionProjectiles;
    public GameObject SelectionAura;
    public GameObject SelectionShields;
    public GameObject SelectionVariations;

    public GameObject SelectionBuff;
    public GameObject SelectionTome;


    void Start() {
        SpellList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + SpellList[Selection].gameObject.transform.name.ToString();
    }
    void Update()
    {

        if (Selection == 0)
        {
            BackText.SetActive(false);
            BackButton.SetActive(false);
        }
        else
        {
            BackText.SetActive(true);
            BackButton.SetActive(true);
        }

        if (demo == T.Spell)
        {
            if (Selection == SpellList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }

            SelectionProjectiles.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(false);
            SelectionSpells.SetActive(true);

            SelectionTome.SetActive(false);
            SelectionBuff.SetActive(false);
        }

        if (demo == T.Projectiles)
        {
            if (Selection == CastingList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(false);
            SelectionProjectiles.SetActive(true);
            SelectionTome.SetActive(false);
            SelectionBuff.SetActive(false);
        }
        if (demo == T.Aura)
        {
            if (Selection == AuraList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionProjectiles.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(false);
            SelectionAura.SetActive(true);

            SelectionTome.SetActive(false);
            SelectionBuff.SetActive(false);
        }
        if (demo == T.Shield)
        {
            if (Selection == ShieldList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionProjectiles.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionVariations.SetActive(false);
            SelectionShields.SetActive(true);

            SelectionTome.SetActive(false);
            SelectionBuff.SetActive(false);
        }
        if (demo == T.Variations)
        {
            if (Selection == VariationsList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionProjectiles.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(true);

            SelectionTome.SetActive(false);
            SelectionBuff.SetActive(false);
        }

        if (demo == T.Buff)
        {
            if (Selection == BuffList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionProjectiles.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(false);

            SelectionTome.SetActive(false);
            SelectionBuff.SetActive(true);
        }

        if (demo == T.Tome)
        {
            if (Selection == TomeList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionProjectiles.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(false);

            SelectionTome.SetActive(true);
            SelectionBuff.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Back();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Next();
        }


    }
    public void Back()
    {

        if (demo == T.Spell)
        {
            if (Selection < SpellList.Length && Selection != 0)
            {
                SpellList[Selection].SetActive(false);
                Selection -= 1;
                SpellList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + SpellList[Selection].gameObject.transform.name.ToString();
            }
        }

        if (demo == T.Projectiles)
        {
            if (Selection < CastingList.Length && Selection != 0)
            {
                CastingList[Selection].SetActive(false);
                Selection -= 1;
                CastingList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + CastingList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Aura)
        {
            if (Selection < AuraList.Length && Selection != 0)
            {
                AuraList[Selection].SetActive(false);
                Selection -= 1;
                AuraList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + AuraList[Selection].gameObject.transform.name.ToString();
            }
        }

        if (demo == T.Shield)
        {
            if (Selection < ShieldList.Length && Selection != 0)
            {
                ShieldList[Selection].SetActive(false);
                Selection -= 1;
                ShieldList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + ShieldList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Variations)
        {
            if (Selection < VariationsList.Length && Selection != 0)
            {
                VariationsList[Selection].SetActive(false);
                Selection -= 1;
                VariationsList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + VariationsList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Buff)
        {
            if (Selection < BuffList.Length && Selection != 0)
            {
                BuffList[Selection].SetActive(false);
                Selection -= 1;
                BuffList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + BuffList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Tome)
        {
            if (Selection < TomeList.Length && Selection != 0)
            {
                TomeList[Selection].SetActive(false);
                Selection -= 1;
                TomeList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + TomeList[Selection].gameObject.transform.name.ToString();
            }
        }
    }

    public void Next()
    {
        if (demo == T.Spell)
        {
            if (Selection < SpellList.Length && Selection != SpellList.Length - 1)
            {
                SpellList[Selection].SetActive(false);
                Selection += 1;
                SpellList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + SpellList[Selection].gameObject.transform.name.ToString();
            }
        }

        if (demo == T.Projectiles)
        {
            if (Selection < CastingList.Length && Selection != CastingList.Length - 1)
            {
                CastingList[Selection].SetActive(false);
                Selection += 1;
                CastingList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + CastingList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Aura)
        {
            if (Selection < AuraList.Length && Selection != AuraList.Length - 1)
            {
                AuraList[Selection].SetActive(false);
                Selection += 1;
                AuraList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + AuraList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Shield)
        {
            if (Selection < ShieldList.Length && Selection != ShieldList.Length - 1)
            {
                ShieldList[Selection].SetActive(false);
                Selection += 1;
                ShieldList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + ShieldList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Variations)
        {
            if (Selection < VariationsList.Length && Selection != VariationsList.Length - 1)
            {
                VariationsList[Selection].SetActive(false);
                Selection += 1;
                VariationsList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + VariationsList[Selection].gameObject.transform.name.ToString();
            }
        }

        if (demo == T.Buff)
        {
            if (Selection < BuffList.Length && Selection != BuffList.Length - 1)
            {
                BuffList[Selection].SetActive(false);
                Selection += 1;
                BuffList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + BuffList[Selection].gameObject.transform.name.ToString();
            }
        }

        if (demo == T.Tome)
        {
            if (Selection < TomeList.Length && Selection != TomeList.Length - 1)
            {
                TomeList[Selection].SetActive(false);
                Selection += 1;
                TomeList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + TomeList[Selection].gameObject.transform.name.ToString();
            }
        }
    }

    public void Spells()
    {
        Last();
        demo = T.Spell;
        Selection = 0;
        SpellList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + SpellList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        CastingGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(false);
        SpellsGroup.SetActive(true);

        BuffGroup.SetActive(false);
        TomeGroup.SetActive(false);
    }
    public void Projectiles()
    {
        Last();
        demo = T.Projectiles;
        Selection = 0;
        CastingList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + CastingList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(false);
        CastingGroup.SetActive(true);

        BuffGroup.SetActive(false);
        TomeGroup.SetActive(false);
    }
    public void Auras()
    {
        Last();
        demo = T.Aura;
        Selection = 0;
        AuraList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + AuraList[Selection].gameObject.transform.name.ToString();


        CastingGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(false);
        AuraGroup.SetActive(true);

        BuffGroup.SetActive(false);
        TomeGroup.SetActive(false);

    }

    public void Shields()
    {
        Last();
        demo = T.Shield;
        Selection = 0;
        ShieldList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + ShieldList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        CastingGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        VariationsGroup.SetActive(false);
        ShieldGroup.SetActive(true);

        BuffGroup.SetActive(false);
        TomeGroup.SetActive(false);
    }

    public void Variations()
    {
        Last();
        demo = T.Variations;
        Selection = 0;
        VariationsList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + VariationsList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        CastingGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(true);

        BuffGroup.SetActive(false);
        TomeGroup.SetActive(false);
    }

    public void Buffs()
    {
        Last();
        demo = T.Buff;
        Selection = 0;
        BuffList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + BuffList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        CastingGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(false);

        BuffGroup.SetActive(true);
        TomeGroup.SetActive(false);
    }

    public void Tomes()
    {
        Last();
        demo = T.Tome;
        Selection = 0;
        TomeList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + TomeList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        CastingGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(false);

        BuffGroup.SetActive(false);
        TomeGroup.SetActive(true);
    }

    public void Last()
    {
        if (Selection < SpellList.Length)
        {
            SpellList[Selection].SetActive(false);
        }
        if (Selection < CastingList.Length)
        {
            CastingList[Selection].SetActive(false);
        }

        if (Selection < AuraList.Length)
        {
            AuraList[Selection].SetActive(false);
        }
        if (Selection < ShieldList.Length)
        {
            ShieldList[Selection].SetActive(false);
        }

        if (Selection < VariationsList.Length)
        {
            VariationsList[Selection].SetActive(false);
        }

        if (Selection < BuffList.Length)
        {
            BuffList[Selection].SetActive(false);
        }

        if (Selection < TomeList.Length)
        {
            TomeList[Selection].SetActive(false);
        }

    
}
}
