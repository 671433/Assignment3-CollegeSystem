using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf_Test2ScofFolding.Data;
using Wpf_Test2ScofFolding.Models;

namespace Wpf_Test2ScofFolding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationDBContext _dbContext;

        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
        public List<Grade> Grades { get; set; }

        public ObservableCollection<GradeViewModel> Searchcoures { get; set; }






        public MainWindow()
        {
            InitializeComponent();
            

            _dbContext = new ApplicationDBContext();

            // Assign the Students property with the DbSet from the database
            Students = _dbContext.Students.ToList();
            Courses = _dbContext.Courses.ToList();
            Grades = _dbContext.Grades.ToList();
           
            _dbContext.Students.Load<Student>();
            _dbContext.Courses.Load<Course>();
            _dbContext.Grades.Load<Grade>();


            Searchcoures = new ObservableCollection<GradeViewModel>();


            // Set DataContext
            DataContext = this;



        }

  

        private void btn_Courses_Click(object sender, RoutedEventArgs e)
        {
            Grid_Student.Visibility = Visibility.Hidden;
            Grid_Courses.Visibility = Visibility.Visible;
            Grid_Grads.Visibility = Visibility.Hidden;
            Grid_WhoFailed.Visibility = Visibility.Hidden;
            Grid_manage.Visibility = Visibility.Hidden;
            studentList.ItemsSource = Students.OrderBy(s => s.Studentname);


        }

        private void btn_student_Click(object sender, RoutedEventArgs e)
        {

            Grid_Student.Visibility = Visibility.Visible;
            Grid_Courses.Visibility = Visibility.Hidden;
            Grid_Grads.Visibility = Visibility.Hidden;
            Grid_WhoFailed.Visibility = Visibility.Hidden;
            Grid_manage.Visibility = Visibility.Hidden;

        }

        private void btn_Grades_Click(object sender, RoutedEventArgs e)
        {
            Grid_Student.Visibility = Visibility.Hidden;
            Grid_Courses.Visibility = Visibility.Hidden;
            Grid_Grads.Visibility = Visibility.Visible;
            Grid_WhoFailed.Visibility = Visibility.Hidden;
            Grid_manage.Visibility = Visibility.Hidden;
        }

        private void btn_WhoFailed_Click(object sender, RoutedEventArgs e)
        {
            Grid_Student.Visibility = Visibility.Hidden;
            Grid_Courses.Visibility = Visibility.Hidden;
            Grid_Grads.Visibility = Visibility.Hidden;
            Grid_WhoFailed.Visibility = Visibility.Visible;
            Grid_manage.Visibility = Visibility.Hidden;
        }

        private void btn_manageParicipants_Click(object sender, RoutedEventArgs e)
        {
            Grid_Student.Visibility = Visibility.Hidden;
            Grid_Courses.Visibility = Visibility.Hidden;
            Grid_Grads.Visibility = Visibility.Hidden;
            Grid_WhoFailed.Visibility = Visibility.Hidden;
            Grid_manage.Visibility = Visibility.Visible;
        }

        private void btn_StudentSerach_Click(object sender, RoutedEventArgs e)
        {
            string textFromSearch = tb_Student.Text.Trim().ToLower();
            if(string.IsNullOrEmpty(textFromSearch))
            {
                //if empty, all students shows with messagebox
                studentList.ItemsSource = Students.OrderBy(s => s.Studentname);
                MessageBox.Show("Sorry! You didn't enter a name, all students shows!");
            }
            else
            {
                var nyList = Students.Where(s => s.Studentname.ToLower().Contains(textFromSearch)).ToList();
                if (nyList.Count > 0)
                {
                    studentList.ItemsSource = nyList;
                }
                else
                {
                    MessageBox.Show("Sorry! Student does NOT found, try agin!");
                }
            }
        }



        private void tb_studentsCountShow_Loaded(object sender, RoutedEventArgs e)
        {
           string studentsConunt  = _dbContext.Students.Count().ToString();
            tb_studentsCountShow.Text = studentsConunt;
        }

        private void btn_searchCourseButton_Click(object sender, RoutedEventArgs e)
        {

            if(courseComboBox.SelectedItem != null)
            {
                Course selectedCourse = (Course)courseComboBox.SelectedItem;
                string selectedCourseCode = selectedCourse.Coursecode.ToString();
                var searcgCours = from grade in _dbContext.Grades
                                  join student in _dbContext.Students
                                  on grade.Studentid equals student.Id
                                  join course in _dbContext.Courses
                                  on grade.Coursecode equals course.Coursecode
                                  where grade.Coursecode == selectedCourseCode
                                  select new GradeViewModel
                                  {
                                      StudentName = student.Studentname,
                                      StudentId = student.Id,
                                      CourseCode = grade.Coursecode,
                                      Semester = course.Semester,
                                      Teacher = course.Teacher,
                                      CourseName = course.Coursename,
                                      Grade1 = grade.Grade1


                                  };

               // CourseList.ItemsSource = _dbContext.Grades.OrderBy(g=> g.Coursecode).ToList();
               CourseList.ItemsSource = searcgCours.ToList();

            }

        }


        private void btn_serachGrads_Click(object sender, RoutedEventArgs e)
        {
            string serachText = tb_serachGrads.Text.ToUpper().Trim();

            List<string> goodInput = ["A","B","C","D","E","F"];
            if (!goodInput.Any(input => serachText.Contains(input)))
            {
                MessageBox.Show("Please choose Grade between serachText a,b,c,d,e,f,A,B,C,D,E,F");
            }


                if ( ! string.IsNullOrWhiteSpace( serachText ))
            {
                var GradList = from grade in _dbContext.Grades
                               join student in _dbContext.Students on grade.Studentid equals student.Id
                               join course in _dbContext.Courses on grade.Coursecode equals course.Coursecode
                               where string.Compare(grade.Grade1.ToUpper(), serachText) <= 0 
                               
                               select new GradeViewModel
                               {
                                   StudentName = student.Studentname,
                                   StudentId = student.Id,
                                   CourseCode = grade.Coursecode,
                                   Semester = course.Semester,
                                   Teacher = course.Teacher,
                                   CourseName = course.Coursename,
                                   Grade1 = grade.Grade1


                               };

                ObservableCollection<GradeViewModel> localGradList = new ObservableCollection<GradeViewModel>(GradList);

                GradsList.ItemsSource = localGradList;
            }
            else
            {
                var GradList2 = from grade in _dbContext.Grades
                               join student in _dbContext.Students on grade.Studentid equals student.Id
                               join course in _dbContext.Courses on grade.Coursecode equals course.Coursecode
                               orderby student.Id
                               select new GradeViewModel
                               {
                                   StudentName = student.Studentname,
                                   StudentId = student.Id,
                                   CourseCode = grade.Coursecode,
                                   Semester = course.Semester,
                                   Teacher = course.Teacher,
                                   CourseName = course.Coursename,
                                   Grade1 = grade.Grade1


                               };

                ObservableCollection<GradeViewModel> localGradList2 = new ObservableCollection<GradeViewModel>(GradList2);

                GradsList.ItemsSource = localGradList2;
            }
        }



        private void btn_whoFailed_Click_1(object sender, RoutedEventArgs e)
        {
            var WhoFailed = from grade in _dbContext.Grades
                            join student in _dbContext.Students on grade.Studentid equals student.Id
                            join course in _dbContext.Courses on grade.Coursecode equals course.Coursecode
                            where grade.Grade1.ToString() == "F"
                            select new GradeViewModel
                            {
                                StudentName = student.Studentname,
                                StudentId = student.Id,
                                CourseCode = grade.Coursecode,
                                Semester = course.Semester,
                                Teacher = course.Teacher,
                                CourseName = course.Coursename,
                                Grade1 = grade.Grade1,
                            };
            ObservableCollection<GradeViewModel> localWhofailedList = new ObservableCollection<GradeViewModel>(WhoFailed);

            WhoFailedList.ItemsSource = localWhofailedList;
        }

        private void btn_addStudentButton_Click(object sender, RoutedEventArgs e)
        {
            string coursecode = tb_courseIdTextBox.Text;
            string studentid = tb_studentIdTextBox.Text;
            string grade = tb_gradeTextBox.Text;


            if(string.IsNullOrEmpty(coursecode) || string.IsNullOrEmpty(studentid) || string.IsNullOrEmpty(grade))
            {
                MessageBox.Show("Please enter all the fields!");
                return;
            }

            var course = _dbContext.Courses.SingleOrDefault(c => c.Coursecode == coursecode);
            var student = _dbContext.Students.SingleOrDefault(s => s.Id == int.Parse(studentid));

            if (course == null) 
            {
                tb_participantResult.Text = "Course not found";
                return;
            }
            if(student == null)
            {
                tb_participantResult.Text = "Student not found";
                return;
            }

            var extitingGrade = _dbContext.Grades.FirstOrDefault(g =>g.Studentid == int.Parse(studentid) && g.Coursecode == coursecode);
            if(extitingGrade != null)
            {
                tb_participantResult.Text = "The student is already registered in the course";
                return;
            }
            var nyGrade = new Grade { Studentid = int.Parse(studentid),Coursecode = coursecode, Grade1= grade };
            _dbContext.Grades.Add(nyGrade);
            _dbContext.SaveChanges();
            tb_participantResult.Text = "Student added to course";
        }

        private void btn_removeStudentButton_Click(object sender, RoutedEventArgs e)
        {
            string coursecode = tb_courseIdTextBox.Text;
            string studentid = tb_studentIdTextBox.Text;
            string grade = tb_gradeTextBox.Text;


            if (string.IsNullOrEmpty(coursecode) || string.IsNullOrEmpty(studentid) )
            {
                MessageBox.Show("Please enter course code and student ID!");
                return;
            }
            var extitingGrade = _dbContext.Grades.FirstOrDefault(g => g.Studentid == int.Parse(studentid) && g.Coursecode == coursecode);
            if(extitingGrade == null)
            {
                tb_participantResult.Text = "The student is NOT registered in the course";
                return;
            }

            _dbContext.Grades.Remove(extitingGrade);
            _dbContext.SaveChanges();
            tb_participantResult.Text = "Student removed from course";
        }
    }
}