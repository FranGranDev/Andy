using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tiles
{
    public interface ITileContact
    {
        public bool Enabled { get; set; }


        public event System.Action OnSelect;
        public event System.Action OnUnselect;
        public event System.Action OnTap;
    }
}
