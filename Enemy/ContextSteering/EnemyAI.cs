using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private List<SteeringBehaviour> steeringBehaviours;
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private AIData aiData;

    [SerializeField] private float detectionDelay = 0.05f;
    [SerializeField] private float aiUpdateDelay = 0.06f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float attackDistance = 0.5f;

    [SerializeField] private ContextSolver movementDirectionSolver;

    private Vector2 movementInput;
    private Vector2? lastKnownPosition;

    private bool following = false;

    private Enemy_Movement movementController;
    // Returns true if we currently have a target
    // public bool HasTarget => aiData.currentTarget != null;

    // Returns true if the target is within attack distance
    // public bool InAttackRange => aiData.currentTarget != null && 
                            //  Vector2.Distance(aiData.currentTarget.position, transform.position) < attackDistance;


    private void Start()
    {
        movementController = GetComponent<Enemy_Movement>();

        // Run detectors repeatedly
        InvokeRepeating(nameof(PerformDetection), 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
            detector.Detect(aiData);
    }

    private void Update()
    {
        if (aiData.currentTarget != null)
        {
            lastKnownPosition = aiData.currentTarget.position;
            if (!following)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            aiData.currentTarget = aiData.targets[0];
        }

        movementController.Move(movementInput);
    }

    private IEnumerator ChaseAndAttack()
    {
        while (aiData.currentTarget != null || lastKnownPosition.HasValue)
        {
            Vector2 targetPos = aiData.currentTarget != null
                ? aiData.currentTarget.position
                : lastKnownPosition.Value;

            float distance = Vector2.Distance(targetPos, transform.position);

            if (distance < attackDistance && aiData.currentTarget != null)
            {
                movementInput = Vector2.zero;
                movementController.Attack();
                yield return new WaitForSeconds(attackDelay);
            }
            else
            {
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
            }

            // Clear last known if reached
            if (aiData.currentTarget == null && distance < 0.1f)
                lastKnownPosition = null;
        }

        movementInput = Vector2.zero;
        following = false;
    }
}
