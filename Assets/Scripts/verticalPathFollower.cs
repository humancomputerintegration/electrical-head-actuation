using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class verticalPathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float errorTolerance;
        public float speedValue = 5;
        private float speed;

        [SerializeField] PIDManager PIDManager;
        [SerializeField] pidNeck controlledObject;
        [SerializeField] Transform Up;
        [SerializeField] Transform Down;
        [SerializeField] Transform Camera;


        private float distanceTravelled;
        private float counter = 0f;
        private float offset;

        void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                speed = speedValue;
            }

            offset = Camera.eulerAngles.x;
        }

        void Update()
        {
            var errorQuaternion = controlledObject.DesiredOrientation * Quaternion.Inverse(Quaternion.identity);

            Vector2 error = new Vector2
            {
                x = Map360To180(errorQuaternion.eulerAngles.x),
                y = Map360To180(errorQuaternion.eulerAngles.y)
            };

            Vector2 output = new Vector2
            {
                x = Map360To180(Camera.eulerAngles.x - offset),
                y = Map360To180(Camera.eulerAngles.y)
            };

            float totalError = Mathf.Sqrt(Mathf.Pow((error.x - output.x), 2f));

            //the following two if paragraphs are for preventing error accumulation.
            if (totalError <= errorTolerance)
            {
                if ((transform.position - Up.position).magnitude <= 0.14f || (transform.position - Down.position).magnitude <= 0.12f)
                {
                    counter += Time.deltaTime;
                    if (counter < 0.5f)
                    {
                        speedValue = 0f;
                    }
                    else
                    {
                        speedValue = 5f;
                    }
                }
                else
                {
                    counter = 0;
                }

                speed = speedValue;
            }

            if (totalError >= errorTolerance)
            {
                speed = 0;
            }

            if (pathCreator != null && PIDManager.verticalTrigger == true)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        float Map360To180(float degree)
        {
            if (degree > 180)
            {
                return degree - 360;
            }

            return degree;
        }
    }
}