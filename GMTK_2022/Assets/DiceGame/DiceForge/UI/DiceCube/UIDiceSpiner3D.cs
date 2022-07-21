using UnityEngine;

namespace DiceGame
{
    public class UIDiceSpiner3D : MonoBehaviour
    {
        [SerializeField] float animationDuration = 1;
        private float animationTime;

        private Transform dice;

        private RotationRequest rotationRequest;

        bool upSignal = false;
        public void ViewUp() => upSignal = true;
        private bool UpSignal()
        {
            if (upSignal)
            {
                upSignal = false;
                return true;
            }
            return Input.GetKey(KeyCode.UpArrow);
        }

        bool downSignal = false;
        public void ViewDown() => downSignal = true;
        private bool DownSignal()
        {
            if (downSignal)
            {
                downSignal = false;
                return true;
            }
            return Input.GetKey(KeyCode.DownArrow);
        }

        bool rightSignal = false;
        public void ViewRight() => rightSignal = true;
        private bool RightSignal()
        {
            if (rightSignal)
            {
                rightSignal = false;
                return true;
            }
            return Input.GetKey(KeyCode.RightArrow);
        }

        bool leftSignal = false;
        public void ViewLeft() => leftSignal = true;
        private bool LeftSignal()
        {
            if (leftSignal)
            {
                leftSignal = false;
                return true;
            }
            return Input.GetKey(KeyCode.LeftArrow);
        }

        private void Start()
        {
            dice = transform;
        }

        void Update()
        {
            if (RotationRequestInProgress())
            {
                AnimateRotationDice();
            }
            else
            {
                rotationRequest = GetRotationRequest();
                animationTime = 0;
            }
        }

        private bool RotationRequestInProgress()
        {
            return rotationRequest != null;
        }

        private void AnimateRotationDice()
        {
            animationTime += Time.deltaTime;
            LimitAnimationTime();

            var p = animationTime / animationDuration;
            var t = EasingFunction.EaseInOutSine(0, 1, p);

            dice.rotation = Quaternion.Lerp(rotationRequest.InitialRotation, rotationRequest.TargetRotation, t);

            CheckAnimationEnded();
        }

        private void LimitAnimationTime()
        {
            if (animationTime > animationDuration)
            {
                animationTime = animationDuration;
            }
        }

        private void CheckAnimationEnded()
        {
            if (animationTime == animationDuration)
            {
                rotationRequest = null;
            }
        }

        private RotationRequest GetRotationRequest()
        {
            bool leftKeyDown = LeftSignal();
            bool rightKeyDown = RightSignal();
            if (leftKeyDown || rightKeyDown)
            {
                var rotation = rightKeyDown ? 90 : -90;
                var initialRotation = dice.rotation;
                dice.Rotate(new Vector3(0, rotation, 0), Space.World);
                var targetRotation = dice.rotation;
                dice.rotation = initialRotation;

                return new RotationRequest()
                {
                    InitialRotation = initialRotation,
                    TargetRotation = targetRotation,
                };
            }

            var downKeyDown = DownSignal();
            var upKeyDown = UpSignal();
            if (downKeyDown || upKeyDown)
            {
                var rotation = downKeyDown ? 90 : -90;
                var initialRotation = dice.rotation;
                dice.Rotate(new Vector3(rotation, 0, 0), Space.World);
                var targetRotation = dice.rotation;
                dice.rotation = initialRotation;

                return new RotationRequest()
                {
                    InitialRotation = initialRotation,
                    TargetRotation = targetRotation,
                };
            }

            return null;
        }
    }

    public class RotationRequest
    {
        public Quaternion InitialRotation { get; internal set; }
        public Quaternion TargetRotation { get; internal set; }
    }
}
