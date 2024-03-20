using Resources;
using Structure;
using UnityEngine;

namespace Manager
{
    public class InputHelper : MonoBehaviour
    {
        private Camera _mainCamera;
        private const string NODE_LAYER = "Node";

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
                SetTargetColor(GetSelectionNode() is not null ? GetSelectionNode().ColorType : ColorType.None);
            }

            if (Input.GetMouseButton(0))
            {
                SelectNode();
            }

            if (Input.GetMouseButtonUp(0))
            {
                SetTargetColor(ColorType.None);
            }
        }

        private void SelectNode()
        {
            if (GetSelectionNode() is null) { return; }

            GridManager.Instance.AddToOccupiedGrids(GetSelectionNode().PositionID);
        }

        private void SetTargetColor(ColorType colorType)
        {
            GridManager.Instance.TargetColorType = colorType;
        }

        private void SetComponentValues()
        {
            _mainCamera = Camera.main;
        }

        private INode GetSelectionNode()
        {
            var hit = Physics2D.Raycast(GetMousePosition(), Vector2.zero, 0.1f, LayerMask.GetMask(NODE_LAYER));
            return hit.collider?.GetComponent<INode>();
        }

        private Vector2 GetMousePosition()
        {
            return new Vector2(_mainCamera.ScreenToWorldPoint(Input.mousePosition).x,
                _mainCamera.ScreenToWorldPoint(Input.mousePosition).y);
        }
    }
}
