using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using Game.Services;
using DG.Tweening;


namespace Game.Tiles
{
    public class TileStack : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float flyTime;
        [SerializeField] private Ease flyEase;
        [Header("Cells")]
        [SerializeField] private List<Cell> cells;


        private Queue<Tween> activeTweens = new Queue<Tween>();


        public event System.Action OnStackFull;


        public bool Add(Tile tile)
        {
            Cell target = cells
                .FirstOrDefault(x => x.Empty);
            if(target == null)
            {
                return false;
            }

            Cell equal = cells
                .LastOrDefault(x => !x.Empty && x.Type == tile.TileType);

            if(equal != null)
            {
                MoveForOneTile(equal);
                target = cells[cells.IndexOf(equal) + 1];
            }

            SetTile(target, tile);


            return true;
        }
        public void Clear()
        {
            activeTweens.Clear();
            cells
                .Where(x => !x.Empty)
                .ToList()
                .ForEach(x => x.Demolish());
        }

        private void SetTile(Cell target, Tile tile)
        {
            target.Accept(tile);
            tile.Disabled = true;

            tile.transform.SetParent(target.Holder);
            activeTweens.Enqueue(tile.transform.DOLocalMove(Vector3.zero, flyTime)
                .SetEase(flyEase)
                .OnComplete(() =>
                {
                    activeTweens.Dequeue();
                    CheckState();
                }));
        }
        private void MoveForOneTile(Cell from)
        {
            int startIndex = cells.IndexOf(from) + 1;

            for(int i = cells.Count - 1; i > startIndex; i--)
            {
                Tile left = cells[i - 1].Tile;

                if (left == null)
                    continue;

                cells[i - 1].Clear();
                SetTile(cells[i], left);
            }
        }
        private void MoveTiles()
        {
            for(int i = 1; i < cells.Count; i++)
            {
                if (cells[i].Empty)
                    continue;

                int free = -1;
                for(int a = 0; a < i; a++)
                {
                    if(cells[a].Empty)
                    {
                        free = a;
                        break;
                    }
                }
                if(free != -1)
                {
                    Tile tile = cells[i].Tile;
                    cells[i].Clear();
                    SetTile(cells[free], tile);
                }
            }
        }
        private void CheckState()
        {
            if (activeTweens.Count > 0)
                return;

            List<List<Cell>> resultList = new List<List<Cell>>();

            // Группируем ячейки по типу
            var groupedCells = cells
                .Where(x => !x.Empty)
                .GroupBy(c => c.Type);

            foreach (var group in groupedCells)
            {
                // Выбираем только группы, у которых количество ячеек >= 3
                if (group.Count() >= 3)
                {
                    var cellList = group.Take(3).ToList();

                    resultList.Add(cellList);
                }
            }

            // Выводим результат
            foreach (var list in resultList)
            {
                foreach (var cell in list)
                {
                    cell.Demolish();
                }
            }

            MoveTiles();


            if (cells.Count(x => x.Empty) == 0)
            {
                OnStackFull?.Invoke();
                Debug.Log("Lose Game");
            }
        }





        [System.Serializable]
        private class Cell
        {
            [SerializeField] private Transform transform;


            public Transform Holder => transform;

            public Tile Tile { get; private set; }

            public bool Empty
            {
                get => Tile == null;
            }
            public TileTypes Type
            {
                get => Tile ? Tile.TileType : default;
            }

            public void Accept(Tile tile)
            {
                Tile = tile;
            }
            public void Demolish()
            {
                Destroy(Tile.gameObject);
                Clear();
            }
            public void Clear()
            {
                Tile = null;
            }
        }
    }
}
