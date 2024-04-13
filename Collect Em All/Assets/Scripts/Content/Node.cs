using Manager;
using Resources;
using Structure;
using UnityEngine;

namespace Content
{
    public class Node : Structure.Content, INode
    {
        public Vector2 PositionID { get; set; }
        public ColorType ColorType { get; set; }
        public bool IsEmpty { get; set; }

        private const string NODE = "Node";
        private const float ANIMATION_DURATION = 0.2f;

        public void Initialize(Vector2 positionId)
        {
            var colorData = GridManager.Instance.ColorResources.GetRandomColor();
            ColorType = colorData.ColorType;
            SetColor(NODE, colorData.Color);
            PositionID = positionId;
            transform.position = PositionID;
            IsEmpty = false;
            SetLinePosition(NODE, PositionID, 0);
        }

        public void FillDownNode(INode node)
        {
            PlayMoveAnimationY(node.PositionID.y, 0, ANIMATION_DURATION, null, null);
            IsEmpty = false;
            PositionID = node.PositionID;
            SetLinePosition(NODE, PositionID, 0);
        }

        public void RespawnNode(Vector2 position)
        {
            Initialize(position);
            IsEmpty = true;
            gameObject.SetActive(false);
            PlayPunchScaleAnimation(ANIMATION_DURATION, ANIMATION_DURATION, () => gameObject.SetActive(true), null);
        }

        public void AddLineRendererPosition(Vector2 position) => AddLinePosition(NODE, position);

        public void ClearLineRenderer() => ClearLineRenderer(NODE);

        [ContextMenu("Node Info")] public void Info() =>
            Debug.Log($"Position ID: {PositionID}, Color Type: {ColorType}, Is Empty: {IsEmpty}");
    }
}
