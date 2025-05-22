using DG.Tweening;
using UnityEngine;

namespace OthelloGameProj
{
    /// <summary>
    /// オセロの石の動き制御
    /// </summary>
    public class StoneMovement : MonoBehaviour
    {
        [SerializeField, Header("上昇する高さ")]
        public float height = 0.1f;
        [SerializeField, Header("動く時間")]
        public float moveDuration = 1.0f;
        [SerializeField, Header("動く時間")]
        public float rotDuration = 1.0f;

        Sequence sequence;
        float stoneRotY = 0;

        private void Start()
        {
            stoneRotY = transform.position.y;
        }

        /// <summary>
        /// 石をひっくり返す(コールバック処理あり)
        /// </summary>
        /// <param name="startAction">開始時のコールバック処理</param>
        /// <param name="completeAction">完了時のコールバック処理</param>
        /// <param name="killAction">処理削除時のコールバック処理</param>
        public void Movement(TweenCallback startAction, TweenCallback completeAction, TweenCallback killAction)
        {
            sequence = DOTween.Sequence()
                .Append(transform.DOMoveY(height, moveDuration))
                .Append(transform.DORotate(transform.rotation.eulerAngles + Vector3.right * 180f, rotDuration, RotateMode.FastBeyond360))
                .Append(transform.DOMoveY(stoneRotY, moveDuration))
                .SetLink(gameObject);
            sequence.Play()
                .OnStart(startAction)
                .OnComplete(completeAction)
                .OnKill(killAction);
        }

        public void Reset()
        {
            sequence.Kill();
        }

        private void OnDisable()
        {
            sequence?.Kill();
        }
    }
}
