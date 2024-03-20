using System;
using Resources;
using Structure;
using UnityEngine;

namespace Content
{
    public class Node : MonoBehaviour, INode
    {
        public Vector2Int PositionID { get; set; }
        public ColorType ColorType { get; set; }
        public bool IsEmpty { get; set; }

        [SerializeField] public SpriteRenderer NodeSpriteRenderer;

        private readonly Vector3 GRID_SCALE = Vector3.one * 0.9f;


        public void Initialize(ColorData colorData, Vector2Int positionId)
        {
            ColorType = colorData.ColorType;
            PositionID = positionId;
            NodeSpriteRenderer.color = colorData.Color;
            transform.position = new Vector2(positionId.x, positionId.y);
            transform.localScale = GRID_SCALE;
        }
    }
}
