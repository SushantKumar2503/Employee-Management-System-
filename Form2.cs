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

namespace ProjectCA3
{
    public partial class Form2 : Form
    {
        int fType = 0;
        public Form2(int formType)
        {
            fType = formType;
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int Id = Convert.ToInt32(txtId.Text.ToString());
                // Address of sql server and database
                string connection_string = "Data Source=SUSHANT-PC\\SQLEXPRESS;Initial Catalog=ems;Integrated Security=True";

                // Establish connection
                SqlConnection con = new SqlConnection(connection_string);

                // Open Connection
                con.Open();

                // Query Prepare
                string query = "select * from employee where id = "+Id+"";

                // Execute Query
                SqlCommand cmd = new SqlCommand(query, con);
                var reader = cmd.ExecuteReader();
                reader.Read();

                var name = Convert.ToString(reader["name"]);
                var address = Convert.ToString(reader["address"]);
                var contact = Convert.ToString(reader["contact"]);
                var email = Convert.ToString(reader["email"]);
                var desigination = Convert.ToString(reader["designation"]);
                var department = Convert.ToString(reader["department"]);
                var dateOfJoin = Convert.ToString(reader["joinDate"]);
                var wageRate = Convert.ToString(reader["wageRate"]);
                var hourWorked = Convert.ToString(reader["workedHour"]);

                // Close Connection
                con.Close();

                var addEmp = new AddEmployee(fType);
                addEmp.LoadData(Id.ToString(), name, address, contact, email, desigination, department, dateOfJoin, wageRate, hourWorked);
                addEmp.ShowDialog();
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            string currentText = txtId.Text;
            if (!string.IsNullOrEmpty(currentText) && !currentText.All(char.IsDigit))
            {
                txtId.Text = new string(currentText.Where(char.IsDigit).ToArray());
                txtId.SelectionStart = txtId.Text.Length;
            }
        }
    }
}
