using Resources;
using Structure;
using UnityEngine;

namespace Content
{
    public class Node : MonoBehaviour, INode
    {
        public Vector2 PositionID { get; set; }
        public ColorType ColorType { get; set; }
        public bool IsEmpty { get; set; }

        [SerializeField] public SpriteRenderer NodeSpriteRenderer;


        public void Initialize(ColorData colorData, Vector2 positionId)
        {
            ColorType = colorData.ColorType;
            PositionID = positionId;
            NodeSpriteRenderer.color = colorData.Color;
            transform.position = new Vector2(positionId.x, positionId.y);
        }
    }
}
