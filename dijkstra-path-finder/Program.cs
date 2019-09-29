using System;
using System.Collections.Generic;
using System.Linq;

namespace dijkstra_path_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            Node[] nodes = new Node[5];
            nodes[0] = new Node
            {
                Name = "A",
                NeighborNodes = new Dictionary<string, double>()
            };

            nodes[1] = new Node
            {
                Name = "B",
                NeighborNodes = new Dictionary<string, double>()
            };

            nodes[2] = new Node
            {
                Name = "C",
                NeighborNodes = new Dictionary<string, double>()
            };

            nodes[3] = new Node
            {
                Name = "D",
                NeighborNodes = new Dictionary<string, double>()
            };

            nodes[4] = new Node
            {
                Name = "E",
                NeighborNodes = new Dictionary<string, double>()
            };

            nodes[0].NeighborNodes.Add("B", 6);
            nodes[0].NeighborNodes.Add("D", 1);

            nodes[1].NeighborNodes.Add("A", 6);
            nodes[1].NeighborNodes.Add("D", 2);
            nodes[1].NeighborNodes.Add("E", 2);
            nodes[1].NeighborNodes.Add("C", 5);

            nodes[2].NeighborNodes.Add("B", 5);
            nodes[2].NeighborNodes.Add("E", 5);

            nodes[3].NeighborNodes.Add("A", 1);
            nodes[3].NeighborNodes.Add("B", 2);
            nodes[3].NeighborNodes.Add("E", 1);

            nodes[4].NeighborNodes.Add("D", 1);
            nodes[4].NeighborNodes.Add("B", 2);
            nodes[4].NeighborNodes.Add("C", 5);

            var dict = BuildMap(nodes, "A");

            PrintMap(dict);

            var result = FindShortestPath(dict, "A", "C");

            Console.WriteLine($"Shortest path betwen A and C cost you {result.Item1}");
            Console.WriteLine("Path:");
            PrintPath(result.Item2);
        }

        static void PrintPath(List<string> path)
        {
            foreach(var node in path)
            {
                Console.Write(node);
                if(node != path[path.Count - 1])
                {
                    Console.Write("->");
                }                
            }
            Console.WriteLine();
        }

        static (double, List<string>) FindShortestPath(Dictionary<string, (double, string)> map, string source, string destination)
        {
            double distance = map[destination].Item1;
            string curNodeName = map[destination].Item2;
            List<string> path = new List<string>();
            path.Add(destination);
            do
            {
                path.Insert(0, curNodeName);
                curNodeName = map[curNodeName].Item2;
            } while (!string.IsNullOrEmpty(curNodeName));

            return (distance, path);
        }

        static void PrintMap(Dictionary<string, (double, string)> map)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("|" + "Dijkstra Map Source Node = A".PadRight(35) + "|");
            Console.WriteLine("------------------------------------");
            Console.WriteLine("|" + "Node".PadRight(11) + "|" + "Cost".PadRight(11) + "|" + "Last Node".PadRight(11) + "|");
            Console.WriteLine("------------------------------------");
            foreach (var line in map)
            {
                Console.WriteLine($"|{line.Key.PadRight(11)}|{line.Value.Item1.ToString().PadRight(11)}|{line.Value.Item2.PadRight(11)}|");
                Console.WriteLine("------------------------------------");
            }
        }

        static Dictionary<string, (double, string)> BuildMap(Node[] nodes, string source)
        {
            Dictionary<string, (double, string, bool)> dictMap = new Dictionary<string, (double, string, bool)>();
            foreach(var node in nodes)
            {
                if(node.Name == source)
                {
                    dictMap.Add(source, (0, "", false));
                }
                else
                {
                    dictMap.Add(node.Name, (double.PositiveInfinity, string.Empty, false));
                }
            }

            Node curNode = nodes.Where(x => x.Name == source).FirstOrDefault();

            while(true)
            {                
                foreach(var node in curNode.NeighborNodes)
                {
                    if(dictMap[node.Key].Item3)
                    {
                        continue;
                    }
                    double distance = dictMap[curNode.Name].Item1 + node.Value;
                    if(distance < dictMap[node.Key].Item1)
                    {
                        dictMap[node.Key] = (distance, curNode.Name, dictMap[node.Key].Item3);
                    }
                }

                dictMap[curNode.Name] = (dictMap[curNode.Name].Item1, dictMap[curNode.Name].Item2, true);
                if (dictMap.Where(x => x.Value.Item3 == false).ToList().Count == 0)
                {
                    break;
                }
                string curNodeName = dictMap.Where(x => x.Value.Item3 == false).Aggregate((x1, x2) => x1.Value.Item1 < x2.Value.Item1 ? x1 : x2).Key;
                curNode = nodes.Where(x => x.Name == curNodeName).FirstOrDefault();                
            }
            return dictMap.ToDictionary(k => k.Key, k => (k.Value.Item1, k.Value.Item2));
        }
    }

    public struct Node
    {
        public string Name;
        public Dictionary<string, double> NeighborNodes;
    }
}
