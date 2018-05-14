using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NowhereUnity.Movement{

    public enum PlayerKeyEvent {
        Press,
        Pressed,
        Release
    }

	public interface IPlayerInputMoving {
        void MovePlayer(Vector3 input);
    }

    public interface IPlayerInputAiming {
        void AimPlayer(Vector3 aim);
    }

    public interface IPlayerInputJumping {
        void JumpPlayer(PlayerKeyEvent a);
    }


    // Callback plan
    public static class PlayerInputDelegates {

        public delegate void OnMove(Vector3 move);
        public delegate void OnAiming(Vector3 aim);
        public delegate void OnJumpStart();
        public delegate void OnJumpHovering();
        public delegate void OnJumpQuit();

        public delegate void OnSprinting();
    }
}