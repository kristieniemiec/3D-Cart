﻿//Code adapted from https://www.patrykgalach.com/2020/03/16/pick-up-items-in-unity/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    //Character camera
    [SerializeField]
    private Camera characterCamera;

    //Slot for holding item
    [SerializeField]
    private Transform slot;

    //Currently held item
    private PickableItem pickedItem;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            //Check if any item held, drop if so
            if(pickedItem)
            {
                DropItem(pickedItem);
            }
            //Otherwise, try to pick up item in cursor
            else
            {  
                //Create ray to check item in front of camera
                var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
                RaycastHit hit;

                //Check if the item is pickable and then pick up it so
                if(Physics.Raycast(ray, out hit, 10))
                {
                    print("Ray hit");
                    var pickable = hit.transform.GetComponent<PickableItem>();

                    if (pickable)
                    {
                        PickItem(pickable);
                    }
                }
            }
        }
    }

    //Method to pick-up item 
    private void PickItem(PickableItem item)
    {
        //Assign this class's reference to new item
        pickedItem = item;

        //Disable rigidbody & reset velocities
        item.Rb.isKinematic = true;
        item.Rb.velocity = Vector3.zero;
        item.Rb.angularVelocity = Vector3.zero;

        //move item to character's slot
        item.transform.SetParent(slot);

        //Reset position and rotation
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
    }

    //Method to drop item
    private void DropItem(PickableItem item)
    {
        //Remove reference
        pickedItem = null;

        //Remove Parent
        item.transform.SetParent(null);

        //Re-enable rigidbody
        item.Rb.isKinematic = false;

        //Throw item forward slightly
        item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
    }
}