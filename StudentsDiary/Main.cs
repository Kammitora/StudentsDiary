using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class Main : Form
    {

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private List<Student> studentsForRefresh;

        public bool IsMaximize
        {
            get
            {
                return Settings.Default.isMaximize;
            }
            set
            {
                Settings.Default.isMaximize = value;
            }
        }

        public Main()
        {
            InitializeComponent();
            RefreshDiary();
            SetColumnHeader();

            if (IsMaximize)
            {
                WindowState = FormWindowState.Maximized;
            }

        }

        private void RefreshDiary()
        {
            
            if (cmbFilter.SelectedIndex == 0)
            {
                studentsForRefresh = _fileHelper.DeserializeFromFile().OrderBy(x => x.Id).ToList();
            }
            else
            {

                studentsForRefresh = _fileHelper.DeserializeFromFile().Where(x => x.GroupId == cmbFilter.SelectedItem.ToString()).OrderBy(x => x.Id).ToList();
            }
            dgvDiary.DataSource = studentsForRefresh;


        }

        private void SetColumnHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Klasa";
            dgvDiary.Columns[4].HeaderText = "Zajęcia dodatkowe";
            dgvDiary.Columns[5].HeaderText = "Uwagi";
            dgvDiary.Columns[6].HeaderText = "Matematyka";
            dgvDiary.Columns[7].HeaderText = "Technologia";
            dgvDiary.Columns[8].HeaderText = "Fizyka";
            dgvDiary.Columns[9].HeaderText = "Język polski";
            dgvDiary.Columns[10].HeaderText = "Język obcy";

        }




        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznaczyć ucznie, którego dane chcesz edytować.");
                return;
            }

            var addEditStudent = new AddEditStudent(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.ShowDialog();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznaczyć ucznie, którego dane chcesz edytować.");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete =
                MessageBox.Show($"Czy na pewno chcesz usubąć ucznia {(selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim()} ?",
                "Usuwanie ucznia", MessageBoxButtons.YesNo);

            if (confirmDelete == DialogResult.Yes)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }

        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void dgvDiary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                IsMaximize = true;

            else
                IsMaximize = false;

            Settings.Default.Save();

        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
                RefreshDiary();
        }
    }

}
