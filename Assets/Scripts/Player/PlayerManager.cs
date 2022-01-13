using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using FG.Assignment.InputManager;


namespace FG.Assignment.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance;

        public Transform playerUnits;
        public Transform enemyUnits;

        private void Awake() //sets stats for all enemy and friendly units.
        {
            Instance = this;
            Units.UnitHandler.instance.SetBasicUnitStats(playerUnits);
            Units.UnitHandler.instance.SetBasicUnitStats(enemyUnits);
        }

        
       
        private void Update() //handles the unit movements.
        {
            InputHandler.Instance.HandleUnitMovement();
        }
    }
}

