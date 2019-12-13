using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3_Course_
{
    
    public partial class Form1 : Form
    {
        int numOfHeads = 0;
        public Form1()
        {
            InitializeComponent();
        }

        public class Net
        {
            int[,] Matrix;
            public Net(int numOfVertexes, int[,] graph)
            {
                Matrix = new int[numOfVertexes +2, numOfVertexes +2];
                for (int i = 0; i < numOfVertexes; i++)
                    for (int j = 0; j < numOfVertexes; j++)
                        Matrix[i, j] = graph[i, j];

                for (int i = 0; i < numOfVertexes/2; i++)
                {
                    Matrix[numOfVertexes, i] = 1;
                    Matrix[i, numOfVertexes] = 1;
                    Matrix[numOfVertexes + 1, i] = 0;
                    Matrix[i, numOfVertexes + 1] = 0;
                }
                for(int i=numOfVertexes/2;i<numOfVertexes;i++)
                {
                    Matrix[numOfVertexes, i] = 0;
                    Matrix[i, numOfVertexes] = 0;
                    Matrix[numOfVertexes + 1, i] = 1;
                    Matrix[i, numOfVertexes + 1] = 1;
                }
                Matrix[numOfVertexes, numOfVertexes] = 0;
                Matrix[numOfVertexes + 1, numOfVertexes + 1] = 0;
                Matrix[numOfVertexes, numOfVertexes + 1] = 0;
                Matrix[numOfVertexes + 1, numOfVertexes] = 0;
            }
            public int[,] getNet() { return Matrix; }
        }

        public class FordFalkerson
        {
            int V;
            public FordFalkerson(int Vertex)
            {
                V = Vertex;
                /*Net = new int[BiVertex * 2, BiVertex * 2];
                for (int i = 0; i < BiVertex - 1; i++)
                    for (int j = 0; j < BiVertex - 1; j++)
                        if (matrix[i, j] == 0)
                        {
                            Net[i, j + BiVertex - 1] = 0;
                            Net[j + BiVertex - 1, i] = 0;
                        }
                        else
                        {
                            Net[i, j + BiVertex - 1] = 1;
                            Net[j + BiVertex - 1, i] = 1;
                        }
                for (int i = 0; i < BiVertex * 2; i++)
                {
                    Net[i, i] = 0;
                    if (i > 0 && i % 2 == 1)
                    {
                        Net[i, i - 1] = 0;
                        Net[i - 1, i] = 0;
                    }
                }
                for (int i = 0; i < BiVertex - 1; i++)
                {
                    Net[BiVertex * 2 - 1, i] = 1;
                    Net[i, BiVertex * 2 - 1] = 1;
                    Net[BiVertex * 2 - 2, i] = 0;
                    Net[i, BiVertex * 2 - 2] = 0;
                }
                for(int i=BiVertex-1;i<BiVertex*2-2;i++)
                {
                    Net[BiVertex * 2 - 1, i] = 0;
                    Net[i, BiVertex * 2 - 1] = 0;
                    Net[BiVertex * 2 - 2, i] = 1;
                    Net[i, BiVertex * 2 - 2] = 1;
                }*/
            }

            /* Returns true if there is a path 
    from source 's' to sink 't' in residual 
    graph. Also fills parent[] to store the 
    path */
            bool bfs(int[,] rGraph, int s, int t, int[] parent)
            {
                // Create a visited array and mark  
                // all vertices as not visited 
                bool[] visited = new bool[V];
                for (int i = 0; i < V; ++i)
                    visited[i] = false;

                // Create a queue, enqueue source vertex and mark 
                // source vertex as visited 
                List<int> queue = new List<int>();
                queue.Add(s);
                visited[s] = true;
                parent[s] = -1;

                // Standard BFS Loop 
                while (queue.Count != 0)
                {
                    int u = queue[0];
                    queue.RemoveAt(0);

                    for (int v = 0; v < V; v++)
                    {
                        if (visited[v] == false && rGraph[u, v] > 0)
                        {
                            queue.Add(v);
                            parent[v] = u;
                            visited[v] = true;
                        }
                    }
                }

                // If we reached sink in BFS  
                // starting from source, then 
                // return true, else false 
                return (visited[t] == true);
            }

            // Returns tne maximum flow 
            // from s to t in the given graph 
           
            public int[] fordFulkerson(int[,] graph, int s, int t)
            {
                int u, v;

                // Create a residual graph and fill  
                // the residual graph with given  
                // capacities in the original graph as 
                // residual capacities in residual graph 

                // Residual graph where rGraph[i,j]  
                // indicates residual capacity of  
                // edge from i to j (if there is an  
                // edge. If rGraph[i,j] is 0, then  
                // there is not) 
                int[,] rGraph = new int[V, V];

                for (u = 0; u < V; u++)
                    for (v = 0; v < V; v++)
                        rGraph[u, v] = graph[u, v];

                // This array is filled by BFS and to store path 
                int[] parent = new int[V];

                int max_flow = 0; // There is no flow initially 

                // Augment the flow while tere is path from source 
                // to sink 
                while (bfs(rGraph, s, t, parent))
                {
                    // Find minimum residual capacity of the edhes 
                    // along the path filled by BFS. Or we can say 
                    // find the maximum flow through the path found. 
                    int path_flow = int.MaxValue;
                    for (v = t; v != s; v = parent[v])
                    {
                        u = parent[v];
                        path_flow = Math.Min(path_flow, rGraph[u, v]);
                    }

                    // update residual capacities of the edges and 
                    // reverse edges along the path 
                    for (v = t; v != s; v = parent[v])
                    {
                        u = parent[v];
                        rGraph[u, v] -= path_flow;
                        rGraph[v, u] += path_flow;
                    }

                    // Add path flow to overall flow 
                    max_flow += path_flow;
                }

                // Return the overall flow 
                return parent;
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Введите количество вершин графа";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int newNumber = int.Parse(this.textBox1.Text);
                if (newNumber  <1)
                    throw new System.ArgumentOutOfRangeException("Invalid vertex number");
                if (numOfHeads <= newNumber)
                {
                    int difference = newNumber - numOfHeads;
                    for (int i = 0; i < difference; i++)
                        this.dataGridView1.Columns.Add("", "");
                    if (difference > 0)
                        this.dataGridView1.Rows.Add(difference);
                    numOfHeads = newNumber;
                }
                else
                {
                    int difference = numOfHeads - newNumber;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (numOfHeads > newNumber)
                        {
                            dataGridView1.Rows.Remove(row);
                        }
                        numOfHeads--;
                    }
                    for (int i = 0; i < difference; i++)
                        dataGridView1.Columns.RemoveAt(i);
                    numOfHeads = newNumber;
                }
            }
            catch(System.ArgumentOutOfRangeException a)
            {
                MessageBox.Show(a.Message);
            }
            catch(System.FormatException a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[,] Matrix = new int[numOfHeads, numOfHeads];
            for (int i = 0; i < numOfHeads; i++)
                for (int j = 0; j < numOfHeads; j++)
                    Matrix[i, j] = Convert.ToInt32(this.dataGridView1[j,i].Value);
            Net net = new Net(numOfHeads, Matrix);    
            FordFalkerson ff = new FordFalkerson(numOfHeads + 2);
           
            int[] answer = new int[(numOfHeads + 1) * 2];
            answer = ff.fordFulkerson(net.getNet(), numOfHeads, numOfHeads + 1);
            label2.Text = "";
            for (int i = 0; i < answer.Length; i++)
            {
                label2.Text += answer[i].ToString() + " ";
            }
        }

    }
}
