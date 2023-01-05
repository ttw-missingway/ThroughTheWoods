using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Systems
{

    [CreateAssetMenu(fileName = "newDecision", menuName = "Decision", order = 4)]
    public class DecisionData : ScriptableObject
    {
        public string Narration;
        public List<ActionPositionPair> ActionToPerform;
        public List<StatusEnemyPair> InfluencingStatuses;
        public List<Terrain> InfluencingTerrain;
        public List<WeatherData> InfluencingWeather;
        public List<ObstacleData> InfluencingObstacles;
        public List<TrapData> InfluencingTraps;
        public List<ActionPositionPair> InfluencingAbilities; 
        public ActionData OpportunityAbility;
        public List<ActionData> ReactiveAbilities;
        public List<StatusEnemyPair> ReactiveStatuses;
        public DecisionData ReactivePath;
        public DecisionData DefaultPath;
        public DecisionData SuccessPath;
        public DecisionData FailurePath;
    }

    [Serializable]
    public struct StatusEnemyPair{
        public StatusEffect StatusEffect;
        public EnemyActor Enemy;
    }

    [Serializable]
    public struct ActionPositionPair{
        public ActionData Action;
        public int Position;
    }

    public enum PathModel{
        Default,
        SuccessFailure,
        Opportunity
    }

    public enum PlayerActor{
        Art,
        Aisling,
        Nicolo,
        Kazan,
        Wren,
        Freyja,
        Krusk,
        Kanaloa,
        Whisperwill,
        Giles,
        Rusalka,
        Unit,
        Lupin,
        Feist,
        Suzume,
        Raza
    }

    public enum EnemyActor{
        ShadowWolf,
        Golem,
        ShadowWolves,
        Gevaudan,
        ChoCho,
        Poltergeist,
        BabaYaga,
        Itzcoatl,
        Camazotz,
        Wendigo,
        Kraken,
        Atlas,
        Twins,
        Suzume,
        Daphne,
        Ipabog,
        Airavata,
        Centauride,
        DeepOnes,
        Djinn,
        Feist,
        Simurgh,
        Anansi,
        Puck,
        Coyote
    }
}