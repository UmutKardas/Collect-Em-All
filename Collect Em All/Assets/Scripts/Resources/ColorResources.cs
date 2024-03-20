using System.Collections.Generic;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(fileName = "ColorResources", menuName = "Scriptable/ColorResources")]
    public class ColorResources : ScriptableObject
    {
        [SerializeField] private List<ColorData> colorDatas;

        public ColorData GetColor(ColorType colorType)
        {
            return colorDatas.Find(x => x.ColorType == colorType);
        }

        public ColorData GetRandomColor()
        {
            return colorDatas[Random.Range(0, colorDatas.Count)];
        }
    }

    public enum ColorType
    {
        None,
        Red,
        Green,
        Blue,
        Yellow,
        Purple,
        Orange
    }

    [System.Serializable]
    public struct ColorData
    {
        public ColorType ColorType;
        public Color Color;
    }
}
