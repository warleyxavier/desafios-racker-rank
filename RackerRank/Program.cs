using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace RackerRank
{
    class SinglyLinkedListNode
    {
        public int data;
        public SinglyLinkedListNode next;

        public SinglyLinkedListNode(int nodeData)
        {
            this.data = nodeData;
            this.next = null;
        }
    }

    class SinglyLinkedList
    {
        public SinglyLinkedListNode head;
        public SinglyLinkedListNode tail;

        public SinglyLinkedList()
        {
            this.head = null;
            this.tail = null;
        }

    }

    class MyQueue<T>
    {
        private Stack<T> stack1 = new Stack<T>();
        private Stack<T> stack2 = new Stack<T>();

        public void enqueue(T value)
        {
            stack1.Push(value);
        }

        public T dequeue()
        {
            if (size() == 0)
                throw new Exception("Sem registros");

            if (stack2.Count() == 0)
                migrateDataBetweenStacks();

            return stack2.Pop();
        }

        private void migrateDataBetweenStacks()
        {
            while (stack1.Count() > 0)
                stack2.Push(stack1.Pop());
        }

        public T peek()
        {
            if (size() == 0)
                throw new Exception("Sem registros");

            if (stack2.Count() == 0)
                migrateDataBetweenStacks();

            return stack2.Peek();
        }

        public int size()
        {
            return this.stack1.Count() + this.stack2.Count();
        }

    }

    class Result
    {
        static int[] parent;
        static int[] size;

        public static void Union(int a, int b)
        {
            int x = Find(a);
            int y = Find(b);

            if (x == y) return;
            if (size[a] > size[b])
            {
                parent[b] = a;
                size[a] += size[b];
            }
            else
            {
                parent[a] = b;
                size[b] += size[a];
            }
        }
        public static int Find(int a)
        {
            int root = a;

            while (parent[root] != root)
            {
                root = parent[root];
            }

            while (a != root)
            {
                int temp = parent[a];
                parent[a] = root;
                a = temp;
            }
            return root;
        }
        public static int kruskals(int gNodes, List<int> gFrom, List<int> gTo, List<int> gWeight)
        {

            int min = 0;
            parent = new int[gNodes + 1];
            size = new int[gNodes + 1];

            for (int i = 1; i < gNodes; i++)
            {
                parent[i] = i;
                size[i] = 1;
            }

            for (int i = 0; i < gWeight.Count; i++)
            {
                for (int j = 0; j < gWeight.Count - i - 1; j++)
                {
                    if (gWeight[j] > gWeight[j + 1])
                    {
                        int temp = gWeight[j];
                        gWeight[j] = gWeight[j + 1];
                        gWeight[j + 1] = temp;
                        temp = gTo[j];
                        gTo[j] = gTo[j + 1];
                        gTo[j + 1] = temp;
                        temp = gFrom[j];
                        gFrom[j] = gFrom[j + 1];
                        gFrom[j + 1] = temp;
                    }
                    else if (gWeight[j] == gWeight[j + 1])
                    {
                        int sum1 = gFrom[j] + gTo[j] + gWeight[j];
                        int sum2 = gFrom[j + 1] + gTo[j + 1] + gWeight[j + 1];
                        if (sum1 >= sum2)
                        {
                            int temp = gWeight[j];
                            gWeight[j] = gWeight[j + 1];
                            gWeight[j + 1] = temp;
                            temp = gTo[j];
                            gTo[j] = gTo[j + 1];
                            gTo[j + 1] = temp;
                            temp = gFrom[j];
                            gFrom[j] = gFrom[j + 1];
                            gFrom[j + 1] = temp;
                        }
                    }
                }
            }

            int count = 0;

            for (int i = 0; i < gWeight.Count && count < gNodes; i++)
            {
                int x = Find(gFrom[i]);
                int y = Find(gTo[i]);
                if (x == y)
                    continue;
                else
                {
                    count++;
                    min += gWeight[i];
                    Union(x, y);
                }
            }

            return min;
        }

    }

    class Node
    {
        public int Value { get; private set; }
        public char Letter { get; private set; }
        public List<Node> Children { get; private set; }

        public Node()
        {
            this.Value = 0;
            this.Children = new List<Node>();
        }

        public Node(char letter)
        {
            this.Letter = letter;
            this.Value = 0;
            this.Children = new List<Node>();
        }

        public Node findOrCreate(char letter)
        {
            this.Value++;
            return find(letter) ?? addChild(letter);

        }

        public Node find(char letter)
        {
            return this.Children.Where(child => child.Letter == letter).FirstOrDefault();
        }

        public Node addChild(char letter)
        {
            Node node = new Node(letter);
            this.Children.Add(node);
            return node;
        }

    }

    class Tree
    {
        private Node root;

        public Tree()
        {
            root = new Node();
        }

        public int countWordOcurrence(string word)
        {
            var currentNode = this.root;

            foreach (var letter in word)
            {
                var child = currentNode.find(letter);

                if (child == null)
                    return 0;

                currentNode = child;
            }

            return currentNode.Value;
        }

        public void add(string word)
        {
            var currentNode = this.root;

            foreach (var letter in word)
            {
                currentNode = currentNode.findOrCreate(letter);
            }

            currentNode.findOrCreate('*');
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            equalStacks();
            Console.ReadLine();
        }

        static int migratoryBirds(List<int> arr)
        {
            Dictionary<int, int> birds = new Dictionary<int, int>();

            arr.ForEach(bird =>
            {
                int birdCounter = 0;

                if (birds.TryGetValue(bird, out birdCounter))
                    birdCounter++;

                birds[bird] = birdCounter;
            });

            int maxCountOfBirds = birds.Values.Max();

            int smallerBirdFounded = Int32.MaxValue;

            foreach (var bird in birds)
            {
                if (bird.Value != maxCountOfBirds)
                    continue;

                if (bird.Key < smallerBirdFounded)
                    smallerBirdFounded = bird.Key;
            }

            return smallerBirdFounded;
        }

        static int[] contacts()
        {
            string[][] queries = new string[][]
            {
                new string [] {"add", "ed"},
                new string [] {"add", "eddie"},
                new string [] {"add", "edward"},
                new string [] { "find ", "ed"},
                new string [] {"add", "edwina"},
                new string [] { "find ", "edw"},
                new string [] { "find ", "a"},
            };

            List<int> results = new List<int>();

            Tree tree = new Tree();

            foreach (var query in queries)
            {
                string operation = query[0];
                string value = query[1];

                if (operation.Equals("add"))
                    tree.add(value);
                else
                    results.Add(tree.countWordOcurrence(value));
            }

            return results.ToArray();
        }

        static string kangaroo(int x1, int v1, int x2, int v2)
        {
            return v1 > v2 && ((x1 - x2) % (v2 - v1)) == 0 ? "YES" : "NO";
        }

        static string biggerIsGreater(string w)
        {
            for (int i = w.Length - 2; i >= 0; i--)
            {
                for (int j = w.Length - 1; j > i; j--)
                {
                    if (w[j] <= w[i])
                        continue;

                    string result = String.Empty;

                    for (int k = 0; k < i; k++)
                        result += w[k];

                    result += w[j];

                    for (int t = w.Length - 1; t > i; t--)
                        result += t != j ? w[t] : w[i];

                    return result;
                }
            }
            return "no answer";

        }

        static int maxMin(int k, int[] arr)
        {
            int minimumValue = Int32.MaxValue;

            Array.Sort(arr);

            for (int counter = 0; counter < arr.Length - k + 1; counter++)
            {
                int combination = arr[counter + k - 1 - 1] - arr[counter];

                if (combination < minimumValue)
                    minimumValue = combination;
            }

            return minimumValue;

        }


        static void divisibleSumPairs()
        {
            int k = 3;
            int[] ar = new int[] { 1, 3, 2, 6, 1, 2 };

            int totalCombinations = 0;

            for (int posicao = 0; posicao < ar.Length - 1; posicao++)
                for (int posicaoCombinatoria = posicao + 1; posicaoCombinatoria < ar.Length; posicaoCombinatoria++)
                {
                    int result = ar[posicao] + ar[posicaoCombinatoria];
                    result = (ar[posicao] + ar[posicaoCombinatoria]) % k;

                    if (ar[posicao] + ar[posicaoCombinatoria] % k == 0)
                        totalCombinations++;
                }


            Console.WriteLine(totalCombinations);
        }

        static int birthday(List<int> s, int d, int m)
        {
            int totalCombinations = 0;

            for (int counter = 0; counter <= s.Count - m; counter++)
            {
                var combination = s.GetRange(counter, m).Sum();

                if (combination == d)
                    totalCombinations++;
            }
            return totalCombinations;
        }

        public static void kruskals()
        {
            //int res = Result.kruskals(4, gFrom, gTo, gWeight);

            /* List<int> gFrom = new List<int>() { 1, 1, 1, 1, 2, 3, 4 };
             List<int> gTo = new List<int>() { 2, 3, 4, 5, 3, 4, 5 };
             List<int> gWeight = new List<int>() { 20, 50, 70, 90, 30, 40, 60 };*/

            List<int> gFrom = new List<int>() { 1, 3, 4, 1, 3 };
            List<int> gTo = new List<int>() { 2, 2, 3, 4, 1 };
            List<int> gWeight = new List<int>() { 1, 150, 99, 100, 200 };

            ordKruskals(gFrom, gTo, gWeight);
            //List<string> mappedNodes = mappNodes(gFrom, gTo, gWeight);

            List<int> primaryTree = new List<int>();
            List<int> secondaryTree = new List<int>();

            int sumValues = 0;

            for (int position = 0; position < gWeight.Count(); position++)
            {
                int weight = gWeight[position];

                int from = gFrom[position];
                int to = gTo[position];

                if (primaryTree.Contains(from) && primaryTree.Contains(to))
                    continue;

                sumValues += weight;

                if ((primaryTree.Contains(from) || primaryTree.Contains(to)) && (secondaryTree.Contains(from) || secondaryTree.Contains(to)))
                {
                    unionTrees(primaryTree, secondaryTree);
                    addInTree(from, to, primaryTree);
                }
                else
                {
                    if (primaryTree.Count() == 0 || primaryTree.Contains(from) || primaryTree.Contains(to))
                        addInTree(from, to, primaryTree);
                    else
                        addInTree(from, to, secondaryTree);
                }
            }

            Console.WriteLine(sumValues);
        }

        static void addInTree(int from, int to, List<int> tree)
        {
            if (!tree.Contains(from))
                tree.Add(from);

            if (!tree.Contains(to))
                tree.Add(to);
        }

        static void unionTrees(List<int> primaryTree, List<int> secondaryTree)
        {
            primaryTree.AddRange(secondaryTree);
            secondaryTree.Clear();
        }

        static void ordKruskals(List<int> gFrom, List<int> gTo, List<int> gWeight)
        {
            for (int i = 0; i < gWeight.Count; i++)
            {
                for (int j = 0; j < gWeight.Count - i - 1; j++)
                {
                    if (gWeight[j] > gWeight[j + 1])
                    {
                        int temp = gWeight[j];
                        gWeight[j] = gWeight[j + 1];
                        gWeight[j + 1] = temp;
                        temp = gTo[j];
                        gTo[j] = gTo[j + 1];
                        gTo[j + 1] = temp;
                        temp = gFrom[j];
                        gFrom[j] = gFrom[j + 1];
                        gFrom[j + 1] = temp;
                    }
                    else if (gWeight[j] == gWeight[j + 1])
                    {
                        int sum1 = gFrom[j] + gTo[j] + gWeight[j];
                        int sum2 = gFrom[j + 1] + gTo[j + 1] + gWeight[j + 1];
                        if (sum1 >= sum2)
                        {
                            int temp = gWeight[j];
                            gWeight[j] = gWeight[j + 1];
                            gWeight[j + 1] = temp;
                            temp = gTo[j];
                            gTo[j] = gTo[j + 1];
                            gTo[j + 1] = temp;
                            temp = gFrom[j];
                            gFrom[j] = gFrom[j + 1];
                            gFrom[j + 1] = temp;
                        }
                    }
                }
            }
        }

        static void breakingRecords()
        {
            int[] scores = new int[] { 10, 5, 20, 20, 4, 5, 2, 25, 1 };

            int biggerPunctuation = scores[0];
            int smallerPunctuation = scores[0];

            int countRecordsBreakeds = 0;
            int countWithLessPoints = 0;

            for (int position = 1; position < scores.Length; position++)
            {
                int currentScore = scores[position];

                if (currentScore == scores[position - 1])
                    continue;

                if (currentScore > biggerPunctuation)
                {
                    biggerPunctuation = currentScore;
                    countRecordsBreakeds++;
                }

                if (currentScore > smallerPunctuation)
                {
                    smallerPunctuation = currentScore;
                    countWithLessPoints++;
                }
            }

        }

        public static void gradingStudents(List<int> grades)
        {
            const int MENOR_NOTA_ACEITA = 38;
            const int MENOR_MARGEM_ACEITA = 3;

            List<int> results = new List<int>();

            grades.ForEach(nota =>
            {
                int result = 0;

                if (nota < MENOR_NOTA_ACEITA || nota % 5 < MENOR_MARGEM_ACEITA)
                    result = nota;
                else
                    result = nota + (5 - nota % 5);

                results.Add(result);
            });

        }

        static void circularArrayRotation()
        {
            int[] a = new int[] { 29261, 80254, 86934, 3704, 76338, 96698, 47885, 88475, 65211, 65976, 75238, 58566, 28684, 20348, 45383, 58620, 48360, 99801, 51885, 82661, 83066, 14311, 24803, 99267, 21541, 93195, 21194, 20775, 64817, 42323, 7640, 10429, 38928, 94573, 30484, 15265, 7622, 78368, 3739, 72833, 60696, 95328, 31398, 5731, 15676, 93132, 64351, 64035, 9284, 32587, 46695, 92349, 46897, 87850, 7968, 84789, 81044, 45513, 5563, 62212, 87836, 13202, 88993, 26763, 24127, 19476, 42028, 31748, 14196, 62118, 4580, 91243, 73798, 52329, 96973, 89473, 61812, 77675, 69859, 71095, 10261, 32905, 79796, 57157, 20754, 87763, 41945, 1798, 33275, 63859, 80361, 37462, 93413, 69353, 64225, 17539, 5181, 22604, 49286, 19376, 1073, 70218, 26970, 74870, 38898, 23942, 80694, 710, 1617, 50552, 88156, 11877, 83457, 67951, 85386, 4210, 55713, 43682, 22359, 5340, 23893, 2720, 59153, 17305, 88424, 23377, 51195, 93604, 62332, 480, 29331, 79757, 87049, 56300, 54626, 25947, 96594, 35320, 26656, 98210, 2223, 31163, 26438, 85679, 99114, 28175, 89889, 71178, 88209, 12247, 76517, 12101, 31318, 35670, 45757, 19742, 75398, 96951, 29697, 54082, 13782, 75380, 33838, 831, 31679, 4815, 26777, 28272, 56486, 69784, 42833, 58709, 946, 85623, 44387, 59, 13797, 50627, 87589, 2005, 62874, 80457, 14105, 94191, 32478, 59861, 30284, 7876, 73163, 59981, 78309, 86945, 35360, 28498, 87775, 83390, 49664, 30903, 28014, 6150, 686, 70846, 81210, 17983, 56468, 41948, 34394, 86617, 92575, 21982, 88621, 71800, 2438, 19078, 82342, 34916, 95290, 12626, 59143, 68453, 88958, 37451, 71749, 24317, 82300, 59523, 24058, 31963, 90425, 52071, 54464, 7462, 39269, 35673, 25444, 12088, 93973, 76189, 98704, 86547, 98170, 3677, 74698, 16960, 22754, 57039, 51875, 34395, 86016, 11017, 19199, 74973, 64819, 90947, 15641, 63470, 66821, 39699, 95432, 73597, 91769, 49896, 81058, 31037, 1920, 22854, 43125, 12244, 99042, 58180, 15142, 13564, 61856, 89839, 30523, 961, 63230, 98749, 51708, 49245, 26117, 70906, 24218, 90935, 78205, 39858, 54404, 45025, 95908, 66187, 34974, 87677, 32434, 32383, 35065, 50706, 55236, 78189, 62949, 70630, 36369, 78091, 545, 14576, 67929, 47419, 15537, 31158, 46167, 67244, 96755, 72283, 54501, 37324, 79569, 32705, 77181, 50324, 94082, 73089, 16510, 45407, 77117, 65296, 77789, 12181, 16001, 49377, 6722, 78949, 36358, 59442, 73391, 36902, 74017, 41320, 84320, 5905, 88829, 46838, 89500, 1935, 19120, 44001, 39258, 98688, 93057, 32791, 49011, 3490, 22231, 81872, 48896, 99347, 47167, 26685, 27879, 79519, 92413, 34600, 74820, 28770, 94041, 48210, 65671, 84410, 5881, 66342, 90314, 11062, 13179, 96166, 12996, 32298, 40166, 52254, 47337, 49574, 85044, 12699, 53064, 7274, 94570, 18311, 22972, 58089, 61347, 50850, 37607, 53759, 1802, 12426, 82528, 12194, 60636, 64550, 96603, 66516, 30891, 3269, 93929, 60421, 99434, 6925, 9070, 55951, 59178, 56406, 5524, 60573, 69104, 74939, 84198, 80026, 93250, 7169, 38114, 54596, 74370, 92072, 24707, 76171, 4498, 7234, 88365, 81485, 71784, 84967, 64352, 19026, 4587, 58281, 79447, 20372, 65205, 88516, 92674, 40734, 44922, 98198, 17658, 30377, 89488, 1855, 10402, 99089, 25375, 64867, 70037, 99744, 56939, 94743, 75915, 77788, 1976, 64279, 75624, 90111, 65597, 39975, 9137, 70184, 98255, 88583, 6907, 79811, 93450, 99581, 36896, 38371, 14130, 54553, 85100, 3617, 72759, 11853, 19058, 98133, 76720, 89094, 97877, 50010, 188, 73791, 44149, 18515, 54421, 19772, 8626, 20017, 59746, 17762, 6552, 74353, 22696, 13459, 70515, 16145, 29391, 7411, 70868, 43520, 78315, 55967, 63488, 51074, 84171, 82545, 49206, 60890, 87990, 63434, 27251, 4529, 53576 };
            int k = 100000;
            int[] queries = new int[] { 8,
323,
475,
409,
330,
282,
387,
133,
120,
424,
193,
425,
380,
99,
435,
107,
396,
506,
467,
64,
189,
242,
315,
110,
504,
396,
2,
143,
44,
425,
138,
52,
150,
16,
379,
480,
298,
251,
98,
335,
77,
208,
246,
457,
308,
83,
50,
106,
75,
434,
88,
264,
161,
320,
291,
150,
118,
293,
210,
162,
121,
266,
132,
271,
282,
511,
237,
497,
164,
252,
317,
241,
461,
480,
100,
254,
49,
67,
277,
124,
501,
365,
305,
147,
170,
81,
215,
206,
292,
425,
368,
413,
93,
417,
169,
375,
413,
323,
357,
494,
61,
160,
137,
7,
42,
155,
178,
91,
222,
455,
132,
209,
223,
437,
273,
310,
436,
405,
1,
130,
233,
287,
28,
326,
189,
114,
187,
5,
438,
29,
416,
416,
106,
39,
423,
149,
194,
3,
157,
416,
375,
290,
27,
83,
129,
218,
311,
482,
108,
229,
97,
341,
1,
125,
70,
108,
157,
257,
113,
80,
203,
14,
413,
310,
485,
238,
376,
164,
241,
18,
498,
101,
225,
442,
102,
355,
62,
330,
322,
171,
44,
337,
429,
46,
462,
499,
154,
21,
158,
184,
18,
362,
115,
431,
74,
86,
154,
450,
167,
312,
468,
67,
331,
96,
427,
433,
451,
489,
165,
175,
62,
209,
512,
492,
172,
377,
393,
243,
398,
37,
427,
417,
316,
460,
250,
390,
463,
405,
325,
115,
202,
195,
100,
18,
291,
12,
368,
144,
501,
450,
237,
481,
145,
234,
375,
317,
13,
253,
46,
329,
207,
390,
148,
8,
252,
398,
315,
200,
288,
125,
316,
408,
238,
333,
343,
446,
345,
114,
76,
248,
49,
230,
214,
194,
381,
74,
429,
395,
245,
392,
209,
369,
184,
357,
295,
437,
240,
95,
122,
446,
138,
355,
256,
293,
173,
84,
224,
435,
198,
217,
169,
165,
447,
300,
276,
314,
292,
190,
194,
22,
499,
320,
308,
169,
79,
88,
91,
236,
101,
130,
84,
239,
486,
340,
17,
144,
425,
158,
497,
25,
376,
151,
107,
308,
451,
384,
107,
228,
59,
218,
167,
476,
455,
476,
130,
19,
481,
221,
173,
67,
268,
257,
223,
239,
83,
157,
301,
508,
316,
283,
450,
177,
351,
43,
402,
287,
427,
427,
433,
403,
130,
2,
364,
71,
395,
411,
7,
362,
34,
180,
346,
303,
438,
55,
27,
6,
212,
245,
431,
445,
13,
283,
107,
281,
243,
427,
54,
155,
339,
404,
476,
469,
323,
242,
457,
204,
139,
465,
483,
90,
47,
314,
393,
402,
286,
338,
325,
416,
68,
241,
346,
514,
442,
371,
280,
170,
283,
251,
243,
107,
57,
121,
61,
381,
363,
436,
502,
419,
303,
387,
427,
350,
186,
305,
155,
390,
45,
480,
291,
114,
124,
122,
30,
51,
493,
310,
221,
261,
479,
381,
368,
21,
502,
347,
319,
268,
268,
223,
89,
56,
95,
1,
323,
199,
224,
478,
74,
269,
444,
365,
383,
53,
487,
330,
21,
383,
43,
242,
129,
7,
109,
415,
460,
13,
247,
182,
281,
0,
405,
371, };

            a = rotateArray(a, k);

            IList<int> results = new List<int>();

            foreach (var query in queries)
                results.Add(a[query]);

            results.ToArray();

            foreach (var result in results)
                Console.WriteLine(result);
        }

        static int[] rotateArray(int[] array, int countRotates)
        {
            int[] rotatedArray = new int[array.Length];

            for (int position = 0; position < array.Length; position++)
            {
                int newPositionOfValue = position + countRotates;

                if (newPositionOfValue > array.Length - 1)
                    newPositionOfValue = newPositionOfValue % array.Length;

                rotatedArray[newPositionOfValue] = array[position];
            }

            return rotatedArray;



            for (int counter = 0; counter < countRotates; counter++)
            {
                int lastValue = array.Last();

                for (int position = array.Length - 1; position > 0; position--)
                {
                    array[position] = array[position - 1];
                }

                array[0] = lastValue;
            }
        }

        public static void getTotalX()
        {
            List<int> a = new List<int>() { 2, 4 };
            List<int> b = new List<int>() { 16, 32, 96 };

            int result = 0;

            for (var potencialValor = 1; potencialValor <= 100; potencialValor++)
            {
                if (a.Count(value => potencialValor % value == 0) != a.Count())
                    continue;

                if (b.Count(value => value % potencialValor == 0) != b.Count())
                    continue;

                result++;
            }

            Console.WriteLine(result);
        }

        static void countApplesAndOranges()
        {
            int s = 7;
            int t = 10;
            int a = 4;
            int b = 12;
            int[] apples = new int[] { 2, 3, -4 };
            int[] oranges = new int[] { 3, -2, -4 };

            int countApples = countFruits(s, t, a, apples);
            int countOranges = countFruits(s, t, b, oranges);

        }

        static int countFruits(int firstLimit, int lastLimit, int position, int[] fruits)
        {
            int result = 0;

            foreach (var fruit in fruits)
            {
                int newPosition = position + fruit;

                if (newPosition >= firstLimit && newPosition <= lastLimit)
                    result++;
            }

            return result;
        }

        static int surfaceArea(int[][] area)
        {
            var total = 0;

            for (var row = 0; row < area.Length; row++)
            {
                for (var column = 0; column < area[row].Length; column++)
                {
                    int cubeArea = area[row][column] * 4 + 2;

                    if (column >= 1)
                        total -= Math.Min(area[row][column - 1], area[row][column]) * 2;

                    if (row >= 1)
                        total -= Math.Min(area[row - 1][column], area[row][column]) * 2;

                    total += cubeArea;
                }
            }

            return total;
        }

        static void encryption()
        {
            //string s = "if man was meant to stay on the ground god would have given us roots";
            string s = "iffactsdontfittotheorychangethefacts";

            string phraseWithoutSpaces = s.Replace(" ", "");

            int columnsCount = (int)Math.Sqrt(phraseWithoutSpaces.Length);

            if (columnsCount * columnsCount != phraseWithoutSpaces.Length)
                columnsCount++;

            IList<string> rows = new List<string>();

            while (phraseWithoutSpaces.Length > 0)
            {
                int countToCut = columnsCount < phraseWithoutSpaces.Length ? columnsCount : phraseWithoutSpaces.Length;
                string rowValue = phraseWithoutSpaces.Substring(0, countToCut);
                rows.Add(rowValue);
                phraseWithoutSpaces = phraseWithoutSpaces.Remove(0, countToCut);
            }

            IList<string> results = new List<string>();

            for (int contador = 0; contador < columnsCount; contador++)
            {
                string rowValue = String.Empty;

                foreach (var row in rows)
                {
                    if (row.Length <= contador)
                        continue;

                    rowValue += row[contador];
                }

                results.Add(rowValue);
            }

            string result = string.Join(" ", results);
            Console.WriteLine(result);
        }

        static void extraLongFactorials()
        {
            int n = 25;

            BigInteger result = n;

            for (int contador = n - 1; contador > 1; contador--)
            {
                result *= contador;
            }

            Console.WriteLine(result);
        }

        static void saveThePrisoner()
        {
            int quantidadePrisioneiros = 3;
            int quantidadeDoces = 7;
            int ultimoPrisioneiroAComer = 3;

            int quantidadeInteracoes = quantidadeDoces > quantidadePrisioneiros ? quantidadeDoces % quantidadePrisioneiros : quantidadeDoces;

            for (var contador = 0; contador < quantidadeInteracoes - 1; contador++)
                ultimoPrisioneiroAComer = ultimoPrisioneiroAComer == quantidadePrisioneiros ? 0 : ultimoPrisioneiroAComer + 1;

            Console.WriteLine(ultimoPrisioneiroAComer);
        }

        static void viralAdvertising()
        {
            int result = calculeCumulativeLikesDay(5, 3);

            Console.WriteLine(result);
        }

        static int calculeCumulativeLikesDay(int peoplesCount, int dayNumber, int previousCumulativeLikes = 0, int currentDay = 1)
        {
            int countLikes = peoplesCount / 2;

            if (dayNumber == currentDay)
                return countLikes + previousCumulativeLikes;

            return calculeCumulativeLikesDay(countLikes * 3, dayNumber, countLikes + previousCumulativeLikes, currentDay + 1);
        }

        static List<int> processarOperacoes()
        {
            List<string> queries = new List<string>()
            {
                "1 286789035",
                "1 255653921",
                "1 274310529",
                "1 494521015",
                "3",
                "2 255653921",
                "2 286789035",
                "3",
                "1 236295092",
                "1 254828111",
                "2 254828111",
                "1 465995753",
                "1 85886315",
                "1 7959587",
                "1 20842598",
                "2 7959587",
                "3",
                "1 -51159108",
                "3",
                "2 -51159108",
                "3",
                "1 789534713"
            };

            List<int> heap = new List<int>();
            List<int> results = new List<int>();

            queries.ForEach(query =>
            {
                string[] queryValues = query.Split(' ');

                switch (queryValues[0])
                {
                    case "1":
                        {
                            int valor = Int32.Parse(queryValues[1]);

                            if (heap.Count() > 0 && heap[0] >= valor)
                                heap.Insert(0, valor);
                            else
                                heap.Add(valor);
                        }
                        break;
                    case "2":
                        {
                            int valorARemover = Int32.Parse(queryValues[1]);
                            bool precisaOrdenarLista = valorARemover == heap[0];

                            heap.Remove(valorARemover);

                            if (precisaOrdenarLista)
                                heap.Sort();
                        }
                        break;
                    default:
                        results.Add(heap[0]);
                        break;
                }
            });

            return results;
        }

        static int hurdleRace(int k, int[] height)
        {
            int quantidadePulosNecessarios = height.Max() - k;
            return quantidadePulosNecessarios > 0 ? quantidadePulosNecessarios : 0;
        }

        static string angryProfessor(int k, int[] a)
        {
            return a.Count(value => value <= 0) >= k ? "NO" : "YES";

        }

        static void getMoneySpent()
        {
            int maiorValorPermitido = 60;

            int[] keyboards = new int[] { 40, 50, 60 };
            int[] drives = new int[] { 5, 8, 12 };

            int maiorSomaEncontrada = -1;

            foreach (var keyboard in keyboards)
                foreach (var drive in drives)
                {
                    int possivelSoma = keyboard + drive;
                    if (possivelSoma <= maiorValorPermitido && possivelSoma > maiorSomaEncontrada)
                        maiorSomaEncontrada = possivelSoma;
                }

            Console.WriteLine(maiorSomaEncontrada);
        }

        public static void countingValleys()
        {
            string path = "UDDDUDUU";

            const char DOWN = 'D';

            int quantidadeVales = 0;
            int altitude = 0;

            for (var posicao = 0; posicao < path.Length; posicao++)
            {
                char passo = path[posicao];

                altitude = passo == DOWN ? altitude - 1 : altitude + 1;

                if (passo == DOWN && altitude == -1)
                    quantidadeVales++;
            }

            Console.WriteLine(quantidadeVales);
        }

        public static void pickingNumbers()
        {
            List<int> a = new List<int>() { 1, 1, 2, 2, 4, 4, 5, 5, 5 };

            a.Sort();

            var itemsDistintos = a.Distinct().ToList();
            List<int> tamanhosSubsArrays = new List<int>();

            foreach (var item in itemsDistintos)
                tamanhosSubsArrays.Add(retornarTamanhoSubArrayComDiferencaMaximaDeUm(item, a));

            Console.WriteLine(tamanhosSubsArrays.Max());
        }

        static int retornarTamanhoSubArrayComDiferencaMaximaDeUm(int valorMinimo, List<int> lista)
        {
            const int DIFERENCA_MINIMA = 1;

            int tamanhoSubArray = 0;

            int indiceInicial = lista.FindIndex(value => valorMinimo == value);

            for (int posicao = indiceInicial; posicao < lista.Count(); posicao++)
            {
                int valorDaPosicao = lista[posicao];

                if (valorDaPosicao - valorMinimo > DIFERENCA_MINIMA)
                    break;

                tamanhoSubArray++;
            }

            return tamanhoSubArray;
        }



        static void catAndMouse()
        {
            int x = 1;
            int y = 3;
            int z = 2;

            int distanciaGatoA = calcularDistancia(x, z);
            int distanciaGatoB = calcularDistancia(y, z);

            if (distanciaGatoA == distanciaGatoB)
            {
                Console.WriteLine("Mouse C");
                return;
            }

            Console.WriteLine(distanciaGatoA < distanciaGatoB ? "Cat A" : "Cat B");
        }

        static int calcularDistancia(int primeiroValor, int segundoValor)
        {
            return primeiroValor > segundoValor ? primeiroValor - segundoValor : segundoValor - primeiroValor;
        }



        static void timeConversion()
        {
            string tempoParaConverter = "12:01:00AM";
            var tempoQuebrado = tempoParaConverter.Split(':');
            bool ehAM = tempoParaConverter.EndsWith("AM");

            int horas = Int32.Parse(tempoQuebrado[0]);
            string minutos = tempoQuebrado[1];
            string segundos = tempoQuebrado[2].Substring(0, 2);

            if (ehAM && horas == 12)
                horas = 0;
            else if (!ehAM && horas != 12)
                horas += 12;

            Console.WriteLine($"{horas.ToString().PadLeft(2, '0')}:{minutos}:{segundos}");
        }

        public static int birthdayCakeCandles(List<int> candles)
        {
            int maiorVela = candles.Max();
            return candles.Count(vela => vela == maiorVela);
        }


        static void miniMaxSum()
        {
            long[] arr = new long[] { 256741038, 623958417, 467905213, 714532089, 938071625 };

            List<long> somas = new List<long>();

            for (var posicao = 0; posicao < arr.Count(); posicao++)
                somas.Add(arr.Sum() - arr[posicao]);

            Console.WriteLine($"{somas.Min()} {somas.Max()}");
        }

        static void staircase(int n)
        {
            int VALOR_DIVISAO_DIAGONAL_SECUNDARIA = n - 1;

            for (var contadorLinha = 0; contadorLinha < n; contadorLinha++)
            {
                string linha = String.Empty;
                for (var contadorColuna = 0; contadorColuna < n; contadorColuna++)
                    linha += (contadorLinha + contadorColuna < VALOR_DIVISAO_DIAGONAL_SECUNDARIA) ? ' ' : '#';

                Console.WriteLine(linha);
            }
        }

        static void plusMinus()
        {
            int[] arr = new int[] { 1, 1, 0, -1, -1 };

            double quantidadeValoresPositivos = arr.Count(value => value > 0);
            double quantidadeValoresNegativos = arr.Count(value => value < 0);
            double quantidadeValoresZerados = arr.Count() - (quantidadeValoresPositivos + quantidadeValoresNegativos);

            int quantidadeRegistros = arr.Count();

            double resultadoDivisaoParaValoresPositivos = quantidadeValoresPositivos / quantidadeRegistros;
            double resultadoDivisaoParaValoresNegativos = quantidadeValoresNegativos / quantidadeRegistros;
            double resultadoDivisaoParaValoresZerados = quantidadeValoresZerados / quantidadeRegistros;

            Console.WriteLine(resultadoDivisaoParaValoresPositivos.ToString("N6"));
            Console.WriteLine(resultadoDivisaoParaValoresNegativos.ToString("N6"));
            Console.WriteLine(resultadoDivisaoParaValoresZerados.ToString("N6"));

        }

        public static int diagonalDifference(List<List<int>> arr)
        {
            int totalDiagonalPrimaria = 0;
            int totalDiagonalSecundaria = 0;

            int referenciaIndiceDiagonalSecundaria = arr.Count() - 1;

            for (var posicaoLinha = 0; posicaoLinha < arr.Count(); posicaoLinha++)
            {
                List<int> colunas = arr[posicaoLinha];

                for (var posicaoColuna = 0; posicaoColuna < colunas.Count(); posicaoColuna++)
                {
                    if (posicaoLinha == posicaoColuna)
                        totalDiagonalPrimaria += colunas[posicaoColuna];

                    if (posicaoLinha + posicaoColuna == referenciaIndiceDiagonalSecundaria)
                        totalDiagonalSecundaria += colunas[posicaoColuna];
                }
            }

            return Math.Abs(totalDiagonalPrimaria - totalDiagonalSecundaria);
        }

        static long aVeryBigSum(long[] ar)
        {
            return ar.Sum();
        }

        static List<int> compareTriplets(List<int> a, List<int> b)
        {
            int pontosAlice = 0;
            int pontosBob = 0;

            for (var posicao = 0; posicao < a.Count() - 1; posicao++)
            {
                if (a[posicao] == b[posicao])
                    continue;

                if (a[posicao] > b[posicao])
                {
                    pontosAlice++;
                    continue;
                }

                pontosBob++;
            }

            return new List<int>() { pontosAlice, pontosBob };
        }

        static int simpleArraySum(int[] ar)
        {
            return ar.Sum();

        }

        static int powerSum(int x, int n, int somaDoIncremento = 1)
        {
            var expoente = x - Math.Pow(somaDoIncremento, n);

            if (expoente > 0)
                return powerSum(Convert.ToInt32(expoente), n, somaDoIncremento + 1) + powerSum(x, n, somaDoIncremento + 1);

            return expoente < 0 ? 0 : 1;
        }

        static void calcularLucro()
        {
            List<int> valores = new List<int>() { 1, 2, 100 };

            Console.WriteLine(stockmax(valores));
        }

        public static long stockmax(List<int> prices)
        {
            long lucroTotal = 0;
            int indiceMaiorPreco = prices.Count() - 1;

            for (var posicao = prices.Count() - 2; posicao > 0; posicao--)
                if (prices[indiceMaiorPreco] >= prices[posicao])
                {
                    lucroTotal += prices[indiceMaiorPreco] - prices[posicao];
                }
                else
                    indiceMaiorPreco = posicao;

            return lucroTotal;
        }


        static void removerArvoreGrafo()
        {
            var from = new List<int> {
2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30  };
            var to = new List<int> {
1, 2, 3, 2, 4, 4, 1, 5, 4, 4, 8, 2, 2, 8, 10, 1, 17, 18, 4, 15, 20, 2, 12, 21, 17, 5, 27, 4, 25};

            var mapeamentoNos = mapearNos(from, to);
            var quantidadeArestasRemovidas = retornaQuantidadeArestasRemovidas(mapeamentoNos);
            Console.WriteLine(quantidadeArestasRemovidas);

        }

        static IDictionary<int, IList<int>> mapearNos(IList<int> from, IList<int> to)
        {
            var mapeamentos = new Dictionary<int, IList<int>>();

            for (var posicao = 0; posicao < to.Count; posicao++)
            {
                int chave = to[posicao];

                IList<int> nos;

                if (!mapeamentos.TryGetValue(chave, out nos))
                    nos = new List<int>();

                nos.Add(from[posicao]);

                mapeamentos[chave] = nos;
            }

            return mapeamentos;
        }

        static int retornaQuantidadeArestasRemovidas(IDictionary<int, IList<int>> mapeamentos)
        {
            int quantidadeTotalArestasRemovidas = 0;

            IList<int> chaves = mapeamentos.Keys.ToList();

            foreach (var chave in chaves)
            {
                if (!mapeamentos.ContainsKey(chave))
                    continue;

                quantidadeTotalArestasRemovidas += removerArestas(mapeamentos, mapeamentos[chave], chave);
            }

            return quantidadeTotalArestasRemovidas;
        }

        static int removerArestas(IDictionary<int, IList<int>> mapeamentos, IList<int> nos, int pai)
        {
            int quantidadeArestasRemovidas = 0;

            foreach (var chave in nos)
            {
                IList<int> nosDoNo;

                if (!mapeamentos.TryGetValue(chave, out nosDoNo))
                    continue;

                if (nosDoNo.Count > 1)
                    continue;

                removerNoDaArvoreComSeusFilhos(chave, mapeamentos);
                quantidadeArestasRemovidas++;

                if (pai == 1)
                    continue;

                if (!mapeamentos.TryGetValue(pai, out nosDoNo))
                    continue;

                if (nosDoNo.Count == 0)
                    continue;

                removerNoDaArvoreComSeusFilhos(nosDoNo.First(), mapeamentos);
                quantidadeArestasRemovidas++;
            }

            return quantidadeArestasRemovidas;
        }

        static void removerNoDaArvoreComSeusFilhos(int chave, IDictionary<int, IList<int>> mapeamentos)
        {
            int pai = chave;
            IList<int> nos;

            while (mapeamentos.TryGetValue(pai, out nos))
            {
                foreach (var item in nos)
                    removerNoDaArvoreComSeusFilhos(item, mapeamentos);

                mapeamentos.Remove(pai);
            }
        }

        static void verificarDiferencaListas()
        {
            int[] array1 = new int[] { 203, 204, 205, 206, 207, 208, 203, 204, 205, 206 };
            int[] array2 = new int[] { 203, 204, 204, 205, 206, 207, 205, 208, 203, 206, 205, 206, 204 };

            int[] resultado = missingNumbers(array1, array2);

            Console.WriteLine(String.Join(' ', resultado));
        }

        static int[] missingNumbers(int[] arr, int[] brr)
        {
            IDictionary<int, int> countArr = new Dictionary<int, int>();
            IDictionary<int, int> countBrr = new Dictionary<int, int>();

            foreach (var value in arr)
                countArr[value] = countArr.ContainsKey(value) ? countArr[value] + 1 : 1;

            foreach (var value in brr)
                countBrr[value] = countBrr.ContainsKey(value) ? countBrr[value] + 1 : 1;

            var resultado = new List<int>();

            foreach (KeyValuePair<int, int> value in countBrr)
            {
                var recorrenciasNoPrimeiroArray = 0;

                if (!countArr.TryGetValue(value.Key, out recorrenciasNoPrimeiroArray))
                {
                    resultado.Add(value.Key);
                    continue;
                }

                if (value.Value == recorrenciasNoPrimeiroArray)
                    continue;

                resultado.Add(value.Key);
            }

            resultado.Sort();

            return resultado.ToArray();
        }

        static void calcularValorSorvete()
        {
            int[] resultado = icecreamParlor(3, new int[] { 1, 2 });
            Console.WriteLine(String.Join(' ', resultado));
        }

        static int[] icecreamParlor(int m, int[] arr)
        {
            int[] resultado = new int[2];

            for (var posicaoI = 0; posicaoI < arr.Length; posicaoI++)
                for (var posicaoJ = posicaoI + 1; posicaoJ < arr.Length; posicaoJ++)
                    if (arr[posicaoI] + arr[posicaoJ] == m)
                    {
                        resultado[0] = posicaoI + 1;
                        resultado[1] = posicaoJ + 1;
                        break;
                    }

            return resultado;
        }

        static void ordenarArray()
        {
            int[] dados = new int[]{
                1, 4, 3, 5, 6, 2
            };

            for (var posicao = 1; posicao < dados.Length; posicao++)
                insertionSort2(posicao, dados);
        }

        static void insertionSort1(int n, int[] arr)
        {
            var ultimoValor = arr.Last();
            var posicaoAtual = arr.Length - 1;
            var valorPosicaoAnterior = arr[posicaoAtual - 1];

            do
            {
                arr[posicaoAtual] = arr[posicaoAtual - 1];
                posicaoAtual--;
                valorPosicaoAnterior = posicaoAtual > 0 ? arr[posicaoAtual - 1] : arr[posicaoAtual];
            } while (valorPosicaoAnterior > ultimoValor && posicaoAtual > 0);

            arr[posicaoAtual] = ultimoValor;

            String.Join(' ', arr);
        }

        static void insertionSort2(int indice, int[] arr)
        {
            var ultimoValor = arr[indice];
            var posicaoAtual = indice;
            var valorPosicaoAnterior = arr[posicaoAtual - 1];

            while (valorPosicaoAnterior > ultimoValor && posicaoAtual > 0)
            {
                var temp = arr[posicaoAtual];
                arr[posicaoAtual] = arr[posicaoAtual - 1];
                arr[posicaoAtual - 1] = temp;
                posicaoAtual--;
                valorPosicaoAnterior = posicaoAtual > 0 ? arr[posicaoAtual - 1] : arr[posicaoAtual];
            }
            Console.WriteLine(String.Join(' ', arr));
        }

        static void verificarDiferencaMensagemSOS()
        {
            var stringPassada = "SOSSPSSQSSOR";
            var stringEsperada = gerarStringEsperada(stringPassada.Length);

            var quantidadeLetrasIncorretas = 0;

            for (var posicao = 0; posicao <= stringEsperada.Length - 1; posicao++)
                if (stringEsperada[posicao] != stringPassada[posicao])
                    quantidadeLetrasIncorretas++;

            Console.WriteLine(quantidadeLetrasIncorretas);
        }

        static string gerarStringEsperada(int quantidade)
        {
            const string SOS = "SOS";

            var stringEsperada = String.Empty;

            for (var contador = 0; contador < quantidade / SOS.Length; contador++)
                stringEsperada += SOS;

            return stringEsperada;
        }

        static void calcularQuantidadeDeLetrasQueFaltamEmSenha()
        {
            const int QUANTIDADE_MINIMA = 6;

            var quantidadeLetras = 0;
            string senha = "AUzs-nV";

            if (!Regex.IsMatch(senha, "[a-z]"))
                quantidadeLetras++;

            if (!Regex.IsMatch(senha, "[A-Z]"))
                quantidadeLetras++;

            if (!Regex.IsMatch(senha, "[0-9]"))
                quantidadeLetras++;

            if (!Regex.IsMatch(senha, "[!@#$%^&*()_+]"))
                quantidadeLetras++;

            int quantidadeLetrasQueFaltamParaTamanhoMinimo = QUANTIDADE_MINIMA - senha.Length;

            if (quantidadeLetrasQueFaltamParaTamanhoMinimo > quantidadeLetras)
                Console.WriteLine(quantidadeLetrasQueFaltamParaTamanhoMinimo);

            Console.WriteLine(quantidadeLetras);



            Console.WriteLine(quantidadeLetras);
        }

        static void contarLetrasMaiuculas()
        {
            var frase = "saveChangesInTheEditor";

            var resultado = Regex.Match(frase, "[A-Z]");

            var quantidadeAcertos = 0;

            while (resultado.Success)
            {
                quantidadeAcertos++;
                resultado = resultado.NextMatch();
            }

            Console.WriteLine(quantidadeAcertos);
            Console.WriteLine(resultado.Groups.Count);
        }

        static void testarQueueCaseira()
        {
            MyQueue<int> myQueue = new MyQueue<int>();

            IList<string> operations = new List<string>();
            operations.Add("1 42");
            operations.Add("2");
            operations.Add("1 14");
            operations.Add("3");
            operations.Add("1 28");
            operations.Add("3");
            operations.Add("1 60");
            operations.Add("1 78");
            operations.Add("2");
            operations.Add("2");

            foreach (var operation in operations)
            {
                var valores = operation.Split(' ');

                switch (valores[0])
                {
                    case "1":
                        myQueue.enqueue(Int32.Parse(valores[1]));
                        break;
                    case "2":
                        myQueue.dequeue();
                        break;
                    case "3":
                        Console.WriteLine(myQueue.peek());
                        break;
                }

            }
        }

        static void retornarValoresPilha()
        {
            List<string> ops = new List<string>();

            ops.Add("1 1");
            ops.Add("1 44");
            ops.Add("3");
            ops.Add("3");
            ops.Add("2");
            ops.Add("3");
            ops.Add("3");
            ops.Add("1 3");
            ops.Add("1 37");
            ops.Add("2");
            ops.Add("3");
            ops.Add("1 29");
            ops.Add("3");
            ops.Add("1 73");
            ops.Add("1 51");
            ops.Add("3");
            ops.Add("3");
            ops.Add("3");
            ops.Add("1 70");
            ops.Add("3");
            ops.Add("1 8");
            ops.Add("2");
            ops.Add("1 49");
            ops.Add("1 56");
            ops.Add("1 81");
            ops.Add("2");
            ops.Add("1 59");
            ops.Add("1 44");
            ops.Add("2");
            ops.Add("3");
            ops.Add("3");
            ops.Add("2");
            ops.Add("3");
            ops.Add("3");
            ops.Add("1 4");
            ops.Add("3");
            ops.Add("1 89");
            ops.Add("2");
            ops.Add("1 37");
            ops.Add("1 50");
            ops.Add("1 64");
            ops.Add("2");
            ops.Add("1 49");
            ops.Add("1 35");
            ops.Add("1 85");
            ops.Add("3");
            ops.Add("1 41");
            ops.Add("2");
            ops.Add("3");
            ops.Add("3");
            ops.Add("1 86");
            ops.Add("3");
            ops.Add("1 60");
            ops.Add("1 8");
            ops.Add("3");
            ops.Add("1 100");
            ops.Add("3");
            ops.Add("1 83");
            ops.Add("3");
            ops.Add("1 47");
            ops.Add("2");
            ops.Add("1 78");
            ops.Add("2");
            ops.Add("1 55");
            ops.Add("1 97");
            ops.Add("2");
            ops.Add("3");
            ops.Add("1 40");

            List<int> res = getMax(ops);

            Console.WriteLine(String.Join("\n", res));
        }

        static List<int> getMax(List<string> operations)
        {
            var resultados = new List<int>();
            var calculos = new Stack<int>();

            var maiorValor = Int32.MinValue;

            foreach (var operacao in operations)
            {
                var valoresOperacao = operacao.Split(' ');

                switch (valoresOperacao[0])
                {
                    case "1":
                        {
                            var valorConvertido = Int32.Parse(valoresOperacao[1]);
                            if (maiorValor < valorConvertido)
                                maiorValor = valorConvertido;
                            calculos.Push(maiorValor);
                        }
                        break;
                    case "2":
                        {
                            var valorRemovido = calculos.Pop();
                            maiorValor = calculos.Count == 0 ? Int32.MinValue : calculos.Peek();
                        }
                        break;
                    default:
                        resultados.Add(calculos.Peek());
                        break;
                }
            }

            return resultados;
        }

        static void verificarTamanhoPilha()
        {
            List<int> h1 = "3 2 1 1 1".Split(' ').ToList().Select(h1Temp => Convert.ToInt32(h1Temp)).ToList();
            List<int> h2 = "4 3 2".Split(' ').ToList().Select(h2Temp => Convert.ToInt32(h2Temp)).ToList();
            List<int> h3 = "1 1 4 1".Split(' ').ToList().Select(h3Temp => Convert.ToInt32(h3Temp)).ToList();

          //  Console.WriteLine(retornarValorComum(h1, h2, h3));
        }

        static int equalStacks()
        {
            List<int> h1 = new List<int>() { 3, 2, 1, 1, 1 };
            List<int> h2 = new List<int>() { 4, 3, 2 };
            List<int> h3 = new List<int>() { 1, 1, 4, 1 };

            int[] somaPrimeiraPilhaRevertida = retornarArraySomado(h1);
            int[] somaSegundaPilhaRevertida = retornarArraySomado(h2);
            int[] somaTerceiraPilhaRevertida = retornarArraySomado(h3);

            int[] menorArray = retornarMenorArray(somaPrimeiraPilhaRevertida, somaSegundaPilhaRevertida, somaTerceiraPilhaRevertida);

            int[] primeiroArrayParaPesquisar = menorArray == somaPrimeiraPilhaRevertida ? somaSegundaPilhaRevertida : somaPrimeiraPilhaRevertida;
            int[] segundoArrayParaPesquisar = menorArray == somaSegundaPilhaRevertida ? somaTerceiraPilhaRevertida : somaSegundaPilhaRevertida;

            for (int posicao = menorArray.Length - 1; posicao >= 0; posicao--)
            {
                int value = menorArray[posicao];

                if (primeiroArrayParaPesquisar.Contains(value) && segundoArrayParaPesquisar.Contains(value))
                    return value;
            }

            return 0;
        }

        static int[] retornarArraySomado(IList<int> arrayParaSomar)
        {
            int[] somas = new int[arrayParaSomar.Count()];
            int somasCounter = 0;

            for (var contador = arrayParaSomar.Count() - 1; contador >= 0; contador--)
            {
                somas[somasCounter] =  arrayParaSomar[contador] + (somasCounter == 0 ? 0 : somas[somasCounter - 1]);
                somasCounter++;
            }

            return somas;
        }

        static int[] retornarMenorArray(int[] h1, int[] h2, int[] h3)
        {
            int[] menorArray = h1;

            if (menorArray.Count() > h2.Count())
                menorArray = h2;

            if (menorArray.Count() > h3.Count())
                menorArray = h3;

            return menorArray;
        }

        static void inseriPrimeiroRegistroListaLigada()
        {
            SinglyLinkedList llist = new SinglyLinkedList();

            for (int i = 0; i < 10; i++)
            {
                int llistItem = new Random().Next();
                SinglyLinkedListNode llist_head = insertNodeAtHead(llist.head, llistItem);
                llist.head = llist_head;
            }

            SinglyLinkedListNode node = llist.head;

            while (node != null)
            {
                Console.WriteLine(node.data + " ");
                node = node.next;
            }

        }

        static SinglyLinkedListNode insertNodeAtHead(SinglyLinkedListNode llist, int data)
        {
            var node = new SinglyLinkedListNode(data);

            /*  if (llist != null)           
                  node.next = llist;*/

            return node;
        }

        static void pesquisarItens()
        {
            string[] strings = new string[]
            {
                "3",
                "def",
                "de",
                "fgh",
                "3"
            };


            string[] queries = new string[]
            {
                "de",
                "lmn",
                "fgh"
            };

            int quantidadeItensPesquisa = queries.Length;
            int[] quantidades = new int[quantidadeItensPesquisa];

            for (int contador = 0; contador <= quantidadeItensPesquisa - 1; contador++)
                quantidades[contador] = strings.Where(value => value.Equals(queries[contador])).Count();

            Console.WriteLine(string.Join(", ", quantidades));
        }

        static void rotacionarArray()
        {
            List<int> valores = new List<int>()
            {
                1, 2, 3, 4, 5
            };

            Console.WriteLine(String.Join(", ", rotateLeft(2, valores)));
        }

        public static List<int> rotateLeft(int d, List<int> arr)
        {
            int quantidadeExecucoes = 0;

            while (quantidadeExecucoes < d)
            {
                quantidadeExecucoes++;

                int primeiroValor = arr[0];

                arr.RemoveAt(0);
                arr.Add(primeiroValor);
            }

            return arr;
        }

        static void calcularMaiorSomaAmpulheta()
        {
            int[][] arr = new int[6][];

            arr[0] = Array.ConvertAll("-1 -1 0 -9 -2 -2".Split(' '), arrTemp => Convert.ToInt32(arrTemp));
            arr[1] = Array.ConvertAll("-2 -1 -6 -8 -2 -5".Split(' '), arrTemp => Convert.ToInt32(arrTemp));
            arr[2] = Array.ConvertAll("-1 -1 -1 -2 -3 -4".Split(' '), arrTemp => Convert.ToInt32(arrTemp));
            arr[3] = Array.ConvertAll("-1 -9 -2 -4 -4 -5".Split(' '), arrTemp => Convert.ToInt32(arrTemp));
            arr[4] = Array.ConvertAll("-7 -3 -3 -2 -9 -9".Split(' '), arrTemp => Convert.ToInt32(arrTemp));
            arr[5] = Array.ConvertAll("-1 -3 -1 -2 -4 -5".Split(' '), arrTemp => Convert.ToInt32(arrTemp));

            Console.WriteLine(hourglassSum(arr));
        }

        static int hourglassSum(int[][] arr)
        {
            int maiorValor = Int32.MinValue;

            for (int linha = 0; linha <= 3; linha++)
                for (int coluna = 0; coluna <= 3; coluna++)
                {
                    int valorAmpulheta = 0;
                    bool ehSegundaLinha = true;

                    for (int linhaInterna = linha; linhaInterna <= linha + 2; linhaInterna++)
                    {
                        ehSegundaLinha = !ehSegundaLinha;

                        bool ehColunaValidaParaSegundaLinha = true;
                        for (int colunaInterna = coluna; colunaInterna <= coluna + 2; colunaInterna++)
                        {
                            ehColunaValidaParaSegundaLinha = !ehColunaValidaParaSegundaLinha;

                            if (ehSegundaLinha && !ehColunaValidaParaSegundaLinha)
                                continue;

                            valorAmpulheta += arr[linhaInterna][colunaInterna];
                        }
                    }

                    if (valorAmpulheta > maiorValor)
                        maiorValor = valorAmpulheta;
                }

            return maiorValor;
        }
    }
}
