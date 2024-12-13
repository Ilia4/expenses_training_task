using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace расходы
{
    public partial class Form1 : Form
    {
        private List<Expense> ExpenseList = new List<Expense>();

        public Form1()
        {
            InitializeComponent();
        }

        public class Expense
        {
            public string Category { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sum = inputSum.Text + ",00";

            if (string.IsNullOrEmpty(sum))
            {
                MessageBox.Show("Заполните строку.", "Пустая строка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (decimal.TryParse(sum, out decimal amount))
            {
                var categoryValue = category.SelectedItem;
                if (categoryValue == null)
                {
                    MessageBox.Show("Выберите категорию!", "Пустое поле", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {



                    if (amount > 0)
                    {

                        dateTimePicker1.CustomFormat = "dd.MM.yyy";

                        var task = new Expense
                        {
                            Category = categoryValue.ToString(),
                            Amount = amount,
                            Date = dateTimePicker1.Value,
                        };

                        ExpenseList.Add(task);
                        UpdateData();
                        ResetFields();
                    }
                    else
                    {
                        // Если сумма меньше или равна 0
                        MessageBox.Show("Введите число больше 0.", "Неверный формат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                // Если не удалось преобразовать в decimal
                MessageBox.Show("Введите корректное число.", "Неверный формат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void ResetFields()
        {
            inputSum.Clear();
            dateTimePicker1.Value = DateTime.Now;
            category.SelectedIndex = -1;
            MessageBox.Show("Добавлена новая запись.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CustomizeGridView()
        {
            if (dataGridView1.Columns["Category"] != null)
                dataGridView1.Columns["Category"].HeaderText = "Категория";

            if (dataGridView1.Columns["Amount"] != null)
                dataGridView1.Columns["Amount"].HeaderText = "Сумма";

            if (dataGridView1.Columns["Date"] != null)
                dataGridView1.Columns["Date"].HeaderText = "Дата";
            dataGridView1.Columns["Date"].DefaultCellStyle.Format = "dd.MM.yyyy";
        }

        private void UpdateData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ExpenseList;
            CustomizeGridView();
        }

        private void inputSum_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем ввод только цифр, точку и Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            // Запрещаем ввод более одной точки
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void DeleteRows()
        {
            if (dataGridView1.Rows.Count == 0) 
            { 
                MessageBox.Show("Выберите хотя бы одну строку для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                {
                    int rowIndex = selectedRow.Index;

                    if (rowIndex >= 0 && rowIndex < ExpenseList.Count)
                    {
                        ExpenseList.RemoveAt(rowIndex);
                        UpdateData();
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteRows();
        }
    }
}
