using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tiles
{
    public class TileContact : MonoBehaviour, ITileContact
    {
        [SerializeField] private new Collider2D collider;

        public bool Enabled
        {
            get => collider.enabled;
            set => collider.enabled = value;
        }

        public event System.Action OnSelect;
        public event System.Action OnUnselect;
        public event System.Action OnTap;
    }
}
