using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbsorb : MonoBehaviour
{
    public bool hasAbility;
    public bool Absorbing;
    public CustomTrigger AbsorbRangeTrigger;

    public List<string> AbsorbaleTags;
    public float AbsorbDuration = 1f;

    public AbilityType ActiveAbility;

    //Temp
    public SpriteRenderer AbsorbRenderer;

    private void Awake()
    {
        AbsorbRangeTrigger.OnTriggerEnter += OnAbsorbRangeTriggerEnter;
    }

    private void OnAbsorbRangeTriggerEnter(Collider2D Collider)
    {
        if (Absorbing)
        {
            foreach (string tag in AbsorbaleTags)
            {
                if (Collider.tag == tag)
                {
                    AbilityType newAbility = Collider.GetComponent<TestAbilityGiver>().EnemyType;
                    ChangeActiveAbility(newAbility);
                }
            }
        }
    }

    public void ChangeActiveAbility(AbilityType type)
    {
        ActiveAbility = type;
        ChangeAppearance();
        if (type != AbilityType.None)
            hasAbility = true;
        else
            hasAbility = false;
        Debug.Log("Active Ability changed to: " + ActiveAbility.ToString());
    }

    public void OnSpecial(InputValue value)
    {
        float buttonValue = value.Get<float>();
        if (buttonValue > 0f)
        {
            if (hasAbility)
            {
                AbilityAction();
            }
            else
            {
               StartCoroutine(AbsorbAction());

            }
        }
    }

    public void OnRemoveAbility(InputValue value)
    {
        float buttonValue = value.Get<float>();
        if (buttonValue > 0f)
        {
            if (hasAbility)
            {
                ChangeActiveAbility(AbilityType.None);
                hasAbility = false;
            }
        }
    }

    public IEnumerator AbsorbAction()
    {
        Absorbing = true;
        AbsorbRenderer.enabled = true;

        float timer = 0f;

        while (timer < AbsorbDuration)
        {
            if (hasAbility)
                break;

            timer += Time.deltaTime;
            yield return null;
        }
        Absorbing = false;
        AbsorbRenderer.enabled = false;
    }



    public void AbilityAction()
    {
        switch (ActiveAbility)
        {
            case AbilityType.None:
                StartCoroutine(AbsorbAction());
                break;
            case AbilityType.Blue:
                Debug.Log("Blue Ability Called");
                // Implement Blue ability action
                break;
            case AbilityType.Red:
                Debug.Log("Red Ability Called");
                // Implement Red ability action
                break;
            case AbilityType.Green:
                Debug.Log("Green Ability Called");
                // Implement Green ability action
                break;
            case AbilityType.Pink:
                Debug.Log("Pink Ability Called");
                // Implement Pink ability action
                break;
            case AbilityType.Purple:
                Debug.Log("Purple Ability Called");
                // Implement Purple ability action
                break;
            case AbilityType.Yellow:
                Debug.Log("Yellow Ability Called");
                // Implement Yellow ability action
                break;
            case AbilityType.Orange:
                Debug.Log("Orange Ability Called");
                // Implement Orange ability action
                break;
            default:
                StartCoroutine(AbsorbAction());
                break;
        }
    }

    //temporary Visualization of Active Ability
    public void ChangeAppearance()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        switch (ActiveAbility)
        {
            case AbilityType.None:
                sprite.color = Color.white;
                break;
            case AbilityType.Blue:
                sprite.color = Color.blue;
                // Implement Blue ability action
                break;
            case AbilityType.Red:
                sprite.color = Color.red;
                // Implement Red ability action
                break;
            case AbilityType.Green:
                sprite.color = Color.green;
                // Implement Green ability action
                break;
            case AbilityType.Pink:
                sprite.color = Color.hotPink;
                // Implement Pink ability action
                break;
            case AbilityType.Purple:
                sprite.color = Color.purple;
                // Implement Purple ability action
                break;
            case AbilityType.Yellow:
                sprite.color = Color.yellow;
                // Implement Yellow ability action
                break;
            case AbilityType.Orange:
                sprite.color = Color.orange;
                // Implement Orange ability action
                break;
            default:
                sprite.color = Color.white;
                break;
        }
    }

}
