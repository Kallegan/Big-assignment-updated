using System;
using System.Collections;
using System.Collections.Generic;
using FG.Assignment.Player;
using UnityEngine;
using FG.Assignment.Units.Player;


namespace FG.Assignment.InputManager
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler Instance;

        private RaycastHit hit; //what we hit with our ray.

        public List<Transform> selectedUnits = new List<Transform>(); //stores selected units in a list.

        private bool isDragging = false; //default set to false and changes as we hold mouse 0/left mouse.

        private Vector3 mousePos; //stores current mouse position.

        private void Awake() //activates instance on start.
        {
            Instance = this;
        }

        private void OnGUI()
        {
            if (isDragging) //draws a rectangle when dragging mouse. 
            {
                Rect rect = MultiSelect.GetScreenRect(mousePos, Input.mousePosition); //detects where the mouse was first pressed and where we currently are.
                MultiSelect.DrawScreenRect(rect, new Color(0f, 0f, 0f, 0.25f)); //uses the rect created in multiselect to draw the rectangle. 
                MultiSelect.DrawScreenRectBorder(rect, 1, Color.blue);
            }
        }

        public void HandleUnitMovement()
        {
            if (Input.GetMouseButtonDown(0)) //left click
            {
                mousePos = Input.mousePosition; //starting point of our drag.
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //sends a ray to see if we hit something.
                
                if (Physics.Raycast(ray, out hit)) //if the ray hits
                {
                    LayerMask layerHit = hit.transform.gameObject.layer; //give us the layer of the gameobject our ray hits and store it in layerHit.

                    switch (layerHit.value)
                    {
                        case 8: //units layer
                            SelectUnit(hit.transform, Input.GetKey(KeyCode.LeftShift)); //allows multiselect if shift key is down.
                            break;
                        default: //works as an else statement.
                            isDragging = true;
                            DeselectUnits(); //deselect units if none are clicked.
                            break;
                    }
                }
                
            }

            if (Input.GetMouseButtonUp(0)) //when letting go of left mouse.
            {
                foreach (Transform child in Player.PlayerManager.Instance.playerUnits) //enables us to select all the units.
                {
                    foreach (Transform unit in child)
                    {
                        if (isWithinSelectionBounds(unit))
                        {
                            SelectUnit(unit, true); 
                        }
                    }
                }
                isDragging = false; //sets dragging to false again.
            }

            if (Input.GetMouseButtonDown(1) && HaveSelectedUnits()) //if rightmousebutton is pressed and units are stored.
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
                if (Physics.Raycast(ray, out hit)) 
                {
                    LayerMask layerHit = hit.transform.gameObject.layer; //give us the layer of the gameobject our ray hits and store it in layerHit.

                    switch (layerHit.value)
                    {
                        case 8: //units layer
                           
                            break;
                        case 9: //enemy unit layer. If hit, attack.
                            //add attack / attackmove.
                            
                            break;
                        default:
                            foreach (Transform unit in selectedUnits) //makes all the units stored in our list of selected units 
                            {
                                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>(); //grabbing the player units component in the selected units.
                                pU.MoveUnit(hit.point); //telling units to move to the point. 
                            }
                            
                            break;
                    }
                }
                
            }
        }

        private void SelectUnit(Transform unit, bool canMultiselect = false) //set a highlight on targeted unit, else deselect.
        {
            if (!canMultiselect)
            {
                DeselectUnits();
            }
            selectedUnits.Add(unit);
            
            unit.Find("Highlight").gameObject.SetActive(true);
            
        }

        private void DeselectUnits()
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].Find("Highlight").gameObject.SetActive(false); //removes highlighted units when deselected. 
            }
            selectedUnits.Clear(); //clears list of selected units.
        }

        private bool isWithinSelectionBounds(Transform tf) //checks if units are inside selection grid.
        {
            if (!isDragging)
            {
                return false;
            }

            Camera cam = Camera.main;
            Bounds vpBounds = MultiSelect.GetVpBounds(cam, mousePos, Input.mousePosition);
            return vpBounds.Contains(cam.WorldToViewportPoint(tf.position));
        }

        private bool HaveSelectedUnits() //checks if any units are selected so it avoids running code that should only be ran when units are targeted when they're not.
        {
            if (selectedUnits.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

