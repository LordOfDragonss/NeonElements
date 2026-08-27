using UnityEngine;

//Checks if the player's currently active ability matches this ability type
public class PlayerAbilityCondition : Condition
{
    private PlayerAbsorb playerAbsorb;

    [Tooltip("The ability the player must have currently active for this condition to be satisfied.")]
    public AbilityType abilityType;

    private void Start()
    {
        playerAbsorb = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerAbsorb>();
    }

    public override bool IsSatisfied()
    {
        if (playerAbsorb == null)
        {
            playerAbsorb = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<PlayerAbsorb>();
        }

        return playerAbsorb.ActiveAbility == abilityType;
    }
}