using UnityEngine;
using UnityEngine.InputSystem;

namespace OthelloGameProj
{
    /// <summary>
    /// プレイヤーの入力管理まとめ
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 MousePos { get; private set; }
        public bool IsClick { get; private set; }

        GameInput gameInput;

        private void Awake()
        {
            gameInput = new GameInput();
            gameInput.Player.MousePos.started += MousePosAction;
            gameInput.Player.MousePos.performed += MousePosAction;
            gameInput.Player.MousePos.canceled += MousePosAction;
            gameInput.Player.Click.started += ClickAction;
            gameInput.Player.Click.performed += ClickAction;
            gameInput.Player.Click.canceled += ClickAction;

            gameInput.Enable();
        }

        private void ClickAction(InputAction.CallbackContext obj)
        {
            // オプション画面が開いていたら未クリックにして処理を終える
            if (OthelloGameManager.Instance.IsOpenOption)
            {
                IsClick = false;
                return;
            }

            IsClick = obj.ReadValueAsButton();
        }

        private void MousePosAction(InputAction.CallbackContext obj)
        {
            MousePos = obj.ReadValue<Vector2>();
        }

        private void OnDisable()
        {
            gameInput.Player.MousePos.started -= MousePosAction;
            gameInput.Player.MousePos.performed -= MousePosAction;
            gameInput.Player.MousePos.canceled -= MousePosAction;
            gameInput.Player.Click.started -= ClickAction;
            gameInput.Player.Click.performed -= ClickAction;
            gameInput.Player.Click.canceled -= ClickAction;

            gameInput?.Disable();
        }

        private void OnDestroy()
        {
            gameInput?.Dispose();
        }
    }
}
