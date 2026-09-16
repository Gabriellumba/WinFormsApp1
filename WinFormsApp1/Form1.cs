using System.Media;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string[]> _chatbotbrain;
        private Random random;
        public Form1()
        {
            InitializeComponent();
            Initializechatbot();
        }
        private void Initializechatbot()
        {
            _chatbotbrain = new Dictionary<string, string[]>
            {
                { "hello", new string[] { "Hi there!", "Hello!", "Greetings!" } },
                { "hi", new string[] { "Hello!", "Hi!", "Hey there!" } },
                { "how are you", new string[] { "I'm good, thank you!", "Doing well, how about you?", "I'm fine, thanks for asking!" } },
                { "what is your name", new string[] { "I am a chatbot.", "You can call me Chatbot.", "I don't have a name." } },
                { "what can you do", new string[] { "I can chat with you.", "I can answer your questions.", "I can provide information." } },
                { "password",new string[] {"be aware to use correct password that is very strong.","you must put at your disposition a book of password."} },
                {"phishing", new string[] {"Phishing is a type of online scam where attackers try to trick you into providing personal information.", "Be cautious of emails or messages asking for sensitive information."} },
                { "bye", new string[] { "Goodbye!", "See you later!", "Take care!" } }

            };
            random = new Random();

        }


        private void button2_Click(object sender, EventArgs e)
        {
            string userinput = textBox1.Text.ToLower().Trim();
            if (_chatbotbrain.ContainsKey(userinput)) ;
            {
                string[] responses = _chatbotbrain[userinput];
                int randomIndex = random.Next(responses.Length);
                string response = responses[randomIndex];
                label2.Text = response;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            playgreeting();
            displayLogo();
        }
        private void playgreeting()
        {
            SoundPlayer player = new SoundPlayer("voice/Recording.Wav");
            player.Play();
        }
        private void displayLogo()
        {
            pictureBox1.Image = Image.FromFile("images/LOGO EL THE .png");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            label2.Text = "";

        }

        
    }
}
