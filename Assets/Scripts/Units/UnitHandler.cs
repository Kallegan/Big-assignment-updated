using System;
using System.Collections;
using System.Collections.Generic;
using FG.Assignment.Player;
using UnityEngine;
using UnityEngine.UI;
using FG.Assignment.Player;

namespace FG.Assignment.Units
{
    public class UnitHandler : MonoBehaviour
    {
        public static UnitHandler instance;
        
        [SerializeField]
        private BasicUnit worker, warrior, healer;

        public LayerMask pUnitLayer, EunitLayer;

        private void Awake() //instance starts before any other scripts needs to referense it.
        {
            instance = this;
        }

        private void Start() //sets referense to our layers / player and enemy.
        {
            pUnitLayer = LayerMask.NameToLayer("PlayerUnits");
            EunitLayer = LayerMask.NameToLayer("EnemyUnits");
        }

        //function used for integer 
        public UnitStatTypes.Base GetBasicUnitStats(string type)
        {
            BasicUnit unit;
            switch (type) //switch statement to declare the type of unit to give it the correct stats.
            {
                case "worker":
                    unit = worker;
                    break;
                case "warrior":
                    unit = warrior;
                    break;
                
                case "healer":
                    unit = healer;
                    break;
                default: // if none of the cases are correct.
                    Debug.Log($"Unit Type: {type} could not be found or does not exist!");
                    return null;
            }

            return unit.baseStats;
        }

        public void SetBasicUnitStats(Transform type) //set stats on both enemy and player units depending on template selected.
        {
            Transform pUnits = PlayerManager.Instance.playerUnits;
            Transform eUnits = PlayerManager.Instance.enemyUnits;
            
            
            foreach (Transform child in type)
            {
                foreach (Transform unit in child)
                {
                    string unitName =
                        child.name.Substring(0, child.name.Length - 1).ToLower(); // make it so the plural form of the units matches by subtracting the end letter S and making all letter to lowercase. 
                    var stats = GetBasicUnitStats(unitName); //using var because it can handle all the integers without typing all of the different ints.
                   
                    //sets unit stats
                    if (type == pUnits)
                    {
                        Player.PlayerUnit pU = unit.GetComponent<Player.PlayerUnit>();
                        pU.baseStats = GetBasicUnitStats(unitName);
                    }
                    else if (type == eUnits)
                    {
                        Enemy.EnemyUnit eU = unit.GetComponent<Enemy.EnemyUnit>();
                        eU.baseStats = GetBasicUnitStats(unitName);
                    }
                }
            }
        }
        
    }
}

