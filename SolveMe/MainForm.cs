using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoPhaseSolver;

namespace SolveMe
{
    public partial class MainForm : Form
    {
        public enum Colors { red, orange, white, yellow, green, blue }
        Colors[] myCube = new Colors[54];
        readonly Cube c = TwoPhaseSolver.Move.randmove(1).apply(new Cube());
        readonly Cubie[] corners = new Cubie[8];
        readonly Cubie[] edges = new Cubie[12];
        List<string> lstMerged = new List<string>();
        public string fridrichSolution = string.Empty;
        public MainForm()
        {
            InitializeComponent();
            InitializeColor();
            label1.Font = new Font("Comic Sans MS", 9);
            label2.Font = new Font("Comic Sans MS", 12);
            label3.Font = new Font("Comic Sans MS", 12);
            label4.Font = new Font("Comic Sans MS", 9);
            label1.Text = "";
            label2.Text = "Kociemba Solution:";
            label3.Text = "Fridrich Solution:";
            label4.Text = "";
            SetEventForButton();
            Text = "Trần Quang Vinh";
            Bitmap bmpIcon = Base64StringToBitmap("/9j/4AAQSkZJRgABAQEAYABgAAD/4QEFRXhpZgAATU0AKgAAAAgACwEAAAMAAAABAMgAAAEBAAMAAAABAMgAAAECAAMAAAADAAAAkgEGAAMAAAABAAIAAAEVAAMAAAABAAMAAAEaAAUAAAABAAAAmAEbAAUAAAABAAAAoAEoAAMAAAABAAIAAAExAAIAAAALAAAAqAEyAAIAAAAUAAAAs4dpAAQAAAABAAAAxwAAAAAACAAIAAgAEk+AAAAnEAAST4AAACcQUGhvdG9TY2FwZQAyMDE4OjExOjIzIDE1OjQ2OjE1AAAEkAAABwAAAAQwMjIxoAEAAwAAAAH//wAAoAIABAAAAAEAAAOEoAMABAAAAAEAAAOEAAAAAP/hDk1odHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+Cjx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IlhNUCBDb3JlIDQuMS4xLUV4aXYyIj4KIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIKICAgIHhtbG5zOnhhcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIgogICAgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIKICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgIHhtbG5zOnBob3Rvc2hvcD0iaHR0cDovL25zLmFkb2JlLmNvbS9waG90b3Nob3AvMS4wLyIKICAgIHhtbG5zOnhhcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgeGFwTU06RG9jdW1lbnRJRD0iRTg3QkEwMTRDQzY1RENDQTg3NzA1MzQyMEQ4QkUwQTciCiAgIHhhcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NzBDODFEM0RGQ0VFRTgxMTlCRjM4MkI1NDE1MjkwQTEiCiAgIHhhcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0iRTg3QkEwMTRDQzY1RENDQTg3NzA1MzQyMEQ4QkUwQTciCiAgIGRjOmZvcm1hdD0iaW1hZ2UvanBlZyIKICAgcGhvdG9zaG9wOkxlZ2FjeUlQVENEaWdlc3Q9IjJEQzc3MkY3MTkwRkI3REI1QjY4QTlENEVGRUY5MjA2IgogICBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIgogICB4YXA6Q3JlYXRlRGF0ZT0iMjAxOC0xMS0yM1QxNTozNjoyOCswNzowMCIKICAgeGFwOk1vZGlmeURhdGU9IjIwMTgtMTEtMjNUMTU6NDY6MTUrMDc6MDAiCiAgIHhhcDpNZXRhZGF0YURhdGU9IjIwMTgtMTEtMjNUMTU6NDY6MTUrMDc6MDAiCiAgIHhhcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiPgogICA8eGFwTU06SGlzdG9yeT4KICAgIDxyZGY6U2VxPgogICAgIDxyZGY6bGkKICAgICAgc3RFdnQ6YWN0aW9uPSJzYXZlZCIKICAgICAgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDo0NDJFNEQyMkZDRUVFODExOTg5RkQ3MjYzNEEyMTMwOSIKICAgICAgc3RFdnQ6d2hlbj0iMjAxOC0xMS0yM1QxNTo0NTozMCswNzowMCIKICAgICAgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiCiAgICAgIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4KICAgICA8cmRmOmxpCiAgICAgIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiCiAgICAgIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6NzBDODFEM0RGQ0VFRTgxMTlCRjM4MkI1NDE1MjkwQTEiCiAgICAgIHN0RXZ0OndoZW49IjIwMTgtMTEtMjNUMTU6NDY6MTUrMDc6MDAiCiAgICAgIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIgogICAgICBzdEV2dDpjaGFuZ2VkPSIvIi8+CiAgICA8L3JkZjpTZXE+CiAgIDwveGFwTU06SGlzdG9yeT4KICA8L3JkZjpEZXNjcmlwdGlvbj4KIDwvcmRmOlJERj4KPC94OnhtcG1ldGE+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAKPD94cGFja2V0IGVuZD0idyI/Pv/tACxQaG90b3Nob3AgMy4wADhCSU0EBAAAAAAADxwBWgADGyVHHAIAAAIAAQD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCABkAGQDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hp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");
            Icon = Icon.FromHandle(bmpIcon.GetHicon());
            //calculateAverageMoves();
        }
        public void SetEventForButton()
        {
            for (int i = 0; i < 54; i++)
            {
                string s = "button" + i;
                Button btn = this.Controls.Find(s, true).FirstOrDefault() as Button;
                btn.Click += Button_ChangeColor;
            }
            string[] colors = { "Red", "Green", "Yellow", "Orange", "Blue", "White" };
            foreach (string color in colors)
            {
                Button btn = this.Controls.Find(color, true).FirstOrDefault() as Button;
                btn.Click += Button_ChangeRadio;
            }
        }
        public void InitializeColor()
        {
            for (int i = 0; i < 54; i++)
            {
                myCube[i] = Colors.white;
            }
        }
        public void Button_ChangeColor(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Color currentColor = new Color();
            if (radioGreen.Checked)
            {
                currentColor = Color.Green;
            }
            else if (radioRed.Checked)
            {
                currentColor = Color.Red;
            }
            else if (radioYellow.Checked)
            {
                currentColor = Color.Yellow;
            }
            else if (radioOrange.Checked)
            {
                currentColor = Color.Orange;
            }
            else if (radioBlue.Checked)
            {
                currentColor = Color.Blue;
            }
            else
            {
                currentColor = Color.White;
            }
            if (btn.BackColor == currentColor)
            {
                btn.BackColor = Color.White;
            }
            else btn.BackColor = currentColor;
        }
        public void Button_ChangeRadio(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string radioName = "radio" + btn.Name;
            RadioButton RadioBtn = this.Controls.Find(radioName, true).FirstOrDefault() as RadioButton;
            RadioBtn.Checked = true;
        }
        public void ButtonSolve_Click(object sender, EventArgs e)
        {
            getCurrentScramble();
            CenterOrient();
            BindingStateForKociembaCube();
            if (checkFinished())
            {
                MessageBox.Show("This cube is already solved.\n\nTry something harder :)");
            }
            else
            {
                fridrichSolution = Solve(ref myCube);
                if (checkFinished())
                {
                    //execute kociemba algorithm
                    c.corners = corners;
                    c.edges = edges;
                    List<string> result = Search.patternSolve(c, TwoPhaseSolver.Move.None, 22, printInfo: true);
                    label1.Text = "Phase 1: " + result[0] + "\nPhase 2: " + result[1];
                    label2.Text = "Kociemba Solution: " + result[2] + " moves";
                    label3.Text = "Fridrich Solution: " + lstMerged.Count + " moves";
                    label4.Text = fridrichSolution;
                }
                else
                {
                    MessageBox.Show("This cube cannot be solved.");
                }
            }
        }
        public void BindingStateForKociembaCube()
        {
            #region Corner0
            //0-0
            if (myCube[8] == Colors.white && myCube[11] == Colors.green && myCube[18] == Colors.red)
            {
                corners[0] = new Cubie(0, 0);
            }
            else if (myCube[8] == Colors.red && myCube[11] == Colors.white && myCube[18] == Colors.green)
            {
                corners[0] = new Cubie(0, 2);
            }
            else if (myCube[8] == Colors.green && myCube[11] == Colors.red && myCube[18] == Colors.white)
            {
                corners[0] = new Cubie(0, 1);
            }
            //0-1
            else if (myCube[8] == Colors.white && myCube[11] == Colors.orange && myCube[18] == Colors.green)
            {
                corners[0] = new Cubie(1, 0);
            }
            else if (myCube[8] == Colors.green && myCube[11] == Colors.white && myCube[18] == Colors.orange)
            {
                corners[0] = new Cubie(1, 2);
            }
            else if (myCube[8] == Colors.orange && myCube[11] == Colors.green && myCube[18] == Colors.white)
            {
                corners[0] = new Cubie(1, 1);
            }
            //0-2
            else if (myCube[8] == Colors.white && myCube[11] == Colors.blue && myCube[18] == Colors.orange)
            {
                corners[0] = new Cubie(2, 0);
            }
            else if (myCube[8] == Colors.orange && myCube[11] == Colors.white && myCube[18] == Colors.blue)
            {
                corners[0] = new Cubie(2, 2);
            }
            else if (myCube[8] == Colors.blue && myCube[11] == Colors.orange && myCube[18] == Colors.white)
            {
                corners[0] = new Cubie(2, 1);
            }
            //0-3
            else if (myCube[8] == Colors.white && myCube[11] == Colors.red && myCube[18] == Colors.blue)
            {
                corners[0] = new Cubie(3, 0);
            }
            else if (myCube[8] == Colors.blue && myCube[11] == Colors.white && myCube[18] == Colors.red)
            {
                corners[0] = new Cubie(3, 2);
            }
            else if (myCube[8] == Colors.red && myCube[11] == Colors.blue && myCube[18] == Colors.white)
            {
                corners[0] = new Cubie(3, 1);
            }
            //0-4
            else if (myCube[8] == Colors.yellow && myCube[11] == Colors.red && myCube[18] == Colors.green)
            {
                corners[0] = new Cubie(4, 0);
            }
            else if (myCube[8] == Colors.green && myCube[11] == Colors.yellow && myCube[18] == Colors.red)
            {
                corners[0] = new Cubie(4, 2);
            }
            else if (myCube[8] == Colors.red && myCube[11] == Colors.green && myCube[18] == Colors.yellow)
            {
                corners[0] = new Cubie(4, 1);
            }
            //0-5
            else if (myCube[8] == Colors.yellow && myCube[11] == Colors.green && myCube[18] == Colors.orange)
            {
                corners[0] = new Cubie(5, 0);
            }
            else if (myCube[8] == Colors.orange && myCube[11] == Colors.yellow && myCube[18] == Colors.green)
            {
                corners[0] = new Cubie(5, 2);
            }
            else if (myCube[8] == Colors.green && myCube[11] == Colors.orange && myCube[18] == Colors.yellow)
            {
                corners[0] = new Cubie(5, 1);
            }
            //0-6
            else if (myCube[8] == Colors.yellow && myCube[11] == Colors.orange && myCube[18] == Colors.blue)
            {
                corners[0] = new Cubie(6, 0);
            }
            else if (myCube[8] == Colors.blue && myCube[11] == Colors.yellow && myCube[18] == Colors.orange)
            {
                corners[0] = new Cubie(6, 2);
            }
            else if (myCube[8] == Colors.orange && myCube[11] == Colors.blue && myCube[18] == Colors.yellow)
            {
                corners[0] = new Cubie(6, 1);
            }
            //0-7
            else if (myCube[8] == Colors.yellow && myCube[11] == Colors.blue && myCube[18] == Colors.red)
            {
                corners[0] = new Cubie(7, 0);
            }
            else if (myCube[8] == Colors.red && myCube[11] == Colors.yellow && myCube[18] == Colors.blue)
            {
                corners[0] = new Cubie(7, 2);
            }
            else if (myCube[8] == Colors.blue && myCube[11] == Colors.red && myCube[18] == Colors.yellow)
            {
                corners[0] = new Cubie(7, 1);
            }
            #endregion
            #region Corner1
            //1-0
            if (myCube[6] == Colors.white && myCube[38] == Colors.green && myCube[9] == Colors.red)
            {
                corners[1] = new Cubie(0, 0);
            }
            else if (myCube[6] == Colors.red && myCube[38] == Colors.white && myCube[9] == Colors.green)
            {
                corners[1] = new Cubie(0, 2);
            }
            else if (myCube[6] == Colors.green && myCube[38] == Colors.red && myCube[9] == Colors.white)
            {
                corners[1] = new Cubie(0, 1);
            }
            //1-1
            else if (myCube[6] == Colors.white && myCube[38] == Colors.orange && myCube[9] == Colors.green)
            {
                corners[1] = new Cubie(1, 0);
            }
            else if (myCube[6] == Colors.green && myCube[38] == Colors.white && myCube[9] == Colors.orange)
            {
                corners[1] = new Cubie(1, 2);
            }
            else if (myCube[6] == Colors.orange && myCube[38] == Colors.green && myCube[9] == Colors.white)
            {
                corners[1] = new Cubie(1, 1);
            }
            //1-2
            else if (myCube[6] == Colors.white && myCube[38] == Colors.blue && myCube[9] == Colors.orange)
            {
                corners[1] = new Cubie(2, 0);
            }
            else if (myCube[6] == Colors.orange && myCube[38] == Colors.white && myCube[9] == Colors.blue)
            {
                corners[1] = new Cubie(2, 2);
            }
            else if (myCube[6] == Colors.blue && myCube[38] == Colors.orange && myCube[9] == Colors.white)
            {
                corners[1] = new Cubie(2, 1);
            }
            //1-3
            else if (myCube[6] == Colors.white && myCube[38] == Colors.red && myCube[9] == Colors.blue)
            {
                corners[1] = new Cubie(3, 0);
            }
            else if (myCube[6] == Colors.blue && myCube[38] == Colors.white && myCube[9] == Colors.red)
            {
                corners[1] = new Cubie(3, 2);
            }
            else if (myCube[6] == Colors.red && myCube[38] == Colors.blue && myCube[9] == Colors.white)
            {
                corners[1] = new Cubie(3, 1);
            }
            //1-4
            else if (myCube[6] == Colors.yellow && myCube[38] == Colors.red && myCube[9] == Colors.green)
            {
                corners[1] = new Cubie(4, 0);
            }
            else if (myCube[6] == Colors.green && myCube[38] == Colors.yellow && myCube[9] == Colors.red)
            {
                corners[1] = new Cubie(4, 2);
            }
            else if (myCube[6] == Colors.red && myCube[38] == Colors.green && myCube[9] == Colors.yellow)
            {
                corners[1] = new Cubie(4, 1);
            }
            else //1-5
            if (myCube[6] == Colors.yellow && myCube[38] == Colors.green && myCube[9] == Colors.orange)
            {
                corners[1] = new Cubie(5, 0);
            }
            else if (myCube[6] == Colors.orange && myCube[38] == Colors.yellow && myCube[9] == Colors.green)
            {
                corners[1] = new Cubie(5, 2);
            }
            else if (myCube[6] == Colors.green && myCube[38] == Colors.orange && myCube[9] == Colors.yellow)
            {
                corners[1] = new Cubie(5, 1);
            }
            else //1-6
            if (myCube[6] == Colors.yellow && myCube[38] == Colors.orange && myCube[9] == Colors.blue)
            {
                corners[1] = new Cubie(6, 0);
            }
            else if (myCube[6] == Colors.blue && myCube[38] == Colors.yellow && myCube[9] == Colors.orange)
            {
                corners[1] = new Cubie(6, 2);
            }
            else if (myCube[6] == Colors.orange && myCube[38] == Colors.blue && myCube[9] == Colors.yellow)
            {
                corners[1] = new Cubie(6, 1);
            }
            //1-7
            else if (myCube[6] == Colors.yellow && myCube[38] == Colors.blue && myCube[9] == Colors.red)
            {
                corners[1] = new Cubie(7, 0);
            }
            else if (myCube[6] == Colors.red && myCube[38] == Colors.yellow && myCube[9] == Colors.blue)
            {
                corners[1] = new Cubie(7, 2);
            }
            else if (myCube[6] == Colors.blue && myCube[38] == Colors.red && myCube[9] == Colors.yellow)
            {
                corners[1] = new Cubie(7, 1);
            }
            #endregion
            #region Corner2
            //2-0
            if (myCube[0] == Colors.white && myCube[29] == Colors.green && myCube[36] == Colors.red)
            {
                corners[2] = new Cubie(0, 0);
            }
            else if (myCube[0] == Colors.red && myCube[29] == Colors.white && myCube[36] == Colors.green)
            {
                corners[2] = new Cubie(0, 2);
            }
            else if (myCube[0] == Colors.green && myCube[29] == Colors.red && myCube[36] == Colors.white)
            {
                corners[2] = new Cubie(0, 1);
            }
            //2-1
            else if (myCube[0] == Colors.white && myCube[29] == Colors.orange && myCube[36] == Colors.green)
            {
                corners[2] = new Cubie(1, 0);
            }
            else if (myCube[0] == Colors.green && myCube[29] == Colors.white && myCube[36] == Colors.orange)
            {
                corners[2] = new Cubie(1, 2);
            }
            else if (myCube[0] == Colors.orange && myCube[29] == Colors.green && myCube[36] == Colors.white)
            {
                corners[2] = new Cubie(1, 1);
            }
            //2-2
            else if (myCube[0] == Colors.white && myCube[29] == Colors.blue && myCube[36] == Colors.orange)
            {
                corners[2] = new Cubie(2, 0);
            }
            else if (myCube[0] == Colors.orange && myCube[29] == Colors.white && myCube[36] == Colors.blue)
            {
                corners[2] = new Cubie(2, 2);
            }
            else if (myCube[0] == Colors.blue && myCube[29] == Colors.orange && myCube[36] == Colors.white)
            {
                corners[2] = new Cubie(2, 1);
            }
            //2-3
            else if (myCube[0] == Colors.white && myCube[29] == Colors.red && myCube[36] == Colors.blue)
            {
                corners[2] = new Cubie(3, 0);
            }
            else if (myCube[0] == Colors.blue && myCube[29] == Colors.white && myCube[36] == Colors.red)
            {
                corners[2] = new Cubie(3, 2);
            }
            else if (myCube[0] == Colors.red && myCube[29] == Colors.blue && myCube[36] == Colors.white)
            {
                corners[2] = new Cubie(3, 1);
            }
            //2-4
            else if (myCube[0] == Colors.yellow && myCube[29] == Colors.red && myCube[36] == Colors.green)
            {
                corners[2] = new Cubie(4, 0);
            }
            else if (myCube[0] == Colors.green && myCube[29] == Colors.yellow && myCube[36] == Colors.red)
            {
                corners[2] = new Cubie(4, 2);
            }
            else if (myCube[0] == Colors.red && myCube[29] == Colors.green && myCube[36] == Colors.yellow)
            {
                corners[2] = new Cubie(4, 1);
            }
            //2-5
            else if (myCube[0] == Colors.yellow && myCube[29] == Colors.green && myCube[36] == Colors.orange)
            {
                corners[2] = new Cubie(5, 0);
            }
            else if (myCube[0] == Colors.orange && myCube[29] == Colors.yellow && myCube[36] == Colors.green)
            {
                corners[2] = new Cubie(5, 2);
            }
            else if (myCube[0] == Colors.green && myCube[29] == Colors.orange && myCube[36] == Colors.yellow)
            {
                corners[2] = new Cubie(5, 1);
            }
            //2-6
            else if (myCube[0] == Colors.yellow && myCube[29] == Colors.orange && myCube[36] == Colors.blue)
            {
                corners[2] = new Cubie(6, 0);
            }
            else if (myCube[0] == Colors.blue && myCube[29] == Colors.yellow && myCube[36] == Colors.orange)
            {
                corners[2] = new Cubie(6, 2);
            }
            else if (myCube[0] == Colors.orange && myCube[29] == Colors.blue && myCube[36] == Colors.yellow)
            {
                corners[2] = new Cubie(6, 1);
            }
            //2-7
            else if (myCube[0] == Colors.yellow && myCube[29] == Colors.blue && myCube[36] == Colors.red)
            {
                corners[2] = new Cubie(7, 0);
            }
            else if (myCube[0] == Colors.red && myCube[29] == Colors.yellow && myCube[36] == Colors.blue)
            {
                corners[2] = new Cubie(7, 2);
            }
            else if (myCube[0] == Colors.blue && myCube[29] == Colors.red && myCube[36] == Colors.yellow)
            {
                corners[2] = new Cubie(7, 1);
            }
            #endregion
            #region Corner3
            //3-0
            if (myCube[2] == Colors.white && myCube[20] == Colors.green && myCube[27] == Colors.red)
            {
                corners[3] = new Cubie(0, 0);
            }
            else if (myCube[2] == Colors.red && myCube[20] == Colors.white && myCube[27] == Colors.green)
            {
                corners[3] = new Cubie(0, 2);
            }
            else if (myCube[2] == Colors.green && myCube[20] == Colors.red && myCube[27] == Colors.white)
            {
                corners[3] = new Cubie(0, 1);
            }
            //3-1
            else if (myCube[2] == Colors.white && myCube[20] == Colors.orange && myCube[27] == Colors.green)
            {
                corners[3] = new Cubie(1, 0);
            }
            else if (myCube[2] == Colors.green && myCube[20] == Colors.white && myCube[27] == Colors.orange)
            {
                corners[3] = new Cubie(1, 2);
            }
            else if (myCube[2] == Colors.orange && myCube[20] == Colors.green && myCube[27] == Colors.white)
            {
                corners[3] = new Cubie(1, 1);
            }
            //3-2
            else if (myCube[2] == Colors.white && myCube[20] == Colors.blue && myCube[27] == Colors.orange)
            {
                corners[3] = new Cubie(2, 0);
            }
            else if (myCube[2] == Colors.orange && myCube[20] == Colors.white && myCube[27] == Colors.blue)
            {
                corners[3] = new Cubie(2, 2);
            }
            else if (myCube[2] == Colors.blue && myCube[20] == Colors.orange && myCube[27] == Colors.white)
            {
                corners[3] = new Cubie(2, 1);
            }
            //3-3
            else if (myCube[2] == Colors.white && myCube[20] == Colors.red && myCube[27] == Colors.blue)
            {
                corners[3] = new Cubie(3, 0);
            }
            else if (myCube[2] == Colors.blue && myCube[20] == Colors.white && myCube[27] == Colors.red)
            {
                corners[3] = new Cubie(3, 2);
            }
            else if (myCube[2] == Colors.red && myCube[20] == Colors.blue && myCube[27] == Colors.white)
            {
                corners[3] = new Cubie(3, 1);
            }
            //3-4
            else if (myCube[2] == Colors.yellow && myCube[20] == Colors.red && myCube[27] == Colors.green)
            {
                corners[3] = new Cubie(4, 0);
            }
            else if (myCube[2] == Colors.green && myCube[20] == Colors.yellow && myCube[27] == Colors.red)
            {
                corners[3] = new Cubie(4, 2);
            }
            else if (myCube[2] == Colors.red && myCube[20] == Colors.green && myCube[27] == Colors.yellow)
            {
                corners[3] = new Cubie(4, 1);
            }
            //3-5
            else if (myCube[2] == Colors.yellow && myCube[20] == Colors.green && myCube[27] == Colors.orange)
            {
                corners[3] = new Cubie(5, 0);
            }
            else if (myCube[2] == Colors.orange && myCube[20] == Colors.yellow && myCube[27] == Colors.green)
            {
                corners[3] = new Cubie(5, 2);
            }
            else if (myCube[2] == Colors.green && myCube[20] == Colors.orange && myCube[27] == Colors.yellow)
            {
                corners[3] = new Cubie(5, 1);
            }
            //3-6
            else if (myCube[2] == Colors.yellow && myCube[20] == Colors.orange && myCube[27] == Colors.blue)
            {
                corners[3] = new Cubie(6, 0);
            }
            else if (myCube[2] == Colors.blue && myCube[20] == Colors.yellow && myCube[27] == Colors.orange)
            {
                corners[3] = new Cubie(6, 2);
            }
            else if (myCube[2] == Colors.orange && myCube[20] == Colors.blue && myCube[27] == Colors.yellow)
            {
                corners[3] = new Cubie(6, 1);
            }
            //3-7
            else if (myCube[2] == Colors.yellow && myCube[20] == Colors.blue && myCube[27] == Colors.red)
            {
                corners[3] = new Cubie(7, 0);
            }
            else if (myCube[2] == Colors.red && myCube[20] == Colors.yellow && myCube[27] == Colors.blue)
            {
                corners[3] = new Cubie(7, 2);
            }
            else if (myCube[2] == Colors.blue && myCube[20] == Colors.red && myCube[27] == Colors.yellow)
            {
                corners[3] = new Cubie(7, 1);
            }
            #endregion
            #region Corner4
            //4-0
            if (myCube[47] == Colors.white && myCube[24] == Colors.green && myCube[17] == Colors.red)
            {
                corners[4] = new Cubie(0, 0);
            }
            else if (myCube[47] == Colors.red && myCube[24] == Colors.white && myCube[17] == Colors.green)
            {
                corners[4] = new Cubie(0, 2);
            }
            else if (myCube[47] == Colors.green && myCube[24] == Colors.red && myCube[17] == Colors.white)
            {
                corners[4] = new Cubie(0, 1);
            }
            //4-1
            else if (myCube[47] == Colors.white && myCube[24] == Colors.orange && myCube[17] == Colors.green)
            {
                corners[4] = new Cubie(1, 0);
            }
            else if (myCube[47] == Colors.green && myCube[24] == Colors.white && myCube[17] == Colors.orange)
            {
                corners[4] = new Cubie(1, 2);
            }
            else if (myCube[47] == Colors.orange && myCube[24] == Colors.green && myCube[17] == Colors.white)
            {
                corners[4] = new Cubie(1, 1);
            }
            //4-2
            else if (myCube[47] == Colors.white && myCube[24] == Colors.blue && myCube[17] == Colors.orange)
            {
                corners[4] = new Cubie(2, 0);
            }
            else if (myCube[47] == Colors.orange && myCube[24] == Colors.white && myCube[17] == Colors.blue)
            {
                corners[4] = new Cubie(2, 2);
            }
            else if (myCube[47] == Colors.blue && myCube[24] == Colors.orange && myCube[17] == Colors.white)
            {
                corners[4] = new Cubie(2, 1);
            }
            //4-3
            else if (myCube[47] == Colors.white && myCube[24] == Colors.red && myCube[17] == Colors.blue)
            {
                corners[4] = new Cubie(3, 0);
            }
            else if (myCube[47] == Colors.blue && myCube[24] == Colors.white && myCube[17] == Colors.red)
            {
                corners[4] = new Cubie(3, 2);
            }
            else if (myCube[47] == Colors.red && myCube[24] == Colors.blue && myCube[17] == Colors.white)
            {
                corners[4] = new Cubie(3, 1);
            }
            //4-4
            else if (myCube[47] == Colors.yellow && myCube[24] == Colors.red && myCube[17] == Colors.green)
            {
                corners[4] = new Cubie(4, 0);
            }
            else if (myCube[47] == Colors.green && myCube[24] == Colors.yellow && myCube[17] == Colors.red)
            {
                corners[4] = new Cubie(4, 2);
            }
            else if (myCube[47] == Colors.red && myCube[24] == Colors.green && myCube[17] == Colors.yellow)
            {
                corners[4] = new Cubie(4, 1);
            }
            //4-5
            else if (myCube[47] == Colors.yellow && myCube[24] == Colors.green && myCube[17] == Colors.orange)
            {
                corners[4] = new Cubie(5, 0);
            }
            else if (myCube[47] == Colors.orange && myCube[24] == Colors.yellow && myCube[17] == Colors.green)
            {
                corners[4] = new Cubie(5, 2);
            }
            else if (myCube[47] == Colors.green && myCube[24] == Colors.orange && myCube[17] == Colors.yellow)
            {
                corners[4] = new Cubie(5, 1);
            }
            //4-6
            else if (myCube[47] == Colors.yellow && myCube[24] == Colors.orange && myCube[17] == Colors.blue)
            {
                corners[4] = new Cubie(6, 0);
            }
            else if (myCube[47] == Colors.blue && myCube[24] == Colors.yellow && myCube[17] == Colors.orange)
            {
                corners[4] = new Cubie(6, 2);
            }
            else if (myCube[47] == Colors.orange && myCube[24] == Colors.blue && myCube[17] == Colors.yellow)
            {
                corners[4] = new Cubie(6, 1);
            }
            //4-7
            else if (myCube[47] == Colors.yellow && myCube[24] == Colors.blue && myCube[17] == Colors.red)
            {
                corners[4] = new Cubie(7, 0);
            }
            else if (myCube[47] == Colors.red && myCube[24] == Colors.yellow && myCube[17] == Colors.blue)
            {
                corners[4] = new Cubie(7, 2);
            }
            else if (myCube[47] == Colors.blue && myCube[24] == Colors.red && myCube[17] == Colors.yellow)
            {
                corners[4] = new Cubie(7, 1);
            }
            #endregion
            #region Corner5
            //5-0
            if (myCube[45] == Colors.white && myCube[15] == Colors.green && myCube[44] == Colors.red)
            {
                corners[5] = new Cubie(0, 0);
            }
            else if (myCube[45] == Colors.red && myCube[15] == Colors.white && myCube[44] == Colors.green)
            {
                corners[5] = new Cubie(0, 2);
            }
            else if (myCube[45] == Colors.green && myCube[15] == Colors.red && myCube[44] == Colors.white)
            {
                corners[5] = new Cubie(0, 1);
            }
            //5-1
            else if (myCube[45] == Colors.white && myCube[15] == Colors.orange && myCube[44] == Colors.green)
            {
                corners[5] = new Cubie(1, 0);
            }
            else if (myCube[45] == Colors.green && myCube[15] == Colors.white && myCube[44] == Colors.orange)
            {
                corners[5] = new Cubie(1, 2);
            }
            else if (myCube[45] == Colors.orange && myCube[15] == Colors.green && myCube[44] == Colors.white)
            {
                corners[5] = new Cubie(1, 1);
            }
            //5-2
            else if (myCube[45] == Colors.white && myCube[15] == Colors.blue && myCube[44] == Colors.orange)
            {
                corners[5] = new Cubie(2, 0);
            }
            else if (myCube[45] == Colors.orange && myCube[15] == Colors.white && myCube[44] == Colors.blue)
            {
                corners[5] = new Cubie(2, 2);
            }
            else if (myCube[45] == Colors.blue && myCube[15] == Colors.orange && myCube[44] == Colors.white)
            {
                corners[5] = new Cubie(2, 1);
            }
            //5-3
            else if (myCube[45] == Colors.white && myCube[15] == Colors.red && myCube[44] == Colors.blue)
            {
                corners[5] = new Cubie(3, 0);
            }
            else if (myCube[45] == Colors.blue && myCube[15] == Colors.white && myCube[44] == Colors.red)
            {
                corners[5] = new Cubie(3, 2);
            }
            else if (myCube[45] == Colors.red && myCube[15] == Colors.blue && myCube[44] == Colors.white)
            {
                corners[5] = new Cubie(3, 1);
            }
            //5-4
            else if (myCube[45] == Colors.yellow && myCube[15] == Colors.red && myCube[44] == Colors.green)
            {
                corners[5] = new Cubie(4, 0);
            }
            else if (myCube[45] == Colors.green && myCube[15] == Colors.yellow && myCube[44] == Colors.red)
            {
                corners[5] = new Cubie(4, 2);
            }
            else if (myCube[45] == Colors.red && myCube[15] == Colors.green && myCube[44] == Colors.yellow)
            {
                corners[5] = new Cubie(4, 1);
            }
            //5-5
            else if (myCube[45] == Colors.yellow && myCube[15] == Colors.green && myCube[44] == Colors.orange)
            {
                corners[5] = new Cubie(5, 0);
            }
            else if (myCube[45] == Colors.orange && myCube[15] == Colors.yellow && myCube[44] == Colors.green)
            {
                corners[5] = new Cubie(5, 2);
            }
            else if (myCube[45] == Colors.green && myCube[15] == Colors.orange && myCube[44] == Colors.yellow)
            {
                corners[5] = new Cubie(5, 1);
            }
            //5-6
            else if (myCube[45] == Colors.yellow && myCube[15] == Colors.orange && myCube[44] == Colors.blue)
            {
                corners[5] = new Cubie(6, 0);
            }
            else if (myCube[45] == Colors.blue && myCube[15] == Colors.yellow && myCube[44] == Colors.orange)
            {
                corners[5] = new Cubie(6, 2);
            }
            else if (myCube[45] == Colors.orange && myCube[15] == Colors.blue && myCube[44] == Colors.yellow)
            {
                corners[5] = new Cubie(6, 1);
            }
            //5-7
            else if (myCube[45] == Colors.yellow && myCube[15] == Colors.blue && myCube[44] == Colors.red)
            {
                corners[5] = new Cubie(7, 0);
            }
            else if (myCube[45] == Colors.red && myCube[15] == Colors.yellow && myCube[44] == Colors.blue)
            {
                corners[5] = new Cubie(7, 2);
            }
            else if (myCube[45] == Colors.blue && myCube[15] == Colors.red && myCube[44] == Colors.yellow)
            {
                corners[5] = new Cubie(7, 1);
            }
            #endregion
            #region Corner6
            //6-0
            if (myCube[51] == Colors.white && myCube[42] == Colors.green && myCube[35] == Colors.red)
            {
                corners[6] = new Cubie(0, 0);
            }
            else if (myCube[51] == Colors.red && myCube[42] == Colors.white && myCube[35] == Colors.green)
            {
                corners[6] = new Cubie(0, 2);
            }
            else if (myCube[51] == Colors.green && myCube[42] == Colors.red && myCube[35] == Colors.white)
            {
                corners[6] = new Cubie(0, 1);
            }
            //6-1
            else if (myCube[51] == Colors.white && myCube[42] == Colors.orange && myCube[35] == Colors.green)
            {
                corners[6] = new Cubie(1, 0);
            }
            else if (myCube[51] == Colors.green && myCube[42] == Colors.white && myCube[35] == Colors.orange)
            {
                corners[6] = new Cubie(1, 2);
            }
            else if (myCube[51] == Colors.orange && myCube[42] == Colors.green && myCube[35] == Colors.white)
            {
                corners[6] = new Cubie(1, 1);
            }
            //6-2
            else if (myCube[51] == Colors.white && myCube[42] == Colors.blue && myCube[35] == Colors.orange)
            {
                corners[6] = new Cubie(2, 0);
            }
            else if (myCube[51] == Colors.orange && myCube[42] == Colors.white && myCube[35] == Colors.blue)
            {
                corners[6] = new Cubie(2, 2);
            }
            else if (myCube[51] == Colors.blue && myCube[42] == Colors.orange && myCube[35] == Colors.white)
            {
                corners[6] = new Cubie(2, 1);
            }
            //6-3
            else if (myCube[51] == Colors.white && myCube[42] == Colors.red && myCube[35] == Colors.blue)
            {
                corners[6] = new Cubie(3, 0);
            }
            else if (myCube[51] == Colors.blue && myCube[42] == Colors.white && myCube[35] == Colors.red)
            {
                corners[6] = new Cubie(3, 2);
            }
            else if (myCube[51] == Colors.red && myCube[42] == Colors.blue && myCube[35] == Colors.white)
            {
                corners[6] = new Cubie(3, 1);
            }
            //6-4
            else if (myCube[51] == Colors.yellow && myCube[42] == Colors.red && myCube[35] == Colors.green)
            {
                corners[6] = new Cubie(4, 0);
            }
            else if (myCube[51] == Colors.green && myCube[42] == Colors.yellow && myCube[35] == Colors.red)
            {
                corners[6] = new Cubie(4, 2);
            }
            else if (myCube[51] == Colors.red && myCube[42] == Colors.green && myCube[35] == Colors.yellow)
            {
                corners[6] = new Cubie(4, 1);
            }
            //6-5
            else if (myCube[51] == Colors.yellow && myCube[42] == Colors.green && myCube[35] == Colors.orange)
            {
                corners[6] = new Cubie(5, 0);
            }
            else if (myCube[51] == Colors.orange && myCube[42] == Colors.yellow && myCube[35] == Colors.green)
            {
                corners[6] = new Cubie(5, 2);
            }
            else if (myCube[51] == Colors.green && myCube[42] == Colors.orange && myCube[35] == Colors.yellow)
            {
                corners[6] = new Cubie(5, 1);
            }
            //6-6
            else if (myCube[51] == Colors.yellow && myCube[42] == Colors.orange && myCube[35] == Colors.blue)
            {
                corners[6] = new Cubie(6, 0);
            }
            else if (myCube[51] == Colors.blue && myCube[42] == Colors.yellow && myCube[35] == Colors.orange)
            {
                corners[6] = new Cubie(6, 2);
            }
            else if (myCube[51] == Colors.orange && myCube[42] == Colors.blue && myCube[35] == Colors.yellow)
            {
                corners[6] = new Cubie(6, 1);
            }
            //6-7
            else if (myCube[51] == Colors.yellow && myCube[42] == Colors.blue && myCube[35] == Colors.red)
            {
                corners[6] = new Cubie(7, 0);
            }
            else if (myCube[51] == Colors.red && myCube[42] == Colors.yellow && myCube[35] == Colors.blue)
            {
                corners[6] = new Cubie(7, 2);
            }
            else if (myCube[51] == Colors.blue && myCube[42] == Colors.red && myCube[35] == Colors.yellow)
            {
                corners[6] = new Cubie(7, 1);
            }
            #endregion
            #region Corner7
            //7-0
            if (myCube[53] == Colors.white && myCube[33] == Colors.green && myCube[26] == Colors.red)
            {
                corners[7] = new Cubie(0, 0);
            }
            else if (myCube[53] == Colors.red && myCube[33] == Colors.white && myCube[26] == Colors.green)
            {
                corners[7] = new Cubie(0, 2);
            }
            else if (myCube[53] == Colors.green && myCube[33] == Colors.red && myCube[26] == Colors.white)
            {
                corners[7] = new Cubie(0, 1);
            }
            //7-1
            else if (myCube[53] == Colors.white && myCube[33] == Colors.orange && myCube[26] == Colors.green)
            {
                corners[7] = new Cubie(1, 0);
            }
            else if (myCube[53] == Colors.green && myCube[33] == Colors.white && myCube[26] == Colors.orange)
            {
                corners[7] = new Cubie(1, 2);
            }
            else if (myCube[53] == Colors.orange && myCube[33] == Colors.green && myCube[26] == Colors.white)
            {
                corners[7] = new Cubie(1, 1);
            }
            //7-2
            else if (myCube[53] == Colors.white && myCube[33] == Colors.blue && myCube[26] == Colors.orange)
            {
                corners[7] = new Cubie(2, 0);
            }
            else if (myCube[53] == Colors.orange && myCube[33] == Colors.white && myCube[26] == Colors.blue)
            {
                corners[7] = new Cubie(2, 2);
            }
            else if (myCube[53] == Colors.blue && myCube[33] == Colors.orange && myCube[26] == Colors.white)
            {
                corners[7] = new Cubie(2, 1);
            }
            //7-3
            else if (myCube[53] == Colors.white && myCube[33] == Colors.red && myCube[26] == Colors.blue)
            {
                corners[7] = new Cubie(3, 0);
            }
            else if (myCube[53] == Colors.blue && myCube[33] == Colors.white && myCube[26] == Colors.red)
            {
                corners[7] = new Cubie(3, 2);
            }
            else if (myCube[53] == Colors.red && myCube[33] == Colors.blue && myCube[26] == Colors.white)
            {
                corners[7] = new Cubie(3, 1);
            }
            //7-4
            else if (myCube[53] == Colors.yellow && myCube[33] == Colors.red && myCube[26] == Colors.green)
            {
                corners[7] = new Cubie(4, 0);
            }
            else if (myCube[53] == Colors.green && myCube[33] == Colors.yellow && myCube[26] == Colors.red)
            {
                corners[7] = new Cubie(4, 2);
            }
            else if (myCube[53] == Colors.red && myCube[33] == Colors.green && myCube[26] == Colors.yellow)
            {
                corners[7] = new Cubie(4, 1);
            }
            //7-5
            else if (myCube[53] == Colors.yellow && myCube[33] == Colors.green && myCube[26] == Colors.orange)
            {
                corners[7] = new Cubie(5, 0);
            }
            else if (myCube[53] == Colors.orange && myCube[33] == Colors.yellow && myCube[26] == Colors.green)
            {
                corners[7] = new Cubie(5, 2);
            }
            else if (myCube[53] == Colors.green && myCube[33] == Colors.orange && myCube[26] == Colors.yellow)
            {
                corners[7] = new Cubie(5, 1);
            }
            //7-6
            else if (myCube[53] == Colors.yellow && myCube[33] == Colors.orange && myCube[26] == Colors.blue)
            {
                corners[7] = new Cubie(6, 0);
            }
            else if (myCube[53] == Colors.blue && myCube[33] == Colors.yellow && myCube[26] == Colors.orange)
            {
                corners[7] = new Cubie(6, 2);
            }
            else if (myCube[53] == Colors.orange && myCube[33] == Colors.blue && myCube[26] == Colors.yellow)
            {
                corners[7] = new Cubie(6, 1);
            }
            //7-7
            else if (myCube[53] == Colors.yellow && myCube[33] == Colors.blue && myCube[26] == Colors.red)
            {
                corners[7] = new Cubie(7, 0);
            }
            else if (myCube[53] == Colors.red && myCube[33] == Colors.yellow && myCube[26] == Colors.blue)
            {
                corners[7] = new Cubie(7, 2);
            }
            else if (myCube[53] == Colors.blue && myCube[33] == Colors.red && myCube[26] == Colors.yellow)
            {
                corners[7] = new Cubie(7, 1);
            }
            #endregion

            #region Edge0
            //0-0
            if (myCube[5] == Colors.white && myCube[19] == Colors.red)
            {
                edges[0] = new Cubie(0, 0);
            }
            else if (myCube[5] == Colors.red && myCube[19] == Colors.white)
            {
                edges[0] = new Cubie(0, 1);
            }
            //0-1
            else if (myCube[5] == Colors.white && myCube[19] == Colors.green)
            {
                edges[0] = new Cubie(1, 0);
            }
            else if (myCube[5] == Colors.green && myCube[19] == Colors.white)
            {
                edges[0] = new Cubie(1, 1);
            }
            //0-2
            else if (myCube[5] == Colors.white && myCube[19] == Colors.orange)
            {
                edges[0] = new Cubie(2, 0);
            }
            else if (myCube[5] == Colors.orange && myCube[19] == Colors.white)
            {
                edges[0] = new Cubie(2, 1);
            }
            //0-3
            else if (myCube[5] == Colors.white && myCube[19] == Colors.blue)
            {
                edges[0] = new Cubie(3, 0);
            }
            else if (myCube[5] == Colors.blue && myCube[19] == Colors.white)
            {
                edges[0] = new Cubie(3, 1);
            }
            //0-4
            else if (myCube[5] == Colors.yellow && myCube[19] == Colors.red)
            {
                edges[0] = new Cubie(4, 0);
            }
            else if (myCube[5] == Colors.red && myCube[19] == Colors.yellow)
            {
                edges[0] = new Cubie(4, 1);
            }
            //0-5
            else if (myCube[5] == Colors.yellow && myCube[19] == Colors.green)
            {
                edges[0] = new Cubie(5, 0);
            }
            else if (myCube[5] == Colors.green && myCube[19] == Colors.yellow)
            {
                edges[0] = new Cubie(5, 1);
            }
            //0-6
            else if (myCube[5] == Colors.yellow && myCube[19] == Colors.orange)
            {
                edges[0] = new Cubie(6, 0);
            }
            else if (myCube[5] == Colors.orange && myCube[19] == Colors.yellow)
            {
                edges[0] = new Cubie(6, 1);
            }
            //0-7
            else if (myCube[5] == Colors.yellow && myCube[19] == Colors.blue)
            {
                edges[0] = new Cubie(7, 0);
            }
            else if (myCube[5] == Colors.blue && myCube[19] == Colors.yellow)
            {
                edges[0] = new Cubie(7, 1);
            }
            //0-8
            else if (myCube[5] == Colors.green && myCube[19] == Colors.red)
            {
                edges[0] = new Cubie(8, 0);
            }
            else if (myCube[5] == Colors.red && myCube[19] == Colors.green)
            {
                edges[0] = new Cubie(8, 1);
            }
            //0-9
            else if (myCube[5] == Colors.green && myCube[19] == Colors.orange)
            {
                edges[0] = new Cubie(9, 0);
            }
            else if (myCube[5] == Colors.orange && myCube[19] == Colors.green)
            {
                edges[0] = new Cubie(9, 1);
            }
            //0-10
            else if (myCube[5] == Colors.blue && myCube[19] == Colors.orange)
            {
                edges[0] = new Cubie(10, 0);
            }
            else if (myCube[5] == Colors.orange && myCube[19] == Colors.blue)
            {
                edges[0] = new Cubie(10, 1);
            }
            //0-11
            else if (myCube[5] == Colors.blue && myCube[19] == Colors.red)
            {
                edges[0] = new Cubie(11, 0);
            }
            else if (myCube[5] == Colors.red && myCube[19] == Colors.blue)
            {
                edges[0] = new Cubie(11, 1);
            }
            #endregion
            #region Edge1
            //1-0
            if (myCube[7] == Colors.white && myCube[10] == Colors.red)
            {
                edges[1] = new Cubie(0, 0);
            }
            else if (myCube[7] == Colors.red && myCube[10] == Colors.white)
            {
                edges[1] = new Cubie(0, 1);
            }
            //1-1
            else if (myCube[7] == Colors.white && myCube[10] == Colors.green)
            {
                edges[1] = new Cubie(1, 0);
            }
            else if (myCube[7] == Colors.green && myCube[10] == Colors.white)
            {
                edges[1] = new Cubie(1, 1);
            }
            //1-2
            else if (myCube[7] == Colors.white && myCube[10] == Colors.orange)
            {
                edges[1] = new Cubie(2, 0);
            }
            else if (myCube[7] == Colors.orange && myCube[10] == Colors.white)
            {
                edges[1] = new Cubie(2, 1);
            }
            //1-3
            else if (myCube[7] == Colors.white && myCube[10] == Colors.blue)
            {
                edges[1] = new Cubie(3, 0);
            }
            else if (myCube[7] == Colors.blue && myCube[10] == Colors.white)
            {
                edges[1] = new Cubie(3, 1);
            }
            //1-4
            else if (myCube[7] == Colors.yellow && myCube[10] == Colors.red)
            {
                edges[1] = new Cubie(4, 0);
            }
            else if (myCube[7] == Colors.red && myCube[10] == Colors.yellow)
            {
                edges[1] = new Cubie(4, 1);
            }
            //1-5
            else if (myCube[7] == Colors.yellow && myCube[10] == Colors.green)
            {
                edges[1] = new Cubie(5, 0);
            }
            else if (myCube[7] == Colors.green && myCube[10] == Colors.yellow)
            {
                edges[1] = new Cubie(5, 1);
            }
            //1-6
            else if (myCube[7] == Colors.yellow && myCube[10] == Colors.orange)
            {
                edges[1] = new Cubie(6, 0);
            }
            else if (myCube[7] == Colors.orange && myCube[10] == Colors.yellow)
            {
                edges[1] = new Cubie(6, 1);
            }
            //1-7
            else if (myCube[7] == Colors.yellow && myCube[10] == Colors.blue)
            {
                edges[1] = new Cubie(7, 0);
            }
            else if (myCube[7] == Colors.blue && myCube[10] == Colors.yellow)
            {
                edges[1] = new Cubie(7, 1);
            }
            //1-8
            else if (myCube[7] == Colors.green && myCube[10] == Colors.red)
            {
                edges[1] = new Cubie(8, 0);
            }
            else if (myCube[7] == Colors.red && myCube[10] == Colors.green)
            {
                edges[1] = new Cubie(8, 1);
            }
            //1-9
            else if (myCube[7] == Colors.green && myCube[10] == Colors.orange)
            {
                edges[1] = new Cubie(9, 0);
            }
            else if (myCube[7] == Colors.orange && myCube[10] == Colors.green)
            {
                edges[1] = new Cubie(9, 1);
            }
            //1-10
            else if (myCube[7] == Colors.blue && myCube[10] == Colors.orange)
            {
                edges[1] = new Cubie(10, 0);
            }
            else if (myCube[7] == Colors.orange && myCube[10] == Colors.blue)
            {
                edges[1] = new Cubie(10, 1);
            }
            //1-11
            else if (myCube[7] == Colors.blue && myCube[10] == Colors.red)
            {
                edges[1] = new Cubie(11, 0);
            }
            else if (myCube[7] == Colors.red && myCube[10] == Colors.blue)
            {
                edges[1] = new Cubie(11, 1);
            }
            #endregion
            #region Edge2
            //2-0
            if (myCube[3] == Colors.white && myCube[37] == Colors.red)
            {
                edges[2] = new Cubie(0, 0);
            }
            else if (myCube[3] == Colors.red && myCube[37] == Colors.white)
            {
                edges[2] = new Cubie(0, 1);
            }
            //2-1
            else if (myCube[3] == Colors.white && myCube[37] == Colors.green)
            {
                edges[2] = new Cubie(1, 0);
            }
            else if (myCube[3] == Colors.green && myCube[37] == Colors.white)
            {
                edges[2] = new Cubie(1, 1);
            }
            //2-2
            else if (myCube[3] == Colors.white && myCube[37] == Colors.orange)
            {
                edges[2] = new Cubie(2, 0);
            }
            else if (myCube[3] == Colors.orange && myCube[37] == Colors.white)
            {
                edges[2] = new Cubie(2, 1);
            }
            //2-3
            else if (myCube[3] == Colors.white && myCube[37] == Colors.blue)
            {
                edges[2] = new Cubie(3, 0);
            }
            else if (myCube[3] == Colors.blue && myCube[37] == Colors.white)
            {
                edges[2] = new Cubie(3, 1);
            }
            //2-4
            else if (myCube[3] == Colors.yellow && myCube[37] == Colors.red)
            {
                edges[2] = new Cubie(4, 0);
            }
            else if (myCube[3] == Colors.red && myCube[37] == Colors.yellow)
            {
                edges[2] = new Cubie(4, 1);
            }
            //2-5
            else if (myCube[3] == Colors.yellow && myCube[37] == Colors.green)
            {
                edges[2] = new Cubie(5, 0);
            }
            else if (myCube[3] == Colors.green && myCube[37] == Colors.yellow)
            {
                edges[2] = new Cubie(5, 1);
            }
            //2-6
            else if (myCube[3] == Colors.yellow && myCube[37] == Colors.orange)
            {
                edges[2] = new Cubie(6, 0);
            }
            else if (myCube[3] == Colors.orange && myCube[37] == Colors.yellow)
            {
                edges[2] = new Cubie(6, 1);
            }
            //2-7
            else if (myCube[3] == Colors.yellow && myCube[37] == Colors.blue)
            {
                edges[2] = new Cubie(7, 0);
            }
            else if (myCube[3] == Colors.blue && myCube[37] == Colors.yellow)
            {
                edges[2] = new Cubie(7, 1);
            }
            //2-8
            else if (myCube[3] == Colors.green && myCube[37] == Colors.red)
            {
                edges[2] = new Cubie(8, 0);
            }
            else if (myCube[3] == Colors.red && myCube[37] == Colors.green)
            {
                edges[2] = new Cubie(8, 1);
            }
            //2-9
            else if (myCube[3] == Colors.green && myCube[37] == Colors.orange)
            {
                edges[2] = new Cubie(9, 0);
            }
            else if (myCube[3] == Colors.orange && myCube[37] == Colors.green)
            {
                edges[2] = new Cubie(9, 1);
            }
            //2-10
            else if (myCube[3] == Colors.blue && myCube[37] == Colors.orange)
            {
                edges[2] = new Cubie(10, 0);
            }
            else if (myCube[3] == Colors.orange && myCube[37] == Colors.blue)
            {
                edges[2] = new Cubie(10, 1);
            }
            //2-11
            else if (myCube[3] == Colors.blue && myCube[37] == Colors.red)
            {
                edges[2] = new Cubie(11, 0);
            }
            else if (myCube[3] == Colors.red && myCube[37] == Colors.blue)
            {
                edges[2] = new Cubie(11, 1);
            }
            #endregion
            #region Edge3
            //3-0
            if (myCube[1] == Colors.white && myCube[28] == Colors.red)
            {
                edges[3] = new Cubie(0, 0);
            }
            else if (myCube[1] == Colors.red && myCube[28] == Colors.white)
            {
                edges[3] = new Cubie(0, 1);
            }
            //3-1
            else if (myCube[1] == Colors.white && myCube[28] == Colors.green)
            {
                edges[3] = new Cubie(1, 0);
            }
            else if (myCube[1] == Colors.green && myCube[28] == Colors.white)
            {
                edges[3] = new Cubie(1, 1);
            }
            //3-2
            else if (myCube[1] == Colors.white && myCube[28] == Colors.orange)
            {
                edges[3] = new Cubie(2, 0);
            }
            else if (myCube[1] == Colors.orange && myCube[28] == Colors.white)
            {
                edges[3] = new Cubie(2, 1);
            }
            //3-3
            else if (myCube[1] == Colors.white && myCube[28] == Colors.blue)
            {
                edges[3] = new Cubie(3, 0);
            }
            else if (myCube[1] == Colors.blue && myCube[28] == Colors.white)
            {
                edges[3] = new Cubie(3, 1);
            }
            //3-4
            else if (myCube[1] == Colors.yellow && myCube[28] == Colors.red)
            {
                edges[3] = new Cubie(4, 0);
            }
            else if (myCube[1] == Colors.red && myCube[28] == Colors.yellow)
            {
                edges[3] = new Cubie(4, 1);
            }
            //3-5
            else if (myCube[1] == Colors.yellow && myCube[28] == Colors.green)
            {
                edges[3] = new Cubie(5, 0);
            }
            else if (myCube[1] == Colors.green && myCube[28] == Colors.yellow)
            {
                edges[3] = new Cubie(5, 1);
            }
            //3-6
            else if (myCube[1] == Colors.yellow && myCube[28] == Colors.orange)
            {
                edges[3] = new Cubie(6, 0);
            }
            else if (myCube[1] == Colors.orange && myCube[28] == Colors.yellow)
            {
                edges[3] = new Cubie(6, 1);
            }
            //3-7
            else if (myCube[1] == Colors.yellow && myCube[28] == Colors.blue)
            {
                edges[3] = new Cubie(7, 0);
            }
            else if (myCube[1] == Colors.blue && myCube[28] == Colors.yellow)
            {
                edges[3] = new Cubie(7, 1);
            }
            //3-8
            else if (myCube[1] == Colors.green && myCube[28] == Colors.red)
            {
                edges[3] = new Cubie(8, 0);
            }
            else if (myCube[1] == Colors.red && myCube[28] == Colors.green)
            {
                edges[3] = new Cubie(8, 1);
            }
            //3-9
            else if (myCube[1] == Colors.green && myCube[28] == Colors.orange)
            {
                edges[3] = new Cubie(9, 0);
            }
            else if (myCube[1] == Colors.orange && myCube[28] == Colors.green)
            {
                edges[3] = new Cubie(9, 1);
            }
            //3-10
            else if (myCube[1] == Colors.blue && myCube[28] == Colors.orange)
            {
                edges[3] = new Cubie(10, 0);
            }
            else if (myCube[1] == Colors.orange && myCube[28] == Colors.blue)
            {
                edges[3] = new Cubie(10, 1);
            }
            //3-11
            else if (myCube[1] == Colors.blue && myCube[28] == Colors.red)
            {
                edges[3] = new Cubie(11, 0);
            }
            else if (myCube[1] == Colors.red && myCube[28] == Colors.blue)
            {
                edges[3] = new Cubie(11, 1);
            }
            #endregion
            #region Edge4
            //4-0
            if (myCube[50] == Colors.white && myCube[25] == Colors.red)
            {
                edges[4] = new Cubie(0, 0);
            }
            else if (myCube[50] == Colors.red && myCube[25] == Colors.white)
            {
                edges[4] = new Cubie(0, 1);
            }
            //4-1
            else if (myCube[50] == Colors.white && myCube[25] == Colors.green)
            {
                edges[4] = new Cubie(1, 0);
            }
            else if (myCube[50] == Colors.green && myCube[25] == Colors.white)
            {
                edges[4] = new Cubie(1, 1);
            }
            //4-2
            else if (myCube[50] == Colors.white && myCube[25] == Colors.orange)
            {
                edges[4] = new Cubie(2, 0);
            }
            else if (myCube[50] == Colors.orange && myCube[25] == Colors.white)
            {
                edges[4] = new Cubie(2, 1);
            }
            //4-3
            else if (myCube[50] == Colors.white && myCube[25] == Colors.blue)
            {
                edges[4] = new Cubie(3, 0);
            }
            else if (myCube[50] == Colors.blue && myCube[25] == Colors.white)
            {
                edges[4] = new Cubie(3, 1);
            }
            //4-4
            else if (myCube[50] == Colors.yellow && myCube[25] == Colors.red)
            {
                edges[4] = new Cubie(4, 0);
            }
            else if (myCube[50] == Colors.red && myCube[25] == Colors.yellow)
            {
                edges[4] = new Cubie(4, 1);
            }
            //4-5
            else if (myCube[50] == Colors.yellow && myCube[25] == Colors.green)
            {
                edges[4] = new Cubie(5, 0);
            }
            else if (myCube[50] == Colors.green && myCube[25] == Colors.yellow)
            {
                edges[4] = new Cubie(5, 1);
            }
            //4-6
            else if (myCube[50] == Colors.yellow && myCube[25] == Colors.orange)
            {
                edges[4] = new Cubie(6, 0);
            }
            else if (myCube[50] == Colors.orange && myCube[25] == Colors.yellow)
            {
                edges[4] = new Cubie(6, 1);
            }
            //4-7
            else if (myCube[50] == Colors.yellow && myCube[25] == Colors.blue)
            {
                edges[4] = new Cubie(7, 0);
            }
            else if (myCube[50] == Colors.blue && myCube[25] == Colors.yellow)
            {
                edges[4] = new Cubie(7, 1);
            }
            //4-8
            else if (myCube[50] == Colors.green && myCube[25] == Colors.red)
            {
                edges[4] = new Cubie(8, 0);
            }
            else if (myCube[50] == Colors.red && myCube[25] == Colors.green)
            {
                edges[4] = new Cubie(8, 1);
            }
            //4-9
            else if (myCube[50] == Colors.green && myCube[25] == Colors.orange)
            {
                edges[4] = new Cubie(9, 0);
            }
            else if (myCube[50] == Colors.orange && myCube[25] == Colors.green)
            {
                edges[4] = new Cubie(9, 1);
            }
            //4-10
            else if (myCube[50] == Colors.blue && myCube[25] == Colors.orange)
            {
                edges[4] = new Cubie(10, 0);
            }
            else if (myCube[50] == Colors.orange && myCube[25] == Colors.blue)
            {
                edges[4] = new Cubie(10, 1);
            }
            //4-11
            else if (myCube[50] == Colors.blue && myCube[25] == Colors.red)
            {
                edges[4] = new Cubie(11, 0);
            }
            else if (myCube[50] == Colors.red && myCube[25] == Colors.blue)
            {
                edges[4] = new Cubie(11, 1);
            }
            #endregion
            #region Edge5
            //5-0
            if (myCube[46] == Colors.white && myCube[16] == Colors.red)
            {
                edges[5] = new Cubie(0, 0);
            }
            else if (myCube[46] == Colors.red && myCube[16] == Colors.white)
            {
                edges[5] = new Cubie(0, 1);
            }
            //5-1
            else if (myCube[46] == Colors.white && myCube[16] == Colors.green)
            {
                edges[5] = new Cubie(1, 0);
            }
            else if (myCube[46] == Colors.green && myCube[16] == Colors.white)
            {
                edges[5] = new Cubie(1, 1);
            }
            //5-2
            else if (myCube[46] == Colors.white && myCube[16] == Colors.orange)
            {
                edges[5] = new Cubie(2, 0);
            }
            else if (myCube[46] == Colors.orange && myCube[16] == Colors.white)
            {
                edges[5] = new Cubie(2, 1);
            }
            //5-3
            else if (myCube[46] == Colors.white && myCube[16] == Colors.blue)
            {
                edges[5] = new Cubie(3, 0);
            }
            else if (myCube[46] == Colors.blue && myCube[16] == Colors.white)
            {
                edges[5] = new Cubie(3, 1);
            }
            //5-4
            else if (myCube[46] == Colors.yellow && myCube[16] == Colors.red)
            {
                edges[5] = new Cubie(4, 0);
            }
            else if (myCube[46] == Colors.red && myCube[16] == Colors.yellow)
            {
                edges[5] = new Cubie(4, 1);
            }
            //5-5
            else if (myCube[46] == Colors.yellow && myCube[16] == Colors.green)
            {
                edges[5] = new Cubie(5, 0);
            }
            else if (myCube[46] == Colors.green && myCube[16] == Colors.yellow)
            {
                edges[5] = new Cubie(5, 1);
            }
            //5-6
            else if (myCube[46] == Colors.yellow && myCube[16] == Colors.orange)
            {
                edges[5] = new Cubie(6, 0);
            }
            else if (myCube[46] == Colors.orange && myCube[16] == Colors.yellow)
            {
                edges[5] = new Cubie(6, 1);
            }
            //5-7
            else if (myCube[46] == Colors.yellow && myCube[16] == Colors.blue)
            {
                edges[5] = new Cubie(7, 0);
            }
            else if (myCube[46] == Colors.blue && myCube[16] == Colors.yellow)
            {
                edges[5] = new Cubie(7, 1);
            }
            //5-8
            else if (myCube[46] == Colors.green && myCube[16] == Colors.red)
            {
                edges[5] = new Cubie(8, 0);
            }
            else if (myCube[46] == Colors.red && myCube[16] == Colors.green)
            {
                edges[5] = new Cubie(8, 1);
            }
            //5-9
            else if (myCube[46] == Colors.green && myCube[16] == Colors.orange)
            {
                edges[5] = new Cubie(9, 0);
            }
            else if (myCube[46] == Colors.orange && myCube[16] == Colors.green)
            {
                edges[5] = new Cubie(9, 1);
            }
            //5-10
            else if (myCube[46] == Colors.blue && myCube[16] == Colors.orange)
            {
                edges[5] = new Cubie(10, 0);
            }
            else if (myCube[46] == Colors.orange && myCube[16] == Colors.blue)
            {
                edges[5] = new Cubie(10, 1);
            }
            //5-11
            else if (myCube[46] == Colors.blue && myCube[16] == Colors.red)
            {
                edges[5] = new Cubie(11, 0);
            }
            else if (myCube[46] == Colors.red && myCube[16] == Colors.blue)
            {
                edges[5] = new Cubie(11, 1);
            }
            #endregion
            #region Edge6
            //6-0
            if (myCube[48] == Colors.white && myCube[43] == Colors.red)
            {
                edges[6] = new Cubie(0, 0);
            }
            else if (myCube[48] == Colors.red && myCube[43] == Colors.white)
            {
                edges[6] = new Cubie(0, 1);
            }
            //6-1
            else if (myCube[48] == Colors.white && myCube[43] == Colors.green)
            {
                edges[6] = new Cubie(1, 0);
            }
            else if (myCube[48] == Colors.green && myCube[43] == Colors.white)
            {
                edges[6] = new Cubie(1, 1);
            }
            //6-2
            else if (myCube[48] == Colors.white && myCube[43] == Colors.orange)
            {
                edges[6] = new Cubie(2, 0);
            }
            else if (myCube[48] == Colors.orange && myCube[43] == Colors.white)
            {
                edges[6] = new Cubie(2, 1);
            }
            //6-3
            else if (myCube[48] == Colors.white && myCube[43] == Colors.blue)
            {
                edges[6] = new Cubie(3, 0);
            }
            else if (myCube[48] == Colors.blue && myCube[43] == Colors.white)
            {
                edges[6] = new Cubie(3, 1);
            }
            //6-4
            else if (myCube[48] == Colors.yellow && myCube[43] == Colors.red)
            {
                edges[6] = new Cubie(4, 0);
            }
            else if (myCube[48] == Colors.red && myCube[43] == Colors.yellow)
            {
                edges[6] = new Cubie(4, 1);
            }
            //6-5
            else if (myCube[48] == Colors.yellow && myCube[43] == Colors.green)
            {
                edges[6] = new Cubie(5, 0);
            }
            else if (myCube[48] == Colors.green && myCube[43] == Colors.yellow)
            {
                edges[6] = new Cubie(5, 1);
            }
            //6-6
            else if (myCube[48] == Colors.yellow && myCube[43] == Colors.orange)
            {
                edges[6] = new Cubie(6, 0);
            }
            else if (myCube[48] == Colors.orange && myCube[43] == Colors.yellow)
            {
                edges[6] = new Cubie(6, 1);
            }
            //6-7
            else if (myCube[48] == Colors.yellow && myCube[43] == Colors.blue)
            {
                edges[6] = new Cubie(7, 0);
            }
            else if (myCube[48] == Colors.blue && myCube[43] == Colors.yellow)
            {
                edges[6] = new Cubie(7, 1);
            }
            //6-8
            else if (myCube[48] == Colors.green && myCube[43] == Colors.red)
            {
                edges[6] = new Cubie(8, 0);
            }
            else if (myCube[48] == Colors.red && myCube[43] == Colors.green)
            {
                edges[6] = new Cubie(8, 1);
            }
            //6-9
            else if (myCube[48] == Colors.green && myCube[43] == Colors.orange)
            {
                edges[6] = new Cubie(9, 0);
            }
            else if (myCube[48] == Colors.orange && myCube[43] == Colors.green)
            {
                edges[6] = new Cubie(9, 1);
            }
            //6-10
            else if (myCube[48] == Colors.blue && myCube[43] == Colors.orange)
            {
                edges[6] = new Cubie(10, 0);
            }
            else if (myCube[48] == Colors.orange && myCube[43] == Colors.blue)
            {
                edges[6] = new Cubie(10, 1);
            }
            //6-11
            else if (myCube[48] == Colors.blue && myCube[43] == Colors.red)
            {
                edges[6] = new Cubie(11, 0);
            }
            else if (myCube[48] == Colors.red && myCube[43] == Colors.blue)
            {
                edges[6] = new Cubie(11, 1);
            }
            #endregion
            #region Edge7
            //7-0
            if (myCube[52] == Colors.white && myCube[34] == Colors.red)
            {
                edges[7] = new Cubie(0, 0);
            }
            else if (myCube[52] == Colors.red && myCube[34] == Colors.white)
            {
                edges[7] = new Cubie(0, 1);
            }
            //7-1
            else if (myCube[52] == Colors.white && myCube[34] == Colors.green)
            {
                edges[7] = new Cubie(1, 0);
            }
            else if (myCube[52] == Colors.green && myCube[34] == Colors.white)
            {
                edges[7] = new Cubie(1, 1);
            }
            //7-2
            else if (myCube[52] == Colors.white && myCube[34] == Colors.orange)
            {
                edges[7] = new Cubie(2, 0);
            }
            else if (myCube[52] == Colors.orange && myCube[34] == Colors.white)
            {
                edges[7] = new Cubie(2, 1);
            }
            //7-3
            else if (myCube[52] == Colors.white && myCube[34] == Colors.blue)
            {
                edges[7] = new Cubie(3, 0);
            }
            else if (myCube[52] == Colors.blue && myCube[34] == Colors.white)
            {
                edges[7] = new Cubie(3, 1);
            }
            //7-4
            else if (myCube[52] == Colors.yellow && myCube[34] == Colors.red)
            {
                edges[7] = new Cubie(4, 0);
            }
            else if (myCube[52] == Colors.red && myCube[34] == Colors.yellow)
            {
                edges[7] = new Cubie(4, 1);
            }
            //7-5
            else if (myCube[52] == Colors.yellow && myCube[34] == Colors.green)
            {
                edges[7] = new Cubie(5, 0);
            }
            else if (myCube[52] == Colors.green && myCube[34] == Colors.yellow)
            {
                edges[7] = new Cubie(5, 1);
            }
            //7-6
            else if (myCube[52] == Colors.yellow && myCube[34] == Colors.orange)
            {
                edges[7] = new Cubie(6, 0);
            }
            else if (myCube[52] == Colors.orange && myCube[34] == Colors.yellow)
            {
                edges[7] = new Cubie(6, 1);
            }
            //7-7
            else if (myCube[52] == Colors.yellow && myCube[34] == Colors.blue)
            {
                edges[7] = new Cubie(7, 0);
            }
            else if (myCube[52] == Colors.blue && myCube[34] == Colors.yellow)
            {
                edges[7] = new Cubie(7, 1);
            }
            //7-8
            else if (myCube[52] == Colors.green && myCube[34] == Colors.red)
            {
                edges[7] = new Cubie(8, 0);
            }
            else if (myCube[52] == Colors.red && myCube[34] == Colors.green)
            {
                edges[7] = new Cubie(8, 1);
            }
            //7-9
            else if (myCube[52] == Colors.green && myCube[34] == Colors.orange)
            {
                edges[7] = new Cubie(9, 0);
            }
            else if (myCube[52] == Colors.orange && myCube[34] == Colors.green)
            {
                edges[7] = new Cubie(9, 1);
            }
            //7-10
            else if (myCube[52] == Colors.blue && myCube[34] == Colors.orange)
            {
                edges[7] = new Cubie(10, 0);
            }
            else if (myCube[52] == Colors.orange && myCube[34] == Colors.blue)
            {
                edges[7] = new Cubie(10, 1);
            }
            //7-11
            else if (myCube[52] == Colors.blue && myCube[34] == Colors.red)
            {
                edges[7] = new Cubie(11, 0);
            }
            else if (myCube[52] == Colors.red && myCube[34] == Colors.blue)
            {
                edges[7] = new Cubie(11, 1);
            }
            #endregion
            #region Edge8
            //8-0
            if (myCube[14] == Colors.white && myCube[21] == Colors.red)
            {
                edges[8] = new Cubie(0, 0);
            }
            else if (myCube[14] == Colors.red && myCube[21] == Colors.white)
            {
                edges[8] = new Cubie(0, 1);
            }
            //8-1
            else if (myCube[14] == Colors.white && myCube[21] == Colors.green)
            {
                edges[8] = new Cubie(1, 0);
            }
            else if (myCube[14] == Colors.green && myCube[21] == Colors.white)
            {
                edges[8] = new Cubie(1, 1);
            }
            //8-2
            else if (myCube[14] == Colors.white && myCube[21] == Colors.orange)
            {
                edges[8] = new Cubie(2, 0);
            }
            else if (myCube[14] == Colors.orange && myCube[21] == Colors.white)
            {
                edges[8] = new Cubie(2, 1);
            }
            //8-3
            else if (myCube[14] == Colors.white && myCube[21] == Colors.blue)
            {
                edges[8] = new Cubie(3, 0);
            }
            else if (myCube[14] == Colors.blue && myCube[21] == Colors.white)
            {
                edges[8] = new Cubie(3, 1);
            }
            //8-4
            else if (myCube[14] == Colors.yellow && myCube[21] == Colors.red)
            {
                edges[8] = new Cubie(4, 0);
            }
            else if (myCube[14] == Colors.red && myCube[21] == Colors.yellow)
            {
                edges[8] = new Cubie(4, 1);
            }
            //8-5
            else if (myCube[14] == Colors.yellow && myCube[21] == Colors.green)
            {
                edges[8] = new Cubie(5, 0);
            }
            else if (myCube[14] == Colors.green && myCube[21] == Colors.yellow)
            {
                edges[8] = new Cubie(5, 1);
            }
            //8-6
            else if (myCube[14] == Colors.yellow && myCube[21] == Colors.orange)
            {
                edges[8] = new Cubie(6, 0);
            }
            else if (myCube[14] == Colors.orange && myCube[21] == Colors.yellow)
            {
                edges[8] = new Cubie(6, 1);
            }
            //8-7
            else if (myCube[14] == Colors.yellow && myCube[21] == Colors.blue)
            {
                edges[8] = new Cubie(7, 0);
            }
            else if (myCube[14] == Colors.blue && myCube[21] == Colors.yellow)
            {
                edges[8] = new Cubie(7, 1);
            }
            //8-8
            else if (myCube[14] == Colors.green && myCube[21] == Colors.red)
            {
                edges[8] = new Cubie(8, 0);
            }
            else if (myCube[14] == Colors.red && myCube[21] == Colors.green)
            {
                edges[8] = new Cubie(8, 1);
            }
            //8-9
            else if (myCube[14] == Colors.green && myCube[21] == Colors.orange)
            {
                edges[8] = new Cubie(9, 0);
            }
            else if (myCube[14] == Colors.orange && myCube[21] == Colors.green)
            {
                edges[8] = new Cubie(9, 1);
            }
            //8-10
            else if (myCube[14] == Colors.blue && myCube[21] == Colors.orange)
            {
                edges[8] = new Cubie(10, 0);
            }
            else if (myCube[14] == Colors.orange && myCube[21] == Colors.blue)
            {
                edges[8] = new Cubie(10, 1);
            }
            //8-11
            else if (myCube[14] == Colors.blue && myCube[21] == Colors.red)
            {
                edges[8] = new Cubie(11, 0);
            }
            else if (myCube[14] == Colors.red && myCube[21] == Colors.blue)
            {
                edges[8] = new Cubie(11, 1);
            }
            #endregion
            #region Edge9
            //9-0
            if (myCube[12] == Colors.white && myCube[41] == Colors.red)
            {
                edges[9] = new Cubie(0, 0);
            }
            else if (myCube[12] == Colors.red && myCube[41] == Colors.white)
            {
                edges[9] = new Cubie(0, 1);
            }
            //9-1
            else if (myCube[12] == Colors.white && myCube[41] == Colors.green)
            {
                edges[9] = new Cubie(1, 0);
            }
            else if (myCube[12] == Colors.green && myCube[41] == Colors.white)
            {
                edges[9] = new Cubie(1, 1);
            }
            //9-2
            else if (myCube[12] == Colors.white && myCube[41] == Colors.orange)
            {
                edges[9] = new Cubie(2, 0);
            }
            else if (myCube[12] == Colors.orange && myCube[41] == Colors.white)
            {
                edges[9] = new Cubie(2, 1);
            }
            //9-3
            else if (myCube[12] == Colors.white && myCube[41] == Colors.blue)
            {
                edges[9] = new Cubie(3, 0);
            }
            else if (myCube[12] == Colors.blue && myCube[41] == Colors.white)
            {
                edges[9] = new Cubie(3, 1);
            }
            //9-4
            else if (myCube[12] == Colors.yellow && myCube[41] == Colors.red)
            {
                edges[9] = new Cubie(4, 0);
            }
            else if (myCube[12] == Colors.red && myCube[41] == Colors.yellow)
            {
                edges[9] = new Cubie(4, 1);
            }
            //9-5
            else if (myCube[12] == Colors.yellow && myCube[41] == Colors.green)
            {
                edges[9] = new Cubie(5, 0);
            }
            else if (myCube[12] == Colors.green && myCube[41] == Colors.yellow)
            {
                edges[9] = new Cubie(5, 1);
            }
            //9-6
            else if (myCube[12] == Colors.yellow && myCube[41] == Colors.orange)
            {
                edges[9] = new Cubie(6, 0);
            }
            else if (myCube[12] == Colors.orange && myCube[41] == Colors.yellow)
            {
                edges[9] = new Cubie(6, 1);
            }
            //9-7
            else if (myCube[12] == Colors.yellow && myCube[41] == Colors.blue)
            {
                edges[9] = new Cubie(7, 0);
            }
            else if (myCube[12] == Colors.blue && myCube[41] == Colors.yellow)
            {
                edges[9] = new Cubie(7, 1);
            }
            //9-8
            else if (myCube[12] == Colors.green && myCube[41] == Colors.red)
            {
                edges[9] = new Cubie(8, 0);
            }
            else if (myCube[12] == Colors.red && myCube[41] == Colors.green)
            {
                edges[9] = new Cubie(8, 1);
            }
            //9-9
            else if (myCube[12] == Colors.green && myCube[41] == Colors.orange)
            {
                edges[9] = new Cubie(9, 0);
            }
            else if (myCube[12] == Colors.orange && myCube[41] == Colors.green)
            {
                edges[9] = new Cubie(9, 1);
            }
            //9-10
            else if (myCube[12] == Colors.blue && myCube[41] == Colors.orange)
            {
                edges[9] = new Cubie(10, 0);
            }
            else if (myCube[12] == Colors.orange && myCube[41] == Colors.blue)
            {
                edges[9] = new Cubie(10, 1);
            }
            //9-11
            else if (myCube[12] == Colors.blue && myCube[41] == Colors.red)
            {
                edges[9] = new Cubie(11, 0);
            }
            else if (myCube[12] == Colors.red && myCube[41] == Colors.blue)
            {
                edges[9] = new Cubie(11, 1);
            }
            #endregion
            #region Edge10
            //10-0
            if (myCube[32] == Colors.white && myCube[39] == Colors.red)
            {
                edges[10] = new Cubie(0, 0);
            }
            else if (myCube[32] == Colors.red && myCube[39] == Colors.white)
            {
                edges[10] = new Cubie(0, 1);
            }
            //10-1
            else if (myCube[32] == Colors.white && myCube[39] == Colors.green)
            {
                edges[10] = new Cubie(1, 0);
            }
            else if (myCube[32] == Colors.green && myCube[39] == Colors.white)
            {
                edges[10] = new Cubie(1, 1);
            }
            //10-2
            else if (myCube[32] == Colors.white && myCube[39] == Colors.orange)
            {
                edges[10] = new Cubie(2, 0);
            }
            else if (myCube[32] == Colors.orange && myCube[39] == Colors.white)
            {
                edges[10] = new Cubie(2, 1);
            }
            //10-3
            else if (myCube[32] == Colors.white && myCube[39] == Colors.blue)
            {
                edges[10] = new Cubie(3, 0);
            }
            else if (myCube[32] == Colors.blue && myCube[39] == Colors.white)
            {
                edges[10] = new Cubie(3, 1);
            }
            //10-4
            else if (myCube[32] == Colors.yellow && myCube[39] == Colors.red)
            {
                edges[10] = new Cubie(4, 0);
            }
            else if (myCube[32] == Colors.red && myCube[39] == Colors.yellow)
            {
                edges[10] = new Cubie(4, 1);
            }
            //10-5
            else if (myCube[32] == Colors.yellow && myCube[39] == Colors.green)
            {
                edges[10] = new Cubie(5, 0);
            }
            else if (myCube[32] == Colors.green && myCube[39] == Colors.yellow)
            {
                edges[10] = new Cubie(5, 1);
            }
            //10-6
            else if (myCube[32] == Colors.yellow && myCube[39] == Colors.orange)
            {
                edges[10] = new Cubie(6, 0);
            }
            else if (myCube[32] == Colors.orange && myCube[39] == Colors.yellow)
            {
                edges[10] = new Cubie(6, 1);
            }
            //10-7
            else if (myCube[32] == Colors.yellow && myCube[39] == Colors.blue)
            {
                edges[10] = new Cubie(7, 0);
            }
            else if (myCube[32] == Colors.blue && myCube[39] == Colors.yellow)
            {
                edges[10] = new Cubie(7, 1);
            }
            //10-8
            else if (myCube[32] == Colors.green && myCube[39] == Colors.red)
            {
                edges[10] = new Cubie(8, 0);
            }
            else if (myCube[32] == Colors.red && myCube[39] == Colors.green)
            {
                edges[10] = new Cubie(8, 1);
            }
            //10-9
            else if (myCube[32] == Colors.green && myCube[39] == Colors.orange)
            {
                edges[10] = new Cubie(9, 0);
            }
            else if (myCube[32] == Colors.orange && myCube[39] == Colors.green)
            {
                edges[10] = new Cubie(9, 1);
            }
            //10-10
            else if (myCube[32] == Colors.blue && myCube[39] == Colors.orange)
            {
                edges[10] = new Cubie(10, 0);
            }
            else if (myCube[32] == Colors.orange && myCube[39] == Colors.blue)
            {
                edges[10] = new Cubie(10, 1);
            }
            //10-11
            else if (myCube[32] == Colors.blue && myCube[39] == Colors.red)
            {
                edges[10] = new Cubie(11, 0);
            }
            else if (myCube[32] == Colors.red && myCube[39] == Colors.blue)
            {
                edges[10] = new Cubie(11, 1);
            }
            #endregion
            #region Edge11
            //11-0
            if (myCube[30] == Colors.white && myCube[23] == Colors.red)
            {
                edges[11] = new Cubie(0, 0);
            }
            else if (myCube[30] == Colors.red && myCube[23] == Colors.white)
            {
                edges[11] = new Cubie(0, 1);
            }
            //11-1
            else if (myCube[30] == Colors.white && myCube[23] == Colors.green)
            {
                edges[11] = new Cubie(1, 0);
            }
            else if (myCube[30] == Colors.green && myCube[23] == Colors.white)
            {
                edges[11] = new Cubie(1, 1);
            }
            //11-2
            else if (myCube[30] == Colors.white && myCube[23] == Colors.orange)
            {
                edges[11] = new Cubie(2, 0);
            }
            else if (myCube[30] == Colors.orange && myCube[23] == Colors.white)
            {
                edges[11] = new Cubie(2, 1);
            }
            //11-3
            else if (myCube[30] == Colors.white && myCube[23] == Colors.blue)
            {
                edges[11] = new Cubie(3, 0);
            }
            else if (myCube[30] == Colors.blue && myCube[23] == Colors.white)
            {
                edges[11] = new Cubie(3, 1);
            }
            //11-4
            else if (myCube[30] == Colors.yellow && myCube[23] == Colors.red)
            {
                edges[11] = new Cubie(4, 0);
            }
            else if (myCube[30] == Colors.red && myCube[23] == Colors.yellow)
            {
                edges[11] = new Cubie(4, 1);
            }
            //11-5
            else if (myCube[30] == Colors.yellow && myCube[23] == Colors.green)
            {
                edges[11] = new Cubie(5, 0);
            }
            else if (myCube[30] == Colors.green && myCube[23] == Colors.yellow)
            {
                edges[11] = new Cubie(5, 1);
            }
            //11-6
            else if (myCube[30] == Colors.yellow && myCube[23] == Colors.orange)
            {
                edges[11] = new Cubie(6, 0);
            }
            else if (myCube[30] == Colors.orange && myCube[23] == Colors.yellow)
            {
                edges[11] = new Cubie(6, 1);
            }
            //11-7
            else if (myCube[30] == Colors.yellow && myCube[23] == Colors.blue)
            {
                edges[11] = new Cubie(7, 0);
            }
            else if (myCube[30] == Colors.blue && myCube[23] == Colors.yellow)
            {
                edges[11] = new Cubie(7, 1);
            }
            //11-8
            else if (myCube[30] == Colors.green && myCube[23] == Colors.red)
            {
                edges[11] = new Cubie(8, 0);
            }
            else if (myCube[30] == Colors.red && myCube[23] == Colors.green)
            {
                edges[11] = new Cubie(8, 1);
            }
            //11-9
            else if (myCube[30] == Colors.green && myCube[23] == Colors.orange)
            {
                edges[11] = new Cubie(9, 0);
            }
            else if (myCube[30] == Colors.orange && myCube[23] == Colors.green)
            {
                edges[11] = new Cubie(9, 1);
            }
            //11-10
            else if (myCube[30] == Colors.blue && myCube[23] == Colors.orange)
            {
                edges[11] = new Cubie(10, 0);
            }
            else if (myCube[30] == Colors.orange && myCube[23] == Colors.blue)
            {
                edges[11] = new Cubie(10, 1);
            }
            //11-11
            else if (myCube[30] == Colors.blue && myCube[23] == Colors.red)
            {
                edges[11] = new Cubie(11, 0);
            }
            else if (myCube[30] == Colors.red && myCube[23] == Colors.blue)
            {
                edges[11] = new Cubie(11, 1);
            }
            #endregion

            //string s = "";
            //for (int i = 0; i < 9; i++)
            //{
            //    s += edges[i] + "-" + i;
            //}
            //MessageBox.Show(s);
        }
        public bool checkFinished()
        {
            CenterOrient();
            for (int i = 0; i < 54; i++)
            {
                if (i < 9 && myCube[i] != Colors.white) return false;
                else if (i > 8 && i < 18 && myCube[i] != Colors.green) return false;
                else if (i > 17 && i < 27 && myCube[i] != Colors.red) return false;
                else if (i > 26 && i < 36 && myCube[i] != Colors.blue) return false;
                else if (i > 35 && i < 45 && myCube[i] != Colors.orange) return false;
                else if (i > 44 && i < 54 && myCube[i] != Colors.yellow) return false;
            }
            return true;
        }
        public void DoR(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[8];
            Colors temp2 = myCube[5];
            Colors temp3 = myCube[2];
            myCube[8] = myCube[17];
            myCube[17] = myCube[53];
            myCube[53] = myCube[27];
            myCube[27] = temp1;
            myCube[5] = myCube[14];
            myCube[14] = myCube[50];
            myCube[50] = myCube[30];
            myCube[30] = temp2;
            myCube[2] = myCube[11];
            myCube[11] = myCube[47];
            myCube[47] = myCube[33];
            myCube[33] = temp3;
            //face
            temp1 = myCube[18];
            temp2 = myCube[19];
            myCube[18] = myCube[24];
            myCube[24] = myCube[26];
            myCube[26] = myCube[20];
            myCube[20] = temp1;
            myCube[19] = myCube[21];
            myCube[21] = myCube[25];
            myCube[25] = myCube[23];
            myCube[23] = temp2;
        }
        public void DoU(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[9];
            Colors temp2 = myCube[10];
            Colors temp3 = myCube[11];
            myCube[9] = myCube[18];
            myCube[18] = myCube[27];
            myCube[27] = myCube[36];
            myCube[36] = temp1;
            myCube[10] = myCube[19];
            myCube[19] = myCube[28];
            myCube[28] = myCube[37];
            myCube[37] = temp2;
            myCube[11] = myCube[20];
            myCube[20] = myCube[29];
            myCube[29] = myCube[38];
            myCube[38] = temp3;
            //face
            temp1 = myCube[0];
            temp2 = myCube[1];
            myCube[0] = myCube[6];
            myCube[6] = myCube[8];
            myCube[8] = myCube[2];
            myCube[2] = temp1;
            myCube[1] = myCube[3];
            myCube[3] = myCube[7];
            myCube[7] = myCube[5];
            myCube[5] = temp2;

        }
        public void DoF(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[6];
            Colors temp2 = myCube[7];
            Colors temp3 = myCube[8];
            myCube[6] = myCube[44];
            myCube[44] = myCube[47];
            myCube[47] = myCube[18];
            myCube[18] = temp1;
            myCube[7] = myCube[41];
            myCube[41] = myCube[46];
            myCube[46] = myCube[21];
            myCube[21] = temp2;
            myCube[8] = myCube[38];
            myCube[38] = myCube[45];
            myCube[45] = myCube[24];
            myCube[24] = temp3;
            //face
            temp1 = myCube[9];
            temp2 = myCube[10];
            myCube[9] = myCube[15];
            myCube[15] = myCube[17];
            myCube[17] = myCube[11];
            myCube[11] = temp1;
            myCube[10] = myCube[12];
            myCube[12] = myCube[16];
            myCube[16] = myCube[14];
            myCube[14] = temp2;
        }
        public void DoL(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[6];
            Colors temp2 = myCube[3];
            Colors temp3 = myCube[0];
            myCube[6] = myCube[29];
            myCube[29] = myCube[51];
            myCube[51] = myCube[15];
            myCube[15] = temp1;
            myCube[3] = myCube[32];
            myCube[32] = myCube[48];
            myCube[48] = myCube[12];
            myCube[12] = temp2;
            myCube[0] = myCube[35];
            myCube[35] = myCube[45];
            myCube[45] = myCube[9];
            myCube[9] = temp3;
            //face
            temp1 = myCube[36];
            temp2 = myCube[37];
            myCube[36] = myCube[42];
            myCube[42] = myCube[44];
            myCube[44] = myCube[38];
            myCube[38] = temp1;
            myCube[37] = myCube[39];
            myCube[39] = myCube[43];
            myCube[43] = myCube[41];
            myCube[41] = temp2;
        }
        public void DoB(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[0];
            Colors temp2 = myCube[1];
            Colors temp3 = myCube[2];
            myCube[0] = myCube[20];
            myCube[20] = myCube[53];
            myCube[53] = myCube[42];
            myCube[42] = temp1;
            myCube[1] = myCube[23];
            myCube[23] = myCube[52];
            myCube[52] = myCube[39];
            myCube[39] = temp2;
            myCube[2] = myCube[26];
            myCube[26] = myCube[51];
            myCube[51] = myCube[36];
            myCube[36] = temp3;
            //face
            temp1 = myCube[27];
            temp2 = myCube[28];
            myCube[27] = myCube[33];
            myCube[33] = myCube[35];
            myCube[35] = myCube[29];
            myCube[29] = temp1;
            myCube[28] = myCube[30];
            myCube[30] = myCube[34];
            myCube[34] = myCube[32];
            myCube[32] = temp2;
        }
        public void DoD(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[15];
            Colors temp2 = myCube[16];
            Colors temp3 = myCube[17];
            myCube[15] = myCube[42];
            myCube[42] = myCube[33];
            myCube[33] = myCube[24];
            myCube[24] = temp1;
            myCube[16] = myCube[43];
            myCube[43] = myCube[34];
            myCube[34] = myCube[25];
            myCube[25] = temp2;
            myCube[17] = myCube[44];
            myCube[44] = myCube[35];
            myCube[35] = myCube[26];
            myCube[26] = temp3;
            //face
            temp1 = myCube[45];
            temp2 = myCube[46];
            myCube[45] = myCube[51];
            myCube[51] = myCube[53];
            myCube[53] = myCube[47];
            myCube[47] = temp1;
            myCube[46] = myCube[48];
            myCube[48] = myCube[52];
            myCube[52] = myCube[50];
            myCube[50] = temp2;
        }
        public void DoRPrime(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[8];
            Colors temp2 = myCube[5];
            Colors temp3 = myCube[2];
            myCube[8] = myCube[27];
            myCube[27] = myCube[53];
            myCube[53] = myCube[17];
            myCube[17] = temp1;
            myCube[5] = myCube[30];
            myCube[30] = myCube[50];
            myCube[50] = myCube[14];
            myCube[14] = temp2;
            myCube[2] = myCube[33];
            myCube[33] = myCube[47];
            myCube[47] = myCube[11];
            myCube[11] = temp3;
            //face
            temp1 = myCube[18];
            temp2 = myCube[19];
            myCube[18] = myCube[20];
            myCube[20] = myCube[26];
            myCube[26] = myCube[24];
            myCube[24] = temp1;
            myCube[19] = myCube[23];
            myCube[23] = myCube[25];
            myCube[25] = myCube[21];
            myCube[21] = temp2;
        }
        public void DoUPrime(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[9];
            Colors temp2 = myCube[10];
            Colors temp3 = myCube[11];
            myCube[9] = myCube[36];
            myCube[36] = myCube[27];
            myCube[27] = myCube[18];
            myCube[18] = temp1;
            myCube[10] = myCube[37];
            myCube[37] = myCube[28];
            myCube[28] = myCube[19];
            myCube[19] = temp2;
            myCube[11] = myCube[38];
            myCube[38] = myCube[29];
            myCube[29] = myCube[20];
            myCube[20] = temp3;
            //face
            temp1 = myCube[0];
            temp2 = myCube[1];
            myCube[0] = myCube[2];
            myCube[2] = myCube[8];
            myCube[8] = myCube[6];
            myCube[6] = temp1;
            myCube[1] = myCube[5];
            myCube[5] = myCube[7];
            myCube[7] = myCube[3];
            myCube[3] = temp2;
        }
        public void DoFPrime(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[6];
            Colors temp2 = myCube[7];
            Colors temp3 = myCube[8];
            myCube[6] = myCube[18];
            myCube[18] = myCube[47];
            myCube[47] = myCube[44];
            myCube[44] = temp1;
            myCube[7] = myCube[21];
            myCube[21] = myCube[46];
            myCube[46] = myCube[41];
            myCube[41] = temp2;
            myCube[8] = myCube[24];
            myCube[24] = myCube[45];
            myCube[45] = myCube[38];
            myCube[38] = temp3;
            //face
            temp1 = myCube[9];
            temp2 = myCube[10];
            myCube[9] = myCube[11];
            myCube[11] = myCube[17];
            myCube[17] = myCube[15];
            myCube[15] = temp1;
            myCube[10] = myCube[14];
            myCube[14] = myCube[16];
            myCube[16] = myCube[12];
            myCube[12] = temp2;
        }
        public void DoLPrime(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[6];
            Colors temp2 = myCube[3];
            Colors temp3 = myCube[0];
            myCube[6] = myCube[15];
            myCube[15] = myCube[51];
            myCube[51] = myCube[29];
            myCube[29] = temp1;
            myCube[3] = myCube[12];
            myCube[12] = myCube[48];
            myCube[48] = myCube[32];
            myCube[32] = temp2;
            myCube[0] = myCube[9];
            myCube[9] = myCube[45];
            myCube[45] = myCube[35];
            myCube[35] = temp3;
            //face
            temp1 = myCube[36];
            temp2 = myCube[37];
            myCube[36] = myCube[38];
            myCube[38] = myCube[44];
            myCube[44] = myCube[42];
            myCube[42] = temp1;
            myCube[37] = myCube[41];
            myCube[41] = myCube[43];
            myCube[43] = myCube[39];
            myCube[39] = temp2;
        }
        public void DoBPrime(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[0];
            Colors temp2 = myCube[1];
            Colors temp3 = myCube[2];
            myCube[0] = myCube[42];
            myCube[42] = myCube[53];
            myCube[53] = myCube[20];
            myCube[20] = temp1;
            myCube[1] = myCube[39];
            myCube[39] = myCube[52];
            myCube[52] = myCube[23];
            myCube[23] = temp2;
            myCube[2] = myCube[36];
            myCube[36] = myCube[51];
            myCube[51] = myCube[26];
            myCube[26] = temp3;
            //face
            temp1 = myCube[27];
            temp2 = myCube[28];
            myCube[27] = myCube[29];
            myCube[29] = myCube[35];
            myCube[35] = myCube[33];
            myCube[33] = temp1;
            myCube[28] = myCube[32];
            myCube[32] = myCube[34];
            myCube[34] = myCube[30];
            myCube[30] = temp2;
        }
        public void DoDPrime(ref Colors[] myCube)
        {
            //side
            Colors temp1 = myCube[15];
            Colors temp2 = myCube[16];
            Colors temp3 = myCube[17];
            myCube[15] = myCube[24];
            myCube[24] = myCube[33];
            myCube[33] = myCube[42];
            myCube[42] = temp1;
            myCube[16] = myCube[25];
            myCube[25] = myCube[34];
            myCube[34] = myCube[43];
            myCube[43] = temp2;
            myCube[17] = myCube[26];
            myCube[26] = myCube[35];
            myCube[35] = myCube[44];
            myCube[44] = temp3;
            //face
            temp1 = myCube[45];
            temp2 = myCube[46];
            myCube[45] = myCube[47];
            myCube[47] = myCube[53];
            myCube[53] = myCube[51];
            myCube[51] = temp1;
            myCube[46] = myCube[50];
            myCube[50] = myCube[52];
            myCube[52] = myCube[48];
            myCube[48] = temp2;
        }
        public void DoF2(ref Colors[] myCube)
        {
            DoF(ref myCube);
            DoF(ref myCube);
        }
        public void DoR2(ref Colors[] myCube)
        {
            DoR(ref myCube);
            DoR(ref myCube);
        }
        public void DoU2(ref Colors[] myCube)
        {
            DoU(ref myCube);
            DoU(ref myCube);
        }
        public void DoL2(ref Colors[] myCube)
        {
            DoL(ref myCube);
            DoL(ref myCube);
        }
        public void DoD2(ref Colors[] myCube)
        {
            DoD(ref myCube);
            DoD(ref myCube);
        }
        public void DoB2(ref Colors[] myCube)
        {
            DoB(ref myCube);
            DoB(ref myCube);
        }
        public void DoY(ref Colors[] myCube)
        {
            DoU(ref myCube);
            DoDPrime(ref myCube);
            Colors temp1 = myCube[12];
            Colors temp2 = myCube[13];
            Colors temp3 = myCube[14];
            myCube[12] = myCube[21];
            myCube[21] = myCube[30];
            myCube[30] = myCube[39];
            myCube[39] = temp1;
            myCube[13] = myCube[22];
            myCube[22] = myCube[31];
            myCube[31] = myCube[40];
            myCube[40] = temp2;
            myCube[14] = myCube[23];
            myCube[23] = myCube[32];
            myCube[32] = myCube[41];
            myCube[41] = temp3;
        }
        public void DoYPrime(ref Colors[] myCube)
        {
            DoUPrime(ref myCube);
            DoD(ref myCube);
            Colors temp1 = myCube[12];
            Colors temp2 = myCube[13];
            Colors temp3 = myCube[14];
            myCube[12] = myCube[39];
            myCube[39] = myCube[30];
            myCube[30] = myCube[21];
            myCube[21] = temp1;
            myCube[13] = myCube[40];
            myCube[40] = myCube[31];
            myCube[31] = myCube[22];
            myCube[22] = temp2;
            myCube[14] = myCube[41];
            myCube[41] = myCube[32];
            myCube[32] = myCube[23];
            myCube[23] = temp3;
        }
        public void DoY2(ref Colors[] myCube)
        {
            DoY(ref myCube);
            DoY(ref myCube);
        }
        public void Dor(ref Colors[] myCube)
        {
            DoR(ref myCube);
            Colors temp1 = myCube[1];
            Colors temp2 = myCube[4];
            Colors temp3 = myCube[7];
            myCube[1] = myCube[10];
            myCube[10] = myCube[46];
            myCube[46] = myCube[34];
            myCube[34] = temp1;
            myCube[4] = myCube[13];
            myCube[13] = myCube[49];
            myCube[49] = myCube[31];
            myCube[31] = temp2;
            myCube[7] = myCube[16];
            myCube[16] = myCube[52];
            myCube[52] = myCube[28];
            myCube[28] = temp3;
        }
        public void Dor2(ref Colors[] myCube)
        {
            Dor(ref myCube);
            Dor(ref myCube);
        }
        public void DorPrime(ref Colors[] myCube)
        {
            DoRPrime(ref myCube);
            Colors temp1 = myCube[1];
            Colors temp2 = myCube[4];
            Colors temp3 = myCube[7];
            myCube[1] = myCube[34];
            myCube[34] = myCube[46];
            myCube[46] = myCube[10];
            myCube[10] = temp1;
            myCube[4] = myCube[31];
            myCube[31] = myCube[49];
            myCube[49] = myCube[13];
            myCube[13] = temp2;
            myCube[7] = myCube[28];
            myCube[28] = myCube[52];
            myCube[52] = myCube[16];
            myCube[16] = temp3;
        }
        public void Dof(ref Colors[] myCube)
        {
            DoF(ref myCube);
            Colors temp1 = myCube[3];
            Colors temp2 = myCube[4];
            Colors temp3 = myCube[5];
            myCube[3] = myCube[43];
            myCube[43] = myCube[50];
            myCube[50] = myCube[19];
            myCube[19] = temp1;
            myCube[4] = myCube[40];
            myCube[40] = myCube[49];
            myCube[49] = myCube[22];
            myCube[22] = temp2;
            myCube[5] = myCube[37];
            myCube[37] = myCube[48];
            myCube[48] = myCube[25];
            myCube[25] = temp3;
        }
        public void DofPrime(ref Colors[] myCube)
        {
            DoFPrime(ref myCube);
            Colors temp1 = myCube[3];
            Colors temp2 = myCube[4];
            Colors temp3 = myCube[5];
            myCube[3] = myCube[19];
            myCube[19] = myCube[50];
            myCube[50] = myCube[43];
            myCube[43] = temp1;
            myCube[4] = myCube[22];
            myCube[22] = myCube[49];
            myCube[49] = myCube[40];
            myCube[40] = temp2;
            myCube[5] = myCube[25];
            myCube[25] = myCube[48];
            myCube[48] = myCube[37];
            myCube[37] = temp3;
        }
        public void Dof2(ref Colors[] myCube)
        {
            Dof(ref myCube);
            Dof(ref myCube);
        }
        public void DoM2(ref Colors[] myCube)
        {
            Swap(ref myCube[1], ref myCube[46]);
            Swap(ref myCube[4], ref myCube[49]);
            Swap(ref myCube[7], ref myCube[52]);
            Swap(ref myCube[10], ref myCube[34]);
            Swap(ref myCube[13], ref myCube[31]);
            Swap(ref myCube[16], ref myCube[28]);

        }
        public void DoX(ref Colors[] myCube)
        {
            DoR(ref myCube);
            DoLPrime(ref myCube);
            Colors temp1 = myCube[1];
            Colors temp2 = myCube[4];
            Colors temp3 = myCube[7];
            myCube[1] = myCube[10];
            myCube[10] = myCube[46];
            myCube[46] = myCube[34];
            myCube[34] = temp1;
            myCube[4] = myCube[13];
            myCube[13] = myCube[49];
            myCube[49] = myCube[31];
            myCube[31] = temp2;
            myCube[7] = myCube[16];
            myCube[16] = myCube[52];
            myCube[52] = myCube[28];
            myCube[28] = temp3;
        }
        public void DoXPrime(ref Colors[] myCube)
        {
            DoRPrime(ref myCube);
            DoL(ref myCube);
            Colors temp1 = myCube[1];
            Colors temp2 = myCube[4];
            Colors temp3 = myCube[7];
            myCube[1] = myCube[34];
            myCube[34] = myCube[46];
            myCube[46] = myCube[10];
            myCube[10] = temp1;
            myCube[4] = myCube[31];
            myCube[31] = myCube[49];
            myCube[49] = myCube[13];
            myCube[13] = temp2;
            myCube[7] = myCube[28];
            myCube[28] = myCube[52];
            myCube[52] = myCube[16];
            myCube[16] = temp3;
        }
        public void DoX2(ref Colors[] myCube)
        {
            DoX(ref myCube);
            DoX(ref myCube);
        }
        public void DoZ(ref Colors[] myCube)
        {
            DoF(ref myCube);
            DoBPrime(ref myCube);
            Colors temp1 = myCube[3];
            Colors temp2 = myCube[4];
            Colors temp3 = myCube[5];
            myCube[3] = myCube[43];
            myCube[43] = myCube[50];
            myCube[50] = myCube[19];
            myCube[19] = temp1;
            myCube[4] = myCube[40];
            myCube[40] = myCube[49];
            myCube[49] = myCube[22];
            myCube[22] = temp2;
            myCube[5] = myCube[37];
            myCube[37] = myCube[48];
            myCube[48] = myCube[25];
            myCube[25] = temp3;
        }
        public void DoZPrime(ref Colors[] myCube)
        {
            DoFPrime(ref myCube);
            DoB(ref myCube);
            Colors temp1 = myCube[3];
            Colors temp2 = myCube[4];
            Colors temp3 = myCube[5];
            myCube[3] = myCube[19];
            myCube[19] = myCube[50];
            myCube[50] = myCube[43];
            myCube[43] = temp1;
            myCube[4] = myCube[22];
            myCube[22] = myCube[49];
            myCube[49] = myCube[40];
            myCube[40] = temp2;
            myCube[5] = myCube[25];
            myCube[25] = myCube[48];
            myCube[48] = myCube[37];
            myCube[37] = temp3;
        }
        public void DoZ2(ref Colors[] myCube)
        {
            DoZ(ref myCube);
            DoZ(ref myCube);
        }
        public void CenterOrient()
        {
            if (myCube[13] == Colors.green)
            {
                if (myCube[4] == Colors.white)
                {
                    //already oriented
                }
                else if (myCube[4] == Colors.red)
                {
                    Execute("z", ref myCube);
                }
                else if (myCube[4] == Colors.yellow)
                {
                    Execute("z2", ref myCube);
                }
                else if (myCube[4] == Colors.orange)
                {
                    Execute("z'", ref myCube);
                }
            }
            else if (myCube[13] == Colors.red)
            {
                if (myCube[4] == Colors.white)
                {
                    Execute("y'", ref myCube);
                }
                else if (myCube[4] == Colors.blue)
                {
                    Execute("zy'", ref myCube);
                }
                else if (myCube[4] == Colors.yellow)
                {
                    Execute("z2y'", ref myCube);
                }
                else if (myCube[4] == Colors.green)
                {
                    Execute("z'y'", ref myCube);
                }
            }
            else if (myCube[13] == Colors.blue)
            {
                if (myCube[4] == Colors.white)
                {
                    Execute("y2", ref myCube);
                }
                else if (myCube[4] == Colors.orange)
                {
                    Execute("zy2", ref myCube);
                }
                else if (myCube[4] == Colors.yellow)
                {
                    Execute("z2y2", ref myCube);
                }
                else if (myCube[4] == Colors.red)
                {
                    Execute("z'y2", ref myCube);
                }
            }
            else if (myCube[13] == Colors.orange)
            {
                if (myCube[4] == Colors.white)
                {
                    Execute("y", ref myCube);
                }
                else if (myCube[4] == Colors.green)
                {
                    Execute("zy", ref myCube);
                }
                else if (myCube[4] == Colors.yellow)
                {
                    Execute("z2y", ref myCube);
                }
                else if (myCube[4] == Colors.blue)
                {
                    Execute("z'y", ref myCube);
                }
            }
            else if (myCube[13] == Colors.white)
            {
                if (myCube[4] == Colors.blue)
                {
                    Execute("x", ref myCube);
                }
                else if (myCube[4] == Colors.red)
                {
                    Execute("zx", ref myCube);
                }
                else if (myCube[4] == Colors.green)
                {
                    Execute("z2x", ref myCube);
                }
                else if (myCube[4] == Colors.orange)
                {
                    Execute("z'x", ref myCube);
                }
            }
            else if (myCube[13] == Colors.yellow)
            {
                if (myCube[4] == Colors.green)
                {
                    Execute("x'", ref myCube);
                }
                else if (myCube[4] == Colors.red)
                {
                    Execute("zx'", ref myCube);
                }
                else if (myCube[4] == Colors.blue)
                {
                    Execute("z2x'", ref myCube);
                }
                else if (myCube[4] == Colors.orange)
                {
                    Execute("z'x'", ref myCube);
                }
            }
        }
        public void Swap(ref Colors a, ref Colors b)
        {
            Colors temp = a;
            a = b;
            b = temp;
        }
        public string Solve(ref Colors[] myCube)
        {
            DoCross(ref myCube);
            DoLayer2(ref myCube);
            TopCross(ref myCube);
            Oll(ref myCube);
            Last4Corners(ref myCube);
            GreenAlwaysInFront(ref myCube);
            Last4Edges(ref myCube);

            //optimize length of Solution
            string mySolution = OptimizedSteps();
            return mySolution;
        }
        public void DoCross(ref Colors[] myCube)
        {
            //solve the first cross edge
            if (myCube[16] == Colors.yellow && myCube[46] == Colors.green)
            {
                Execute("F2U'R'F", ref myCube);
            }
            else if (myCube[25] == Colors.yellow && myCube[50] == Colors.green)
            {
                Execute("RF", ref myCube);
            }
            else if (myCube[25] == Colors.green && myCube[50] == Colors.yellow)
            {
                Execute("D'", ref myCube);
            }
            else if (myCube[34] == Colors.yellow && myCube[52] == Colors.green)
            {
                Execute("B2UR'F", ref myCube);
            }
            else if (myCube[34] == Colors.green && myCube[52] == Colors.yellow)
            {
                Execute("D2", ref myCube);
            }
            else if (myCube[43] == Colors.yellow && myCube[48] == Colors.green)
            {
                Execute("L'F'", ref myCube);
            }
            else if (myCube[43] == Colors.green && myCube[48] == Colors.yellow)
            {
                Execute("D", ref myCube);

            }
            else if (myCube[12] == Colors.green && myCube[41] == Colors.yellow)
            {
                Execute("F'", ref myCube);
            }
            else if (myCube[12] == Colors.yellow && myCube[41] == Colors.green)
            {
                Execute("LD", ref myCube);
            }
            else if (myCube[14] == Colors.green && myCube[21] == Colors.yellow)
            {
                Execute("F", ref myCube);
            }
            else if (myCube[14] == Colors.yellow && myCube[21] == Colors.green)
            {
                Execute("R'D'", ref myCube);
            }
            else if (myCube[23] == Colors.green && myCube[30] == Colors.yellow)
            {
                Execute("RD'", ref myCube);
            }
            else if (myCube[23] == Colors.yellow && myCube[30] == Colors.green)
            {
                Execute("R2F", ref myCube);
            }
            else if (myCube[32] == Colors.yellow && myCube[39] == Colors.green)
            {
                Execute("L'D", ref myCube);
            }
            else if (myCube[32] == Colors.green && myCube[39] == Colors.yellow)
            {
                Execute("L2F'", ref myCube);
            }
            else if (myCube[7] == Colors.yellow && myCube[10] == Colors.green)
            {
                Execute("F2", ref myCube);
            }
            else if (myCube[7] == Colors.green && myCube[10] == Colors.yellow)
            {
                Execute("U'R'F", ref myCube);
            }
            else if (myCube[5] == Colors.yellow && myCube[19] == Colors.green)
            {
                Execute("UF2", ref myCube);
            }
            else if (myCube[5] == Colors.green && myCube[19] == Colors.yellow)
            {
                Execute("R'F", ref myCube);
            }
            else if (myCube[1] == Colors.yellow && myCube[28] == Colors.green)
            {
                Execute("U2F2", ref myCube);
            }
            else if (myCube[1] == Colors.green && myCube[28] == Colors.yellow)
            {
                Execute("UR'F", ref myCube);
            }
            else if (myCube[3] == Colors.yellow && myCube[37] == Colors.green)
            {
                Execute("U'F2", ref myCube);
            }
            else if (myCube[3] == Colors.green && myCube[37] == Colors.yellow)
            {
                Execute("LF'", ref myCube);
            }
            else
            {
                //already in correct position
            }
            Execute("y", ref myCube);
            //solve the second cross edge
            if (myCube[16] == Colors.yellow && myCube[46] == Colors.red)
            {
                Execute("F2U'R'F", ref myCube);
            }
            else if (myCube[25] == Colors.yellow && myCube[50] == Colors.red)
            {
                Execute("RF", ref myCube);
            }
            else if (myCube[25] == Colors.red && myCube[50] == Colors.yellow)
            {
                Execute("L'D'L", ref myCube);
            }
            else if (myCube[34] == Colors.yellow && myCube[52] == Colors.red)
            {
                Execute("B2UR'F", ref myCube);
            }
            else if (myCube[34] == Colors.red && myCube[52] == Colors.yellow)
            {
                Execute("L'D2L", ref myCube);
            }
            else if (myCube[12] == Colors.red && myCube[41] == Colors.yellow)
            {
                Execute("F'", ref myCube);
            }
            else if (myCube[12] == Colors.yellow && myCube[41] == Colors.red)
            {
                Execute("FU'R'F", ref myCube);
            }
            else if (myCube[14] == Colors.red && myCube[21] == Colors.yellow)
            {
                Execute("F", ref myCube);
            }
            else if (myCube[14] == Colors.yellow && myCube[21] == Colors.red)
            {
                Execute("RUF2", ref myCube);
            }
            else if (myCube[23] == Colors.yellow && myCube[30] == Colors.red)
            {
                Execute("R2F", ref myCube);
            }
            else if (myCube[23] == Colors.red && myCube[30] == Colors.yellow)
            {
                Execute("R'UF2", ref myCube);
            }
            else if (myCube[32] == Colors.red && myCube[39] == Colors.yellow)
            {
                Execute("B'U2F2", ref myCube);
            }
            else if (myCube[32] == Colors.yellow && myCube[39] == Colors.red)
            {
                Execute("B'UR'F", ref myCube);
            }
            else if (myCube[7] == Colors.yellow && myCube[10] == Colors.red)
            {
                Execute("F2", ref myCube);
            }
            else if (myCube[7] == Colors.red && myCube[10] == Colors.yellow)
            {
                Execute("U'R'F", ref myCube);
            }
            else if (myCube[5] == Colors.yellow && myCube[19] == Colors.red)
            {
                Execute("UF2", ref myCube);
            }
            else if (myCube[5] == Colors.red && myCube[19] == Colors.yellow)
            {
                Execute("R'F", ref myCube);
            }
            else if (myCube[1] == Colors.yellow && myCube[28] == Colors.red)
            {
                Execute("U2F2", ref myCube);
            }
            else if (myCube[1] == Colors.red && myCube[28] == Colors.yellow)
            {
                Execute("UR'F", ref myCube);
            }
            else if (myCube[3] == Colors.yellow && myCube[37] == Colors.red)
            {
                Execute("U'F2", ref myCube);
            }
            else if (myCube[3] == Colors.red && myCube[37] == Colors.yellow)
            {
                Execute("LF'L'", ref myCube);
            }
            else
            {
                //already in correct position
            }
            Execute("y", ref myCube);
            //solve the third cross edge
            if (myCube[16] == Colors.yellow && myCube[46] == Colors.blue)
            {
                Execute("F2U'R'F", ref myCube);
            }
            else if (myCube[25] == Colors.yellow && myCube[50] == Colors.blue)
            {
                Execute("RF", ref myCube);
            }
            else if (myCube[25] == Colors.blue && myCube[50] == Colors.yellow)
            {
                Execute("R2UF2", ref myCube);
            }
            else if (myCube[12] == Colors.blue && myCube[41] == Colors.yellow)
            {
                Execute("F'", ref myCube);
            }
            else if (myCube[12] == Colors.yellow && myCube[41] == Colors.blue)
            {
                Execute("FU'R'F", ref myCube);
            }
            else if (myCube[14] == Colors.blue && myCube[21] == Colors.yellow)
            {
                Execute("F", ref myCube);
            }
            else if (myCube[14] == Colors.yellow && myCube[21] == Colors.blue)
            {
                Execute("RUF2", ref myCube);
            }
            else if (myCube[23] == Colors.yellow && myCube[30] == Colors.blue)
            {
                Execute("R2F", ref myCube);
            }
            else if (myCube[23] == Colors.blue && myCube[30] == Colors.yellow)
            {
                Execute("R'UF2", ref myCube);
            }
            else if (myCube[32] == Colors.yellow && myCube[39] == Colors.blue)
            {
                Execute("LU'L'F2", ref myCube);
            }
            else if (myCube[32] == Colors.blue && myCube[39] == Colors.yellow)
            {
                Execute("LU2L'R'F", ref myCube);
            }
            else if (myCube[7] == Colors.yellow && myCube[10] == Colors.blue)
            {
                Execute("F2", ref myCube);
            }
            else if (myCube[7] == Colors.blue && myCube[10] == Colors.yellow)
            {
                Execute("U'R'F", ref myCube);
            }
            else if (myCube[5] == Colors.yellow && myCube[19] == Colors.blue)
            {
                Execute("UF2", ref myCube);
            }
            else if (myCube[5] == Colors.blue && myCube[19] == Colors.yellow)
            {
                Execute("R'F", ref myCube);
            }
            else if (myCube[1] == Colors.yellow && myCube[28] == Colors.blue)
            {
                Execute("U2F2", ref myCube);
            }
            else if (myCube[1] == Colors.blue && myCube[28] == Colors.yellow)
            {
                Execute("UR'F", ref myCube);
            }
            else if (myCube[3] == Colors.yellow && myCube[37] == Colors.blue)
            {
                Execute("U'F2", ref myCube);
            }
            else if (myCube[3] == Colors.blue && myCube[37] == Colors.yellow)
            {
                Execute("LF'L'", ref myCube);
            }
            else
            {
                //already correct
            }
            Execute("y", ref myCube);
            //solve the last cross edge
            if (myCube[16] == Colors.yellow && myCube[46] == Colors.orange)
            {
                Execute("F2U'R'FR", ref myCube);
            }
            else if (myCube[7] == Colors.yellow && myCube[10] == Colors.orange)
            {
                Execute("F2", ref myCube);
            }
            else if (myCube[7] == Colors.orange && myCube[10] == Colors.yellow)
            {
                Execute("U'R'FR", ref myCube);
            }
            else if (myCube[12] == Colors.orange && myCube[41] == Colors.yellow)
            {
                Execute("F'", ref myCube);
            }
            else if (myCube[12] == Colors.yellow && myCube[41] == Colors.orange)
            {
                Execute("FU'R'FR", ref myCube);
            }
            else if (myCube[14] == Colors.orange && myCube[21] == Colors.yellow)
            {
                Execute("F", ref myCube);
            }
            else if (myCube[14] == Colors.yellow && myCube[21] == Colors.orange)
            {
                Execute("RUR'F2", ref myCube);
            }
            else if (myCube[23] == Colors.yellow && myCube[30] == Colors.orange)
            {
                Execute("R2FR2", ref myCube);
            }
            else if (myCube[23] == Colors.orange && myCube[30] == Colors.yellow)
            {
                Execute("R'URF2", ref myCube);
            }
            else if (myCube[32] == Colors.yellow && myCube[39] == Colors.orange)
            {
                Execute("LU'L'F2", ref myCube);
            }
            else if (myCube[32] == Colors.orange && myCube[39] == Colors.yellow)
            {
                Execute("LU2L'R'FR", ref myCube);
            }
            else if (myCube[5] == Colors.yellow && myCube[19] == Colors.orange)
            {
                Execute("UF2", ref myCube);
            }
            else if (myCube[5] == Colors.orange && myCube[19] == Colors.yellow)
            {
                Execute("R'FR", ref myCube);
            }
            else if (myCube[1] == Colors.yellow && myCube[28] == Colors.orange)
            {
                Execute("U2F2", ref myCube);
            }
            else if (myCube[1] == Colors.orange && myCube[28] == Colors.yellow)
            {
                Execute("UR'FR", ref myCube);
            }
            else if (myCube[3] == Colors.yellow && myCube[37] == Colors.orange)
            {
                Execute("U'F2", ref myCube);
            }
            else if (myCube[3] == Colors.orange && myCube[37] == Colors.yellow)
            {
                Execute("LF'L'", ref myCube);
            }
            else
            {
                //already correct
            }
        }
        public void DoLayer2(ref Colors[] myCube)
        {
            //solve the first bottom-corner
            if (myCube[17] == Colors.yellow && myCube[24] == Colors.orange && myCube[47] == Colors.green)
            {
                Execute("RUR'U'RU2R'U'RUR'", ref myCube);
            }
            else if (myCube[17] == Colors.green && myCube[24] == Colors.yellow && myCube[47] == Colors.orange)
            {
                Execute("RUR'U'RUR'", ref myCube);
            }
            else if (myCube[26] == Colors.orange && myCube[33] == Colors.green && myCube[53] == Colors.yellow)
            {
                Execute("R'U2RU'RUR'", ref myCube);
            }
            else if (myCube[26] == Colors.yellow && myCube[33] == Colors.orange && myCube[53] == Colors.green)
            {
                Execute("R'U2RU'F'U'F", ref myCube);
            }
            else if (myCube[26] == Colors.green && myCube[33] == Colors.yellow && myCube[53] == Colors.orange)
            {
                Execute("R'URURUR'", ref myCube);
            }
            else if (myCube[35] == Colors.orange && myCube[42] == Colors.green && myCube[51] == Colors.yellow)
            {
                Execute("LU2L'F'U'F", ref myCube);
            }
            else if (myCube[35] == Colors.yellow && myCube[42] == Colors.orange && myCube[51] == Colors.green)
            {
                Execute("LU2L'RU2R'U'RUR'", ref myCube);
            }
            else if (myCube[35] == Colors.green && myCube[42] == Colors.yellow && myCube[51] == Colors.orange)
            {
                Execute("LU2L'RUR'", ref myCube);
            }
            else if (myCube[44] == Colors.orange && myCube[15] == Colors.green && myCube[45] == Colors.yellow)
            {
                Execute("L'U'LRUR'", ref myCube);
            }
            else if (myCube[44] == Colors.yellow && myCube[15] == Colors.orange && myCube[45] == Colors.green)
            {
                Execute("L'U'LF'U'F", ref myCube);
            }
            else if (myCube[44] == Colors.green && myCube[15] == Colors.yellow && myCube[45] == Colors.orange)
            {
                Execute("L'ULU'RUR'", ref myCube);
            }
            else if (myCube[8] == Colors.orange && myCube[11] == Colors.yellow && myCube[18] == Colors.green)
            {
                Execute("F'U'F", ref myCube);
            }
            else if (myCube[8] == Colors.yellow && myCube[11] == Colors.green && myCube[18] == Colors.orange)
            {
                Execute("RU2R'U'RUR'", ref myCube);
            }
            else if (myCube[8] == Colors.green && myCube[11] == Colors.orange && myCube[18] == Colors.yellow)
            {
                Execute("RUR'", ref myCube);
            }
            else if (myCube[2] == Colors.green && myCube[20] == Colors.orange && myCube[27] == Colors.yellow)
            {
                Execute("URUR'", ref myCube);
            }
            else if (myCube[2] == Colors.orange && myCube[20] == Colors.yellow && myCube[27] == Colors.green)
            {
                Execute("U2RU'R'", ref myCube);
            }
            else if (myCube[2] == Colors.yellow && myCube[20] == Colors.green && myCube[27] == Colors.orange)
            {
                Execute("R2FR2F'", ref myCube);
            }
            else if (myCube[0] == Colors.orange && myCube[29] == Colors.yellow && myCube[36] == Colors.green)
            {
                Execute("RU2R'", ref myCube);
            }
            else if (myCube[0] == Colors.green && myCube[29] == Colors.orange && myCube[36] == Colors.yellow)
            {
                Execute("U2RUR'", ref myCube);
            }
            else if (myCube[0] == Colors.yellow && myCube[29] == Colors.green && myCube[36] == Colors.orange)
            {
                Execute("UR2FR2F'", ref myCube);
            }
            else if (myCube[6] == Colors.yellow && myCube[38] == Colors.green && myCube[9] == Colors.orange)
            {
                Execute("U2R2FR2F'", ref myCube);
            }
            else if (myCube[6] == Colors.orange && myCube[38] == Colors.yellow && myCube[9] == Colors.green)
            {
                Execute("RU'R'", ref myCube);
            }
            else if (myCube[6] == Colors.green && myCube[38] == Colors.orange && myCube[9] == Colors.yellow)
            {
                Execute("U'RUR'", ref myCube);
            }
            else
            {
                //already correct
            }

            //solve the first layer-2 edge
            if (myCube[14] == Colors.green && myCube[21] == Colors.orange)
            {
                Execute("RU'R2U2RU'yL'UL", ref myCube);
            }
            else if (myCube[23] == Colors.orange && myCube[30] == Colors.green)
            {
                Execute("R'U2R2U'R'U'yL'UL", ref myCube);
            }
            else if (myCube[23] == Colors.green && myCube[30] == Colors.orange)
            {
                Execute("R'U'R2U'R2UR2UR'y", ref myCube);
            }
            else if (myCube[32] == Colors.green && myCube[39] == Colors.orange)
            {
                Execute("LU'L'URU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[32] == Colors.orange && myCube[39] == Colors.green)
            {
                Execute("LU2L'U'R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[12] == Colors.orange && myCube[41] == Colors.green)
            {
                Execute("L'ULR'FRF'RUR'y", ref myCube);
            }
            else if (myCube[12] == Colors.green && myCube[41] == Colors.orange)
            {
                Execute("L'U'LURU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[7] == Colors.green && myCube[10] == Colors.orange)
            {
                Execute("URU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[7] == Colors.orange && myCube[10] == Colors.green)
            {
                Execute("U2R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[5] == Colors.orange && myCube[19] == Colors.green)
            {
                Execute("U'R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[5] == Colors.green && myCube[19] == Colors.orange)
            {
                Execute("U2RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[1] == Colors.orange && myCube[28] == Colors.green)
            {
                Execute("R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[1] == Colors.green && myCube[28] == Colors.orange)
            {
                Execute("U'RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[3] == Colors.green && myCube[37] == Colors.orange)
            {
                Execute("RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[3] == Colors.orange && myCube[37] == Colors.green)
            {
                Execute("UR'FRF'RUR'y", ref myCube);
            }
            else
            {
                //already correct
                Execute("y", ref myCube);
            }

            //solve the second bottom-corner
            if (myCube[17] == Colors.red && myCube[24] == Colors.yellow && myCube[47] == Colors.green)
            {
                Execute("RUR'U'RUR'", ref myCube);
            }
            else if (myCube[17] == Colors.yellow && myCube[24] == Colors.green && myCube[47] == Colors.red)
            {
                Execute("RU'R2FRF'", ref myCube);
            }
            else if (myCube[26] == Colors.green && myCube[33] == Colors.red && myCube[53] == Colors.yellow)
            {
                Execute("R'U2RU'RUR'", ref myCube);
            }
            else if (myCube[26] == Colors.yellow && myCube[33] == Colors.green && myCube[53] == Colors.red)
            {
                Execute("R'U2R2U'R'", ref myCube);
            }
            else if (myCube[26] == Colors.red && myCube[33] == Colors.yellow && myCube[53] == Colors.green)
            {
                Execute("R'URURUR'", ref myCube);
            }
            else if (myCube[35] == Colors.green && myCube[42] == Colors.red && myCube[51] == Colors.yellow)
            {
                Execute("LU2L'F'U'F", ref myCube);
            }
            else if (myCube[35] == Colors.yellow && myCube[42] == Colors.green && myCube[51] == Colors.red)
            {
                Execute("LU2L'RU2R'U'RUR'", ref myCube);
            }
            else if (myCube[35] == Colors.red && myCube[42] == Colors.yellow && myCube[51] == Colors.green)
            {
                Execute("LU2L'RUR'", ref myCube);
            }
            else if (myCube[8] == Colors.green && myCube[11] == Colors.yellow && myCube[18] == Colors.red)
            {
                Execute("URU'R'", ref myCube);
            }
            else if (myCube[8] == Colors.yellow && myCube[11] == Colors.red && myCube[18] == Colors.green)
            {
                Execute("RU2R'U'RUR'", ref myCube);
            }
            else if (myCube[8] == Colors.red && myCube[11] == Colors.green && myCube[18] == Colors.yellow)
            {
                Execute("RUR'", ref myCube);
            }
            else if (myCube[2] == Colors.red && myCube[20] == Colors.green && myCube[27] == Colors.yellow)
            {
                Execute("URUR'", ref myCube);
            }
            else if (myCube[2] == Colors.green && myCube[20] == Colors.yellow && myCube[27] == Colors.red)
            {
                Execute("U2RU'R'", ref myCube);
            }
            else if (myCube[2] == Colors.yellow && myCube[20] == Colors.red && myCube[27] == Colors.green)
            {
                Execute("R2FR2F'", ref myCube);
            }
            else if (myCube[0] == Colors.green && myCube[29] == Colors.yellow && myCube[36] == Colors.red)
            {
                Execute("RU2R'", ref myCube);
            }
            else if (myCube[0] == Colors.red && myCube[29] == Colors.green && myCube[36] == Colors.yellow)
            {
                Execute("U2RUR'", ref myCube);
            }
            else if (myCube[0] == Colors.yellow && myCube[29] == Colors.red && myCube[36] == Colors.green)
            {
                Execute("UR2FR2F'", ref myCube);
            }
            else if (myCube[6] == Colors.yellow && myCube[38] == Colors.red && myCube[9] == Colors.green)
            {
                Execute("U2R2FR2F'", ref myCube);
            }
            else if (myCube[6] == Colors.green && myCube[38] == Colors.yellow && myCube[9] == Colors.red)
            {
                Execute("RU'R'", ref myCube);
            }
            else if (myCube[6] == Colors.red && myCube[38] == Colors.green && myCube[9] == Colors.yellow)
            {
                Execute("U'RUR'", ref myCube);
            }
            else
            {
                //already correct
            }

            //solve the second layer-2 edge
            if (myCube[14] == Colors.red && myCube[21] == Colors.green)
            {
                Execute("RU'R2U2RU'yL'UL", ref myCube);
            }
            else if (myCube[23] == Colors.green && myCube[30] == Colors.red)
            {
                Execute("R'U2R2U'R'U'yL'UL", ref myCube);
            }
            else if (myCube[23] == Colors.red && myCube[30] == Colors.green)
            {
                Execute("R'U'R2U'R2UR2UR'y", ref myCube);
            }
            else if (myCube[32] == Colors.red && myCube[39] == Colors.green)
            {
                Execute("LU'L'URU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[32] == Colors.green && myCube[39] == Colors.red)
            {
                Execute("LU2L'U'R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[7] == Colors.red && myCube[10] == Colors.green)
            {
                Execute("URU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[7] == Colors.green && myCube[10] == Colors.red)
            {
                Execute("U2R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[5] == Colors.green && myCube[19] == Colors.red)
            {
                Execute("U'R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[5] == Colors.red && myCube[19] == Colors.green)
            {
                Execute("U2RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[1] == Colors.green && myCube[28] == Colors.red)
            {
                Execute("R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[1] == Colors.red && myCube[28] == Colors.green)
            {
                Execute("U'RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[3] == Colors.red && myCube[37] == Colors.green)
            {
                Execute("RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[3] == Colors.green && myCube[37] == Colors.red)
            {
                Execute("UR'FRF'RUR'y", ref myCube);
            }
            else
            {
                //already correct
                Execute("y", ref myCube);
            }

            //solve the third bottom-corner
            if (myCube[17] == Colors.blue && myCube[24] == Colors.yellow && myCube[47] == Colors.red)
            {
                Execute("RUR'U'RUR'", ref myCube);
            }
            else if (myCube[17] == Colors.yellow && myCube[24] == Colors.red && myCube[47] == Colors.blue)
            {
                Execute("RU'R2FRF'", ref myCube);
            }
            else if (myCube[26] == Colors.red && myCube[33] == Colors.blue && myCube[53] == Colors.yellow)
            {
                Execute("R'U2RU'RUR'", ref myCube);
            }
            else if (myCube[26] == Colors.yellow && myCube[33] == Colors.red && myCube[53] == Colors.blue)
            {
                Execute("R'U2R2U'R'", ref myCube);
            }
            else if (myCube[26] == Colors.blue && myCube[33] == Colors.yellow && myCube[53] == Colors.red)
            {
                Execute("R'URURUR'", ref myCube);
            }
            else if (myCube[8] == Colors.red && myCube[11] == Colors.yellow && myCube[18] == Colors.blue)
            {
                Execute("URU'R'", ref myCube);
            }
            else if (myCube[8] == Colors.yellow && myCube[11] == Colors.blue && myCube[18] == Colors.red)
            {
                Execute("RU2R'U'RUR'", ref myCube);
            }
            else if (myCube[8] == Colors.blue && myCube[11] == Colors.red && myCube[18] == Colors.yellow)
            {
                Execute("RUR'", ref myCube);
            }
            else if (myCube[2] == Colors.blue && myCube[20] == Colors.red && myCube[27] == Colors.yellow)
            {
                Execute("URUR'", ref myCube);
            }
            else if (myCube[2] == Colors.red && myCube[20] == Colors.yellow && myCube[27] == Colors.blue)
            {
                Execute("U2RU'R'", ref myCube);
            }
            else if (myCube[2] == Colors.yellow && myCube[20] == Colors.blue && myCube[27] == Colors.red)
            {
                Execute("R2FR2F'", ref myCube);
            }
            else if (myCube[0] == Colors.red && myCube[29] == Colors.yellow && myCube[36] == Colors.blue)
            {
                Execute("RU2R'", ref myCube);
            }
            else if (myCube[0] == Colors.blue && myCube[29] == Colors.red && myCube[36] == Colors.yellow)
            {
                Execute("U2RUR'", ref myCube);
            }
            else if (myCube[0] == Colors.yellow && myCube[29] == Colors.blue && myCube[36] == Colors.red)
            {
                Execute("UR2FR2F'", ref myCube);
            }
            else if (myCube[6] == Colors.yellow && myCube[38] == Colors.blue && myCube[9] == Colors.red)
            {
                Execute("U2R2FR2F'", ref myCube);
            }
            else if (myCube[6] == Colors.red && myCube[38] == Colors.yellow && myCube[9] == Colors.blue)
            {
                Execute("RU'R'", ref myCube);
            }
            else if (myCube[6] == Colors.blue && myCube[38] == Colors.red && myCube[9] == Colors.yellow)
            {
                Execute("U'RUR'", ref myCube);
            }
            else
            {
                //already correct
            }

            //solve the third layer-2 edge
            if (myCube[14] == Colors.blue && myCube[21] == Colors.red)
            {
                Execute("RU'R2U2RU'yL'UL", ref myCube);
            }
            else if (myCube[23] == Colors.red && myCube[30] == Colors.blue)
            {
                Execute("R'U2R2U'R'U'yL'UL", ref myCube);
            }
            else if (myCube[23] == Colors.blue && myCube[30] == Colors.red)
            {
                Execute("R'U'R2U'R2UR2UR'y", ref myCube);
            }
            else if (myCube[7] == Colors.blue && myCube[10] == Colors.red)
            {
                Execute("URU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[7] == Colors.red && myCube[10] == Colors.blue)
            {
                Execute("U2R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[5] == Colors.red && myCube[19] == Colors.blue)
            {
                Execute("U'R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[5] == Colors.blue && myCube[19] == Colors.red)
            {
                Execute("U2RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[1] == Colors.red && myCube[28] == Colors.blue)
            {
                Execute("R'FRF'RUR'y", ref myCube);
            }
            else if (myCube[1] == Colors.blue && myCube[28] == Colors.red)
            {
                Execute("U'RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[3] == Colors.blue && myCube[37] == Colors.red)
            {
                Execute("RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[3] == Colors.red && myCube[37] == Colors.blue)
            {
                Execute("UR'FRF'RUR'y", ref myCube);
            }
            else
            {
                //already correct
                Execute("y", ref myCube);
            }

            //solve the last bottom-corner
            if (myCube[17] == Colors.orange && myCube[24] == Colors.yellow && myCube[47] == Colors.blue)
            {
                Execute("RUR'U'RUR'", ref myCube);
            }
            else if (myCube[17] == Colors.yellow && myCube[24] == Colors.blue && myCube[47] == Colors.orange)
            {
                Execute("RU'R2FRF'", ref myCube);
            }
            else if (myCube[8] == Colors.blue && myCube[11] == Colors.yellow && myCube[18] == Colors.orange)
            {
                Execute("URU'R'", ref myCube);
            }
            else if (myCube[8] == Colors.yellow && myCube[11] == Colors.orange && myCube[18] == Colors.blue)
            {
                Execute("RU2R'U'RUR'", ref myCube);
            }
            else if (myCube[8] == Colors.orange && myCube[11] == Colors.blue && myCube[18] == Colors.yellow)
            {
                Execute("RUR'", ref myCube);
            }
            else if (myCube[2] == Colors.orange && myCube[20] == Colors.blue && myCube[27] == Colors.yellow)
            {
                Execute("URUR'", ref myCube);
            }
            else if (myCube[2] == Colors.blue && myCube[20] == Colors.yellow && myCube[27] == Colors.orange)
            {
                Execute("U2RU'R'", ref myCube);
            }
            else if (myCube[2] == Colors.yellow && myCube[20] == Colors.orange && myCube[27] == Colors.blue)
            {
                Execute("URU2R'U'RUR'", ref myCube);
            }
            else if (myCube[0] == Colors.blue && myCube[29] == Colors.yellow && myCube[36] == Colors.orange)
            {
                Execute("RU2R'", ref myCube);
            }
            else if (myCube[0] == Colors.orange && myCube[29] == Colors.blue && myCube[36] == Colors.yellow)
            {
                Execute("U2RUR'", ref myCube);
            }
            else if (myCube[0] == Colors.yellow && myCube[29] == Colors.orange && myCube[36] == Colors.blue)
            {
                Execute("U2RU2R'U'RUR'", ref myCube);
            }
            else if (myCube[6] == Colors.yellow && myCube[38] == Colors.orange && myCube[9] == Colors.blue)
            {
                Execute("U'RU2R'U'RUR'", ref myCube);
            }
            else if (myCube[6] == Colors.blue && myCube[38] == Colors.yellow && myCube[9] == Colors.orange)
            {
                Execute("RU'R'", ref myCube);
            }
            else if (myCube[6] == Colors.orange && myCube[38] == Colors.blue && myCube[9] == Colors.yellow)
            {
                Execute("U'RUR'", ref myCube);
            }
            else
            {
                //already correct
            }

            //solve the last layer-2 edge
            if (myCube[14] == Colors.orange && myCube[21] == Colors.blue)
            {
                Execute("RU'R'y'UR'U2RU2R'UR", ref myCube);
            }
            else if (myCube[7] == Colors.orange && myCube[10] == Colors.blue)
            {
                Execute("URU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[7] == Colors.blue && myCube[10] == Colors.orange)
            {
                Execute("U2R'FRF'RUR'", ref myCube);
            }
            else if (myCube[5] == Colors.blue && myCube[19] == Colors.orange)
            {
                Execute("U'R'FRF'RUR'", ref myCube);
            }
            else if (myCube[5] == Colors.orange && myCube[19] == Colors.blue)
            {
                Execute("U2RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[1] == Colors.blue && myCube[28] == Colors.orange)
            {
                Execute("R'FRF'RUR'", ref myCube);
            }
            else if (myCube[1] == Colors.orange && myCube[28] == Colors.blue)
            {
                Execute("U'RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[3] == Colors.orange && myCube[37] == Colors.blue)
            {
                Execute("RU'R'U'yL'UL", ref myCube);
            }
            else if (myCube[3] == Colors.blue && myCube[37] == Colors.orange)
            {
                Execute("UR'FRF'RUR'", ref myCube);
            }
            else
            {
                //already correct
            }
        }
        public void Execute(string moves, ref Colors[] myCube)
        {
            for (int i = 0; i < moves.Length; i++)
            {
                switch (moves[i])
                {
                    case 'R':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("R'");
                                DoRPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("R2");
                                DoR2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("R");
                                DoR(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("R");
                            DoR(ref myCube);
                        }
                        break;
                    case 'L':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("L'");
                                DoLPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("L2");
                                DoL2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("L");
                                DoL(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("L");
                            DoL(ref myCube);
                        }
                        break;
                    case 'U':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("U'");
                                DoUPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("U2");
                                DoU2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("U");
                                DoU(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("U");
                            DoU(ref myCube);
                        }
                        break;
                    case 'F':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("F'");
                                DoFPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("F2");
                                DoF2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("F");
                                DoF(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("F");
                            DoF(ref myCube);
                        }
                        break;
                    case 'B':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("B'");
                                DoBPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("B2");
                                DoB2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("B");
                                DoB(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("B");
                            DoB(ref myCube);
                        }
                        break;
                    case 'D':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("D'");
                                DoDPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("D2");
                                DoD2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("D");
                                DoD(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("D");
                            DoD(ref myCube);
                        }
                        break;
                    case 'y':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("y'");
                                DoYPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("y2");
                                DoY2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("y");
                                DoY(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("y");
                            DoY(ref myCube);
                        }
                        break;
                    case 'r':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("r'");
                                DorPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("r2");
                                Dor2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("r");
                                Dor(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("r");
                            Dor(ref myCube);
                        }
                        break;
                    case 'f':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("f'");
                                DofPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("f2");
                                Dof2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("f");
                                Dof(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("f");
                            Dof(ref myCube);
                        }
                        break;
                    case 'M':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                //DofMrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("M2");
                                DoM2(ref myCube);
                                i++;
                            }
                            else
                            {
                                //DoM(ref myCube);
                            }
                        }
                        else
                        {
                            //DoM(ref myCube);
                        }
                        break;
                    case 'x':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("x'");
                                DoXPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("x2");
                                DoX2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("x");
                                DoX(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("x");
                            DoX(ref myCube);
                        }
                        break;
                    case 'z':
                        if (i != moves.Length - 1)
                        {
                            if (moves[i + 1] == '\'')
                            {
                                lstMerged.Add("z'");
                                DoZPrime(ref myCube);
                                i++;
                            }
                            else if (moves[i + 1] == '2')
                            {
                                lstMerged.Add("z2");
                                DoZ2(ref myCube);
                                i++;
                            }
                            else
                            {
                                lstMerged.Add("z");
                                DoZ(ref myCube);
                            }
                        }
                        else
                        {
                            lstMerged.Add("z");
                            DoZ(ref myCube);
                        }
                        break;
                }
            }
        }
        public void TopCross(ref Colors[] myCube)
        {
            if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white)
            {
                //already correct
            }
            else if (myCube[1] != Colors.white && myCube[3] != Colors.white && myCube[5] != Colors.white && myCube[7] != Colors.white)
            {
                Execute("FRUR'U'F'fRUR'U'f'", ref myCube);
            }
            else if (myCube[7] == Colors.white && myCube[5] == Colors.white && myCube[1] != Colors.white && myCube[3] != Colors.white)
            {
                Execute("fRUR'U'f'", ref myCube);
            }
            else if (myCube[7] == Colors.white && myCube[3] == Colors.white && myCube[1] != Colors.white && myCube[5] != Colors.white)
            {
                Execute("U'fRUR'U'f'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[5] != Colors.white && myCube[7] != Colors.white)
            {
                Execute("U2fRUR'U'f'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[5] == Colors.white && myCube[7] != Colors.white && myCube[3] != Colors.white)
            {
                Execute("UfRUR'U'f'", ref myCube);
            }
            else if (myCube[3] == Colors.white && myCube[5] == Colors.white && myCube[1] != Colors.white && myCube[7] != Colors.white)
            {
                Execute("FRUR'U'F'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[7] == Colors.white && myCube[3] != Colors.white && myCube[5] != Colors.white)
            {
                Execute("UFRUR'U'F'", ref myCube);
            }

        }
        public void Oll(ref Colors[] myCube)
        {
            //Oll 1
            if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[6] == Colors.white && myCube[7] == Colors.white && myCube[11] == Colors.white && myCube[20] == Colors.white && myCube[29] == Colors.white)
            {
                Execute("RUR'URU2R'", ref myCube);
            }
            else if (myCube[0] == Colors.white && myCube[0] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[38] == Colors.white && myCube[11] == Colors.white && myCube[20] == Colors.white)
            {
                Execute("U'RUR'URU2R'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[2] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[11] == Colors.white && myCube[38] == Colors.white && myCube[29] == Colors.white)
            {
                Execute("U2RUR'URU2R'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[8] == Colors.white && myCube[20] == Colors.white && myCube[38] == Colors.white && myCube[29] == Colors.white)
            {
                Execute("URUR'URU2R'", ref myCube);
            }
            //Oll 2
            else if (myCube[1] == Colors.white && myCube[2] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[18] == Colors.white && myCube[9] == Colors.white && myCube[36] == Colors.white)
            {
                Execute("RU2R'U'RU'R'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[8] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[36] == Colors.white && myCube[9] == Colors.white && myCube[27] == Colors.white)
            {
                Execute("U'RU2R'U'RU'R'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[6] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[36] == Colors.white && myCube[18] == Colors.white && myCube[27] == Colors.white)
            {
                Execute("U2RU2R'U'RU'R'", ref myCube);
            }
            else if (myCube[0] == Colors.white && myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[9] == Colors.white && myCube[18] == Colors.white && myCube[27] == Colors.white)
            {
                Execute("URU2R'U'RU'R'", ref myCube);
            }
            //Oll 3
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[36] == Colors.white && myCube[38] == Colors.white && myCube[18] == Colors.white && myCube[20] == Colors.white)
            {
                Execute("R'U'RU'R'URU'R'U2R", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[9] == Colors.white && myCube[11] == Colors.white && myCube[27] == Colors.white && myCube[29] == Colors.white)
            {
                Execute("RU2R'U'RUR'U'RU'R'", ref myCube);
            }
            //Oll 4
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[36] == Colors.white && myCube[38] == Colors.white && myCube[27] == Colors.white && myCube[11] == Colors.white)
            {
                Execute("RU2R2U'R2U'R2U2R", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[18] == Colors.white && myCube[38] == Colors.white && myCube[27] == Colors.white && myCube[29] == Colors.white)
            {
                Execute("U'RU2R2U'R2U'R2U2R", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[9] == Colors.white && myCube[29] == Colors.white && myCube[18] == Colors.white && myCube[20] == Colors.white)
            {
                Execute("U2RU2R2U'R2U'R2U2R", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[36] == Colors.white && myCube[20] == Colors.white && myCube[9] == Colors.white && myCube[11] == Colors.white)
            {
                Execute("URU2R2U'R2U'R2U2R", ref myCube);
            }
            //Oll 5
            else if (myCube[9] == Colors.white && myCube[1] == Colors.white && myCube[2] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[8] == Colors.white && myCube[29] == Colors.white)
            {
                Execute("rUR'U'r'FRF'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[6] == Colors.white && myCube[7] == Colors.white && myCube[8] == Colors.white && myCube[20] == Colors.white && myCube[36] == Colors.white)
            {
                Execute("U'rUR'U'r'FRF'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[6] == Colors.white && myCube[7] == Colors.white && myCube[0] == Colors.white && myCube[11] == Colors.white && myCube[27] == Colors.white)
            {
                Execute("U2rUR'U'r'FRF'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[0] == Colors.white && myCube[7] == Colors.white && myCube[2] == Colors.white && myCube[18] == Colors.white && myCube[38] == Colors.white)
            {
                Execute("UrUR'U'r'FRF'", ref myCube);
            }
            //Oll 6
            else if (myCube[0] == Colors.white && myCube[1] == Colors.white && myCube[27] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[8] == Colors.white && myCube[38] == Colors.white)
            {
                Execute("FRU'R'U'RU2R'U'F'", ref myCube);
            }
            else if (myCube[6] == Colors.white && myCube[1] == Colors.white && myCube[2] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[18] == Colors.white && myCube[29] == Colors.white)
            {
                Execute("U'FRU'R'U'RU2R'U'F'", ref myCube);
            }
            else if (myCube[0] == Colors.white && myCube[1] == Colors.white && myCube[8] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[9] == Colors.white && myCube[20] == Colors.white)
            {
                Execute("U2FRU'R'U'RU2R'U'F'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[2] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[6] == Colors.white && myCube[7] == Colors.white && myCube[11] == Colors.white && myCube[36] == Colors.white)
            {
                Execute("UFRU'R'U'RU2R'U'F'", ref myCube);
            }
            //Oll 7
            else if (myCube[0] == Colors.white && myCube[1] == Colors.white && myCube[2] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[9] == Colors.white && myCube[11] == Colors.white)
            {
                Execute("R2DR'U2RD'R'U2R'", ref myCube);
            }
            else if (myCube[8] == Colors.white && myCube[1] == Colors.white && myCube[2] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[7] == Colors.white && myCube[36] == Colors.white && myCube[38] == Colors.white)
            {
                Execute("U'R2DR'U2RD'R'U2R'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[6] == Colors.white && myCube[7] == Colors.white && myCube[8] == Colors.white && myCube[27] == Colors.white && myCube[29] == Colors.white)
            {
                Execute("U2R2DR'U2RD'R'U2R'", ref myCube);
            }
            else if (myCube[1] == Colors.white && myCube[3] == Colors.white && myCube[4] == Colors.white && myCube[5] == Colors.white && myCube[6] == Colors.white && myCube[7] == Colors.white && myCube[0] == Colors.white && myCube[18] == Colors.white && myCube[20] == Colors.white)
            {
                Execute("UR2DR'U2RD'R'U2R'", ref myCube);
            }
        }
        public void Last4Corners(ref Colors[] myCube)
        {
            if (myCube[9] == myCube[11] && myCube[36] == myCube[38] && myCube[27] == myCube[29] && myCube[18] == myCube[20])
            {
                //already correct
            }
            else if (myCube[9] != myCube[11] && myCube[36] != myCube[38] && myCube[27] != myCube[29] && myCube[18] != myCube[20])
            {
                Execute("RUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
                Last4CornersAgain(ref myCube);
            }
            else if (myCube[9] == myCube[11] && myCube[36] != myCube[38] && myCube[27] != myCube[29] && myCube[18] != myCube[20])
            {
                Execute("URUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
            }
            else if (myCube[9] != myCube[11] && myCube[36] == myCube[38] && myCube[27] != myCube[29] && myCube[18] != myCube[20])
            {
                Execute("RUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
            }
            else if (myCube[9] != myCube[11] && myCube[36] != myCube[38] && myCube[27] == myCube[29] && myCube[18] != myCube[20])
            {
                Execute("U'RUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
            }
            else if (myCube[9] != myCube[11] && myCube[36] != myCube[38] && myCube[27] != myCube[29] && myCube[18] == myCube[20])
            {
                Execute("U2RUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
            }
            if (myCube[13] == Colors.red)
            {
                if (myCube[9] == Colors.red)
                {
                    //already correct
                }
                else if (myCube[9] == Colors.green)
                {
                    Execute("U", ref myCube);
                }
                else if (myCube[9] == Colors.orange)
                {
                    Execute("U2", ref myCube);
                }
                else if (myCube[9] == Colors.blue)
                {
                    Execute("U'", ref myCube);
                }
            }
            else if (myCube[13] == Colors.green)
            {
                if (myCube[9] == Colors.green)
                {
                    //already correct
                }
                else if (myCube[9] == Colors.red)
                {
                    Execute("U'", ref myCube);
                }
                else if (myCube[9] == Colors.blue)
                {
                    Execute("U2", ref myCube);
                }
                else if (myCube[9] == Colors.orange)
                {
                    Execute("U", ref myCube);
                }
            }
            else if (myCube[13] == Colors.orange)
            {
                if (myCube[9] == Colors.orange)
                {
                    //already correct
                }
                else if (myCube[9] == Colors.green)
                {
                    Execute("U'", ref myCube);
                }
                else if (myCube[9] == Colors.red)
                {
                    Execute("U2", ref myCube);
                }
                else if (myCube[9] == Colors.blue)
                {
                    Execute("U", ref myCube);
                }
            }
            else if (myCube[13] == Colors.blue)
            {
                if (myCube[9] == Colors.blue)
                {
                    //already correct
                }
                else if (myCube[9] == Colors.orange)
                {
                    Execute("U'", ref myCube);
                }
                else if (myCube[9] == Colors.green)
                {
                    Execute("U2", ref myCube);
                }
                else if (myCube[9] == Colors.red)
                {
                    Execute("U", ref myCube);
                }
            }
        }
        public void Last4CornersAgain(ref Colors[] myCube)
        {
            if (myCube[9] == myCube[11] && myCube[36] == myCube[38] && myCube[27] == myCube[29] && myCube[18] == myCube[20])
            {
                //already correct
            }
            else if (myCube[9] != myCube[11] && myCube[36] != myCube[38] && myCube[27] != myCube[29] && myCube[18] != myCube[20])
            {
                Execute("RUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
            }
            else if (myCube[9] == myCube[11] && myCube[36] != myCube[38] && myCube[27] != myCube[29] && myCube[18] != myCube[20])
            {
                Execute("URUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
            }
            else if (myCube[9] != myCube[11] && myCube[36] == myCube[38] && myCube[27] != myCube[29] && myCube[18] != myCube[20])
            {
                Execute("RUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
            }
            else if (myCube[9] != myCube[11] && myCube[36] != myCube[38] && myCube[27] == myCube[29] && myCube[18] != myCube[20])
            {
                Execute("U'RUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
            }
            else if (myCube[9] != myCube[11] && myCube[36] != myCube[38] && myCube[27] != myCube[29] && myCube[18] == myCube[20])
            {
                Execute("U2RUR'U'R'FR2U'R'U'RUR'F'", ref myCube);
            }
            if (myCube[13] == Colors.red)
            {
                if (myCube[9] == Colors.red)
                {
                    //already correct
                }
                else if (myCube[9] == Colors.green)
                {
                    Execute("U", ref myCube);
                }
                else if (myCube[9] == Colors.orange)
                {
                    Execute("U2", ref myCube);
                }
                else if (myCube[9] == Colors.blue)
                {
                    Execute("U'", ref myCube);
                }
            }
            else if (myCube[13] == Colors.green)
            {
                if (myCube[9] == Colors.green)
                {
                    //already correct
                }
                else if (myCube[9] == Colors.red)
                {
                    Execute("U'", ref myCube);
                }
                else if (myCube[9] == Colors.blue)
                {
                    Execute("U2", ref myCube);
                }
                else if (myCube[9] == Colors.orange)
                {
                    Execute("U", ref myCube);
                }
            }
            else if (myCube[13] == Colors.orange)
            {
                if (myCube[9] == Colors.orange)
                {
                    //already correct
                }
                else if (myCube[9] == Colors.green)
                {
                    Execute("U'", ref myCube);
                }
                else if (myCube[9] == Colors.red)
                {
                    Execute("U2", ref myCube);
                }
                else if (myCube[9] == Colors.blue)
                {
                    Execute("U", ref myCube);
                }
            }
            else if (myCube[13] == Colors.blue)
            {
                if (myCube[9] == Colors.blue)
                {
                    //already correct
                }
                else if (myCube[9] == Colors.orange)
                {
                    Execute("U'", ref myCube);
                }
                else if (myCube[9] == Colors.green)
                {
                    Execute("U2", ref myCube);
                }
                else if (myCube[9] == Colors.red)
                {
                    Execute("U", ref myCube);
                }
            }
        }
        public void GreenAlwaysInFront(ref Colors[] myCube)
        {
            if (myCube[13] == Colors.green)
            {
                //already correct
            }
            else if (myCube[13] == Colors.red)
            {
                Execute("y'", ref myCube);
            }
            else if (myCube[13] == Colors.blue)
            {
                Execute("y2", ref myCube);
            }
            else if (myCube[13] == Colors.orange)
            {
                Execute("y", ref myCube);
            }
        }
        public void Last4Edges(ref Colors[] myCube)
        {
            if (myCube[10] == Colors.green && myCube[37] == Colors.red && myCube[28] == Colors.orange && myCube[19] == Colors.blue)
            {
                Execute("y2RU'RURURU'R'U'R2", ref myCube);
            }
            else if (myCube[10] == Colors.green && myCube[37] == Colors.blue && myCube[28] == Colors.red && myCube[19] == Colors.orange)
            {
                Execute("y2R2URUR'U'R'U'R'UR'", ref myCube);
            }
            else if (myCube[10] == Colors.blue && myCube[37] == Colors.green && myCube[28] == Colors.orange && myCube[19] == Colors.red)
            {
                Execute("y'RU'RURURU'R'U'R2", ref myCube);
            }
            else if (myCube[10] == Colors.orange && myCube[37] == Colors.blue && myCube[28] == Colors.green && myCube[19] == Colors.red)
            {
                Execute("y'R2URUR'U'R'U'R'UR'", ref myCube);
            }
            else if (myCube[10] == Colors.red && myCube[37] == Colors.orange && myCube[28] == Colors.green && myCube[19] == Colors.blue)
            {
                Execute("yRU'RURURU'R'U'R2", ref myCube);
            }
            else if (myCube[10] == Colors.blue && myCube[37] == Colors.orange && myCube[28] == Colors.red && myCube[19] == Colors.green)
            {
                Execute("yR2URUR'U'R'U'R'UR'", ref myCube);
            }
            else if (myCube[10] == Colors.red && myCube[37] == Colors.green && myCube[28] == Colors.blue && myCube[19] == Colors.orange)
            {
                Execute("RU'RURURU'R'U'R2", ref myCube);
            }
            else if (myCube[10] == Colors.orange && myCube[37] == Colors.red && myCube[28] == Colors.blue && myCube[19] == Colors.green)
            {
                Execute("R2URUR'U'R'U'R'UR'", ref myCube);
            }
            else if (myCube[10] == Colors.blue && myCube[37] == Colors.red && myCube[28] == Colors.green && myCube[19] == Colors.orange)
            {
                Execute("M2UM2U2M2UM2", ref myCube);
            }
            else if (myCube[10] == Colors.orange && myCube[37] == Colors.green && myCube[28] == Colors.red && myCube[19] == Colors.blue)
            {
                Execute("R'U'RU'RURU'R'URUR2U'R'U2", ref myCube);
            }
            else if (myCube[10] == Colors.red && myCube[37] == Colors.blue && myCube[28] == Colors.orange && myCube[19] == Colors.green)
            {
                Execute("UR'U'RU'RURU'R'URUR2U'R'U", ref myCube);
            }
            //done.
        }
        public void getCurrentScramble()
        {
            for (int i = 0; i < 54; i++)
            {
                Button btn = this.Controls.Find("button" + i, true).FirstOrDefault() as Button;
                if (btn.BackColor == Color.Red)
                {
                    myCube[i] = Colors.red;
                }
                else if (btn.BackColor == Color.Green)
                {
                    myCube[i] = Colors.green;
                }
                else if (btn.BackColor == Color.Blue)
                {
                    myCube[i] = Colors.blue;
                }
                else if (btn.BackColor == Color.Yellow)
                {
                    myCube[i] = Colors.yellow;
                }
                else if (btn.BackColor == Color.Orange)
                {
                    myCube[i] = Colors.orange;
                }
                else if (btn.BackColor == Color.White)
                {
                    myCube[i] = Colors.white;
                }
            }
        }
        public void ClearColorOfButton()
        {
            for (int i = 0; i < 54; i++)
            {
                Button Btn = this.Controls.Find("button" + i, true).FirstOrDefault() as Button;
                Btn.BackColor = Color.White;
            }
        }
        public void ButtonReset_Click(object sender, EventArgs e)
        {
            InitializeColor();
            ClearColorOfButton();
            InitializeCube();
            lstMerged.Clear();
            label1.Text = "";
            label2.Text = "Kociemba Solution:";
            label4.Text = "";
            label3.Text = "Fridrich Solution:";
        }
        public void InitializeCube()
        {
            for (int i = 0; i < 54; i++)
            {
                if (i <= 8) myCube[i] = Colors.white;
                else if (i <= 17) myCube[i] = Colors.green;
                else if (i <= 26) myCube[i] = Colors.red;
                else if (i <= 35) myCube[i] = Colors.blue;
                else if (i <= 44) myCube[i] = Colors.orange;
                else myCube[i] = Colors.yellow;
            }
        }
        public void Scramble(string scramble)
        {
            Execute(scramble, ref myCube);
        }

        public string OptimizedSteps()
        {
            for (int i = 0; i < lstMerged.Count - 1; i++)
            {
                if (lstMerged[i] == "R")
                {
                    if (lstMerged[i + 1] == "R")
                    {
                        lstMerged[i] = "R2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "R'")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "R2")
                    {
                        lstMerged[i] = "R'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "R'")
                {
                    if (lstMerged[i + 1] == "R'")
                    {
                        lstMerged[i] = "R2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "R")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "R2")
                    {
                        lstMerged[i] = "R";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "R2")
                {
                    if (lstMerged[i + 1] == "R'")
                    {
                        lstMerged[i] = "R";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "R2")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "R")
                    {
                        lstMerged[i] = "R'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "U")
                {
                    if (lstMerged[i + 1] == "U")
                    {
                        lstMerged[i] = "U2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "U'")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "U2")
                    {
                        lstMerged[i] = "U'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "U'")
                {
                    if (lstMerged[i + 1] == "U'")
                    {
                        lstMerged[i] = "U2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "U")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "U2")
                    {
                        lstMerged[i] = "U";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "U2")
                {
                    if (lstMerged[i + 1] == "U'")
                    {
                        lstMerged[i] = "U";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "U2")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "U")
                    {
                        lstMerged[i] = "U'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "F")
                {
                    if (lstMerged[i + 1] == "F")
                    {
                        lstMerged[i] = "F2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "F'")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "F2")
                    {
                        lstMerged[i] = "F'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "F'")
                {
                    if (lstMerged[i + 1] == "F'")
                    {
                        lstMerged[i] = "F2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "F")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "F2")
                    {
                        lstMerged[i] = "F";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "F2")
                {
                    if (lstMerged[i + 1] == "F'")
                    {
                        lstMerged[i] = "F";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "F2")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "F")
                    {
                        lstMerged[i] = "F'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "L")
                {
                    if (lstMerged[i + 1] == "L")
                    {
                        lstMerged[i] = "L2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "L'")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "L2")
                    {
                        lstMerged[i] = "L'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "L'")
                {
                    if (lstMerged[i + 1] == "L'")
                    {
                        lstMerged[i] = "L2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "L")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "L2")
                    {
                        lstMerged[i] = "L";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "L2")
                {
                    if (lstMerged[i + 1] == "L'")
                    {
                        lstMerged[i] = "L";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "L2")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "L")
                    {
                        lstMerged[i] = "L'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "B")
                {
                    if (lstMerged[i + 1] == "B")
                    {
                        lstMerged[i] = "B2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "B'")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "B2")
                    {
                        lstMerged[i] = "B'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "B'")
                {
                    if (lstMerged[i + 1] == "B'")
                    {
                        lstMerged[i] = "B2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "B")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "B2")
                    {
                        lstMerged[i] = "B";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "B2")
                {
                    if (lstMerged[i + 1] == "B'")
                    {
                        lstMerged[i] = "B";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "B2")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "B")
                    {
                        lstMerged[i] = "B'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "D")
                {
                    if (lstMerged[i + 1] == "D")
                    {
                        lstMerged[i] = "D2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "D'")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "D2")
                    {
                        lstMerged[i] = "D'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "D'")
                {
                    if (lstMerged[i + 1] == "D'")
                    {
                        lstMerged[i] = "D2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "D")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "D2")
                    {
                        lstMerged[i] = "D";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "D2")
                {
                    if (lstMerged[i + 1] == "D'")
                    {
                        lstMerged[i] = "D";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "D2")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "D")
                    {
                        lstMerged[i] = "D'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "y")
                {
                    if (lstMerged[i + 1] == "y")
                    {
                        lstMerged[i] = "y2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "y'")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "y2")
                    {
                        lstMerged[i] = "y'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "y'")
                {
                    if (lstMerged[i + 1] == "y'")
                    {
                        lstMerged[i] = "y2";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "y")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "y2")
                    {
                        lstMerged[i] = "y";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
                else if (lstMerged[i] == "y2")
                {
                    if (lstMerged[i + 1] == "y'")
                    {
                        lstMerged[i] = "y";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "y2")
                    {
                        lstMerged.RemoveAt(i + 1);
                        lstMerged.RemoveAt(i);
                        i = 0;
                    }
                    else if (lstMerged[i + 1] == "y")
                    {
                        lstMerged[i] = "y'";
                        lstMerged.RemoveAt(i + 1);
                        i = 0;
                    }
                }
            }
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < lstMerged.Count; i++)
            {
                result.Append(lstMerged[i] + " ");
                if (i == 23 || i == 46 || i == 69 || i == 92 || i == 115 || i == 138 || i == 161 || i == 184 || i == 207 || i == 230 || i == 253) result.Append("\n\n");
            }
            return string.Join(" ", result);
        }

        public void calculateAverageMoves()
        {
            List<string> result = new List<string>();
            string fileName = "C:/Users/pc/Desktop/100.txt";
            StringBuilder report = new StringBuilder();
            report.Append("Detail:");
            report.Append("\n__________________________________________________________________________________\n");
            int totalMoves = 0;
            string fileOutput = "C:/Users/pc/Desktop/100ScramblesSolutions.txt";
            try
            {
                if (File.Exists(fileOutput))
                {
                    File.Delete(fileOutput);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("error file " + Ex.ToString());
            }
            try
            {
                int countCorrect = 0;
                int countFail = 0;
                int min = 150;
                int max = 0;
                StringBuilder minScript = new StringBuilder();
                StringBuilder minSolution = new StringBuilder();
                StringBuilder maxScript = new StringBuilder();
                StringBuilder maxSolution = new StringBuilder();
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
                    using (StreamWriter sw = File.CreateText(fileOutput))
                    {
                        int count = 1;
                        while ((s = sr.ReadLine()) != null)
                        {
                            InitializeCube();
                            Scramble(s);
                            CenterOrient();
                            BindingStateForKociembaCube();
                            Solve(ref myCube);
                            if (checkFinished() == true)
                            {
                                countCorrect++;
                                StringBuilder process = new StringBuilder();
                                c.corners = corners;
                                c.edges = edges;
                                result.Clear();
                                result = Search.patternSolve(c, TwoPhaseSolver.Move.None, 30, printInfo: true);
                                lstMerged.Clear();
                                report.Append("Scramble " + count + ": " + s + "\n" + "Solution: (" + Int32.Parse(result[2]) + " moves)" + String.Join(" ", lstMerged));
                                report.Append("\n__________________________________________________________________________________\n");
                                totalMoves += Int32.Parse(result[2]);
                                if (Int32.Parse(result[2]) < min)
                                {
                                    min = Int32.Parse(result[2]);
                                    minScript.Clear();
                                    minScript.Append(s);
                                    minSolution.Clear();
                                    minSolution.Append(String.Join(" ", lstMerged));
                                }
                                if (Int32.Parse(result[2]) > max)
                                {
                                    max = Int32.Parse(result[2]);
                                    maxScript.Clear();
                                    maxScript.Append(s);
                                    maxSolution.Clear();
                                    maxSolution.Append(String.Join(" ", lstMerged));
                                }
                            }
                            else
                            {
                                report.Append("Scramble " + count + ": " + s + "\n" + "Fail: Cannot find solution.");
                                report.Append("\n__________________________________________________________________________________");
                                countFail++;
                            }
                            count++;
                        }
                        int average = totalMoves / (countCorrect);
                        sw.WriteLine("Total test cases: " + (countFail + countCorrect).ToString("#,#"));
                        sw.WriteLine("Success: " + countCorrect + "/" + (countFail + countCorrect) + "\nFail: " + countFail + "/" + (countFail + countCorrect));
                        sw.WriteLine("Average number of moves: " + average);
                        sw.WriteLine("Fewest moves case: " + min + " (" + minScript + ")");
                        sw.WriteLine("Solution for fewest moves case: " + minSolution);
                        sw.WriteLine("Most moves: " + max + " (" + maxScript + ")");
                        sw.WriteLine("Solution for most moves case: " + maxSolution);
                        sw.WriteLine(report);
                    }
                }
                MessageBox.Show("Test done.");
                MessageBox.Show("Success cases: " + countCorrect + "/" + (countFail + countCorrect) + "\nFail cases: " + countFail + "/" + (countFail + countCorrect));
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private static Bitmap Base64StringToBitmap(string base64String)
        {
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer)
            {
                Position = 0
            };
            Bitmap bmpReturn = (Bitmap)Image.FromStream(memoryStream);
            memoryStream.Close();
            return bmpReturn;
        }
    }
}
