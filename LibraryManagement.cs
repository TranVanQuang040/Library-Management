using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryManagement
{
    public partial class LibraryManagement : Form
    {

        SqlConnection connection;
        string username;
        

        public LibraryManagement(string usename)

        {
            InitializeComponent();
            connection = new SqlConnection("Server= 21AK22-COM\\SQLEXPRESS;Database=product_management;Integrated Security = true;");
            /*username = usename;*/

        }

        private void LibraryManagement_Load(object sender, EventArgs e)
        {
            GetBooks();     
        }
        
        private void GetBooks()
        {
            string query = "SELECT * FROM Books";
            DataTable table = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                adapter.Fill(table);
            }
            dgvBook.DataSource = table;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Login LibraryManagement = new Login();
            LibraryManagement.Show();

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }


        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                string query = "INSERT INTO Books ( Title, Author, PublishedYear, CopiesAvailable, CategoryID) " +
                               "VALUES (@Title, @Author, @PublishedYear, @CopiesAvailable, @CategoryID)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
                    cmd.Parameters.AddWithValue("@PublishedYear", int.Parse(txtPublishedYear.Text));
                    cmd.Parameters.AddWithValue("@CopiesAvailable", int.Parse(txtCopiesAvailable.Text));
                    cmd.Parameters.AddWithValue("@CategoryID", int.Parse(txtCategoryID.Text));

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Book inserted successfully!");

                
                txtTitle.Clear();
                txtAuthor.Clear();
                txtPublishedYear.Clear();
                txtCopiesAvailable.Clear();
                txtCategoryID.Clear();

                GetBooks(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void bntDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBookID.Text))
                {
                    MessageBox.Show("Please enter the Book ID to delete.");
                    return;
                }

                connection.Open();

                string query = "DELETE FROM Books WHERE BookID = @BookID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@BookID", int.Parse(txtBookID.Text));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book deleted successfully!");
                        GetBooks(); 
                    }
                    else
                    {
                        MessageBox.Show("No book found with the provided Book ID.");
                    }
                }

               
                txtBookID.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBookID.Text))
                {
                    MessageBox.Show("Please enter the Book ID to update.");
                    return;
                }

                connection.Open();

                string query = "UPDATE Books SET Title = @Title, Author = @Author, PublishedYear = @PublishedYear, " +
                               "CopiesAvailable = @CopiesAvailable, CategoryID = @CategoryID WHERE BookID = @BookID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@BookID", int.Parse(txtBookID.Text));
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
                    cmd.Parameters.AddWithValue("@PublishedYear", int.Parse(txtPublishedYear.Text));
                    cmd.Parameters.AddWithValue("@CopiesAvailable", int.Parse(txtCopiesAvailable.Text));
                    cmd.Parameters.AddWithValue("@CategoryID", int.Parse(txtCategoryID.Text));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book updated successfully!");
                        GetBooks(); 
                    }
                    else
                    {
                        MessageBox.Show("No book found with the provided Book ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void dgvBook_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}


