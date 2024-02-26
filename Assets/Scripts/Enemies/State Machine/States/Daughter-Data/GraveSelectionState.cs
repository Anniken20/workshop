using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveSelectionState : EnemyState
{
    private bool startShake;
    [HideInInspector] public DaughterData daughterData;

    public GraveSelectionState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        //graves = FindObjectOfType<GraveContainer>().graves;
        daughterData = (DaughterData)enemy.FindData("DaughterData");
        //daughterData.graveShaker.ShakeGrave();
        FindObjectOfType<GraveContainer>().ShakeGrave();
        //daughterData.graveContainer.ShakeGrave();
        //ShakeGrave();

    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
    /*private GameObject SelectGrave()
    {
        if(graves == null || graves.Length == 0)
        {
            Debug.LogWarning("The graves list is empty or null because of the implication...");
            return null;
        }
        int randomGrave = Random.Range(0, graves.Length);
        return graves[randomGrave];
    }
    private void ShakeGrave()
    {
        var selectedGrave = SelectGrave();
        //selectedGrave.GetComponent<GraveShaker>().SetValues(true, daughterData.shakeSpeed, daughterData.shakeAmount, daughterData.axis.ToString());
        if (selectedGrave != null)
        {
            Debug.Log("Selected Grave: " +selectedGrave.ToString());
            StartCoroutine(Shaking(selectedGrave));
        }
    }
    IEnumerator Shaking(GameObject grave)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < daughterData.shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        daughterData.selectedGrave = grave;
        this.GetComponent<Daughter>().EnterWAM();
    }*/
}