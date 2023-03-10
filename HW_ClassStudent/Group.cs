using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW_Address;


namespace HW_ClassStudent
{
    public class Group
    {
        public const int GEN_STUDENTS = 5;
        public const int MIN_GRADE_EXAM = 40;

        private List<Student> students = new List<Student>();
        private string title;
        private string specialization;
        private int numKurs;

        // случайная генерация студентов с помощью библиотеки 'Faker'
        private void GenStudents()
        {
            for (int i = 0; i < GEN_STUDENTS; i++)
            {
                Address address = new Address();
                students.Add(new Student(Faker.NameFaker.Name(),
                                         Faker.NameFaker.LastName(),
                                         new DateTime(),
                                         address,
                                         Faker.PhoneFaker.Phone()));
            }
        }

        // копия студентов из другого списка в наш список
        private void CopyStudents(List<Student> students)
        {
            if (students != null)
            {
                foreach (Student stud in students)
                {
                    this.students.Add(stud);
                }
            }
        }

        // принимает массив string и заполняет его (имя+фамилия+ID) каждого студента
        private void GetStudentNames(string[] arrNames)
        {
            for (int i = 0; i < students.Count; i++)
            {
                arrNames[i] = students[i].GetName() + " " + students[i].GetSurname() + ", ID(" + students[i].GetID() + ")";
            }
        }

        // удаление студента
        private void DelStudent(Student stud)
        {
            if (stud != null)
            {
                students.Remove(stud);
            }
        }

        // -----

        // конструктор ддл параметров группы
        public Group(string title, string specialization, int numKurs)
        {
            SetTitle(title);
            SetSpecialization(specialization);
            SetNumKurs(numKurs);
        }

        // конструктор по умолчаниюи генерация студентов
        public Group() : this("TitleGroup", "Specialization", 1)
        {
            GenStudents();
        }

        // конструктор для существующего списка студентов
        public Group(List<Student> students) : this("TitleGroup", "Specialization", 1, students) {}

        // конструктор для существующей группы
        public Group(Group group) :
            this(group.GetTitle(), group.GetSpecialization(), group.GetNumKurs(), group.GetStudents()) {}

        // конструктор для существующего списка студентов и доп параметров для группы
        public Group(string title, string specialization, int numKurs, List<Student> students):
            this(title, specialization, numKurs)
        {
            CopyStudents(students);
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetSpecialization()
        {
            return specialization;
        }

        public int GetNumKurs()
        {
            return numKurs;
        }

        public List<Student> GetStudents()
        {
            return students;
        }

        public void SetTitle(string title)
        {
            this.title = (title.Length >= 2) ? title : "TitleGroup";
        }

        public void SetSpecialization(string specialization)
        {
            this.specialization = (specialization.Length >= 3) ? specialization : "Specialization";
        }

        public void SetNumKurs(int numKurs)
        {
            this.numKurs = (numKurs >= 1 && numKurs <= 10) ? numKurs : 1;
        }

        public void ShowGroupInfo()
        {
            Console.WriteLine($"Title group: {title}");
            Console.WriteLine($"Specialization group: {specialization}");
            Console.WriteLine($"Kurs number: {numKurs}");
        }

        public void ShowSortStudents()
        {
            string[] arrNames = new string[students.Count];
            GetStudentNames(arrNames);
            Array.Sort(arrNames);

            ShowGroupInfo();
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {arrNames[i]}");
            }
            Console.WriteLine();
        }

        public void ShowAllStudInfo()
        {
            foreach (Student stud in students)
            {
                Console.WriteLine(stud);
            }
            Console.WriteLine();
        }

        public void AddStudent(Student student)
        {
            if (student != null)
            {
                // если студент которого добавляем есть в списке студентов, то не добавляем (узнаём по ID)
                foreach (Student stud in students)
                {
                    if (stud.GetID() == student.GetID())
                    {
                        return;
                    }
                }
                students.Add(student);
            }
        }

        public Student GetStudentByID(int id)
        {
            foreach (Student stud in students)
            {
                if (stud.GetID() == id)
                {
                    return stud;
                }
            }
            return null;
        }

        public void StudentTransferToAnotherGroup(int id, Group newGroup)
        {
            Student stud = GetStudentByID(id);
            if (stud != null)
            {
                newGroup.AddStudent(GetStudentByID(id));
                DelStudent(stud);
            }
        }

        public void DelStudentsFromExam()
        {
            List<Student> delStudents = new List<Student>();
            foreach (Student stud in students)
            {
                // находим студентов которые провалили экзамены и добавляем в список для дальнейшего удаления
                if (stud.AverageGradeExam() < MIN_GRADE_EXAM)
                {
                    delStudents.Add(stud);
                }
            }
            foreach (Student stud in delStudents)
            {
                DelStudent(stud);
            }
        }

        public void DelBadStudent()
        {
            if (students.Count > 0)
            {
                Student badStud = students[0];
                float result = (badStud.AverageGradeCW() + badStud.AverageGradeHW() + badStud.AverageGradeExam()) / 3;
                float result2 = 0;
                for (int i = 1; i < students.Count; i++)
                {
                    result2 = (students[i].AverageGradeCW() + students[i].AverageGradeHW() + students[i].AverageGradeExam()) / 3;
                    if (result2 < result)
                    {
                        result = result2;
                        badStud = students[i];
                    }
                }
                DelStudent(badStud);
            }
        }
    } 
}
