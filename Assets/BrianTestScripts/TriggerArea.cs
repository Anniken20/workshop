using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
  public int pickup;
  public void OnTriggerEnter(Collider other)
 {
  if(pickup == 1)
  {
    GameEvents.current.DoorwayTriggerEnter(pickup);
  }
 }

  public void OnTriggerExit(Collider other)
 {
   GameEvents.current.DoorwayTriggerExit(pickup);
 }
}
