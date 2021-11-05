using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaffmanCode
{
    public class HuffmanCode
    {
        public string StringToCode { get; private set; }
        private Dictionary<char, int> _frequencies = new Dictionary<char, int>();
        private C5.IntervalHeap<Node> _priorityQueue = new C5.IntervalHeap<Node>();
        private List<KeyValuePair<char, string>> _codes = new List<KeyValuePair<char, string>>();
        private Node _root = new Node(0);
        public List<KeyValuePair<char, string>> Codes { get => _codes; }
        public HuffmanCode(string str)
        {
            StringToCode = str;

            MakeDictionary();
            MakePriorityQueue();

            _root = MakeTree();

            CreateCodeTable(_root, String.Empty);
        }
        private void MakePriorityQueue()
        {
            foreach (KeyValuePair<char, int> keyValue in _frequencies)
                _priorityQueue.Add(new Node(keyValue.Key, keyValue.Value));
        }
        private void MakeDictionary()
        {
            foreach (char character in StringToCode)
            {
                if (_frequencies.ContainsKey(character) == false)
                    _frequencies[character] = 1;
                else
                    _frequencies[character]++;
            }
        }
        private Node MakeTree()
        {
            while (_priorityQueue.Count > 1)
            {
                Node first = _priorityQueue.DeleteMin();
                Node second = _priorityQueue.DeleteMin();
                Node newNode = new Node(first, second);
                _priorityQueue.Add(newNode);
            }

            return _priorityQueue.DeleteMin();
        }
        private void CreateCodeTable(Node root, string code)
        {
            if (root == null) return;
            if (_root == root)
            {
                CreateCodeTable(root.Left, "0");
                CreateCodeTable(root.Right, "1");
                return;
            }
            if (root.Symbol != default(char))
            {
                _codes.Add(new KeyValuePair<char, string>(root.Symbol, code));
            }
            CreateCodeTable(root.Left, code + "0");
            CreateCodeTable(root.Right, code + "1");
        }
        public void PrintTable()
        {
            foreach (KeyValuePair<char, string> keyValue in _codes)
                Console.WriteLine($"{keyValue.Key}: {keyValue.Value}");
        }
        private class Node : IComparable
        {
            public char Symbol { get; set; }
            public int Frequency { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Node(int frequency)
            {
                Frequency = frequency;
                Symbol = default(char);
            }
            public Node(Node left, Node right)
            {
                Frequency = left.Frequency + right.Frequency;
                Left = left;
                Right = right;
            }
            public Node(char symbol, int frequency)
            {
                Symbol = symbol;
                Frequency = frequency;
            }

            public int CompareTo(object obj)
            {
                var value = this.Frequency - (obj as Node).Frequency;
                // nodes are equal, but first one doesn't have symbol
                if (value == 0)
                {
                    if (this.Symbol == default(char))
                        return -1;
                    return 1;
                }
                return value;
            }
            public override string ToString()
            {
                return $"Char: {Symbol}; Frequency: {Frequency}";
            }
        }
    }
}
