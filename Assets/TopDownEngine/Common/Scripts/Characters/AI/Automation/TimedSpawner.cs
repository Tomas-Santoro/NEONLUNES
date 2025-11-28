using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;

namespace MoreMountains.TopDownEngine
{
	/// <summary>
	/// A class meant to be used in conjunction with an object pool (simple or multiple)
	/// to spawn objects regularly, at a frequency randomly chosen between the min and max values set in its inspector
	/// </summary>
	[AddComponentMenu("TopDown Engine/Character/AI/Automation/TimedSpawner")]
	public class TimedSpawner : TopDownMonoBehaviour 
	{
		/// the object pooler associated to this spawner
		public virtual MMObjectPooler ObjectPooler { get; set; }
		
		[Header("Spawn")]
		/// whether or not this spawner can spawn
		[Tooltip("whether or not this spawner can spawn")]
		public bool CanSpawn = true;
		/// the minimum frequency possible, in seconds
		[Tooltip("the minimum frequency possible, in seconds")]
		public float MinFrequency = 1f;
		/// the maximum frequency possible, in seconds
		[Tooltip("the maximum frequency possible, in seconds")]
		public float MaxFrequency = 1f;

		[Header("Debug")]
		[MMInspectorButton("ToggleSpawn")]
		/// a test button to spawn an object
		public bool CanSpawnButton;

		protected float _lastSpawnTimestamp = 0f;
		protected float _nextFrequency = 0f;

		protected float _elapsedTime = 0f;
		protected float _timeFlag1 = 3f;
		protected bool _timeFlag1Enabled = false;
		protected float _timeFlag2 = 30f;
		protected bool _timeFlag2Enabled = false;
		protected float _timeFlag3 = 60f;
        protected bool _timeFlag3Enabled = false;
        //[SerializeField] private float spawnRadius = 3f;
        [SerializeField] private int spawnBatchSize = 1;


        /// <summary>
        /// On Start we initialize our spawner
        /// </summary>
        protected virtual void Start()
		{
			Initialization ();
		}

		/// <summary>
		/// Grabs the associated object pooler if there's one, and initalizes the frequency
		/// </summary>
		protected virtual void Initialization()
		{
			if (GetComponent<MMMultipleObjectPooler>() != null)
			{
				ObjectPooler = GetComponent<MMMultipleObjectPooler>();
			}
			if (GetComponent<MMSimpleObjectPooler>() != null)
			{
				ObjectPooler = GetComponent<MMSimpleObjectPooler>();
			}
			if (ObjectPooler == null)
			{
				Debug.LogWarning(this.name+" : no object pooler (simple or multiple) is attached to this Projectile Weapon, it won't be able to shoot anything.");
				return;
			}
			DetermineNextFrequency ();
		}

		/// <summary>
		/// Every frame we check whether or not we should spawn something
		/// </summary>
		protected virtual void Update()
		{	
			if (!_timeFlag3Enabled) 
					_elapsedTime += Time.deltaTime;

            // NEW: timed enabling of pools
            if (_elapsedTime > _timeFlag1 && !_timeFlag1Enabled)
            {
                if (ObjectPooler is MMMultipleObjectPooler multiPooler)
                {
                    multiPooler.EnableObjects("Enemy_Grunt", true);
					_timeFlag1Enabled=true;
                }
            }
            if (_elapsedTime > _timeFlag2 && !_timeFlag2Enabled)
            {
                if (ObjectPooler is MMMultipleObjectPooler multiPooler)
                {
                    multiPooler.EnableObjects("Enemy_Soldier", true);
					_timeFlag2Enabled=true;
					LowerFrecuency();
                }
            }


            if (_elapsedTime > _timeFlag3 && !_timeFlag3Enabled)
            {
                if (ObjectPooler is MMMultipleObjectPooler multiPooler)
                {
                    multiPooler.EnableObjects("Enemy_Overwatch", true);
					_timeFlag3Enabled=true;
					LowerFrecuency();
                }
            }

            if ((Time.time - _lastSpawnTimestamp > _nextFrequency)  && CanSpawn)
			{
				Spawn ();
			}


		}

		/// <summary>
		/// Spawns an object out of the pool if there's one available.
		/// If it's an object with Health, revives it too.
		/// </summary>
		protected virtual void Spawn()
		{
            for (int i = 0; i < spawnBatchSize; i++)
            {
                GameObject nextGameObject = ObjectPooler.GetPooledGameObject();

                if (nextGameObject == null) { continue; }
                if (nextGameObject.GetComponent<MMPoolableObject>() == null)
                {
                    throw new Exception(gameObject.name + " is trying to spawn objects that don't have a PoolableObject component.");
                }

                nextGameObject.gameObject.SetActive(true);
                nextGameObject.gameObject.MMGetComponentNoAlloc<MMPoolableObject>().TriggerOnSpawnComplete();

                Health objectHealth = nextGameObject.gameObject.MMGetComponentNoAlloc<Health>();
                if (objectHealth != null)
                {
                    objectHealth.Revive();
                }

                //Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spawnRadius;
                //Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

                //nextGameObject.transform.position = spawnPosition;
                nextGameObject.transform.position = this.transform.position;
            }

            _lastSpawnTimestamp = Time.time;
            DetermineNextFrequency();
        }

        /// <summary>
        /// Determines the next frequency by randomizing a value between the two specified in the inspector.
        /// </summary>
        protected virtual void DetermineNextFrequency()
		{
			_nextFrequency = UnityEngine.Random.Range (MinFrequency, MaxFrequency);
		}

		/// <summary>
		/// Toggles spawn on and off
		/// </summary>
		public virtual void ToggleSpawn()
		{
			CanSpawn = !CanSpawn;
		}

		/// <summary>
		/// Turns spawning off
		/// </summary>
		public virtual void TurnSpawnOff()
		{
			CanSpawn = false;
		}

		/// <summary>
		/// Turns spawning on
		/// </summary>
		public virtual void TurnSpawnOn()
		{
			CanSpawn = true;
		}
		public virtual void LowerFrecuency()
		{
			MinFrequency = MinFrequency / 1.3f;
			MaxFrequency = MaxFrequency / 1.3f;
			//spawnBatchSize++;
		}

    }
}