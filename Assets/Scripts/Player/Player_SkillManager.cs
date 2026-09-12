using UnityEngine;
using System.Collections.Generic;
using System.Net;
public class Player_SkillManager : MonoBehaviour
{
    private Dictionary<SkillType, Skill_Base> skillDict;
    public Skill_Dash dash => skillDict[SkillType.Dash] as Skill_Dash;
    public Skill_Shard shard => skillDict[SkillType.TimeShard] as Skill_Shard;
    public Skill_SwordThrow swordThrow => skillDict[SkillType.SwordThrow] as Skill_SwordThrow;
    public Skill_TimeEcho timeEcho => skillDict[SkillType.TimeEcho] as Skill_TimeEcho;
    public Skill_DomainExpansion domainExpansion => skillDict[SkillType.DomainExpansion] as Skill_DomainExpansion;
    public Skill_DoubleJump doubleJump => skillDict[SkillType.Jump] as Skill_DoubleJump;
    public Skill_Base[] allSkills { get; private set; }

    private void Awake()
    {
        var skills = GetComponentsInChildren<Skill_Base>();
        skillDict = new Dictionary<SkillType, Skill_Base>();
        foreach (var skill in skills)
            skillDict[skill.GetSkillType()] = skill;
        
        allSkills = skills;
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in allSkills)
            skill.ReduceCooldownBy(amount);
    }

    public Skill_Base GetSkillByType(SkillType type)
        => skillDict.TryGetValue(type, out var skill) ? skill : null;
}