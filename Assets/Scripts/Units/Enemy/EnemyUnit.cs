using System;
using System.Collections;
using System.Collections.Generic;
using FG.Assignment.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace FG.Assignment.Units.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))] //adds navmeshagent to object.
    public class EnemyUnit : MonoBehaviour
    {
        private NavMeshAgent navAgent; //navAgent for movement and pathfinding.
        
        public UnitStatTypes.Base baseStats; //unit basestats.

        private Collider[] rangeColliders; //uses range and position to determine aggro.

        private Transform aggroTarget; //if in aggrorange, the enemy will lock on that target.

        private Player.PlayerUnit aggroUnit; //unit with the current aggro.

        private bool hasAggro = false; //default aggro is false, and if no target is in range it should be set to default again.

        private float distance; //distance from enemy with aggro to target getting chased.
        
        public GameObject unitStatDisplay; //used for stat display. Currently only displays HP.

        public Image healthBarAmount; //displays remaining hp.

        public float currentHealth; //stores units current HP.

        private float atkCooldown; //stores attackspeed on enemy units. 

        private void Start()
        {
            navAgent = gameObject.GetComponent<NavMeshAgent>(); //set navAgent.
            currentHealth = baseStats.health; //set health on each unit at start to match their template.
        }

        private void Update() //calls methods to ether move to target (if aggro) else search for target withing range to aggro.
        {
            atkCooldown -= Time.deltaTime;
            
            if (!hasAggro) 
            {
                CheckForEnemyTargets();
            }
            else
            {
                Attack();
                MoveToAggroTarget();
            }
        }

        private void LateUpdate() //using lateupdate to call handle health method.
        {
            HandleHealth();
        }

        private void CheckForEnemyTargets()
        {
            rangeColliders = Physics.OverlapSphere(transform.position, baseStats.aggroRange);

            for (int i = 0; i < rangeColliders.Length; i++)
            {
                if (rangeColliders[i].gameObject.layer == UnitHandler.instance.pUnitLayer) //check the object entered is in our layer. 
                {
                    aggroTarget = rangeColliders[i].gameObject.transform; //set aggro on target
                    aggroUnit = aggroTarget.gameObject.GetComponent<Player.PlayerUnit>();
                    hasAggro = true;
                    break;
                }
            }
        }

        private void Attack() /*enemy will attack if attack cooldown is over and if their distance from our units is less than or
        the same as their range. */
        {
            if (atkCooldown <= 0 && distance <= baseStats.atkRange + 1)
            {
                aggroUnit.TakeDamage(baseStats.attack); //does damage to the attacked target based on base stats.
                atkCooldown = baseStats.atkSpeed;
            }
        }
        
        public void TakeDamage(float damage) //will be used for when enemy unit is damaged.
        {
            float totalDamage = damage = baseStats.armor;
            currentHealth -= totalDamage;
        }

        private void MoveToAggroTarget()
        {
            if (aggroTarget == null)
            {
                navAgent.SetDestination(transform.position); //sets new target to our own after target dies / is invalid.
                hasAggro = false;
            }
            else
            {
                distance = Vector3.Distance(aggroTarget.position, transform.position); //moves aggroed target to our position.  
                navAgent.stoppingDistance = (baseStats.atkRange +1 );//stops them before they reach us.

                if (distance <= baseStats.aggroRange) //check if enemy is in range.
                {
                    navAgent.SetDestination(aggroTarget.position); //moves enemy to aggro targets position.
                }
            }
        }
        
        private void HandleHealth()
        {
            Camera camera = Camera.main;
            unitStatDisplay.transform.LookAt(unitStatDisplay.transform.position + 
                                             camera.transform.rotation * Vector3.forward); //turns healthbars to player camera.

            healthBarAmount.fillAmount = currentHealth / baseStats.health;

            if (currentHealth <= 0) //if health drops below or 0, the gameobject is destroyed.
            {
                Die();
            }
        }

        private void Die() //when object dies, it will be removed.
        {
            Destroy(gameObject);
        }
    }

}
