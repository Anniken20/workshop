using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class KnockbackState : EnemyState
{
    private Transform player; // Reference to the player's transform
    private KnockbackData knockbackData;

    //to set this object inactive and reuse it instead of instantiating and destroying everytime
    private GameObject knockbackObj;

    public KnockbackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        knockbackData = (KnockbackData)enemy.FindData("KnockbackData");
        player = ThirdPersonController.Main.transform;
        if (knockbackObj == null)
        {
            knockbackObj = Instantiate(knockbackData.knockbackPrefab);
            knockbackObj.SetActive(false);
        }
        StartCoroutine(WindupRoutine());
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void ExitState()
    {
        nav.updateRotation = true;
        StopAllCoroutines();
        knockbackObj?.SetActive(false);
        base.ExitState();
    }

    private IEnumerator WindupRoutine()
    {
        FacePlayer();
        yield return new WaitForSeconds(knockbackData.windupTime);
        Attack();
    }

    private void Attack()
    {
        FacePlayer();
        knockbackObj.SetActive(true);
        knockbackObj.transform.position = player.transform.position;
        KnockbackZone kbz = knockbackObj.GetComponent<KnockbackZone>();
        kbz.pushDirection = 
            knockbackData.destinationPosition - player.transform.position;
        kbz.knockbackStrength = knockbackData.knockbackPower;
        StartCoroutine(TurnoffKnockbackZoneRoutine());
    }

    private IEnumerator TurnoffKnockbackZoneRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        knockbackObj.SetActive(false);
    }

    private void FacePlayer()
    {
        nav.updateRotation = false;
        Vector3 lookVector = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
        transform.LookAt(lookVector);
    }
}
