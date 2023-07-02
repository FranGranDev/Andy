using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.Tiles.View
{
    public class TileView : MonoBehaviour, ITileView
    {
        [Header("Settings")]
        [SerializeField, Min(1)] private int layerMultiplier;
        [Header("Components")]
        [SerializeField] private SpriteRenderer iconRenderer;

        private List<View> views;

        private bool selected;


        public void Setup(Sprite sprite, Color color)
        {
            iconRenderer.sprite = sprite;
            iconRenderer.color = color;

            views = GetComponentsInChildren<SpriteRenderer>()
                .Select(x => new View(x))
                .ToList();
        }

        public bool Selected
        {
            get => selected;
            set
            {
                if (selected == value)
                    return;
            }
        }
        public int Layer
        {
            set
            {
                foreach(View view in views)
                {
                    view.Layer = value * layerMultiplier;
                }
            }
        }
        public bool Hidden
        {
            set
            {
                foreach (View view in views)
                {
                    view.Hidden = value;
                }
            }
        }


        private class View
        {
            public View(SpriteRenderer spriteRenderer)
            {
                this.spriteRenderer = spriteRenderer;

                startLayer = spriteRenderer.sortingOrder;
                startColor = spriteRenderer.color;
            }

            private SpriteRenderer spriteRenderer;
            private int startLayer;
            private Color startColor;

            public int Layer
            {
                set
                {
                    spriteRenderer.sortingOrder = startLayer + value;
                }
            }
            public bool Hidden
            {
                set
                {
                    spriteRenderer.color = Color.Lerp(startColor, Color.black, value ? 0.5f : 0);
                }
            }
        }
    }
}