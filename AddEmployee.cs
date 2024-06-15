using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace ProjectCA3
{
    public partial class AddEmployee : Form
    {
        private int flg = 1;
        public AddEmployee(int nextEmployeID)
        {
            InitializeComponent();
            txtEmail.Leave += txtEmail_Leave;
            comboBoxDepartment.Items.Add("Administrative");
            comboBoxDepartment.Items.Add("Finance");
            comboBoxDepartment.Items.Add("Customer service");
            comboBoxDepartment.Items.Add("Marketing");
            comboBoxDepartment.Items.Add("IT");
            comboBoxDepartment.SelectedIndex = 0;
            if (nextEmployeID > 0)
            {
                flg = 1;
                txtIdNo.Text = nextEmployeID.ToString();
            }
            else if (nextEmployeID == -1)
            {
                flg = -1;
                btnSave.Text = "Delete";
                btnSave.BackColor = SystemColors.Highlight;
                txtAddress.Enabled = false;
                txtContact.Enabled = false;
                txtDesignation.Enabled = false;
                txtEmail.Enabled = false;
                txtName.Enabled = false;
                txtWage.Enabled = false;
                txtWorkedHour.Enabled = false;
                comboBoxDepartment.Enabled = false;
                dateTimePicker.Enabled = false;
            }
            else
            {
                flg = 0;
            }
        }

        // This method will fill the data of selected id
        public void LoadData(string id, string name, string address, string contact, string email, string desigination, string department, string dateOfJoin, string wageRate, string workedHour)
        {
            txtIdNo.Text = id;
            txtName.Text = name;
            txtAddress.Text = address;
            txtContact.Text = contact;
            txtEmail.Text = email;
            txtDesignation.Text = desigination;
            comboBoxDepartment.Text = department;
            string[] dateParts = dateOfJoin.Split('-');
            dateTimePicker.Value = new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[0]), int.Parse(dateParts[1]));
            txtWage.Text = wageRate;
            txtWorkedHour.Text = workedHour;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(flg == -1)
                {
                    string connection_string = "Data Source=SUSHANT-PC\\SQLEXPRESS;Initial Catalog=ems;Integrated Security=True";
                    SqlConnection con = new SqlConnection(connection_string);
                    con.Open();
                    int EmployeeNo = int.Parse(txtIdNo.Text);
                    string query = "delete from employee where id = "+EmployeeNo+"";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Hide();
                    MessageBox.Show("Data Deleted!");
                }
                else
                {
                    int EmployeeNo = int.Parse(txtIdNo.Text);
                    string name = txtName.Text;
                    string address = txtAddress.Text;
                    string contactNo = txtContact.Text;
                    string email = txtEmail.Text;
                    string desigination = txtDesignation.Text;
                    string department = comboBoxDepartment.Text;
                    string dateOfJoin = dateTimePicker.Value.ToString("MM-dd-yyyy");
                    string wageRate = txtWage.Text;
                    string hourWorked = txtWorkedHour.Text;

                    string connection_string = "Data Source=SUSHANT-PC\\SQLEXPRESS;Initial Catalog=ems;Integrated Security=True";
                    SqlConnection con = new SqlConnection(connection_string);
                    con.Open();
                    string query = "";
                    if (flg == 0)   //update data
                    {
                        query = "update employee set name = '" + name + "', address = '" + address + "', contact = '" + contactNo + "', email = '" + email + "', designation = '" + desigination + "', department = '" + department + "', joinDate = '" + dateOfJoin + "', wageRate = " + wageRate + ", workedHour = " + hourWorked + " where id = " + EmployeeNo + "";
                    }
                    else    // add data
                    {
                        query = "insert into employee values (" + EmployeeNo + ", '" + name + "', '" + address + "', '" + contactNo + "', '" + email + "', '" + desigination + "', '" + department + "', '" + dateOfJoin + "', " + wageRate + ", " + hourWorked + ")";
                    }
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Hide();
                    MessageBox.Show("Data Saved!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            string input = txtAddress.Text;
            string pattern = @"[,@/\\{}\[\].':""!#$%^&*]";
            if (Regex.IsMatch(input, pattern))
            {
                txtAddress.Text = Regex.Replace(input, pattern, "");
                txtAddress.SelectionStart = txtAddress.TextLength;
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            string currentText = txtEmail.Text;
            if (!Regex.IsMatch(currentText, pattern) && currentText != "")
            {
                txtEmail.Clear();
                MessageBox.Show("Invalid email format.");
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string input = txtName.Text;
            string pattern = @"[,@/\\{}\[\].':""!#$%^&*]1234567890";
            if (Regex.IsMatch(input, pattern))
            {
                txtName.Text = Regex.Replace(input, pattern, "");
                txtName.SelectionStart = txtName.TextLength;
            }
        }
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char[] specialCharacters = { ',', '@', '/', '\\', '{', '[', '}', ']', '.', '\'', ':', '"', '!', '#', '$', '%', '^', '&', '*', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            if (specialCharacters.Contains(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            string currentText = txtContact.Text;
            if (!string.IsNullOrEmpty(currentText) && !currentText.All(char.IsDigit))
            {
                txtContact.Text = new string(currentText.Where(char.IsDigit).ToArray());
                txtContact.SelectionStart = txtContact.Text.Length;
            }
        }

        private void txtWage_TextChanged(object sender, EventArgs e)
        {
            string currentText = txtWage.Text;
            if (!string.IsNullOrEmpty(currentText) && !currentText.All(char.IsDigit))
            {
                txtWage.Text = new string(currentText.Where(char.IsDigit).ToArray());
                txtWage.SelectionStart = txtWage.Text.Length;
            }
        }

        private void txtWorkedHour_TextChanged(object sender, EventArgs e)
        {
            string currentText = txtWorkedHour.Text;
            if (!string.IsNullOrEmpty(currentText) && !currentText.All(char.IsDigit))
            {
                txtWorkedHour.Text = new string(currentText.Where(char.IsDigit).ToArray());
                txtWorkedHour.SelectionStart = txtWorkedHour.Text.Length;
            }
        }

        private void txtContact_Leave(object sender, EventArgs e)
        {
            if (txtContact.Text.Length != 10)
            {
                MessageBox.Show("Mobile number must be of length 10.");
            }
        }

        private void txtDesignation_TextChanged(object sender, EventArgs e)
        {
            string currentText = txtDesignation.Text;
            if (!string.IsNullOrEmpty(currentText) && !currentText.Replace(" ", "").All(char.IsLetter))
            {
                txtDesignation.Text = new string(currentText.Where(c => char.IsLetter(c) || c == ' ').ToArray());
                txtDesignation.SelectionStart = txtDesignation.Text.Length;
            }
        }
    }
}
