using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Structure
{
    public abstract class Content : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private GameObject[] gameObjects;
        [HideInInspector] [SerializeField] private SpriteRenderer[] spriteRenderers;
        [HideInInspector] [SerializeField] private LineRenderer[] lineRenderers;

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObjects = GetParentAllChildren(gameObject).ToArray();
            var newSpriteRenderer = new List<SpriteRenderer>();
            var newLinerenderer = new List<LineRenderer>();
            foreach (var obj in gameObjects)
            {
                if (obj.TryGetComponent(out SpriteRenderer spriteRenderer)) { newSpriteRenderer.Add(spriteRenderer); }

                if (obj.TryGetComponent(out LineRenderer lineRenderer)) { newLinerenderer.Add(lineRenderer); }
            }

            spriteRenderers = newSpriteRenderer.ToArray();
            lineRenderers = newLinerenderer.ToArray();
        }

        private List<GameObject> GetParentAllChildren(GameObject parent)
        {
            var childrenList = new List<GameObject>();
            foreach (Transform child in parent.transform)
            {
                var childObject = child.gameObject;
                var flags = GameObjectUtility.GetStaticEditorFlags(childObject);
                if (flags.HasFlag(StaticEditorFlags.BatchingStatic)) { continue; }

                childrenList.Add(childObject);
                if (child.transform.childCount > 0) { childrenList.AddRange(GetParentAllChildren(childObject)); }
            }

            return childrenList;
        }
#endif

        protected void SetColor(string targetName, Color color)
        {
            var targetSpriteRenderer = GetSpriteRenderer(targetName);
            if (targetSpriteRenderer is null) { return; }

            targetSpriteRenderer.color = color;
        }

        protected void SetLinePosition(string targetName, Vector3 position, int index)
        {
            var targetLineRenderer = GetLineRenderer(targetName);
            if (targetLineRenderer is null) { return; }

            targetLineRenderer.SetPosition(index, position);
        }

        protected void AddLinePosition(string targetName, Vector3 position)
        {
            var targetLineRenderer = GetLineRenderer(targetName);
            if (targetLineRenderer is null) { return; }

            if (GetLineRendererPositions(targetName).Contains(position)) { return; }

            targetLineRenderer.positionCount++;
            targetLineRenderer.SetPosition(targetLineRenderer.positionCount - 1, position);
        }

        protected void ClearLineRenderer(string targetName)
        {
            var targetLineRenderer = GetLineRenderer(targetName);
            if (targetLineRenderer is null) { return; }

            targetLineRenderer.positionCount = 1;
        }

        protected void PlayPunchScaleAnimation(float delayBeforeAnimation, float animationDuration,
            TweenCallback beforeAnimationCallback, TweenCallback afterAnimationCallback)
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(delayBeforeAnimation);
            sequence.AppendCallback(beforeAnimationCallback);
            sequence.Append(transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), animationDuration, 2, 1f))
                .OnComplete(afterAnimationCallback);
        }

        protected void PlayMoveAnimationY(float targetYPosition, float delayBeforeAnimation, float animationDuration,
            TweenCallback beforeAnimationCallback, TweenCallback afterAnimationCallback)
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(delayBeforeAnimation);
            sequence.AppendCallback(beforeAnimationCallback);
            sequence.Append(transform.DOMoveY(targetYPosition, animationDuration)).OnComplete(afterAnimationCallback);
        }


        private IEnumerable<Vector2> GetLineRendererPositions(string targetName)
        {
            var lineRenderer = GetLineRenderer(targetName);
            if (lineRenderer is null) { yield break; }

            for (var i = 1; i < lineRenderer.positionCount; i++) { yield return lineRenderer.GetPosition(i); }
        }

        private SpriteRenderer GetSpriteRenderer(string targetName)
        {
            return spriteRenderers.FirstOrDefault(x => x.name.Equals(targetName));
        }

        private LineRenderer GetLineRenderer(string targetName)
        {
            return lineRenderers.FirstOrDefault(x => x.name.Equals(targetName));
        }
    }
}
