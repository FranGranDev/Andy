using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EditorTools;
using Game.Tiles;
using UnityEditor;
using System;

namespace Utils.Tiles
{
    [EditorTool("Tiles Creator", typeof(TilesBoardCreator))]
    public class TilesCreatorTool : EditorTool
    {
        [Header("View")]
        [SerializeField] private Texture2D icon;


        private TilesCreatorSettings settings;
        private TilesBoardCreator tilesHolder;

        private ToolTypes currentTool;
        private TileTypes currentTile;

        private string[] toolOptions;
        private string[] tileNames;

        public override GUIContent toolbarIcon
        {
            get
            {
                return new GUIContent()
                {
                    text = "Tiles Creator",
                    tooltip = "Can create tiles board",
                    image = icon,
                };
            }
        }


        private void OnEnable()
        {
            LoadSettings();

            toolOptions = Enum.GetNames(typeof(ToolTypes));
            tileNames = Enum.GetNames(typeof(TileTypes));
        }
        public override void OnActivated()
        {
            tilesHolder = target as TilesBoardCreator;
        }
        public override void OnWillBeDeactivated()
        {
            tilesHolder = null;
        }


        private void LoadSettings()
        {
            settings = Resources.Load<TilesCreatorSettings>("Utils/TilesCreator");
        }


        public override void OnToolGUI(EditorWindow window)
        {
            Event e = Event.current;

            DrawOptionsGUI(window);

            CheckInput(e, DrawTool(e));

            SceneView.RepaintAll();
        }

        private void CheckInput(Event e, Vector2 point)
        {
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                switch(currentTool)
                {
                    case ToolTypes.Create:
                        Create(point, currentTile);
                        break;
                    case ToolTypes.Delete:
                        Delete(point);
                        break;
                }

            }

            RestoreSelection();
        }
        private void DrawOptionsGUI(EditorWindow window)
        {
            Handles.BeginGUI();

            Rect windowRect = new Rect(10, window.position.height - 275, 200, 225);

            GUILayout.BeginArea(windowRect, "Tool Options", GUI.skin.window);

            GUILayout.Label("Tool Type:", EditorStyles.boldLabel);
            currentTool = (ToolTypes)GUILayout.SelectionGrid((int)currentTool, toolOptions, 2, GUI.skin.toggle);

            GUILayout.Space(10);

            if (currentTool == ToolTypes.Create)
            {
                GUILayout.Label("Tile Type:", EditorStyles.boldLabel);
                currentTile = (TileTypes)GUILayout.SelectionGrid((int)currentTile, tileNames, 2, GUI.skin.toggle);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Clear"))
            {
                Clear();
            }

            if (GUILayout.Button("Save"))
            {
                Save();
            }

            if (GUILayout.Button("Load"))
            {
                Load();
            }

            GUILayout.EndArea();

            Handles.EndGUI();
        }


        private Vector2 DrawTool(Event e)
        {
            Vector2 point;
            switch (currentTool)
            {
                case ToolTypes.Delete:
                    point = DrawDelete(e);
                    break;
                default:
                    point = DrawTile(e);
                    break;
            }

            return point;
        }
        private Vector2 DrawTile(Event e)
        {
            Vector2 point = ToWorld2D(e.mousePosition);

            float offsetX = settings.TileSizeX / 2;
            float offsetY = settings.TileSizeY / 2;
            float x = Mathf.RoundToInt(point.x / offsetX) * offsetX;
            float y = Mathf.RoundToInt(point.y / offsetY) * offsetY;

            point = new Vector2(x, y);

            Vector2 size = new Vector2(settings.TileSizeX, settings.TileSizeY);

            Handles.color = Color.white;
            Handles.DrawWireCube(point, size);

            return point;
        }
        private Vector2 DrawDelete(Event e)
        {
            Vector2 point = ToWorld2D(e.mousePosition);

            float offsetX = settings.TileSizeX / 2;
            float offsetY = settings.TileSizeY / 2;
            float x = Mathf.RoundToInt(point.x / offsetX) * offsetX;
            float y = Mathf.RoundToInt(point.y / offsetY) * offsetY;

            point = new Vector2(x, y);

            Vector2 size = new Vector2(settings.TileSizeX, settings.TileSizeY);

            Handles.color = Color.red;
            Handles.DrawWireCube(point, size);
            Handles.DrawLine(new Vector2(-offsetX, -offsetY) + point, new Vector2(offsetX, offsetY) + point);

            return point;
        }


        private Vector2 ToWorld2D(Vector2 input)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(input);
            Plane plane = new Plane(Vector3.forward, Vector3.zero);

            if(plane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return Vector2.zero;
        }
        private void RestoreSelection()
        {
            if(tilesHolder)
            {
                Selection.activeGameObject = tilesHolder.gameObject;
                EditorGUIUtility.PingObject(tilesHolder);
            }
        }


        private void Create(Vector2 position, TileTypes tileType)
        {
            tilesHolder.CreateAt(position, tileType);
        }
        private void Delete(Vector2 position)
        {
            tilesHolder.DeleteAt(position);
        }

        private void Save()
        {
            tilesHolder.Save();
        }
        private void Clear()
        {
            tilesHolder.Clear();
        }
        private void Load()
        {
            tilesHolder.Load();
        }

        private enum ToolTypes
        {
            Create,
            Delete,
        }
    }
}
