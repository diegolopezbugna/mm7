using System;

namespace Business
{
    public class SkillStatus
    {
        public Skill Skill { get; private set; }
        public int Points { get; private set; }
        public SkillLevel SkillLevel { get; private set; }
        
        public SkillStatus(SkillCode skillCode)
        {
            Skill = Skill.Get(skillCode);
            Points = 1;
            SkillLevel = SkillLevel.Normal;
        }

        public SkillStatus(SkillCode skillCode, int points, SkillLevel skillLevel)
        {
            Skill = Skill.Get(skillCode);
            Points = points;
            SkillLevel = skillLevel;
        }

    }
}

