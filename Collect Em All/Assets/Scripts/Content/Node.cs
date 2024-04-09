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

        public void Initialize(ColorData colorData, Vector2 positionId)
        {
            ColorType = colorData.ColorType;
            SetColor(NODE, colorData.Color);
            PositionID = positionId;
            transform.position = PositionID;
            SetLinePosition(NODE, PositionID, 0);
        }

        public void AddLineRendererPosition(Vector2 position) => AddLinePosition(NODE, position);

        public void ClearLineRenderer() => ClearLineRenderer(NODE);
    }
}
