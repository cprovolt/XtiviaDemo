using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Database_Failover_Demo
{
    public partial class Form1 : Form
    {
        private bool Connected = false;
        private SqlConnection sqlconn;
        private String conn;
              
        private int cooldown;

        public Form1()
        {
            InitializeComponent();
            this.cooldown = 1000;
            this.timerCountdown.Interval = cooldown;
            this.timerCountdown.Tick += TimerCountdown_Tick;
            this.Change_Status(false);
        }

        private void TimerCountdown_Tick(object sender, EventArgs e)
        {
            sqlconn.Close();
            sqlconn.Dispose();

            try
            {

                this.sqlconn = new SqlConnection(conn);
                this.sqlconn.Open();
                this.pbStatus.BackColor = Color.Green;
                this.Change_Status(true);

            }
            catch (Exception ex)
            {
                this.Connected = false;

                this.pbStatus.BackColor = Color.Red;
                this.Change_Status(false);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                this.sqlconn.Close();
                this.sqlconn.Dispose();
                this.timerCountdown.Enabled = false;
                this.txtUser.Focus();
                this.btnConnect.Text = "Connect";
                this.pbStatus.BackColor = Color.Red;
                this.Change_Status(false);
                return;
            } /* end connected */

            /* Determine if the text boxes are not blank for database and username */
            if (this.txtDatabase.Text == "")
            {
                MessageBox.Show("There is no database name set. Please configure this value and try again");
                this.txtDatabase.Focus();
                return;
            }
            else if (this.txtUser.Text == "")
            {
                MessageBox.Show("There is no username name set. Please configure this value and try again");
                this.txtUser.Focus();
                return;
            } /* end block */

            /* connect to the database */
            this.conn = "Data Source=" + this.txtDatabase.Text.ToString()
                    + ";User ID="
                    + this.txtUser.Text.ToString()
                    + ";password=" + this.txtPassword.Text.ToString() + ";";
            //MessageBox.Show(this.conn.ToString());

            try
            {
                this.sqlconn = new SqlConnection(conn);
                this.sqlconn.Open();

                this.pbStatus.BackColor = Color.Green;
                this.timerCountdown.Enabled = true;
                this.Change_Status(true);

                this.btnConnect.Text = "Disconnect";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

          } /* end method */

       
        private void Change_Status(bool value)
        {
            this.Connected = value;

            if(Connected)
            {
                this.tssStatus.Text = "Connected to Database " + this.txtDatabase.Text + ".";
            }
            else
            {
                this.tssStatus.Text = "Not currently connected to a database.";
            }
        }

     
    }
}
