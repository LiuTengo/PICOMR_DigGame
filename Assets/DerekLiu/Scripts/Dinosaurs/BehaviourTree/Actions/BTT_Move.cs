using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class BTT_Move : Action
{
	public float MoveRadius = 2f;      // 在目标周围多大范围内查找可行走点
	public int RacialSampleCount = 3;
	public int RadiusSampleCount = 3;
	
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
			var pos = FindWalkablePositionNearTarget();
			
			navMeshAgent.isStopped = false;
			navMeshAgent.destination = pos[Random.Range(0, pos.Count)];
			bRunning = true;

			// Vector3 destination;
			// if (FindWalkablePositionNearTarget(MoveRadius, out destination))
			// {
			// 	navMeshAgent.isStopped = false;
			// 	navMeshAgent.destination = destination;
			// 	bRunning = true;
			// }
			// else
			// {
			// 	navMeshAgent.destination = transform.position;
			// 	bRunning = true;
			// }
		}
	}

	public override TaskStatus OnUpdate()
	{
		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance 
		    || !navMeshAgent.hasPath)
		{
			return TaskStatus.Success;
		}

		if (navMeshAgent.speed <= 0.00001f)
		{
			return TaskStatus.Success;
		}
		
		animator.SetFloat("MoveSpeed", navMeshAgent.speed);
		Vector3 dir = Vector3.Normalize(navMeshAgent.velocity);
		transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
		return TaskStatus.Running;
	}
	
	public override void OnEnd()
	{
		navMeshAgent.isStopped = true;
		bRunning = false;
		animator.SetFloat("MoveSpeed", 0.0f);
		base.OnEnd();
	}
	
	List<Vector3> FindWalkablePositionNearTarget()
	{
		float RacialInterval = 2*Mathf.PI / (float)RacialSampleCount;
		float RadiusInterval = MoveRadius / (float)RadiusSampleCount;
		List<Vector3> walkablePositions = new List<Vector3>();

		for (int h = 0; h < RacialSampleCount; h++)
		{
			float theta = h * RacialInterval;
			Vector3 dir = new Vector3(Mathf.Cos(theta),0,Mathf.Sin(theta));
			for (int v = 1; v < RadiusSampleCount; v++)
			{
				Vector3 samplePos = transform.position + dir * v*RadiusInterval;
				//FIXME: 采样寻路组件中可走的位置
				NavMesh.SamplePosition(samplePos, out NavMeshHit hit,20.0f,NavMesh.AllAreas);
				if (hit.hit)
				{
					walkablePositions.Add(hit.position);
				}
			}
		}
		
		return walkablePositions;
	}
}