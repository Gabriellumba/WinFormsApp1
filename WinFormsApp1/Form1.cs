using Microsoft.VisualBasic;
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
                {"hello", new string[] { "Hi there!", "Hello!", "Greetings!" } },
                {"hi", new string[] { "Hello!", "Hi!", "Hey there!" } },
                {"my name is " , new string[] { "Nice to meet you!", "Hello!", "Greetings!" } },
                {"how are you", new string[] { "I'm good, thank you!", "Doing well, how about you?", "I'm fine, thanks for asking!" } },
                {"I'm good,too", new string[] { "That's great to hear!", "I'm glad you're doing well!", "Awesome!" } },
                {"what is your name", new string[] { "I am a chatbot.", "You can call me Chatbot.", "I don't have a name." } },
                {"what can you do", new string[] { "I can chat with you.", "I can answer your questions.", "I can provide information." } },
                {"privacy", new string[] {"Privacy is important. Be careful about sharing personal information online.", "Make sure to use strong passwords and enable two-factor authentication."} },
                {"security", new string[] {"Security is crucial. Keep your software up to date and be cautious of suspicious links.", "Use antivirus software and regularly back up your data."} },
                {"password",new string[] {"be aware to use correct password that is very strong.","you must put at your disposition a book of password."} },
                {"phishing", new string[] {"Phishing is a type of online scam where attackers try to trick you into providing personal information.", "Be cautious of emails or messages asking for sensitive information."} },
                {"scam", new string[] {"A scam is a fraudulent scheme or deception.", "Be careful of offers that seem too good to be true."} },
                { "bye", new string[] { "Goodbye!", "See you later!", "Take care!" } }

            };
            random = new Random();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string userinput = textBox1.Text.ToLower().Trim();
            
            // First, try exact match
            if (_chatbotbrain.ContainsKey(userinput)) 
            {
                DisplayRandomResponse(userinput);
            }
            else
            {
                // If no exact match, search for keywords within the user input
                string matchedKey = FindKeywordInSentence(userinput);
                
                if (matchedKey != null)
                {
                    DisplayRandomResponse(matchedKey);
                }
                else
                {
                    label2.Text = "Sorry, I don't understand.";
                }
            }
        }

        private string FindKeywordInSentence(string userInput)
        {
            foreach (var key in _chatbotbrain.Keys)
            {
                if (userInput.Contains(key))
                {
                    return key;
                }
            }
            return null;
        }

        private void DisplayRandomResponse(string key)
        {
            string[] responses = _chatbotbrain[key];
            if (responses.Length > 0)
            {
                int randomIndex = random.Next(responses.Length);
                string randomResponse = responses[randomIndex];
                label2.Text = randomResponse;
            }
            else
            {
                label2.Text = "Sorry, I don't understand.";
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

        // Add this method to handle the TextChanged event for textBox1
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // You can leave this empty or add logic as needed
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

       
    }
}

