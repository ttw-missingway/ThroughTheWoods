using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Systems
{
    public class TTWMath
    {
        public float CalculateLikelihood(ActorStats actorStats, ActionData actionData, EnemyData enemyData)
        {
            float baseValue = actionData.BaseLikelihood;
            ArchetypeEntity archetype = actorStats.AllArchetypes.Where(a => a.Archetype == actionData.Archetype).FirstOrDefault();

            if (actionData.AirResFactor) baseValue *= enemyData.ResistanceToAirEffects;
            if (actionData.ElecResFactor) baseValue *= enemyData.ResistanceToElectricLight;
            if (actionData.GroundResFactor) baseValue *= enemyData.ResistanceToGroundEffects;
            if (actionData.HearingFactor) baseValue *= enemyData.Hearing;
            if (actionData.IntellectFactor) baseValue *= enemyData.Intellect;
            if (actionData.LampResFactor) baseValue *= enemyData.ResistanceToLampLight;
            if (actionData.MagicFactor) baseValue *= enemyData.Magic;
            if (actionData.MagicResFactor) baseValue *= enemyData.MagicResistance;
            if (actionData.MoonResFactor) baseValue *= enemyData.ResistanceToMoonLight;
            if (actionData.PhysResFactor) baseValue *= enemyData.PhysicalResistance;
            if (actionData.SightFactor) baseValue *= enemyData.Sight;
            if (actionData.SizeFactor) baseValue *= enemyData.Size;
            if (actionData.SmellFactor) baseValue *= enemyData.Smell;
            if (actionData.SpeedFactor) baseValue *= enemyData.Speed;
            if (actionData.StrengthFactor) baseValue *= enemyData.Strength;
            if (actionData.SunResFactor) baseValue *= enemyData.ResistanceToSunLight;
            if (actionData.TrapResFactor) baseValue *= enemyData.ResistanceToTraps;
            if (actionData.WaterResFactor) baseValue *= enemyData.ResistanceToWaterEffects;
            if (actionData.WeatherResFactor) baseValue *= enemyData.ResistanceToWeather;


            baseValue *= ConvertArchetypeLevelToMultiplier(archetype.Level);

            return baseValue;
        }

        public float ConvertArchetypeLevelToMultiplier(int level){
            return (level - 1) * 0.1f + 0.5f; 
        }
    }
}