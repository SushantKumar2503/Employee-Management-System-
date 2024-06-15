using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace ProjectCA3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            GetEmployees();
        }

        private void GetEmployees()
        {
            dataGridView.Rows.Clear();
            try
            {
                // Address of sql server and database
                string connection_string = "Data Source=SUSHANT-PC\\SQLEXPRESS;Initial Catalog=ems;Integrated Security=True";

                // Establish connection
                SqlConnection con = new SqlConnection(connection_string);

                // Open Connection
                con.Open();

                // Query Prepare
                string query = "select * from employee";

                // Execute Query
                SqlCommand cmd = new SqlCommand(query, con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView.Rows.Add(reader["id"], reader["name"], reader["address"], reader["contact"], reader["email"], reader["designation"], reader["department"], reader["joinDate"], reader["wageRate"], reader["workedHour"]);
                }

                // Close Connection
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var addEmp = new AddEmployee(GetHighestEmployeeID() + 1);
            addEmp.ShowDialog();
        }
        public int GetHighestEmployeeID()
        {
            int highestEmployeeID = 0;
            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                bool isParsable = int.TryParse(Convert.ToString(item.Cells[0].Value), out int employeeID);
                if (isParsable)
                {
                    highestEmployeeID = employeeID > highestEmployeeID ? employeeID : highestEmployeeID;
                }
            }
            return highestEmployeeID;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            GetEmployees();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var addSearch = new Form2(0);
                addSearch.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var addSearch = new Form2(-1);
                addSearch.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            ExportDataToCSV();
        }

        private void ExportDataToCSV()
        {
            try
            {
                string connection_string = "Data Source=SUSHANT-PC\\SQLEXPRESS;Initial Catalog=ems;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    string query = "select * from employee";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        DataTable dataTable = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                        if (dataTable.Rows.Count > 0)
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                            saveFileDialog.FilterIndex = 0;
                            saveFileDialog.RestoreDirectory = true;

                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                string filePath = saveFileDialog.FileName;

                                using (StreamWriter streamWriter = new StreamWriter(filePath))
                                {
                                    // Write column headers
                                    foreach (DataColumn column in dataTable.Columns)
                                    {
                                        streamWriter.Write(column.ColumnName + ",");
                                    }
                                    streamWriter.WriteLine();

                                    // Write data
                                    foreach (DataRow row in dataTable.Rows)
                                    {
                                        foreach (object item in row.ItemArray)
                                        {
                                            streamWriter.Write(item.ToString() + ",");
                                        }
                                        streamWriter.WriteLine();
                                    }
                                }

                                MessageBox.Show("Data exported successfully!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No data found in the database.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            ImportDataFromCSV();
        }

        private void ImportDataFromCSV()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string query = "insert into employee values ";
                    using (StreamReader streamReader = new StreamReader(filePath))
                    {
                        streamReader.ReadLine();

                        while (!streamReader.EndOfStream)
                        {
                            string temp = "(";
                            string[] fields = streamReader.ReadLine().Split(',');
                            int len = fields.Length;
                            if(len > 10)
                            {
                                len = 10;
                            }
                            for (int i = 0; i < len; i++)
                            {
                                if(i == 0 || i == 8 || i == 9)
                                {
                                    temp += fields[i].Trim();
                                }
                                else
                                {
                                    temp += "'" + fields[i].Trim() + "'";
                                }

                                temp += ",";
                            }
                            temp = temp.Remove(temp.Length - 1);
                            temp += "),";
                            query += temp;
                        }
                        query = query.Remove(query.Length - 1);
                        query += ";";
                    }
                    string connection_string = "Data Source=SUSHANT-PC\\SQLEXPRESS;Initial Catalog=ems;Integrated Security=True";
                    SqlConnection con = new SqlConnection(connection_string);
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data imported successfully!");
                    GetEmployees();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            var report = new Form3(dataGridView);
            report.ShowDialog();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
