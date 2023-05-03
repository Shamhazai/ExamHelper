using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ExamHelper
{
    
    public partial class Form1 : Form
    {
        private List<Question> _questions;
        private readonly Random _random;
        private enum Mode { Exam, Training }
        private Mode mode = Mode.Exam;
        private bool _isTrainingMode = true; // по умолчанию установлен режим тренировки
        private List<Question> _selectedQuestions = new List<Question>();


        public Form1()
        {
            InitializeComponent();
            _random = new Random();
        }

      
        public void Form1_Load(object sender, EventArgs e)
        {
            // Создаем ColumnHeader для заголовка столбца с вопросами
            ColumnHeader chQuestion = new ColumnHeader();
            chQuestion.Text = "Вопрос";
            chQuestion.Width = 400;
            listView1.Columns.Add(chQuestion);

            // Создаем ColumnHeader для заголовка столбца со сложностью вопросов
            ColumnHeader chDifficulty = new ColumnHeader();
            chDifficulty.Text = "Сложность";
            chDifficulty.Width = 100;
            listView1.Columns.Add(chDifficulty);

            // Создаем ColumnHeader для заголовка столбца с ответами
            ColumnHeader chAnswer = new ColumnHeader();
            chAnswer.Text = "Ответ";
            chAnswer.Width = 400;
            listView1.Columns.Add(chAnswer);
        }

        public void btnStart_Click(object sender, EventArgs e)
        {
            // Проверяем, что пользователь ввел число в поле txtDif
            if (!int.TryParse(txtDif.Text, out var totalDifficulty))
            {
                MessageBox.Show("Введите число в поле общей сложности вопросов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Загружаем вопросы из файла
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var questionsFile = openFileDialog.FileName;
                    var questionsData = File.ReadAllLines(questionsFile);
                    _questions = questionsData
                    .Select(q => q.Split(';'))
                    .Select(q => new Question(q[0], q[1], int.Parse(q[2])))
                    .ToList();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось загрузить файл с вопросами.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                return;
            }

            // Выбираем вопросы, пока не достигнута заданная сумма сложностей
            var selectedQuestions = new List<Question>();
            while (selectedQuestions.Sum(q => q.Difficulty) < totalDifficulty)
            {
                var candidateQuestions = _questions // Получаем список вопросов, из которых будем выбирать
                    .Where(q => !selectedQuestions.Contains(q)) // Оставляем только те, которые еще не были выбраны
                    .Where(q => q.Difficulty <= totalDifficulty - selectedQuestions.Sum(sq => sq.Difficulty)) // Оставляем только те, которые поместятся в оставшуюся сумму сложностей
                    .ToList();

                if (candidateQuestions.Count == 0) // Если не нашлось подходящих вопросов, то выходим из цикла
                {
                    break;
                }

                var selectedQuestion = candidateQuestions[_random.Next(candidateQuestions.Count)]; // Выбираем случайный вопрос из оставшихся
                selectedQuestions.Add(selectedQuestion);
            }
            // Очищаем ListView
            listView1.Items.Clear();

            // Отображаем выбранные вопросы в lvQuestions
            listView1.Items.Clear();
            foreach (var question in selectedQuestions)
            {
                var item = new ListViewItem(new[] { question.Text, question.Difficulty.ToString() });
                listView1.Items.Add(item);
            }


        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == "12345")
            {
                _isTrainingMode = true; // установить режим тестирования
                btnStartTest.Enabled = true;
                MessageBox.Show("Вход выполнен успешно!", "Успешный вход");
            }
            else
            {
                MessageBox.Show("Неверный пароль, попробуйте ещё раз.", "Ошибка входа");
            }
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            
     
            // Проверяем, что пользователь ввел число в поле txtDif
            if (!int.TryParse(txtDif.Text, out var totalDifficulty))
            {
                MessageBox.Show("Введите число в поле общей сложности вопросов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Загружаем вопросы из файла
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var questionsFile = openFileDialog.FileName;
                    var questionsData = File.ReadAllLines(questionsFile);
                    _questions = questionsData
                    .Select(q => q.Split(';'))
                    .Select(q => new Question(q[0], q[1], int.Parse(q[2])))
                    .ToList();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось загрузить файл с вопросами.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                return;
            }

            // Выбираем вопросы, пока не достигнута заданная сумма сложностей
            var selectedQuestions = new List<Question>();
            while (selectedQuestions.Sum(q => q.Difficulty) < totalDifficulty)
            {
                var candidateQuestions = _questions // Получаем список вопросов, из которых будем выбирать
                    .Where(q => !selectedQuestions.Contains(q)) // Оставляем только те, которые еще не были выбраны
                    .Where(q => q.Difficulty <= totalDifficulty - selectedQuestions.Sum(sq => sq.Difficulty)) // Оставляем только те, которые поместятся в оставшуюся сумму сложностей
                    .ToList();

                if (candidateQuestions.Count == 0) // Если не нашлось подходящих вопросов, то выходим из цикла
                {
                    break;
                }

                var selectedQuestion = candidateQuestions[_random.Next(candidateQuestions.Count)]; // Выбираем случайный вопрос из оставшихся
                selectedQuestions.Add(selectedQuestion);
            }
            // Очищаем ListView
            listView1.Items.Clear();

            // Отображаем выбранные вопросы в lvQuestions
            listView1.Items.Clear();
            foreach (var question in selectedQuestions)
            {
                var item = new ListViewItem(new[] { question.Text, question.Difficulty.ToString(), question.Answer });
                listView1.Items.Add(item);
            }


        }
    }
    
}
