using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



namespace FG.Assignment.Units.Player
{
    [RequireComponent(typeof(NavMeshAgent))] //makes sure that this script attaches a navmesh agent to the object it's attached to.
    public class PlayerUnit : MonoBehaviour
    {
        private NavMeshAgent navAgent; //creates navmesh agent for path finding and movement.

        public UnitStatTypes.Base baseStats; //collection of all base stats for units.

        public GameObject unitStatDisplay; //will display info about units, only shows health so far.

        public Image healthBarAmount; // simple image that shows remaining hp by reducing its fill by damage and exposing the darker background.

        public float currentHealth; //float that stores units current health.
        
        private void OnEnable()
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
        
        private void Start()
        {
            currentHealth = baseStats.health; //sets base hp to all units using basestats on start.
        }

        private void Update() /*updates with current helpt and handles if object lives or dies. Also flips health bars
        to camera so they are more visable. */
        {
            HandleHealth();
        }

        public void MoveUnit(Vector3 _distination) //moves unit to destination.
        {
            navAgent.SetDestination(_distination);
        }

        public void TakeDamage(float damage) //simple damage calculation where armor reduce damage by same amount as base armor.
        {
            float totalDamage = damage - baseStats.armor;
            currentHealth -= totalDamage;
        }

        private void HandleHealth()
        {
            Camera camera = Camera.main;
            unitStatDisplay.transform.LookAt(unitStatDisplay.transform.position + 
                    camera.transform.rotation * Vector3.forward); //turns healthbars to player camera.

            healthBarAmount.fillAmount = currentHealth / baseStats.health;

            if (currentHealth <= 0) //if health drops below or 0, the gameobject is destroyed.
            {
                Die(); //kills the game object by destroying and removing it from the game. 
            }
            
            
        }

        private void Die()
        {
            InputManager.InputHandler.Instance.selectedUnits.Remove(gameObject.transform); //removes selected target from list if the target dies.
            Destroy(gameObject);
        }
    }
    
  

}
