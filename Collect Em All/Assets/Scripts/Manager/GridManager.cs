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

        private Dictionary<Vector2, Node> _grids = new();
        private List<Vector2> _occupiedGrids = new();

        private const int THRESHOLD_VALUE = 2;

        public ColorType TargetColorType { get; set; }
        public ColorResources ColorResources => colorResources;

        private void Awake()
        {
            Instance = this;
            CreateGrids();
        }

        private void OnEnable()
        {
            InputHelper.OnNodeSelect += AddPositionToOccupiedGrids;
            InputHelper.OnNodeSelectedEnd += OccupiedFinish;
        }

        private void OnDisable()
        {
            InputHelper.OnNodeSelect -= AddPositionToOccupiedGrids;
            InputHelper.OnNodeSelectedEnd -= OccupiedFinish;
        }

        private void CreateGrids()
        {
            var offSet = (gridSize - 1) / 2;
            for (var positionX = -offSet; positionX <= offSet; positionX++)
            {
                for (var positionY = -offSet; positionY <= offSet; positionY++)
                {
                    SetupNode(positionX, positionY);
                }
            }
        }

        private void SetupNode(int positionX, int positionY)
        {
            var node = Instantiate(gridPrefab).GetComponent<Node>();
            var gridPosition = new Vector2(positionX, positionY);
            node.Initialize(gridPosition);
            _grids.Add(gridPosition, node);
        }

        private void AddPositionToOccupiedGrids(Vector2 position)
        {
            if (_occupiedGrids.Contains(position))
            {
                return;
            }

            var lastNode = _occupiedGrids.Count > 0 ? _occupiedGrids.Last() : position;
            if (IsHandleNode(lastNode, position)) { AddOccupiedSuccess(position); }
            else { AddOccupiedFailure(); }
        }

        private void AddOccupiedSuccess(Vector2 position)
        {
            _grids[_occupiedGrids.Count > 0 ? _occupiedGrids.Last() : position].AddLineRendererPosition(position);
            _grids[position].IsEmpty = true;
            _occupiedGrids.Add(position);
        }

        private void AddOccupiedFailure()
        {
            OccupiedFinish();
        }

        private void OccupiedFinish()
        {
            if (IsCountThresholdExceeded()) { FillDownFromPoint(); }

            ResetNode();
            _occupiedGrids.Clear();
        }

        private void FillDownFromPoint()
        {
            foreach (var columnsValue in _occupiedGrids.Select(x => x.x).Distinct().ToList())
            {
                for (var i = 0; i < GetColumnsListOrderBy(columnsValue).Count; i++)
                {
                    var columnsListOrderBy = GetColumnsListOrderBy(columnsValue);
                    var currentPair = columnsListOrderBy.FirstOrDefault(x => x.Value.IsEmpty);
                    if (!columnsListOrderBy.Any(pair => pair.Key.y >= currentPair.Key.y && !pair.Value.IsEmpty))
                    {
                        currentPair.Value.RespawnNode(currentPair.Key);
                        continue;
                    }

                    var upperGrid =
                        columnsListOrderBy.FirstOrDefault(x => !x.Value.IsEmpty && x.Key.y >= currentPair.Key.y);
                    if (upperGrid.Value is null) { continue; }

                    upperGrid.Value.FillDownNode(currentPair.Value);
                    _grids[currentPair.Key] = upperGrid.Value;
                    _grids[upperGrid.Key] = currentPair.Value;
                    _grids[upperGrid.Key].RespawnNode(upperGrid.Key);
                }
            }
        }

        private Dictionary<Vector2, Node> GetColumnsListOrderBy(float columnsValue)
        {
            return new Dictionary<Vector2, Node>(_grids.Where(x => x.Key.x.Equals(columnsValue)).OrderBy(x => x.Key.y)
                .ToList());
        }


        private void ResetNode()
        {
            _grids.Values.ToList().ForEach(x =>
            {
                x.IsEmpty = false;
                x.ClearLineRenderer();
            });
        }


        private bool IsHandleNode(Vector2 handleVector, Vector2 targetVector)
        {
            return _grids[targetVector].ColorType.Equals(TargetColorType) &&
                IsNodeBetweenRange(handleVector, targetVector);
        }


        private bool IsCountThresholdExceeded() => _occupiedGrids.Count > THRESHOLD_VALUE;


        private bool IsNodeBetweenRange(Vector2 handleVector, Vector2 targetVector)
        {
            for (var positionX = -1; positionX <= 1; positionX++)
            {
                for (var positionY = -1; positionY <= 1; positionY++)
                {
                    var neighbor = handleVector + new Vector2(positionX, positionY);
                    if (neighbor == targetVector && _grids.ContainsKey(neighbor))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
