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
    public partial class Login : Form
    {
        SqlConnection connection;
        public Login()
        {
            InitializeComponent();
            connection = new SqlConnection("Server= 21AK22-COM\\SQLEXPRESS;Database=product_management;Integrated Security = true;");
            txtPassword.PasswordChar = '*';
        }     
        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string query = "SELECT * FROM Account WHERE username = @username AND u_password = @password";

            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username); // Ensure parameter names match
                    cmd.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader["u_role"].ToString();
                            if (role.Equals("admin"))
                            {
                                MessageBox.Show("Login successful!", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                                this.Hide();
                                LibraryManagement p = new LibraryManagement(username);
                                p.ShowDialog();
                                this.Dispose();
                            }
                            else if (role.Equals("user"))
                            {
                                MessageBox.Show("Login successful!", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                                this.Hide();
                                ViewBooks vp = new ViewBooks(username);
                                vp.ShowDialog();
                                this.Dispose();
                            }
                            else
                            {
                                MessageBox.Show("You are not allowed to access this system.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Wrong username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
             private void button2_Click(object sender, EventArgs e)
                    {

                    }
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblError_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
