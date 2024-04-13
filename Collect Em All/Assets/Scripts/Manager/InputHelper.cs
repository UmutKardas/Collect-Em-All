using System;
using Resources;
using Structure;
using UnityEngine;
using Constants;

namespace Manager
{
    public class InputHelper : MonoBehaviour
    {
        public static event Action OnNodeSelectedStart, OnNodeSelectedEnd;
        public static event Action<Vector2> OnNodeSelect;

        private Camera _mainCamera;

        private void Awake()
        {
            SetComponentValues();
        }

        private void Update()
        {
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetTargetColor(GetSelectionNode()?.ColorType ?? ColorType.None);
                OnNodeSelectedStart?.Invoke();
            }

            if (Input.GetMouseButton(0)) { SelectNode(); }

            if (Input.GetMouseButtonUp(0))
            {
                SetTargetColor(ColorType.None);
                OnNodeSelectedEnd?.Invoke();
            }
        }

        private void SelectNode()
        {
            var selectionNode = GetSelectionNode();
            if (selectionNode != null) { OnNodeSelect?.Invoke(selectionNode.PositionID); }
        }

        private void SetTargetColor(ColorType colorType) => GridManager.Instance.TargetColorType = colorType;


        private void SetComponentValues()
        {
            _mainCamera = Camera.main;
        }

        private INode GetSelectionNode()
        {
            var hit = Physics2D.Raycast(GetMousePosition(),
                Vector2.zero,
                0.1f,
                LayerMask.GetMask(GameConstants.NODE_LAYER));
            return hit.collider?.GetComponent<INode>();
        }

        private Vector2 GetMousePosition()
        {
            return new Vector2(_mainCamera.ScreenToWorldPoint(Input.mousePosition).x,
                _mainCamera.ScreenToWorldPoint(Input.mousePosition).y);
        }
    }
}
