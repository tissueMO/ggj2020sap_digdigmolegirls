using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kingusp
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class MoguraAnimationController : MonoBehaviour
    {
        private Animator animator = default;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("isDown", true);
        }

        /// <summary>
        /// 向きたい方向を与えると、スプライトがその方向の画像に変化するメソッド。
        /// </summary>
        /// <param name="direction"></param>
        public void UpdateDirection(Vector2 direction)
        {
            bool isDown, isUp, isLeft, isRight;

            direction = Vector3.Normalize(direction);

            isDown = direction.y <= -0.3f;
            isUp = direction.y >= 0.3f;
            isLeft = direction.x <= -0.3f;
            isRight = direction.x >= 0.3f;

            animator.SetBool("isUp", isUp);
            animator.SetBool("isDown", isDown);
            animator.SetBool("isLeft", isLeft);
            animator.SetBool("isRight", isRight);
        }
    }
}