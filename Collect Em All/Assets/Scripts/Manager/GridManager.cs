using System.Collections.Generic;
using System.Linq;
using Content;
using Resources;
using UnityEngine;

namespace Manager
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;

        [SerializeField] private ColorResources colorResources;
        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private int gridSize = 5;

        private Dictionary<Vector2Int, Node> _grids = new();

        private List<Vector2Int> _occupiedGrids = new();

        public ColorType TargetColorType { get; set; }

        private void Awake()
        {
            SetComponentValues();
            CreateGrids();
        }

        private void CreateGrids()
        {
            for (var x = 0; x < gridSize; x++)
            {
                for (var y = 0; y < gridSize; y++)
                {
                    var node = Instantiate(gridPrefab).GetComponent<Node>();
                    var offSet = (gridSize - 1) / 2;
                    node.Initialize(colorResources.GetRandomColor(), new Vector2Int(x - offSet, y - offSet));
                    _grids.Add(new Vector2Int(x - offSet, y - offSet), node);
                }
            }
        }

        private void SetComponentValues()
        {
            Instance = this;
        }

        public void AddToOccupiedGrids(Vector2Int position)
        {
            if (_occupiedGrids.Contains(position)) { return; }

            var lastNode = _occupiedGrids.Count > 0 ? _occupiedGrids.Last() : position;
            if (IsHandleNode(lastNode, position))
            {
                _occupiedGrids.Add(position);
            }
            else
            {
                Debug.LogError("Invalid Move!");
            }
        }

        private bool IsHandleNode(Vector2Int handleVector, Vector2Int targetVector)
        {
            return _grids[targetVector].ColorType.Equals(TargetColorType) &&
                IsNodeBetweenRange(handleVector, targetVector);
        }

        private bool IsNodeBetweenRange(Vector2Int handleVector, Vector2Int targetVector)
        {
            return Vector2Int.Distance(handleVector, targetVector) <= Mathf.Sqrt(2);
        }
    }
}
