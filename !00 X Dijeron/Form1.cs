using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _00_X_Dijeron
{
    public partial class Form1 : Form
    {
        List<Question> questions = new List<Question>();
        private int currentQuestion = 0;
        private int currentScore = 0;
        private TextBox[] textBoxes;
        private TextBox[] scoreBoxes;
        public Form1()
        {
            InitializeComponent();
            textBoxes = new TextBox[] { txtBox1, txtBox2, txtBox3, txtBox4, txtBox5 };
            scoreBoxes = new TextBox[] { txtBoxScore1, txtBoxScore2, txtBoxScore3, txtBoxScore4, txtBoxScore5 };
            btnX.Visible = false;
            GetQuestions();
            LoadQuestions();
        }

        private void GetQuestions()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Seleccionar archivo de preguntas";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs);
                    String content = sr.ReadToEnd();
                    questions = JsonConvert.DeserializeObject<List<Question>>(content);
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Excepcion Ocurrida", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Seleccion cancelada", "No se ha elegido ningun archivo",MessageBoxButtons.OK);
            }
        }

        private void LoadQuestions()
        {
            Question question = questions[currentQuestion];
            lblQuestion.Text = question.content;
            for(int i = 0; i < textBoxes.Length; i++)
            {
                textBoxes[i].UseSystemPasswordChar = true;
            }
            for(int i = 0; i < scoreBoxes.Length; i++)
            {
                scoreBoxes[i].UseSystemPasswordChar = true;
            }
            for (int i = 0; i < question.answers.Count; i++)
            {
                textBoxes[i].Text = question.answers[i].content;
                scoreBoxes[i].Text = question.answers[i].value.ToString();
            }
            rbtnTeam1.Checked = false;
            rbtnTeam2.Checked = false;
        }

        private void ShowAnswer(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int aux = int.Parse(btn.Text)-1;
            textBoxes[aux].UseSystemPasswordChar = false;
            scoreBoxes[aux].UseSystemPasswordChar = false;
            if (!rbtnNone.Checked)
            {
                currentScore += int.Parse(scoreBoxes[aux].Text);
            }
        }
        private void NextQuestion(object sender, EventArgs e)
        {
            if(rbtnTeam1.Checked)
            {
                txtBoxScoreTeam1.Text = (int.Parse(txtBoxScoreTeam1.Text) + currentScore).ToString();
            }
            if (rbtnTeam2.Checked)
            {
                txtBoxScoreTeam2.Text = (int.Parse(txtBoxScoreTeam2.Text) + currentScore).ToString();
            }
            currentScore = 0;
            if (currentQuestion == questions.Count-1)
            {
                if (int.Parse(txtBoxScoreTeam1.Text) > int.Parse(txtBoxScoreTeam2.Text))
                {
                    MessageBox.Show("El ganador es el equipo: 1", "Game Over", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("El ganador es el equipo: 2", "Game Over", MessageBoxButtons.OK);
                }
                return;
            }
            currentQuestion++;
            LoadQuestions();
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            btnX.Visible =! btnX.Visible;
        }
    }
}
