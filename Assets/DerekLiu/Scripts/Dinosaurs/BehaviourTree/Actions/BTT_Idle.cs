using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class BTT_Idle : Action
{
	public float WaitTime;
	public float RandomTime;

	private float stateInterval;
	private float startTime;
	private bool bRunning = false; 
	private Animator animator;
	private NavMeshAgent navMeshAgent;
	
	public override void OnStart()
	{
		if (animator == null)
		{
			animator = gameObject.GetComponent<Animator>();
		}

		if (navMeshAgent == null)
		{
			navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		}
		
		animator.SetFloat("MoveSpeed", 0.0f);

		if (!bRunning)
		{
			navMeshAgent.isStopped = true;
			stateInterval = Mathf.Abs(WaitTime + Random.Range(-RandomTime, RandomTime));
			startTime = Time.time;
			bRunning = true;
		}
	}

	public override TaskStatus OnUpdate()
	{
		if (Time.time - startTime >= stateInterval)
		{
			return TaskStatus.Success;
		}
		
		return TaskStatus.Running;
	}

	public override void OnEnd()
	{
		navMeshAgent.isStopped = false;
		bRunning = false;
		base.OnEnd();
	}
}