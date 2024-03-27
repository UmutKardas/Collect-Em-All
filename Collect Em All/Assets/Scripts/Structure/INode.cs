using Resources;
using UnityEngine;

namespace Structure
{
    public interface INode
    {
        public bool IsEmpty { get; set; }
        public Vector2 PositionID { get; set; }
        public ColorType ColorType { get; set; }
    }
}
