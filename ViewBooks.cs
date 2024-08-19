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
    public partial class ViewBooks : Form
    {
        SqlConnection connection;
        string username;

        public ViewBooks()

        {
            {
                InitializeComponent();
                connection = new SqlConnection("Server= 21AK22-COM\\SQLEXPRESS;Database=product_management;Integrated Security = true;");
                /* this.username = username;*/

            }
        }

        private void ViewBooks_Load(object sender, EventArgs e)
        {
            BorrowBooks();
            GetStatus();
        }

        private void BorrowBooks()
        {
            string query = "SELECT * FROM Loans";
            DataTable table = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                adapter.Fill(table);
            }
            dgvbook.DataSource = table;

        }
        public void GetStatus()
        {
            string query = "SELECT StatusID, StatusName FROM Status";
            DataTable table = new DataTable();

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(table);
                }

                cbstatus.DataSource = table;
                cbstatus.DisplayMember = "StatusName";
                cbstatus.ValueMember = "StatusID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving statuses: " + ex.Message);
            }
        }

        public ViewBooks(string loginuser)
        {
            InitializeComponent();
            connection = new SqlConnection("Server= 21AK22-COM\\SQLEXPRESS;Database=product_management;Integrated Security = true;");
            MessageBox.Show("Welcome" + loginuser);
        }

        

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void bntDelete_Click(object sender, EventArgs e)
        {
            // Validate the input
            if (string.IsNullOrEmpty(txtBookID.Text))
            {
                MessageBox.Show("Please enter a valid Book ID to delete.");
                return;
            }

            if (!int.TryParse(txtBookID.Text, out int bookID))
            {
                MessageBox.Show("Book ID must be a valid integer.");
                return;
            }

            // Confirm deletion
            if (MessageBox.Show(this, "Do you want to delete this book? This will also remove all related loan records.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Open the connection
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    // Begin a transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Delete related records from Loans table
                            string deleteLoansQuery = "DELETE FROM Loans WHERE BookID = @BookID";
                            using (SqlCommand cmd = new SqlCommand(deleteLoansQuery, connection, transaction))
                            {
                                cmd.Parameters.Add("@BookID", SqlDbType.Int).Value = bookID;
                                cmd.ExecuteNonQuery();
                            }

                            // Delete the record from Books table
                            string deleteBookQuery = "DELETE FROM Books WHERE BookID = @BookID";
                            using (SqlCommand cmd = new SqlCommand(deleteBookQuery, connection, transaction))
                            {
                                cmd.Parameters.Add("@BookID", SqlDbType.Int).Value = bookID;
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Commit the transaction
                                    transaction.Commit();
                                    MessageBox.Show("Book and related loans deleted successfully.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    BorrowBooks(); // Refresh the data grid to reflect changes
                                }
                                else
                                {
                                    MessageBox.Show("No book found with the provided Book ID.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction on error
                            transaction.Rollback();
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    // Ensure the connection is closed
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
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

                string query = "UPDATE Books SET LoanID = @LoanID, BookID = @BookID, StudentID = @StudentID, " +
                               "LoanDate = @LoanDate, ReturnDate = @ReturnDate, StatusID = @StatusID WHERE BookID = @BookID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@LoanID", int.Parse(txtBookID.Text));
                    cmd.Parameters.AddWithValue("@BookID", txtLoanID.Text);
                    cmd.Parameters.AddWithValue("@StudentID", txtStudentID.Text);
                    cmd.Parameters.AddWithValue("@LoanDate", int.Parse(txtLoanDate.Text));
                    cmd.Parameters.AddWithValue("@ReturnDate", int.Parse(txtReturnDate.Text));
                    cmd.Parameters.AddWithValue("@StatusID", int.Parse(cbstatus.Text));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book updated successfully!");
                        BorrowBooks(); // Refresh the DataGridView after updating
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

        private void btnInsert_Click(object sender, EventArgs e)
        {           
            try
            {
                connection.Open();

                string query = "INSERT INTO Loans (BookID, StudentID, LoanDate, ReturnDate, StatusID) " +
                               "VALUES (@BookID, @StudentID, @LoanDate, @ReturnDate, @StatusID)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    int bookID, studentID, statusID;
                    DateTime loanDate, returnDate;

                    // Kiểm tra và chuyển đổi BookID
                    if (!int.TryParse(txtBookID.Text, out bookID))
                    {
                        MessageBox.Show("BookID is not a valid integer.");
                        return;
                    }
                    cmd.Parameters.AddWithValue("@BookID", bookID);

                    // Kiểm tra và chuyển đổi StudentID
                    if (!int.TryParse(txtStudentID.Text, out studentID))
                    {
                        MessageBox.Show("StudentID is not a valid integer.");
                        return;
                    }
                    cmd.Parameters.AddWithValue("@StudentID", studentID);

                    // Kiểm tra và chuyển đổi LoanDate
                    if (!DateTime.TryParse(txtLoanDate.Text, out loanDate))
                    {
                        MessageBox.Show("LoanDate is not a valid date.");
                        return;
                    }
                    cmd.Parameters.AddWithValue("@LoanDate", loanDate);

                    // Kiểm tra và chuyển đổi ReturnDate
                    if (string.IsNullOrEmpty(txtReturnDate.Text))
                    {
                        cmd.Parameters.AddWithValue("@ReturnDate", DBNull.Value);
                    }
                    else if (!DateTime.TryParse(txtReturnDate.Text, out returnDate))
                    {
                        MessageBox.Show("ReturnDate is not a valid date.");
                        return;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                    }

                    // Kiểm tra và chuyển đổi StatusID
                    if (!int.TryParse(cbstatus.Text, out statusID))
                    {
                        // Nếu StatusID là tên trạng thái, cần phải tra cứu StatusID từ tên trạng thái
                        string statusQuery = "SELECT StatusID FROM Status WHERE StatusName = @StatusName";
                        using (SqlCommand statusCmd = new SqlCommand(statusQuery, connection))
                        {
                            statusCmd.Parameters.AddWithValue("@StatusName", cbstatus.Text);
                            object result = statusCmd.ExecuteScalar();

                            if (result == null)
                            {
                                MessageBox.Show("Status not found.");
                                return;
                            }

                            statusID = (int)result;
                        }
                    }
                    cmd.Parameters.AddWithValue("@StatusID", statusID);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Book inserted successfully!");

                // Xóa các trường sau khi chèn dữ liệu
                txtBookID.Clear();
                txtStudentID.Clear();
                txtLoanDate.Clear();
                txtReturnDate.Clear();
                cbstatus.SelectedIndex = -1;

                BorrowBooks();
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

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
