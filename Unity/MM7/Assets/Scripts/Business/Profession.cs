﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Business
{
    public enum ProfessionCode
    {
        Knight,
        Paladin,
        Ranger,
        Cleric,
        Druid,
        Sorcerer,
        Archer,
        Monk,
        Thief,
    }
    
    public class Profession
    {
        public ProfessionCode ProfessionCode { get; private set; }
        public string Name { get { return ProfessionCode.ToString(); } }
        public int StartingHitPoints { get; private set; }
        public int HitPointsPerLevel { get; private set; }
        public int StartingSpellPoints { get; private set; }
        public int SpellPointsPerLevel { get; private set; }
        public IList<SkillCode> DefaultSkills { get; private set; }
        public IList<SkillCode> AvailableSkills { get; private set; }

        public Profession(ProfessionCode code) {
            ProfessionCode = code;
        }

        public Func<PlayingCharacter, int> GetSpellPointsAttributeValue { get; private set; }


        public static Profession Knight() {
            return new Profession(ProfessionCode.Knight) {
                StartingHitPoints = 40,
                HitPointsPerLevel = 5,
                StartingSpellPoints = 0,
                SpellPointsPerLevel = 0,
                DefaultSkills = new SkillCode[] { SkillCode.Sword, SkillCode.Leather },
                AvailableSkills = new SkillCode[] { 
                    SkillCode.Axe, SkillCode.Spear, SkillCode.Bow, 
                    SkillCode.Mace, SkillCode.Shield, SkillCode.Chain, 
                    SkillCode.BodyBuilding, SkillCode.Perception, SkillCode.Armsmaster
                },
                GetSpellPointsAttributeValue = (playingCharacter) => 0,
            };
        }

        public static Profession Paladin() {
            return new Profession(ProfessionCode.Paladin) {
                StartingHitPoints = 30,
                HitPointsPerLevel = 5,
                StartingSpellPoints = 4,
                SpellPointsPerLevel = 1,
                DefaultSkills = new SkillCode[] { SkillCode.Mace, SkillCode.Spirit },
                AvailableSkills = new SkillCode[] { 
                    SkillCode.Sword, SkillCode.Dagger, SkillCode.Axe,
                    SkillCode.Shield, SkillCode.Leather, SkillCode.Merchant,
                    SkillCode.RepairItem, SkillCode.BodyBuilding, SkillCode.Armsmaster
                },
                GetSpellPointsAttributeValue = (playingCharacter) => playingCharacter.Personality,
            };
        }

        public static Profession Ranger() {
            return new Profession(ProfessionCode.Ranger) {
                StartingHitPoints = 30,
                HitPointsPerLevel = 4,
                StartingSpellPoints = 0,
                SpellPointsPerLevel = 0,
                DefaultSkills = new SkillCode[] { SkillCode.Axe, SkillCode.Perception },
                AvailableSkills = new SkillCode[] { 
                    SkillCode.Sword, SkillCode.Dagger, SkillCode.Bow,
                    SkillCode.Leather, SkillCode.BodyBuilding, SkillCode.DisarmTrap,
                    SkillCode.Dodging, SkillCode.IdentifyMonster, SkillCode.Armsmaster
                },
                GetSpellPointsAttributeValue = (playingCharacter) => 0,
            };
        }

        public static Profession Cleric() {
            return new Profession(ProfessionCode.Cleric) {
                StartingHitPoints = 25,
                HitPointsPerLevel = 2,
                StartingSpellPoints = 10,
                SpellPointsPerLevel = 3,
                DefaultSkills = new SkillCode[] { SkillCode.Mace, SkillCode.Body },
                AvailableSkills = new SkillCode[] { 
                    SkillCode.Shield, SkillCode.Leather, SkillCode.Spirit,
                    SkillCode.Mind, SkillCode.Merchant, SkillCode.RepairItem,
                    SkillCode.Meditation, SkillCode.Alchemy, SkillCode.Learning,
                },
                GetSpellPointsAttributeValue = (playingCharacter) => playingCharacter.Personality,
            };
        }

        public static Profession Druid() {
            return new Profession(ProfessionCode.Druid) {
                StartingHitPoints = 20,
                HitPointsPerLevel = 2,
                StartingSpellPoints = 10,
                SpellPointsPerLevel = 3,
                DefaultSkills = new SkillCode[] { SkillCode.Dagger, SkillCode.Earth },
                AvailableSkills = new SkillCode[] { 
                    SkillCode.Mace, SkillCode.Leather, SkillCode.Water,
                    SkillCode.Spirit, SkillCode.Body, SkillCode.Meditation,
                    SkillCode.Perception, SkillCode.Alchemy, SkillCode.Learning,
                },
                GetSpellPointsAttributeValue = (playingCharacter) => (playingCharacter.Personality + playingCharacter.Intellect) / 2,
            };
        }

        public static Profession Sorcerer() {
            return new Profession(ProfessionCode.Sorcerer) {
                StartingHitPoints = 20,
                HitPointsPerLevel = 2,
                StartingSpellPoints = 15,
                SpellPointsPerLevel = 3,
                DefaultSkills = new SkillCode[] { SkillCode.Staff, SkillCode.Fire },
                AvailableSkills = new SkillCode[] { 
                    SkillCode.Dagger, SkillCode.Leather, SkillCode.Air,
                    SkillCode.Water, SkillCode.Earth, SkillCode.IdentifyItem,
                    SkillCode.Merchant, SkillCode.IdentifyMonster, SkillCode.Alchemy,
                },
                GetSpellPointsAttributeValue = (playingCharacter) => playingCharacter.Intellect,
            };
        }

        public static Profession Archer() {
            return new Profession(ProfessionCode.Archer) {
                StartingHitPoints = 30,
                HitPointsPerLevel = 3,
                StartingSpellPoints = 5,
                SpellPointsPerLevel = 1,
                DefaultSkills = new SkillCode[] { SkillCode.Bow, SkillCode.Air },
                AvailableSkills = new SkillCode[] { 
                    SkillCode.Sword, SkillCode.Axe, SkillCode.Spear,
                    SkillCode.Leather, SkillCode.Fire, SkillCode.Water,
                    SkillCode.Perception, SkillCode.Armsmaster, SkillCode.Learning,
                },
                GetSpellPointsAttributeValue = (playingCharacter) => playingCharacter.Intellect,
            };
        }

        public static Profession Monk() {
            return new Profession(ProfessionCode.Monk) {
                StartingHitPoints = 35,
                HitPointsPerLevel = 5,
                StartingSpellPoints = 0,
                SpellPointsPerLevel = 0,
                DefaultSkills = new SkillCode[] { SkillCode.Dodging, SkillCode.Unarmed },
                AvailableSkills = new SkillCode[] { 
                    SkillCode.Staff, SkillCode.Sword, SkillCode.Dagger,
                    SkillCode.Spear, SkillCode.Leather, SkillCode.BodyBuilding,
                    SkillCode.Perception, SkillCode.IdentifyMonster, SkillCode.Armsmaster,
                },
                GetSpellPointsAttributeValue = (playingCharacter) => 0,
            };
        }

        public static Profession Thief() {
            return new Profession(ProfessionCode.Thief) { 
                StartingHitPoints = 35,
                HitPointsPerLevel = 4,
                StartingSpellPoints = 0,
                SpellPointsPerLevel = 0,
                DefaultSkills = new SkillCode[] { SkillCode.Dagger, SkillCode.Stealing },
                AvailableSkills = new SkillCode[] { 
                    SkillCode.Sword, SkillCode.Bow, SkillCode.Leather,
                    SkillCode.IdentifyItem, SkillCode.Merchant, SkillCode.Perception,
                    SkillCode.DisarmTrap, SkillCode.Dodging, SkillCode.Alchemy,
                },
                GetSpellPointsAttributeValue = (playingCharacter) => 0,
            };
        }

    }
}
