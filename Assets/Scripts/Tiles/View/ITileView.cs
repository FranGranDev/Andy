using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tiles
{
    public interface ITileView
    {
        void Setup(Sprite sprite, Color color);

        bool Selected { get; set; }
        bool Hidden { set; }
        int Layer { set; }
    }
}