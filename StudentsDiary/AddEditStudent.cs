using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;
        private Student _student;


        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;

            GetStudentData();
            tbFirstName.Select();


        }


        private void GetStudentData()
        {
            if (_studentId != 0)
            {

                Text = "Edytowanie danych ucznia";
                _student = _fileHelper.DeserializeFromFile().FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                {
                    throw new Exception("Brak użytkownika o podanym Id");

                }

                FillTextBoxes();

            }

        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbForeignLang.Text = _student.ForeignLang;
            tbMath.Text = _student.Math;
            tbPhysics.Text = _student.Physics;
            tbPolishLang.Text = _student.PolishLang;
            tbTechnology.Text = _student.Technology;
            rtbComments.Text = _student.Comments;
            cbExtraClasses.Checked = _student.ExtraClasses;
            cmbGroupId.SelectedItem = _student.GroupId;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent(students);



            AddNewUserToList(students);

            _fileHelper.SerializeToFile(students);

            Close();
        }

        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                ExtraClasses = cbExtraClasses.Checked,
                Comments = rtbComments.Text,
                ForeignLang = tbForeignLang.Text,
                PolishLang = tbPolishLang.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                Technology = tbTechnology.Text,
                GroupId = cmbGroupId.SelectedItem.ToString()
        };

            students.Add(student);
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
        }

    }
}
