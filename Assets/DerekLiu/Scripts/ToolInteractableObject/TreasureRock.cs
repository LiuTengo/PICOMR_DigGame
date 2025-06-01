using System;
using System.Collections;
using Unity.XR.PXR;
using UnityEngine;

namespace DerekLiu.Scripts
{
    public class TreasureRock : TreasureSandBase
    {
        [SerializeField] private Transform visual;
        [SerializeField] private GameObject pieces;
        [SerializeField] private AnimationCurve knockCurve;
        [SerializeField] private float knockDuration = 0.1f;

        private void OnEnable()
        {
            SetSpawnGameObject(TreasureManager.instance.GetRandomTreasurePrefab());
        }

        public override void OnToolInteract(DerekDigGameTool tool)
        {
            base.OnToolInteract(tool);
            
            if (tool.toolType == DerekDigGameToolType.Hammer)
            {
                OnHit();
            }
        }
        
        // Sets number of pickups to spawn.
        public override void OnHit()
        {
            currentInteractCount++;
    
            // Knock animation.
            StartCoroutine(Animate());
            //Play FeedBack And FeedBack
            PXR_Input.SendHapticImpulse(PXR_Input.VibrateType.RightController, 0.5f, 600, 150);
            particleSystem.Play();
            audioSource?.PlayOneShot(audioClip);

            if (currentInteractCount == (maxInteractCount/2))
            {
                Vector3 position = visual.position;
                Quaternion rotation = visual.rotation;
                var newVisual = Instantiate(pieces, position, rotation,transform);
                newVisual.transform.localScale = visual.localScale;
                
                Destroy(visual.gameObject);
                
                visual = newVisual.transform;
            }
            else if(currentInteractCount >= maxInteractCount)
            {
                //Spawn Treasure
                Vector3 position = visual.position;
                Quaternion rotation = visual.rotation;
                TreasureManager.instance.SpawnTreasure(spawnIndex,position, rotation);
                
                sandManager.DestroyTreasureSandInPlane(this);
            }
        }
    
        private IEnumerator Animate() //Knock animation coroutine.
        {
            float t = 0;
            while (t < knockDuration)
            {
                float v = knockCurve.Evaluate(t / knockDuration);
                transform.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(knockAngle), v);
                t += Time.deltaTime;
                
                yield return null;
            }
            triggered = false; 
        }
    }
}