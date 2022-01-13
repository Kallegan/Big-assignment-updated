using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FG.Assignment.Units
{
    
    [CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/Basic")]
    public class BasicUnit : ScriptableObject
    {
        public enum UnitType //simple unit template using enum. 
        {
            Worker,
            Warrior,
            Healer,
        };
        
        [Space(7)] //adding spaces in the unity menu so its easier to read.
        [Header("Unit Settings")] //header that splits the settings from base stats.
        [Space(20)]
        public bool isPlayerUnit;

        public UnitType type;
        
        public new string name;
        
        public GameObject heroesPrefab; //player prefab
        public GameObject monsterPrefab; //enemy prefab

        [Space(7)] 
        [Header("Unit Base Stats")] 
        [Space(20)]

        public UnitStatTypes.Base baseStats;

    }
}

