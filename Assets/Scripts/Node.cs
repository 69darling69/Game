using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Node : MonoBehaviour
{
    private int _layer = 0;
    private int _position = 0;
    private int? _seed = null;
    private NodesBuilder _nodesBuilder;
    private List<Node> _childs;

    public int Layer
    {
        get => _layer;
        set
        {
            _layer = value;
            UpdatePosition();
        }
    }
    public int Position
    {
        get => _position;
        set
        {
            _position = value;
            UpdatePosition();
        }
        
    }

    public int? Seed
    {
        get => _seed;
        set
        {
            if (_seed != null)
            {
                throw new Exception("Can't switch seed");
            }
            _seed = value;
            gameObject.name = _seed.ToString();
        }
    }

    public void AddChild(Node child)
    {
        _childs.Add(child);
        // TODO: Transfer responsibility for drawing lines to the builder
        DrawLine(transform.position, child.transform.position);
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {

        LineRenderer lineRenderer = new GameObject().AddComponent<LineRenderer>();
        //lineRenderer.material = new Material(Shader.Find("Sprite-Lit-Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = .1f;
        lineRenderer.endWidth = .1f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private void Awake()
    {
        _childs = new();
    }

    private void Start()
    {
        _nodesBuilder = Transform.FindObjectOfType<NodesBuilder>();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // TODO: Dynamic placement
        transform.position = new Vector2(Position * 2 - Layer, 4 - Layer * 2);
    }

    private void OnMouseDown() => _nodesBuilder.CreateChilds(this);
}
