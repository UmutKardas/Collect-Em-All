using System.Collections.Generic;
using System.Linq;
using Content;
using Resources;
using UnityEngine;

namespace Manager
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private ColorResources colorResources;
        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private int gridSize = 5;

        private Dictionary<Vector2, Node> _grids = new();
        private List<Vector2> _occupiedGrids = new();

        public static ColorType TargetColorType { get; set; }

        private void Awake()
        {
            CreateGrids();
        }

        private void OnEnable()
        {
            InputHelper.OnNodeSelect += AddToOccupiedGrids;
            InputHelper.OnNodeSelectedEnd += OccupiedFinish;
        }

        private void OnDisable()
        {
            InputHelper.OnNodeSelect -= AddToOccupiedGrids;
            InputHelper.OnNodeSelectedEnd -= OccupiedFinish;
        }

        private void CreateGrids()
        {
            var offSet = (gridSize - 1) / 2;
            for (var positionX = -offSet; positionX <= offSet; positionX++)
            {
                for (var positionY = -offSet; positionY <= offSet; positionY++) { SetupNode(positionX, positionY); }
            }
        }

        private void SetupNode(int positionX, int positionY)
        {
            var node = Instantiate(gridPrefab).GetComponent<Node>();
            var gridPosition = new Vector2(positionX, positionY);
            node.Initialize(colorResources.GetRandomColor(), gridPosition);
            _grids.Add(gridPosition, node);
        }

        private void AddToOccupiedGrids(Vector2 position)
        {
            if (_occupiedGrids.Contains(position)) { return; }

            var lastNode = _occupiedGrids.Count > 0 ? _occupiedGrids.Last() : position;
            if (IsHandleNode(lastNode, position)) { OccupiedSuccess(position); }
            else { OccupiedFail(); }
        }

        private void OccupiedSuccess(Vector2 position)
        {
            _grids[_occupiedGrids.Count > 0 ? _occupiedGrids.Last() : position].AddLineRendererPosition(position);
            _occupiedGrids.Add(position);
        }

        private void OccupiedFail()
        {
            OccupiedFinish();
        }

        private void OccupiedFinish()
        {
            ClearAllLineRenderers();
            _occupiedGrids.Clear();
        }

        private void ClearAllLineRenderers()
        {
            _occupiedGrids.ForEach(x => _grids[x].ClearLineRenderer());
        }

        private bool IsHandleNode(Vector2 handleVector, Vector2 targetVector)
        {
            return _grids[targetVector].ColorType.Equals(TargetColorType) &&
                IsNodeBetweenRange(handleVector, targetVector);
        }

        private bool IsNodeBetweenRange(Vector2 handleVector, Vector2 targetVector)
        {
            for (var positionX = -1; positionX <= 1; positionX++)
            {
                for (var positionY = -1; positionY <= 1; positionY++)
                {
                    var neighbor = handleVector + new Vector2(positionX, positionY);
                    if (neighbor == targetVector && _grids.ContainsKey(neighbor))
                        return true;
                }
            }

            return false;
        }
    }
}
