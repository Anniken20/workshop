using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
ALL STATES FROM THE ENEMYAI CONTROLLER ANIMATOR NEED TO BE IN HERE PLEASE
Essentially if we want to change or move things here we can obviously do
But it will need to be added to AI script with the logic for it!

10/26/23 Anniken
*/
public enum AIState
{
    Idle,
    Walk,
    Die,
    MeleeAttack,
    RangeAttack,
    ChargeAttack,
    SlamAttack,
    LobAttack,
    AoEAttack,
    LaunchAttack,
    Evade,
    TakeCover,
    Chase,
    Hide
}