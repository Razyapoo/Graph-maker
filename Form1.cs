using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SystAnalys_lr1
{
    public partial class Form1 : Form
    {
        DrawGraph G;
        List<Vrchol> V;
        List<Hrana> E;
        int[,] AMatrix; //matice sousednosti
        int[,] IMatrix; //matice incidence

        int selected1; //spojeni hranou vrcholu
        int selected2;

        public Form1()
        {
            InitializeComponent();
            V = new List<Vrchol>();
            G = new DrawGraph(sheet.Width, sheet.Height);
            E = new List<Hrana>();
            sheet.Image = G.GetBitmap();
        }

        //tlacitko - vyber vrcholu
        private void selectButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = false;
            drawVrcholButton.Enabled = true;
            drawHranaButton.Enabled = true;
            deleteButton.Enabled = true;
            G.clearSheet();
            G.drawALLGraph(V, E);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
        }

        //tlacitko - kresleni vrcholu
        private void drawVrcholButton_Click(object sender, EventArgs e)
        {
            drawVrcholButton.Enabled = false;
            selectButton.Enabled = true;
            drawHranaButton.Enabled = true;
            deleteButton.Enabled = true;
            G.clearSheet();
            G.drawALLGraph(V, E);
            sheet.Image = G.GetBitmap();
        }

        //tlacitko - kresleni hrany
        private void drawHranaButton_Click(object sender, EventArgs e)
        {
            drawHranaButton.Enabled = false;
            selectButton.Enabled = true;
            drawVrcholButton.Enabled = true;
            deleteButton.Enabled = true;
            G.clearSheet();
            G.drawALLGraph(V, E);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
            selected2 = -1;
        }

        //tlacitko - mazani prvku 
        private void deleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            selectButton.Enabled = true;
            drawVrcholButton.Enabled = true;
            drawHranaButton.Enabled = true;
            G.clearSheet();
            G.drawALLGraph(V, E);
            sheet.Image = G.GetBitmap();
        }

        //tlacitko - smazat graf
        private void deleteALLButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = true;
            drawVrcholButton.Enabled = true;
            drawHranaButton.Enabled = true;
            deleteButton.Enabled = true;
            const string message = "Smazat graf?";
            const string caption = "Smazat";
            var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (MBSave == DialogResult.Yes)
            {
                V.Clear();
                E.Clear();
                G.clearSheet();
                listBoxMatrix.Items.Clear();
                listBoxHrany.Items.Clear();
                sheet.Image = G.GetBitmap();
            }
        }

        //Matice sousednosti
        private void buttonAdj_Click(object sender, EventArgs e)
        {
            createAdjAndOut();
        }

        //napsat do tabulky vrcholy a smer hrany mezi nimi
        private void listBoxHrany_add(int j)
        {
            string sOut = "";
            if (E[j].v4)
                sOut = (E[j].v1 + 1) + " ---> " + (E[j].v2 + 1) + " (" + E[j].v3 + ")";
            else
                sOut = (E[j].v1 + 1) + " <-> " + (E[j].v2 + 1) + " (" + E[j].v3 + ")";
            listBoxHrany.Items.Add(sOut);
        }
        private void listBoxHrany_hrane()
        {
            if (listBoxHrany.Items.Count == 0)
            {
                for (int j = 0; E.Count > j; j++)
                {
                    listBoxHrany_add(j);
                }
            }
            else
                if (!listBoxHrany.Items[listBoxHrany.Items.Count - 1].ToString().Contains("->"))
                {
                    listBoxHrany.Items.Clear();
                    for (int j = 0; E.Count > j; j++)
                    {
                        listBoxHrany_add(j);
                    }
                }
                else listBoxHrany_add(E.Count - 1);

        }
        private void sheet_MouseClick(object sender, MouseEventArgs e)
        {
            //hledani stupne vrcholu
            if (selectButton.Enabled == false)
            {
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        if (selected1 != -1)
                        {
                            selected1 = -1;
                            G.clearSheet();
                            G.drawALLGraph(V, E);
                            sheet.Image = G.GetBitmap();
                        }
                        if (selected1 == -1)
                        {
                            G.drawSelectedVrchol(V[i].x, V[i].y);
                            selected1 = i;
                            sheet.Image = G.GetBitmap();
                            //createAdjAndOut();
                            listBoxMatrix.Items.Clear();
                            int degree = 0;
                            for (int j = 0; j < V.Count; j++)
                                degree += AMatrix[selected1, j];
                            listBoxMatrix.Items.Add("Stupen vrcholu №" + (selected1 + 1) + " je rovna " + degree);
                            break;
                        }
                    }
                }
            }
            //je stisknuto tlacitko "kreslit vrchol"
            if (drawVrcholButton.Enabled == false)
            {
                V.Add(new Vrchol(e.X, e.Y));
                G.drawVrchol(e.X, e.Y, V.Count.ToString());
                sheet.Image = G.GetBitmap();
            }
            //je stisknuto tlacitko  "kreslit hranu"
            if (drawHranaButton.Enabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    for (int i = 0; i < V.Count; i++)
                    {
                        if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                        {
                            if (selected1 == -1)
                            {
                                G.drawSelectedVrchol(V[i].x, V[i].y);
                                selected1 = i;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                            if (selected2 == -1)
                            {
                                G.drawSelectedVrchol(V[i].x, V[i].y);
                                selected2 = i;
                                int per1 = 0;
                                bool per2 = false;
                                Form2 _Hrana = new Form2();
                                if (_Hrana.ShowDialog(this) == DialogResult.OK)
                                {
                                    per1 = Int32.Parse(_Hrana.v_v3.Text);
                                    if (_Hrana.v_v4.Checked == true)
                                    {
                                        per2 = true;
                                    };
                                }
                                _Hrana.Dispose();
                                E.Add(new Hrana(selected1, selected2, per1, per2));
                                G.drawHrana(V[selected1], V[selected2], E[E.Count - 1], E.Count - 1);
                                listBoxHrany_hrane();
                                selected1 = -1;
                                selected2 = -1;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                        }
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if ((selected1 != -1) &&
                        (Math.Pow((V[selected1].x - e.X), 2) + Math.Pow((V[selected1].y - e.Y), 2) <= G.R * G.R))
                    {
                        G.drawVrchol(V[selected1].x, V[selected1].y, (selected1 + 1).ToString());
                        selected1 = -1;
                        sheet.Image = G.GetBitmap();
                    }
                }
            }
            //je stisknuto tlacitko "smazat prvek"
            if (deleteButton.Enabled == false)
            {
                bool flag = false; //удалили ли что-нибудь по ЭТОМУ клику
                //hledame, zda li bylo stisknuto tlacitko vrchol
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        for (int j = 0; j < E.Count; j++)
                        {
                            if ((E[j].v1 == i) || (E[j].v2 == i))
                            {
                                E.RemoveAt(j);
                                j--;
                            }
                            else
                            {
                                if (E[j].v1 > i) E[j].v1--;
                                if (E[j].v2 > i) E[j].v2--;
                            }
                        }
                        listBoxHrany.Items.Clear();
                        listBoxHrany_hrane();
                        V.RemoveAt(i);
                        flag = true;
                        break;
                    }
                }
                //hledame, zda li bylo stisknuto tlacitko hrana
                if (!flag)
                {
                    for (int i = 0; i < E.Count; i++)
                    {
                        if (E[i].v1 == E[i].v2) //zda-li je to smycka
                        {
                            if ((Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) <= ((G.R + 2) * (G.R + 2))) &&
                                (Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) >= ((G.R - 2) * (G.R - 2))))
                            {
                                E.RemoveAt(i);
                                listBoxHrany.Items.Clear();
                                listBoxHrany_hrane();
                                flag = true;
                                break;
                            }
                        }
                        else //neni smycka
                        {
                            if (((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) <= (e.Y + 4) &&
                                ((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) >= (e.Y - 4))
                            {
                                if ((V[E[i].v1].x <= V[E[i].v2].x && V[E[i].v1].x <= e.X && e.X <= V[E[i].v2].x) ||
                                    (V[E[i].v1].x >= V[E[i].v2].x && V[E[i].v1].x >= e.X && e.X >= V[E[i].v2].x))
                                {
                                    E.RemoveAt(i);
                                    listBoxHrany.Items.Clear();
                                    listBoxHrany_hrane();
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                // Jest-li neco bylo vyskrtnuto, aktualizujeme graf
                if (flag)
                {
                    G.clearSheet();
                    G.drawALLGraph(V, E);
                    sheet.Image = G.GetBitmap();
                }
            }
        }

        //vytvoreni matice sousednosti a vyvod v listbox
        private void createAdjAndOut()
        {
            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            listBoxMatrix.Items.Clear();
            string sOut = "    ";
            for (int i = 0; i < V.Count; i++)
                sOut += (i + 1) + " ";
            listBoxMatrix.Items.Add(sOut);
            for (int i = 0; i < V.Count; i++)
            {
                sOut = (i + 1) + " | ";
                for (int j = 0; j < V.Count; j++)
                    sOut += AMatrix[i, j] + " ";
                listBoxMatrix.Items.Add(sOut);
            }
        }

        //hledani elementarnich retezcu
        private void chainButton_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
            int[] color = new int[V.Count];
            for (int i = 0; i < V.Count - 1; i++)
                for (int j = i + 1; j < V.Count; j++)
                {
                    for (int k = 0; k < V.Count; k++)
                        color[k] = 1;
                    DFSchain(i, j, E, color, (i + 1).ToString());
                }
        }
        //pruhod do hloubky, hledani elementarnich retezcu. Pouzivame obarveni: 1 - white; 2 - black;
        private void DFSchain(int u, int endV, List<Hrana> E, int[] color, string s)
        {
            //vrchol nemusime barvit, jestli u == endV, protoze do ni muze vest vice cest. 
            if (u != endV)
                color[u] = 2;
            else
            {
                listBoxMatrix.Items.Add(s);
                return;
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    DFSchain(E[w].v2, endV, E, color, s + "-" + (E[w].v2 + 1).ToString());
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    DFSchain(E[w].v1, endV, E, color, s + "-" + (E[w].v1 + 1).ToString());
                    color[E[w].v1] = 1;
                }
            }
        }

        //hledani elementarnich cyklu
        private void cycleButton_Click(object sender, EventArgs e)
        {
            listBoxMatrix.Items.Clear();
            int[] color = new int[V.Count];
            for (int i = 0; i < V.Count; i++)
            {
                for (int k = 0; k < V.Count; k++)
                    color[k] = 1;
                List<int> cycle = new List<int>();
                cycle.Add(i + 1);
                DFScycle(i, i, E, color, -1, cycle);
            }
        }
        //pruhod do hloubky, hledani elementarnich cyklu, 1 - white, 2 - black;
        //Vrchol, pro ktery hledame cyklus, barvit v cernou barvu nebudeme. Tedy, aby program fungoval spravne
        //udelame promennou unavailableHrana, ve ktere budeme uchovavat cislo vrcholu, ktery odstranime pri prohledavani grafu.
        //To potrebujeme jen na prvni hladine rekurze, aby odstranit spatne cykly (napr. 1-2-1), kdyz mame 2 vrcholu.
        private void DFScycle(int u, int endV, List<Hrana> E, int[] color, int unavailableHrana, List<int> cycle)
        {
            //Je-li u == endV, pak tento vrchol prebarvovat nemusime,  jinak proste se nevratime do nej. 
            if (u != endV)
                color[u] = 2;
            else
            {
                if (cycle.Count >= 2)
                {
                    cycle.Reverse();
                    string s = cycle[0].ToString();
                    for (int i = 1; i < cycle.Count; i++)
                        s += "-" + cycle[i].ToString();
                    bool flag = false; //есть ли палиндром для этого цикла графа в листбоксе?
                    for (int i = 0; i < listBoxMatrix.Items.Count; i++)
                        if (listBoxMatrix.Items[i].ToString() == s)
                        {
                            flag = true;
                            break;
                        }
                    if (!flag)
                    {
                        cycle.Reverse();
                        s = cycle[0].ToString();
                        for (int i = 1; i < cycle.Count; i++)
                            s += "-" + cycle[i].ToString();
                        listBoxMatrix.Items.Add(s);
                    }
                    return;
                }
            }
            for (int w = 0; w < E.Count; w++)
            {
                if (w == unavailableHrana)
                    continue;
                if (color[E[w].v2] == 1 && E[w].v1 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].v2 + 1);
                    DFScycle(E[w].v2, endV, E, color, w, cycleNEW);
                    color[E[w].v2] = 1;
                }
                else if (color[E[w].v1] == 1 && E[w].v2 == u)
                {
                    List<int> cycleNEW = new List<int>(cycle);
                    cycleNEW.Add(E[w].v1 + 1);
                    DFScycle(E[w].v1, endV, E, color, w, cycleNEW);
                    color[E[w].v1] = 1;
                }
            }
        }
        // uchovani obrazku
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (sheet.Image != null)
            {

                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Ulozit jako...";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        sheet.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Neni mozne ulozit obrazek", "Chyba",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //nejkratsi cesta
        private void button1_Click(object sender, EventArgs e)
        {

            Form3 _kr_path = new Form3();
            if (_kr_path.ShowDialog(this) == DialogResult.OK)
            {
                int per1 = Int32.Parse(_kr_path.v_p_1.Text) - 1;
                int per2 = Int32.Parse(_kr_path.v_p_2.Text) - 1;
                AMatrix = new int[V.Count, V.Count];
                G.fillAdjacencyMatrix(V.Count, E, AMatrix);
                for (int i = 0; i < V.Count; i++)
                {
                    for (int j = 0; j < V.Count; j++)
                    {
                        if (AMatrix[i, j] == 0) { AMatrix[i, j] = int.MaxValue; }
                    }
                }

                int[] d/*vzdalenost od start do vrcholu*/ = new int[V.Count], p/*predchudce*/ = new int[V.Count];
                bool[] f = new bool[V.Count];

                //inicializace
                for (int i = 0; i < V.Count; i++) { p[i] = per1; d[i] = AMatrix[per1, i]; }
                f[per1] = true;
                p[per1] = -1;

                for (int i = 0; i < V.Count - 1; i++)
                {
                    /*hledame ten vrchol mezi neotevrenymi vrcholy, ktery ma nejkratsi cestu ze startu */
                    int k = 0, mindist = int.MaxValue;
                    for (int j = 0; j < V.Count; j++) { if (!f[j] && mindist > d[j]) { mindist = d[j]; k = j; } }

                    f[k] = true; /*delame vrchol otevrenym*/

                    /*aktualizujeme nejkratsi cesty*/
                    for (int j = 0; j < V.Count; j++)
                    {
                        if (AMatrix[k, j] == int.MaxValue) { continue; } //zda-li existuje hrana k--j
                        int l = d[k] + AMatrix[k, j];
                        if (!f[j] && d[j] > l) { d[j] = l; p[j] = k; }
                    }
                }


                int prev = per2; string path = "";
                for (; ; )
                {
                    if (path == "")
                        path = (prev + 1).ToString();
                    else
                        path = (prev + 1) + " -> " + path;
                    if (prev == per1) { break; }
                    prev = p[prev];
                }
                if (d[per2] == int.MaxValue)
                    listBoxMatrix.Items.Add("Cesta neexistuje");
                else
                {
                    listBoxMatrix.Items.Add(path);
                    listBoxMatrix.Items.Add("délka - " + d[per2]);
                }

            }
            _kr_path.Dispose();
        }
        //Minimalni kostry
        private void button2_Click(object sender, EventArgs e)
        {
            List<Hrana> Ed = new List<Hrana>();
            foreach (Hrana s in E)
                Ed.Add(s);
            Hrana add = new Hrana(0, 0, int.MaxValue, true);
            for (int i = Ed.Count; i < V.Count; i++)
                Ed.Add(add);
            int[] nodes = new int[Ed.Count];
            int posledni_n = selected1;
            listBoxMatrix.Items.Clear();
            listBoxMatrix.Items.Add("Minimalni kostry:");
            for (int i = 0; i < V.Count; i++)
            {
                nodes[i] = -1 - i;
            }
            var sVes = from h in Ed orderby h.v3 select h;
            foreach (var s in sVes)
            {
                for (int i = 0; i < Ed.Count; i++)
                { // pokud neprojdeme vsechny vrcholy
                    int c = getColor(s.v2, nodes);
                    if (getColor(s.v1, nodes) != c)
                    {
                        // Jestli hrana spojuje vrcholy ruznych barev - vkladame do zasobniku a prebarvime vrcholy
                        nodes[selected1] = s.v2;
                        listBoxMatrix.Items.Add((s.v1 + 1) + " " + (s.v2 + 1) + " (" + s.v3 + ")");
                    }
                }
            }
            selected1 = posledni_n;
            Ed.Clear();
        }

        private int getColor(int n, int[] nodes)
        {
            int c;
            if (nodes[n] < 0)
            {
                selected1 = n;
                return nodes[selected1];
            }
            c = getColor(nodes[n], nodes);
            nodes[n] = selected1;
            return c;
        }
        //komponenta souvislosti
        private void button3_Click(object sender, EventArgs e)
        {
            int[] nodes = new int[V.Count];
            int num = 0;
            listBoxMatrix.Items.Clear();
            listBoxMatrix.Items.Add("Komponenty souvislosti:");
            List<Hrana> Ed = new List<Hrana>();
            foreach (Hrana s in E) //udelame vlatnosti hran
            {
                Hrana add = new Hrana(0, 0, 0, true);
                Ed.Add(s);
                if (!s.v4)
                {
                    add.v1 = s.v2; // 1 --> 2
                    add.v2 = s.v1; // 2 --> 1
                    add.v3 = s.v3; // ochodnoceni
                    add.v4 = s.v4; //boolean 
                    Ed.Add(add);
                }
            }
            for (int i = 0; i < V.Count; i++) //nastaveni cisla komponenty
            {
                nodes[i] = -1;
            }
            foreach (var ed in Ed) // hledame cislo komponenty 
            {
                if (nodes[ed.v1] == -1 && nodes[ed.v2] == -1)
                    num++;
                else
                    if (nodes[ed.v1] > 0) num = nodes[ed.v1];
                    else num = nodes[ed.v2];
                dfs(ed.v1, nodes, num, Ed);
            }
            int k = 0;
            k = nodes.Length;

            string[] l = new string[k];
            for (int i = 0; i < k; i++)
            {
                if (nodes[i] < 0) nodes[i] = 0;
                l[nodes[i]] = "";
            }
            for (int i = 0; i < k; i++)
                l[nodes[i]] = l[nodes[i]] + (i + 1) + " ";
            for (int i = 0; i < k; i++) //piseme do tabulky postupne vrcholy kazde komonenty  
            {
                if ((i == 0) && (l[i] != null)) { for (int j = 0; j < l[i].Length / 2; j++) listBoxMatrix.Items.Add(l[i].Split(' ')[j]); }
                else
                    if (l[i] != null) listBoxMatrix.Items.Add(l[i]);
            }
        }
        private void dfs(int u, int[] nodes, int num, List<Hrana> Ed) //private dfs pro zjisteni, zda-li vrcholy jsou ve stejne komponente
        {
            nodes[u] = 0;
            foreach (var v in Ed)
                if (u == v.v1 && nodes[v.v2] == -1)
                {
                    dfs(v.v2, nodes, num, Ed);
                }
            nodes[u] = num;
        }

        private bool dfsl(int u, int[] nodes, int[,] Ed, int[] result) //vrátí true nebo false. Zda-li byl najden cyklus
        {
            //Jestli vrchol je černý, pak neděláme nic
            if (nodes[u] == 1) return false;
            //Jestli vrchol je šedý, pak graf obsachuje cyklus
            if (nodes[u] == 0) return true;
            nodes[u] = 0; 
            bool prov = true;
            for (int i = 0; i < V.Count; i++)
            { //pustíme pruchod ode všech vrcholu, sousedních s v
                if (Ed[u, i] > 0)
                {
                    prov = dfsl(i, nodes, Ed, result);
                    if (prov) return true;
                }
            }
            //pokud neni cykl
            nodes[u] = 1; 
            for (int v = 0; v < V.Count; v++)
            {
                if (result[v] == 0) { result[v] = u + 1; break; }
            }
            return false;
        }
        private void button4_Click(object sender, EventArgs e) //topologicke usporadani vrcholu 
        {
            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, AMatrix);
            int[] result = new int[V.Count];
            int[] nodes = new int[V.Count];
            for (int i = 0; i < V.Count; i++) //nastaveni cisla komponenty
            {
                nodes[i] = -1;
            }
            listBoxMatrix.Items.Clear();
            listBoxMatrix.Items.Add("Topologicke usporadani:");
            for (int i = 0; i < V.Count; i++) //pro každý vrchol grafu
                if (dfsl(i, nodes, AMatrix, result)) //pustímé pro ní DFS
                    result[0] = -1;
            string s = "";
            if (result[0] < 0) s = "řešení nejsou"; else
            for (int i = V.Count; i > 0; i--)
                s += (result[i-1]) + " ";
            listBoxMatrix.Items.Add(s);
        }
    }
}
