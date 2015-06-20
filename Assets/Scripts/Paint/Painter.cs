using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Painter : MonoBehaviour
{
    #region Singleton

    private static Painter instance;

    internal static Painter Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType(typeof(Painter)) as Painter;
            if (instance != null) return instance;

            var container = new GameObject("LevelDesigner");
            instance = container.AddComponent<Painter>();
            return instance;
        }
    }

    #endregion

    public enum Tool
    {
        None,
        Brush,
        Eraser
    }

    public Texture2D sourceBaseTex;
    private Texture2D[] _canvases = new Texture2D[2];
    private Vector2 _dragStart;
    private Vector2 _dragEnd;
    private Vector2 _preDrag;
    public Drawing.Samples antiAlias = Drawing.Samples.Samples4;
    public Tool tool = Tool.Brush;
    public Color col = Color.black;
    public BrushTool brush = new BrushTool();
    public EraserTool eraser = new EraserTool();
    public float canvasSize = 512;
    private float _ratio = 1;
    private int _canvasIndex = 0;

    void Start()
    {
        _canvasIndex = 0;
        _canvases[0] = Instantiate(sourceBaseTex);
        _canvases[1] = Instantiate(sourceBaseTex);
        brush.hardness = 1;
        brush.width = 5;

        eraser.hardness = 1;
        eraser.width = 2;

        _ratio = canvasSize / _canvases[0].width;
    }

    public Texture2D GetActiveCanvas()
    {
        return _canvases[_canvasIndex];
    }

    public void SetActiveCanvass(int index)
    {
        if (index >= _canvases.Length || index < 0) throw new IndexOutOfRangeException();
        _canvasIndex = index;
    }

    void OnGUI()
    {
        int padding = 100;
        for (int i = 0; i < _canvases.Length; i++)
        {
            GUI.DrawTexture(new Rect(padding, 0, _canvases[i].width * _ratio, _canvases[i].height * _ratio), _canvases[i]);
            padding += (int)(_canvases[i].width * _ratio) + 25;
        }
    }

    void Update()
    {
        Rect imgRect = new Rect(5 + 100, 5, GetActiveCanvas().width * _ratio, GetActiveCanvas().height * _ratio);
        Vector2 mouse = Input.mousePosition;
        mouse.y = Screen.height - mouse.y;


        if (Input.GetMouseButtonDown(1))
        {
            tool = Tool.Eraser;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            tool = Tool.Brush;
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (imgRect.Contains(mouse))
            {
                _dragStart = mouse - new Vector2(imgRect.x, imgRect.y);
                _dragStart.y = imgRect.height - _dragStart.y;
                _dragStart.x = Mathf.Round(_dragStart.x / _ratio);
                _dragStart.y = Mathf.Round(_dragStart.y / _ratio);

                _dragEnd = mouse - new Vector2(imgRect.x, imgRect.y);
                _dragEnd.x = Mathf.Clamp(_dragEnd.x, 0, imgRect.width);
                _dragEnd.y = imgRect.height - Mathf.Clamp(_dragEnd.y, 0, imgRect.height);
                _dragEnd.x = Mathf.Round(_dragEnd.x / _ratio);
                _dragEnd.y = Mathf.Round(_dragEnd.y / _ratio);
            }
            else
            {
                _dragStart = Vector3.zero;
            }

        }
        if (Input.GetMouseButton(0))
        {
            if (_dragStart == Vector2.zero)
            {
                return;
            }
            _dragEnd = mouse - new Vector2(imgRect.x, imgRect.y);
            _dragEnd.x = Mathf.Clamp(_dragEnd.x, 0, imgRect.width);
            _dragEnd.y = imgRect.height - Mathf.Clamp(_dragEnd.y, 0, imgRect.height);
            _dragEnd.x = Mathf.Round(_dragEnd.x / _ratio);
            _dragEnd.y = Mathf.Round(_dragEnd.y / _ratio);

            if (tool == Tool.Brush)
            {
                Brush(_dragEnd, _preDrag);
            }
            if (tool == Tool.Eraser)
            {
                Eraser(_dragEnd, _preDrag);
            }

        }
        if (Input.GetMouseButtonUp(0) && _dragStart != Vector2.zero)
        {
            _dragStart = Vector2.zero;
            _dragEnd = Vector2.zero;
        }
        _preDrag = _dragEnd;
    }

    void Brush(Vector2 p1, Vector2 p2)
    {
        Drawing.numSamples = antiAlias;
        if (p2 == Vector2.zero)
        {
            p2 = p1;
        }
        Drawing.PaintLine(p1, p2, brush.width, col, brush.hardness, GetActiveCanvas());
        GetActiveCanvas().Apply();
    }

    void Eraser(Vector2 p1, Vector2 p2)
    {
        Drawing.numSamples = antiAlias;
        if (p2 == Vector2.zero)
        {
            p2 = p1;
        }
        Drawing.PaintLine(p1, p2, eraser.width, Color.white, eraser.hardness, GetActiveCanvas());
        GetActiveCanvas().Apply();
    }

    public class EraserTool
    {
        public float width = 1;
        public float hardness = 1;
    }
    public class BrushTool
    {
        public float width = 1;
        public float hardness = 0;
        public float spacing = 10;
    }
}
