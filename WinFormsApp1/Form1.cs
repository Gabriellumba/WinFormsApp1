using System.Media;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            playgreeting();
            displayLogo();
        }
        private void playgreeting()
        {
            SoundPlayer player= new SoundPlayer("voice/Recording.Wav");
            player.Play();
        }
        private void displayLogo()
        {
            pictureBox1.Image = Image.FromFile("images/LOGO EL THE .png");

        }
    }
}
