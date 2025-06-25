using System;
using UnityEngine;

namespace script
{
    public enum AntAnimationType
    {
        Idle,
        Walking,
        Die
    }
    public class AntAnimatorController : MonoBehaviour
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int Die = Animator.StringToHash("Die");
        [SerializeField] private Animator animator;

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAnimation(AntAnimationType antAnimationType)
        {
            switch (antAnimationType)
            {
                case AntAnimationType.Idle:
                    animator?.SetBool(IsWalking, false);
                    break;
                case AntAnimationType.Walking:
                    animator?.SetBool(IsWalking, true);
                    break;
                case AntAnimationType.Die:
                    animator?.SetTrigger(Die);
                    break;
            }
        }
    }
}