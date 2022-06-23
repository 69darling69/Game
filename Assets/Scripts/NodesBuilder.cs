using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesBuilder : MonoBehaviour
{
    [SerializeField][Min(0)] private int _numberOfChilds = 2;
    [SerializeField] private GameObject _nodePrefab;

    private Dictionary<Tuple<int, int>, Node> _nodes;
    private int _seed;

    private void Start()
    {
        _seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        _nodes = new();
        Node root = CreateNode(0, 0);
    }

    public void CreateChilds(Node parent)
    {
        for (int i = 0; i < _numberOfChilds; i++)
        {
            Node child = CreateNode(parent.Layer + 1, parent.Position + i);
            parent.AddChild(child);
        }
    }

    private void AddNodeToDict(Node node)
    {
        _nodes.Add(new Tuple<int, int>(node.Layer, node.Position), node);
    }

    private Node CreateNode(int layer, int position)
    {
        // ?????????? ????, ???? ??? ??? ????
        Node node;
        if (_nodes.TryGetValue(new Tuple<int, int>(layer, position), out node))
        {
            return node;
        }

        // ??????? ????? ????, ????????? ?? ? ???????
        node = Instantiate(_nodePrefab, transform).GetComponent<Node>();
        node.Layer = layer;
        node.Position = position;
        // TODO: ???????? ????? ?? ??????? ??????????
        node.Seed = _seed + node.Layer * 1000 + node.Position;
        AddNodeToDict(node);

        // ????????? ?????
        for (int i = 0; i < _numberOfChilds; i++)
        {
            Node child;
            if (_nodes.TryGetValue(new Tuple<int, int>(node.Layer + 1, node.Position + i), out child))
            {
                node.AddChild(child);
            }
        }

        // ????????? ?????????, ? ??? ??? ? ??? ????? ???????
        for (int i = 0; i < _numberOfChilds; i++)
        {
            Node parent;
            if (_nodes.TryGetValue(new Tuple<int, int>(node.Layer - 1, node.Position - i), out parent))
            {
                parent.AddChild(node);
            }
        }

        return node;
    }
}
