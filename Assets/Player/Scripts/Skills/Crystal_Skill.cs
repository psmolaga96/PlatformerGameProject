using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal Mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Exposive")]
    [SerializeField] private bool canExplode;

    [Header("Moving")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("MultiStacking")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float mulitStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();
            

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;
            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            { 
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));  
    }
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if(crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if(crystalLeft.Count <= 0)
                {
                    cooldown = mulitStackCooldown;
                    RefillCrystal();
                }
            return true;
            }
        }
        return false;
    }
    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = mulitStackCooldown;
        RefillCrystal();
    }
}
