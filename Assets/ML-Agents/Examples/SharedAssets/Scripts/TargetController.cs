using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Unity.MLAgentsExamples
{
    /// <summary>
    /// Utility class to allow target placement and collision detection with an agent
    /// Add this script to the target you want the agent to touch.
    /// Callbacks will be triggered any time the target is touched with a collider tagged as 'tagToDetect'
    /// </summary>
    public class TargetController : MonoBehaviour
    {

        [Header("Collider Tag To Detect")]
        public string tagToDetect = "agent";

        [Header("Target Placement")]
        public float spawnRadius;
        public bool respawnIfTouched;

        [Header("Next Target")]
        public GameObject nextTarget; // The next target to activate

        [Header("Target Fell Protection")]
        public bool respawnIfFallsOffPlatform = true;
        public float fallDistance = 5;

        private Vector3 m_startingPos;
        private WalkerAgent walkerAgent; // Reference to the WalkerAgent

        [System.Serializable]
        public class TriggerEvent : UnityEvent<Collider> { }

        [Header("Trigger Callbacks")]
        public TriggerEvent onTriggerEnterEvent = new TriggerEvent();
        public TriggerEvent onTriggerStayEvent = new TriggerEvent();
        public TriggerEvent onTriggerExitEvent = new TriggerEvent();

        [System.Serializable]
        public class CollisionEvent : UnityEvent<Collision> { }

        [Header("Collision Callbacks")]
        public CollisionEvent onCollisionEnterEvent = new CollisionEvent();
        public CollisionEvent onCollisionStayEvent = new CollisionEvent();
        public CollisionEvent onCollisionExitEvent = new CollisionEvent();

        void OnEnable()
        {
            m_startingPos = transform.position;
            if (respawnIfTouched)
            {
                MoveTargetToRandomPosition();
            }

            walkerAgent = FindObjectOfType<WalkerAgent>(); // Find the WalkerAgent in the scene
        }

        void Update()
        {
            if (respawnIfFallsOffPlatform)
            {
                if (transform.position.y < m_startingPos.y - fallDistance)
                {
                    Debug.Log($"{transform.name} Fell Off Platform");
                    MoveTargetToRandomPosition();
                }
            }
        }

        public void MoveTargetToRandomPosition()
        {
            var newTargetPos = m_startingPos + (Random.insideUnitSphere * spawnRadius);
            newTargetPos.y = m_startingPos.y;
            transform.position = newTargetPos;
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.transform.CompareTag(tagToDetect))
            {
                onCollisionEnterEvent.Invoke(col);
                if (respawnIfTouched)
                {
                    MoveTargetToRandomPosition();
                }

                if (nextTarget != null)
                {
                    nextTarget.SetActive(true); // Activate the next target
                }

                gameObject.SetActive(false); // Deactivate current target

                if (walkerAgent != null)
                {
                    walkerAgent.UpdateLastActiveTargetIndex(transform);
                }
            }
        }

        private void OnCollisionStay(Collision col)
        {
            if (col.transform.CompareTag(tagToDetect))
            {
                onCollisionStayEvent.Invoke(col);
            }
        }

        private void OnCollisionExit(Collision col)
        {
            if (col.transform.CompareTag(tagToDetect))
            {
                onCollisionExitEvent.Invoke(col);
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(tagToDetect))
            {
                onTriggerEnterEvent.Invoke(col);
            }
        }

        private void OnTriggerStay(Collider col)
        {
            if (col.CompareTag(tagToDetect))
            {
                onTriggerStayEvent.Invoke(col);
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag(tagToDetect))
            {
                onTriggerExitEvent.Invoke(col);
            }
        }
    }
}
