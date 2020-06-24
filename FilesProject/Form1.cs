using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace FilesProject
{
    public partial class Form1 : Form
    {
        public int x;
        public string[] keywords;
        class fille
        {
            string filename = "Files.xml";
            XmlWriter writer;
            XmlDocument doc = new XmlDocument();
            public fille()
            {
                
               
            }
            public void addFile(string name, string path, string category, List<String> keyWord)
            {
                if (!File.Exists(filename))
                {
                    writer = XmlWriter.Create(filename);
                    writer.WriteStartDocument();

                    writer.WriteStartElement("Files");

                    writer.WriteStartElement("File");

                    writer.WriteStartElement("Name");
                    writer.WriteString(name);
                    writer.WriteEndElement();

                    writer.WriteStartElement("Path");
                    writer.WriteString(path);
                    writer.WriteEndElement();


                    writer.WriteStartElement("Category");

                    writer.WriteStartElement("Name");
                    writer.WriteString(category);
                    writer.WriteEndElement();
                    

                    foreach(string s in keyWord)
                    {

                        writer.WriteStartElement("Keyword");
                        writer.WriteString(s);
                        writer.WriteEndElement();
                    }


                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                    writer.Close();
                }
                else
                {
                    doc.Load(filename);
                    XmlElement newFile = doc.CreateElement("File");
                    XmlElement node = doc.CreateElement("Name");
                    node.InnerText = name;
                    newFile.AppendChild(node);

                    node = doc.CreateElement("Path");
                    node.InnerText = path;
                    newFile.AppendChild(node);

                    node = doc.CreateElement("Category");

                    XmlElement x = doc.CreateElement("Name");
                    x.InnerText = category;
                    node.AppendChild(x);

                    foreach(string s in keyWord)
                    {
                        x = doc.CreateElement("Keyword");
                        x.InnerText = s;
                        node.AppendChild(x);
                    }
                    newFile.AppendChild(node);

                    XmlElement root = doc.DocumentElement;
                    root.AppendChild(newFile);
                    doc.Save(filename);

                }

            }
            public bool checkFileName(string file)
            {
                doc.Load(filename);
                foreach (XmlNode node in doc.SelectNodes("Files/File"))
                {
                    if (node.SelectSingleNode("Name").InnerText == file)
                    {
                        doc.Save(filename);
                        return true;
                    }
                }
                doc.Save(filename);
                return false;
            }
            public bool checkCategoryName(string file,string cat)
            {
                doc.Load(filename);
                foreach (XmlNode node in doc.SelectNodes("Files/File"))
                {
                    if (node.SelectSingleNode("Name").InnerText == file)
                    {
                        foreach(XmlNode n in node.SelectNodes("Category"))
                        {
                            if (n.SelectSingleNode("Name").InnerText == cat)
                            {
                                doc.Save(filename);
                                return false;
                            }
                        }
                    }
                }
                doc.Save(filename);
                return true;
            }
            public void AddNewCategory(string file,string cat,List<string>keywords)
            {
                doc.Load(filename);
                XmlElement category = doc.CreateElement("Category");
                XmlElement node = doc.CreateElement("Name");
                node.InnerText = cat;
                category.AppendChild(node);

                foreach(string s in keywords)
                {
                    node = doc.CreateElement("Keyword");
                    node.InnerText = s;
                    category.AppendChild(node);
                }

                foreach(XmlNode n in doc.SelectNodes("Files/File"))
                {
                    if(n.SelectSingleNode("Name").InnerText==file)
                    {
                        n.AppendChild(category);
                        doc.Save(filename);
                        break;

                    }
                }
            }
            public string[] DisplayAllFiles()
            {
                doc.Load(filename);
                string str = "";
                int i = 0;
                
                int x = doc.SelectNodes("Files/File").Count;
                string[] arr = new string[x];
                int h;
                int k; 
                foreach (XmlNode node in doc.SelectNodes("Files/File"))
                {
                    str = "File Name: ";
                    str += node.SelectSingleNode("Name").InnerText+"\tPath: ";
                    str += node.SelectSingleNode("Path").InnerText + "\t";
                    h = 1;
                    foreach (XmlNode n in node.SelectNodes("Category"))
                    {
                        k = 1;
                        str+= "[Category "+h+": ";
                        str += n.SelectSingleNode("Name").InnerText ;
                        foreach(XmlNode nn in n.SelectNodes("Keyword"))
                        {
                            str += "\t Keyword Name "+k+": "; 
                            str += nn.InnerText;
                            k++;
                        }
                        str += "]";
                        h++;
                    }
                    arr[i] = str;
                    i++;
                }
                return arr;
            }
            //
            public List<string> DisplayFileNames()
            {
                doc.Load(filename);
                List<string> arr = new List<string>();
                
                foreach(XmlNode node in doc.SelectNodes("Files/File"))
                {
                    arr.Add( node.SelectSingleNode("Name").InnerText);
                }
                return arr;
            }
            public List<string> DisplayCategory(string file)
            {
                List<string> arr = new List<string>();
                doc.Load(filename);
                foreach (XmlNode node in doc.SelectNodes("Files/File"))
                    if (node.SelectSingleNode("Name").InnerText == file)
                    {
                        foreach (XmlNode n in node.SelectNodes("Category"))
                            arr.Add(n.SelectSingleNode("Name").InnerText);
                        break;
                    }
                return arr;
            }
            public List<string> DisplayKeyWords(string file, string category)
            {
                List<string> arr = new List<string>();
                doc.Load(filename);
                foreach (XmlNode node in doc.SelectNodes("Files/File"))
                    if (node.SelectSingleNode("Name").InnerText == file)
                        foreach (XmlNode n in node.SelectNodes("Category"))
                            if (n.SelectSingleNode("Name").InnerText == category)
                                foreach (XmlNode x in n.SelectNodes("Keyword"))
                                    arr.Add(x.InnerText);
                return arr;
            }
            public string getPath(string file)
            {
                doc.Load(filename);
                string s;
                foreach (XmlNode node in doc.SelectNodes("Files/File"))
                {
                    if (node.SelectSingleNode("Name").InnerText == file)
                    {
                        s = node.SelectSingleNode("Path").InnerText;
                        return s;
                    }


                }
                return s = "";
            }
            public List<string> AllCategories()
            {
                doc.Load(filename);
                List<string> list = new List<string>();
                XmlNodeList nodeList = doc.GetElementsByTagName("Category");
                foreach (XmlNode s in nodeList)
                    list.Add(s.SelectSingleNode("Name").InnerText);
                return list;
            }
            public List<string> DisplayFileName(string category)
            {
                List<string> list = new List<string>();
                doc.Load(filename);
                foreach (XmlNode node in doc.SelectNodes("Files/File"))
                    foreach (XmlNode n in node.SelectNodes("Category"))
                        if (n.SelectSingleNode("Name").InnerText == category)
                            list.Add(node.SelectSingleNode("Name").InnerText);
                return list;
            }

            public List<string> SearchInFile(string filename,string keyword)
            {
                
                List<string> list = new List<string>();
                string path = getPath(filename)+filename+".txt";

                FileStream fs = new FileStream(path, FileMode.Open);

                StreamReader reader = new StreamReader(fs);
                string str ;
                int pos = 1;
                
                while (reader.Peek() != -1)
                {
                    str = reader.ReadLine();
                    if (str.Contains(keyword))
                    {
                        int count = 0;
                        string[] part = str.Split(' ');
                        foreach(string s in part)
                            if(s.Equals(keyword))
                                count++;
                        list.Add(keyword + "  appeared " + count + "  Times in Line " + pos);
                    }
                    pos++;
                }
                fs.Close();
                reader.Close();
               
                return list;
            }

           
        }
        fille file = new fille();
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        int i = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            x = int.Parse(textBox5.Text);
            if (textBox4.Text=="")
            {
                MessageBox.Show("Enter Keyword "+label5.Text);
            }
            else
            {
                
                if (i < x)
                {
                    keyWordList.Add(textBox4.Text);
                    textBox4.Text = "";
                    if(i+1 < x)
                    {
                        label5.Text = (int.Parse(label5.Text) + 1).ToString();
                        i++;
                    }
                    else
                    {
                        MessageBox.Show("Successfully Added Keywords");

                        i = 0;
                        button3.Show();
                        textBox4.Text = "";
                        label5.Text = "1";
                    }
                    
                }
                else
                {
                    MessageBox.Show("Successfully Added Keywords");

                    i = 0;
                    button3.Show();
                    textBox4.Text = "";
                    label5.Text = "1";
                }
                
                


            }
        }

        public List<string> keyWordList = new List<string>();
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(textBox2.Text + textBox1.Text+".txt"))
                {
                    {
                        int x = int.Parse(textBox5.Text);
                        if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox5.Text == "" || int.Parse(textBox5.Text) <= 0)
                        {
                            MessageBox.Show("INVALID INPUTS !!");
                        }
                        //else if (file.checkFileName(textBox1.Text))
                        //    MessageBox.Show("Already Exists File Name !!");

                        else
                        {
                            panel1.Show();
                            label4.Show();
                            label5.Show();
                            textBox4.Show();
                            button1.Show();
                            x = int.Parse(textBox5.Text);
                            i = 0;
                            keyWordList.Clear();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("File is not created !!");
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show("No. of Keywords must be Integers !!");

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            file.addFile(textBox1.Text, textBox2.Text, textBox3.Text, keyWordList);
            MessageBox.Show("Successfully Added File");
            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox5.Text = "";
            panel1.Hide();
            label4.Hide();
            label5.Hide();
            textBox4.Hide();
            button1.Hide();
            label5.Text = "1";
            button3.Hide();
            
            i = 1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "" || textBox9.Text == "" || textBox8.Text == "" || int.Parse(textBox8.Text) <= 0)
            {
                MessageBox.Show("INVALID INPUTS !");
            }
            else if (!file.checkFileName(textBox6.Text))
                MessageBox.Show("INVALID File Name !");
            else if(!file.checkCategoryName(textBox6.Text,textBox9.Text))
                MessageBox.Show("Already Exists This Category !");
            else
            {
                panel2.Show();
                label10.Show();
                label9.Show();
                button6.Show();
                textBox7.Show();
                x = int.Parse(textBox8.Text);
                keyWordList.Clear();
                j = 0;
            }

        }
        public int j = 0;
        
        private void button6_Click(object sender, EventArgs e)
        {
            x = int.Parse(textBox8.Text);
            
            if (textBox7.Text == "")
            {
                MessageBox.Show("Enter Keyword " + label9.Text);
            }
            else
            {
                if (j < x)
                {
                    keyWordList.Add(textBox7.Text);
                    textBox7.Text = "";
                    if (j + 1 < x)
                    {
                        label9.Text = (int.Parse(label9.Text) + 1).ToString();
                        j++;
                    }
                    else
                    {
                        MessageBox.Show("Successfully Added Keywords");
                        textBox7.Text = "";
                        label9.Text = "1";
                        button5.Show();
                        j = 0;
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            file.AddNewCategory(textBox6.Text, textBox9.Text, keyWordList);
            MessageBox.Show("Successfully Added Category");
            textBox6.Text = ""; textBox9.Text = ""; textBox8.Text = ""; textBox7.Text = "";
            panel2.Hide();
            label10.Hide();
            label9.Hide();
            textBox7.Hide();
            button5.Hide();
            label9.Text = "1";
            button6.Hide();
            j = 0;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] arr = file.DisplayAllFiles();
            for (int i = 0; i < arr.Length; i++)
            {
                listBox1.Items.Add(arr[i]);
                listBox1.Items.Add("_________________________________________________________________");
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Select File");
            List<string> arr = file.DisplayFileNames();
            for (int i = 0; i < arr.Count; i++)
                comboBox1.Items.Add(arr[i]);
            comboBox1.Show();
            button9.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text =="" || comboBox1.Text == "Select File")
            {
                MessageBox.Show("You must Select File !! ");
            }
            else if(!file.checkFileName(comboBox1.Text))
            {
                MessageBox.Show("You must Select File from this Box !! ");
            }
            else
            {
                List<string> arr = file.DisplayCategory(comboBox1.Text);
                listBox2.Items.Clear();
                foreach (string s in arr)
                    listBox2.Items.Add(s);
                listBox2.Show();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Select File");
            List<string> arr = file.DisplayFileNames();
            for (int i = 0; i < arr.Count; i++)
                comboBox2.Items.Add(arr[i]);
            comboBox2.Show();
            button11.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "" || comboBox2.Text == "Select File")
            {
                MessageBox.Show("You must Select File !! ");
            }
            
            else if (!file.checkFileName(comboBox2.Text))
            {
                MessageBox.Show("You must Select File from this Box !! ");
            }
            
            else
            {
                List<string> arr = file.DisplayCategory(comboBox2.Text);
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Select Category");
                foreach (string s in arr)
                    comboBox3.Items.Add(s);
                comboBox3.Show();
                button12.Show();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "" || comboBox3.Text == "Select File")
            {
                MessageBox.Show("You must Select Category !! ");
            }
            else if (comboBox3.Text == "" || comboBox3.Text == "Select Category")
            {
                MessageBox.Show("You must Select Category !! ");
            }
            else if (file.checkCategoryName(comboBox2.Text, comboBox3.Text))
            {
                MessageBox.Show("You must Select Category from this Box !! ");
            }
            else
            {
                List<string> arr = new List<string>();
                arr = file.DisplayKeyWords(comboBox2.Text, comboBox3.Text);
                comboBox4.Items.Clear();
                comboBox4.Items.Add("Select Keyword");
                foreach (string s in arr)
                    comboBox4.Items.Add(s);
                button13.Show();
                
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string s = file.getPath(comboBox2.Text);
            if (s == "")
                MessageBox.Show("No Path here !!");
            else
            {
                string stri = file.getPath(comboBox2.Text) + comboBox2.Text+".txt";
                if (File.Exists(stri))
                {
                    FileStream fs = new FileStream(stri, FileMode.Open);
                    
                    StreamReader reader = new StreamReader(fs);
                    string str = "";
                    while (reader.Peek() != -1)
                    {
                        str += reader.ReadLine();
                        
                    }
                    richTextBox1.Text = str;
                    comboBox4.Show();
                    button14.Show();
                    fs.Close();
                    reader.Close();
                }
                else
                    MessageBox.Show("File is not Created !!");


                //Stream myStream;
                //OpenFileDialog openFileDialog = new OpenFileDialog();
                //if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    string strfile = openFileDialog.FileName;
                //    if (strfile == comboBox2.Text)
                //    {
                //        if ((myStream = openFileDialog.OpenFile()) != null)
                //        {
                //            strfile = openFileDialog.FileName;
                //            string filetext = File.ReadAllText(strfile);
                //            richTextBox1.Text = filetext;
                //        }
                //    }
                //    else
                //        MessageBox.Show("NOT the Path of this File !!");
                //}

            }
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (!(comboBox4.Text == "")&&comboBox4.FindString(comboBox4.Text)!=-1)
            {
                int index = 0;
                string temp = richTextBox1.Text;
                richTextBox1.Text = "";
                richTextBox1.Text = temp;
                while (index < richTextBox1.Text.LastIndexOf(comboBox4.Text))
                {
                    richTextBox1.Find(comboBox4.Text, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionBackColor = Color.Pink;
                    index = richTextBox1.Text.IndexOf(comboBox4.Text, index) + 1;
                }
            }
            else
            {
                MessageBox.Show("You must select a keyword from ComboBox");
            }
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            List<string> Fname = new List<string>();
            List<string> keywords = new List<string>();
            Fname = file.DisplayFileName(comboBox5.Text);
            
            
            foreach (string s in Fname)
            {
                keywords = file.DisplayKeyWords(s, comboBox5.Text);
                listBox3.Items.Add(s);

                foreach (string k in keywords)
                    listBox3.Items.Add(k);
                listBox3.Items.Add("__________________________________________________");
                

            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            comboBox5.Items.Clear();
            comboBox5.Items.Add("Select Category");
            List<string> list;
            list = file.AllCategories();
            foreach(string s in list)
            {
                comboBox5.Items.Add(s);
            }
            comboBox5.Show();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            comboBox6.Items.Clear();
            comboBox6.Items.Add("Select Category");
            List<string> list;
            list = file.AllCategories();
            foreach (string s in list)
            {
                comboBox6.Items.Add(s);
            }
            comboBox6.Show();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox4.Items.Clear();
            List<string> list = file.DisplayFileName(comboBox6.Text);
            foreach(string s in list)
            {
                listBox4.Items.Add(s);
                List<string> listkey = file.DisplayKeyWords(s, comboBox6.Text);
                foreach (string k in listkey)
                {
                    List<string>listsearch = file.SearchInFile(s, k);
                    foreach(string l in listsearch)
                        listBox4.Items.Add(l);
                }
                listBox4.Items.Add("_________________________________________________");
            }
            
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
