using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NowhereUnity.Movement {

    public enum MotorAmountSpace {
        Absolute,
        Relative
    }

    public interface IMotorMoving {
        void Move(Vector3 velocity, Space directionSpace, MotorAmountSpace speedSpace);
    }

    public interface IMotorLooking {
        void SetLookDirection(Vector3 direction, float factor, Space directionSpace);
    }

    public interface IMotorJumping {
        void Jump(float power, MotorAmountSpace powerSpace);
    }

    public interface IStrafeHandler {
        void Strafe(Vector3 velocity);
    }

    public interface ILockDirectionHandler {

        Vector3 motorLookDirection { get; set; }
        bool motorAutomaticLook { get; set; }
    }
}