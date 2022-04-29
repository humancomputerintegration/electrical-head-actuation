using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class pidNeckController : MonoBehaviour
{
    #region Read-only & Static Fields
    private readonly PidController[] _internalController;
    #endregion

    #region Constructors
    public pidNeckController(float kp, float ki, float kd)
    {
        if (kp < 0.0f)
            throw new ArgumentOutOfRangeException("kp", "kp must be a non-negative number.");

        if (ki < 0.0f)
            throw new ArgumentOutOfRangeException("ki", "ki must be a non-negative number.");

        if (kd < 0.0f)
            throw new ArgumentOutOfRangeException("kd", "kd must be a non-negative number.");

        this._internalController = new[]
                                   {
                                           new PidController(kp, ki, kd),
                                           new PidController(kp, ki, kd)
                                       };
    }
    #endregion

    #region Instance Properties
    public float Kp
    {
        get
        {
            return this._internalController[0].Kp;
        }
        set
        {
            if (value < 0.0f)
                throw new ArgumentOutOfRangeException("value", "Kp must be a non-negative number.");

            this._internalController[0].Kp = value;
            this._internalController[1].Kp = value;
        }
    }

    public float Ki
    {
        get
        {
            return this._internalController[0].Ki;
        }
        set
        {
            if (value < 0.0f)
                throw new ArgumentOutOfRangeException("value", "Ki must be a non-negative number.");

            this._internalController[0].Ki = value;
            this._internalController[1].Ki = value;
        }
    }

    public float Kd
    {
        get
        {
            return this._internalController[0].Kd;
        }
        set
        {
            if (value < 0.0f)
                throw new ArgumentOutOfRangeException("value", "Kd must be a non-negative number.");

            this._internalController[0].Kd = value;
            this._internalController[1].Kd = value;
        }
    }
    #endregion

    #region Class Methods

    /*public Vector2 ComputePID(Quaternion currentOrientation, Quaternion desiredOrientation, Vector2 previousError, float deltaTime)
    {
        var errorQuaternion = desiredOrientation * Quaternion.Inverse(currentOrientation);
        Vector2 error()
        {
            x = errorQuaternion.eulerAngles.x;
            y = errorQuaternion.eulerAngles.y;

            x = Map360To180(x);
            y = Map360To180(y);
        };

        Vector2 delta()
        {
            x = x - previousError.x;
            y = y - previousError.y;
        };

        PID = ComputeOutput(error, delta, deltaTime);

        previousError = error;
    }*/

    public Vector2 ComputeOutput(Vector2 error, Vector2 delta, float deltaTime)
    {
        var output = new Vector2
        {
            x = this._internalController[0].ComputeOutput(error.x, delta.x, deltaTime),
            y = this._internalController[1].ComputeOutput(error.y, delta.y, deltaTime)
        };

        return output;
    }
    #endregion
}
