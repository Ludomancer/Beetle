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
    private Texture2D _paintCanvas;
    private Vector2 _dragStart;
    private Vector2 _dragEnd;
    private Vector2 _preDrag;
    public Drawing.Samples antiAlias = Drawing.Samples.Samples4;
    public Tool tool = Tool.Brush;
    public Color col = Color.black;
    public BrushTool brush = new BrushTool();
    public EraserTool eraser = new EraserTool();
    private float _ratio = 1;
    private int _paddingLeft;


    private int _currentCanvasSize;
    private int _lockCount;

    public Texture2D PaintCanvas
    {
        get { return _paintCanvas; }
        set
        {
            _paintCanvas = value;
            if (_paintCanvas) SetCanvasSize((int)(Screen.height * 0.8f));
        }
    }

    void Awake()
    {
        brush.hardness = 1;
        brush.width = 8;
        brush.spacing = 1;

        eraser.hardness = 1;
        eraser.width = 16;
    }

    public void SetCanvasSize(int newSize)
    {
        if (PaintCanvas)
        {
            _currentCanvasSize = newSize;
            _ratio = _currentCanvasSize / (float)PaintCanvas.width;
            _paddingLeft = (int)((Screen.width - _currentCanvasSize) * 0.5f);
        }
    }

    public void Lock()
    {
        _lockCount++;
    }

    public void Unlock(bool force = false)
    {
        if (force) _lockCount = 0;
        else if (_lockCount > 0) _lockCount--;
    }

    void OnGUI()
    {
        if (_lockCount > 0 || !PaintCanvas) return;
        GUI.DrawTexture(new Rect(_paddingLeft, 0, _currentCanvasSize, _currentCanvasSize), PaintCanvas);
    }

    void Update()
    {
        if (_lockCount > 0 || !PaintCanvas) return;
        Rect imgRect = new Rect(_paddingLeft, 0, _currentCanvasSize, _currentCanvasSize);
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

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
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
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
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
        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && _dragStart != Vector2.zero)
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
        Drawing.PaintLine(p1, p2, brush.width, col, brush.hardness, PaintCanvas);
        PaintCanvas.Apply();
    }

    void Eraser(Vector2 p1, Vector2 p2)
    {
        Drawing.numSamples = antiAlias;
        if (p2 == Vector2.zero)
        {
            p2 = p1;
        }
        Drawing.PaintLine(p1, p2, eraser.width, Color.white, eraser.hardness, PaintCanvas);
        PaintCanvas.Apply();
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
