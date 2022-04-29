using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class sinePathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float errorTolerance = 20f;
        public float speedValue = 5;
        private float speed;
        private int endTimer;

        [SerializeField] PIDManager PIDManager;
        [SerializeField] pidNeck controlledObject;
        [SerializeField] Transform finalDestination;


        private float distanceTravelled;
        private float counter;

        void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                speed = speedValue;
            }
        }

        void Update()
        {
            if (pathCreator != null && PIDManager.sineTrigger == true)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
            //the following two if paragraphs are for preventing error accumulation.
            if (controlledObject.totalError >= errorTolerance)
            {
                speed = 0;
            }

            if (speed == 0 && controlledObject.totalError <= errorTolerance)
            {
                speed = speedValue;
            }

            //if (PIDManager.sineTrigger == true && endTimer >= 600)
            //{
            //    UnityEditor.EditorApplication.isPlaying = false;
            //}

            endTimer += 1;
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}