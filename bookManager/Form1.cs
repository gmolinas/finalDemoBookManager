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

namespace bookManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView.Visible = false;
            addBook.Visible = false;
            removeBook.Visible = false;
        }

        string booksDir;
        string positionDir;


        void loadData(String[] Vector, String[] location)
        {
            dataGridView.Rows.Clear();

            foreach (var item in Vector)
            {
                String[] elements = item.Split(new char[] { '\u002c' });
                string estante = "No Asignado";
                int cantidad = 1;

                foreach (var element in location)
                {
                    String[] pos = element.Split(new char[] { '\u002c' });

                    if (pos[0].Trim() == elements[1].Trim())
                        estante = pos[1];

                }


                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells[2].Value.ToString().Trim() == elements[2].Trim())
                    {
                        dataGridView.Rows.Remove(row);
                        cantidad++;
                    }
                }

                dataGridView.Rows.Add(elements[0], elements[1], elements[2], estante, cantidad);



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            OpenFileDialog openFilebooks = new OpenFileDialog();
            DialogResult result = openFilebooks.ShowDialog();

            if (result == DialogResult.OK) // Test result.
            {

                booksDir = openFilebooks.FileName;
                string file2;
                DialogResult result2;

                do
                {
                    MessageBox.Show("A continuacion Cargue las direcciones de las estanterias");

                    OpenFileDialog openFileposition = new OpenFileDialog();
                    result2 = openFileposition.ShowDialog();
                    positionDir = openFileposition.FileName;

                }
                while (result2 != DialogResult.OK);

                try
                {
                    dataGridView.Visible = true;
                    TextBack.Visible = false;
                    addBook.Visible = true;
                    removeBook.Visible = true;


                    FileStream position = new FileStream(positionDir, FileMode.Open);
                    StreamReader positionReader = new StreamReader(position);
                    String[] location = positionReader.ReadToEnd().Split(new char[] { '\n' });



                    FileStream books = new FileStream(booksDir, FileMode.Open);
                    StreamReader booksReader = new StreamReader(books);
                    String[] Vector = booksReader.ReadToEnd().Split(new char[] { '\u003b' });

                    loadData(Vector,  location);

                    books.Close();
                    booksReader.Close();
                    position.Close();
                    positionReader.Close();

                }
                catch (IOException)
                {
                }
            }
        }
        private void addBook_Click(object sender, EventArgs e)
        {

            FileStream position = new FileStream(positionDir, FileMode.Open);
            StreamReader positionReader = new StreamReader(position);
            String[] location = positionReader.ReadToEnd().Split(new char[] { '\n' });

            FileStream books = new FileStream(booksDir, FileMode.Open);
            StreamReader booksReader = new StreamReader(books);
            String[] Vector = booksReader.ReadToEnd().Split(new char[] { '\u003b' });


            StreamWriter booksWriter = new StreamWriter(books);


            if(String.IsNullOrEmpty(textBoxAuthor.Text)|| String.IsNullOrEmpty(textBoxGenero.Text) || String.IsNullOrEmpty(textBoxTitulo.Text))
            {
                MessageBox.Show("No se puede añadir campos vacios");

            }
            else
            {
                booksWriter.Write(booksReader.ReadToEnd() + $";{textBoxAuthor.Text}, {textBoxGenero.Text}, {textBoxTitulo.Text}");
                booksWriter.Close();
                loadData(Vector, location);
            }



            books.Close();
            booksReader.Close();
            position.Close();
            positionReader.Close();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

      
    }
}
