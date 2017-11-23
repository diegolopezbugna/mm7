using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using UnityStandardAssets.Characters.FirstPerson;

public class PartyAttack : MonoBehaviour, PartyAttacksViewInterface {

    private const int CHARS = 4;

    [SerializeField]
    private Rigidbody arrow;

    [SerializeField]
    private GameObject[] weaponContainers;

    private float[] lastAttack;
    private int lastCharAttacker = -1;
    private List<Animator> weaponAnimators = new List<Animator>();

	void Start () 
    {
        foreach (var wc in weaponContainers)
        {
            weaponAnimators.Add(wc.GetComponentInChildren<Animator>());
        }
    }
	
	void Update () 
    {
	}

    public void DoAttack(PlayingCharacter attackingChar, Vector3? targetPoint, Transform targetTransform) 
    {
        var partyAttacksUseCase = new PartyAttacksUseCase(this, Party.Instance, targetPoint, targetTransform, FirstPersonController.Instance.transform);

        if (targetTransform != null && targetTransform.tag.StartsWith("Enemy"))
        {
            var enemyAttackBehaviour = targetTransform.GetComponent<EnemyAttack>();
            if (enemyAttackBehaviour != null) // TODO: prevent attacking a dying enemy
            {
                partyAttacksUseCase.TryHit(attackingChar, enemyAttackBehaviour.ArmorClass);
            }
        }
        else
        {
            partyAttacksUseCase.HitNothing(attackingChar);
        }
    }

    public void ThrowArrowToTarget(PlayingCharacter attackingChar, Transform targetTransform,  Vector3 targetPoint, bool didHit, int damage) {
        var origin = GetProjectilOrigin(attackingChar);
        var a = Instantiate(arrow);

        var enemyAttackBehaviour = targetTransform.GetComponent<EnemyAttack>();
        if (enemyAttackBehaviour != null) // TODO: prevent attacking a dying enemy
        {
            a.GetComponent<ArrowMove>().SetTarget(targetTransform, didHit, () =>
                {
                    var scriptHealth = targetTransform.GetComponent<EnemyHealth>();
                    if (scriptHealth != null && scriptHealth.IsActive())
                    {
                        // TODO: move some of this logic to the useCase?
                        enemyAttackBehaviour.AlertOthers();
                        scriptHealth.TakeHit(damage);
                    }
                });
        }

        a.transform.position = origin;
        a.transform.LookAt(targetPoint);
        // TODO: if enemy is moving, calc rotation to catch it
        a.velocity = a.transform.forward * 40f;
    }

    public void ThrowArrowToNonInteractiveObjects(PlayingCharacter attackingChar, Vector3? targetPoint) {
        var origin = GetProjectilOrigin(attackingChar);
        var a = Instantiate(arrow);

        a.transform.position = origin;
        if (targetPoint.HasValue)
            a.transform.LookAt(targetPoint.Value);
        else
            a.transform.rotation = transform.rotation;

        a.velocity = a.transform.forward * 40f;
    }

    // TODO: make ThrowArrow behave like ThrowSpell
    public void ShowSpellFx(PlayingCharacter attackingChar, SpellInfo spell, Vector3? targetPoint, Action<Transform> onCollision) {
        // TODO: onCOllission?? Immolation?
        if (spell.Code == (int)SpellCodes.Air_LightningBolt)
        {
            var spellFX = InstantiateSpellFx(spell, GetProjectilOrigin(attackingChar), transform.rotation);
            spellFX.transform.LookAt(targetPoint.Value);
        }
        else
        {
            if (targetPoint != null)
            {
                if (spell.Code == (int)SpellCodes.Fire_Incinerate)  // TODO: make it better!!!
                {
                    // spell needs to start on floor
                    RaycastHit hitInfo;
                    LayerMask floorLayer = 1 << 9;
                    if (Physics.Raycast(targetPoint.Value, Vector3.down, out hitInfo, 500f, floorLayer))
                        InstantiateSpellFx(spell, hitInfo.point, transform.rotation);
                    else
                        InstantiateSpellFx(spell, targetPoint.Value, transform.rotation);
                }
                else
                {
                    InstantiateSpellFx(spell, targetPoint.Value, transform.rotation);
                }
            }
            else
                InstantiateSpellFx(spell, transform.position + Vector3.down, transform.rotation);
        }
    }

    // TODO: make ThrowArrow behave like ThrowSpell
    public void ThrowSpellFx(PlayingCharacter attackingChar, SpellInfo spell, Vector3 targetPoint, Action<Transform> onCollision) {
        var origin = GetProjectilOrigin(attackingChar);
        var spellFX = InstantiateSpellFx(spell, origin, transform.rotation);

        if (onCollision != null)
        {
            var transformMotion = spellFX.GetComponentInChildren<RFX4_TransformMotion>();
            if (transformMotion != null)
                transformMotion.CollisionEnter += (object sender, RFX4_TransformMotion.RFX4_CollisionInfo e) => onCollision(e.Hit.transform);
        }

        spellFX.transform.LookAt(targetPoint);
    }

    private GameObject InstantiateSpellFx(SpellInfo spell, Vector3 position, Quaternion rotation) {
        var spellFxPrefab = Resources.Load<GameObject>("SpellsFX/" + spell.SpellFxName);    // TODO: cache!!!
        if (spellFxPrefab == null)
        {
            Debug.LogError("SpellFx prefab " + spell.SpellFxName + " not found");
            return null;
        }
        var spellFX = Instantiate(spellFxPrefab, position, transform.rotation) as GameObject;
        return spellFX;
    }

    public void HandToHandAttack(PlayingCharacter attackingChar, Transform targetTransform, bool didHit, int damage) {
        int charIndex = Game.Instance.PartyStats.Chars.IndexOf(attackingChar);
        weaponAnimators[charIndex].SetTrigger("Attack");

        if (targetTransform != null)
        {
            if (didHit)
            {
                var scriptHealth = targetTransform.GetComponent<EnemyHealth>();
                if (scriptHealth != null && scriptHealth.IsActive())
                {
                    // TODO: move some of this logic to the useCase?
                    scriptHealth.TakeHit(damage);
                }
            }
        }
    }

    public void AddMessage(string message) {
        MessagesScroller.Instance.AddMessage(message);
    }

    private Vector3 GetProjectilOrigin(PlayingCharacter attackingChar)
    {
        int charIndex = Game.Instance.PartyStats.Chars.IndexOf(attackingChar);
        return transform.TransformPoint((charIndex - 1.5f) / 2f, 0.5f, 0);

    }
}
