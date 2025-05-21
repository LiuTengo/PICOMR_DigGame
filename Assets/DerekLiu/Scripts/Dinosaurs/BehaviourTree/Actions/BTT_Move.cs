using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class BTT_Move : Action
{
	public float MoveRadius = 2f;      // 在目标周围多大范围内查找可行走点
	public int MaxSampleTries = 3;      // 最大尝试次数
	
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

		if (!bRunning)
		{
			Vector3 destination;
			if (FindWalkablePositionNearTarget(MoveRadius, out destination))
			{
				navMeshAgent.isStopped = false;
				navMeshAgent.destination = destination;
				bRunning = true;
			}
			else
			{
				navMeshAgent.destination = transform.position;
				bRunning = true;
			}
		}
	}

	public override TaskStatus OnUpdate()
	{
		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
		{
			return TaskStatus.Success;
		}
		else
		{
			animator.SetFloat("MoveSpeed", navMeshAgent.velocity.magnitude);
		}
		return TaskStatus.Running;
	}
	
	public override void OnEnd()
	{
		navMeshAgent.isStopped = true;
		bRunning = false;
		animator.SetFloat("MoveSpeed", 0.0f);
		base.OnEnd();
	}
	
	bool FindWalkablePositionNearTarget(float radius, out Vector3 result)
	{
		for (int i = 0; i < MaxSampleTries; i++)
		{
			Vector3 randomOffset = Random.insideUnitSphere * radius;
			randomOffset.y = 0f;
			Vector3 samplePos = transform.position + randomOffset;

			NavMeshHit hit;
			if (NavMesh.SamplePosition(samplePos, out hit, 10.0f, NavMesh.AllAreas))
			{
				result = hit.position;
				// 可选：确保距离目标不小于最小距离
				if (Vector3.Distance(result, transform.position) > navMeshAgent.stoppingDistance)
					return true;
			}
		}

		result = transform.position;
		return false;
	}
}