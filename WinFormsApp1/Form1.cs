using Microsoft.VisualBasic;
using System.Media;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Dictionary to store chatbot responses organized by keywords
        private Dictionary<string, string[]> _chatbotbrain;
        
        // Random instance for selecting random responses
        private Random random;
        
        // Stores the user's name once provided
        private string userName = string.Empty;
        
        // List to maintain conversation history with sender and message pairs
        private List<(string Sender, string Message)> conversationHistory = new List<(string, string)>();
        
        // Maximum number of visible message interactions (user + bot pairs) in the chat display
        private const int MaxVisibleInteractions = 2;
        
        // Timer for processing delay before showing chatbot response
        private System.Windows.Forms.Timer processingTimer;
        
        // Timer for the greeting audio duration
        private System.Windows.Forms.Timer greetingAudioTimer;
        
        // Timer for delay before asking for user's name
        private System.Windows.Forms.Timer askNameTimer;
        
        // Temporary storage for chatbot response while timer is running
        private string pendingResponse = string.Empty;
        
        // Delay in milliseconds before displaying chatbot response
        private const int ProcessingDelay = 1500; // milliseconds
        
        // Delay in milliseconds for the greeting audio to finish
        private const int GreetingAudioDelay = 10000; // milliseconds - adjust based on your audio length
        
        // Delay in milliseconds before asking for user's name if no input
        private const int AskNameDelay = 5000; // milliseconds - delay before asking for name
        
        // Flag to track if the chatbot has already asked for the user's name
        private bool nameAsked = false;
        
        // Flag to track if the greeting audio has finished playing
        private bool audioFinished = false;
       
        /// <summary>
        /// Constructor - Initializes the form and all necessary components
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            Initializechatbot();
            InitializeProcessingTimer();
            InitializeGreetingAudioTimer();
            InitializeAskNameTimer();
        }

        /// <summary>
        /// Initializes the processing timer used for chatbot response delay
        /// </summary>
        private void InitializeProcessingTimer()
        {
            processingTimer = new System.Windows.Forms.Timer();
            processingTimer.Interval = ProcessingDelay;
            processingTimer.Tick += ProcessingTimer_Tick;
        }

        /// <summary>
        /// Initializes the greeting audio timer to track audio playback duration
        /// </summary>
        private void InitializeGreetingAudioTimer()
        {
            greetingAudioTimer = new System.Windows.Forms.Timer();
            greetingAudioTimer.Interval = GreetingAudioDelay;
            greetingAudioTimer.Tick += GreetingAudioTimer_Tick;
        }

        /// <summary>
        /// Initializes the ask name timer for delaying the name request
        /// </summary>
        private void InitializeAskNameTimer()
        {
            askNameTimer = new System.Windows.Forms.Timer();
            askNameTimer.Interval = AskNameDelay;
            askNameTimer.Tick += AskNameTimer_Tick;
        }

        /// <summary>
        /// Handles the processing timer tick event - displays chatbot response after delay
        /// </summary>
        private void ProcessingTimer_Tick(object sender, EventArgs e)
        {
            processingTimer.Stop();
            
            // Remove the processing indicator
            RemoveProcessingIndicator();
            
            // Display the chatbot response
            if (!string.IsNullOrEmpty(pendingResponse))
            {
                AddMessageToHistory("Chatbot", pendingResponse);
                pendingResponse = string.Empty;
            }
        }

        /// <summary>
        /// Handles the greeting audio timer tick event - marks audio as finished and starts name ask timer
        /// </summary>
        private void GreetingAudioTimer_Tick(object sender, EventArgs e)
        {
            greetingAudioTimer.Stop();
            audioFinished = true;
            
            // Start a timer to ask for the name if user doesn't input anything
            askNameTimer.Start();
        }

        /// <summary>
        /// Handles the ask name timer tick event - asks for user's name if they haven't provided it
        /// </summary>
        private void AskNameTimer_Tick(object sender, EventArgs e)
        {
            askNameTimer.Stop();
            
            // Only ask for name if user hasn't typed anything and hasn't provided their name
            if (string.IsNullOrEmpty(userName) && !nameAsked)
            {
                nameAsked = true;
                AskForUserName();
            }
        }

        /// <summary>
        /// Initializes the chatbot's knowledge base with keywords and responses
        /// </summary>
        private void Initializechatbot()
        {
            _chatbotbrain = new Dictionary<string, string[]>
            {
                {"hello", new string[] { "Hi there, {name}!", "Hello {name}!", "Greetings, {name}!" } },
                {"hi", new string[] { "Hello {name}!", "Hi {name}!", "Hey there, {name}!" } },
                {"my name is ", new string[] { "Nice to meet you, {name}!", "Hello {name}, welcome!", "Greetings, {name}!" } },
                {"thanks", new string[] { "You're welcome ! and how can I help you with ? , {name}!" } },
                {"how are you", new string[] { "I'm good, thank you {name}!", "Doing well, how about you {name}?", "I'm fine, thanks for asking {name}!" } },
                {"i'm good,too", new string[] { "That's great to hear, {name}!", "I'm glad you're doing well, {name}!", "Awesome, {name}!" } },
                {"what is your name", new string[] { "I am a chatbot, {name}.", "You can call me Chatbot, {name}.", "I don't have a name, but you're {name}." } },
                {"what can you do", new string[] { "I can chat with you, {name}.", "I can answer your questions, {name}.", "I can provide information, {name}." } },
                {"privacy", new string[] {"Privacy is important, {name}. Be careful about sharing personal information online.", "Make sure to use strong passwords and enable two-factor authentication, {name}."} },
                {"security", new string[] {"Security is crucial, {name}. Keep your software up to date and be cautious of suspicious links.", "Use antivirus software and regularly back up your data, {name}."} },
                {"password",new string[] {"Be aware to use a correct password that is very strong, {name}.","You must have a book of passwords at your disposition, {name}."} },
                {"phishing", new string[] {"Phishing is a type of online scam where attackers try to trick you into providing personal information, {name}.", "Be cautious of emails or messages asking for sensitive information, {name}."} },
                {"scam", new string[] {"A scam is a fraudulent scheme or deception, {name}.", "Be careful of offers that seem to be too good to be true, {name}."} },
                {"bye", new string[] { "Goodbye {name}!", "See you later, {name}!", "Take care, {name}!" } }

            };
            random = new Random();

        }

        /// <summary>
        /// Handles the send button click event - processes user input and triggers chatbot response
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            string userinput = textBox1.Text.ToLower().Trim();
            
            if (string.IsNullOrEmpty(userinput))
            {
                return;
            }

            // Stop the ask name timer if user types something
            if (askNameTimer.Enabled)
            {
                askNameTimer.Stop();
            }

            // Add user message to conversation history
            AddMessageToHistory("You", textBox1.Text);
            
            // If audio hasn't finished yet, wait for it to finish
            if (!audioFinished)
            {
                return;
            }

            // Process user input based on whether name has been provided
            if (string.IsNullOrEmpty(userName) && !nameAsked)
            {
                ProcessInitialGreeting(userinput);
            }
            else
            {
                // Show processing indicator
                ShowProcessingIndicator();
                ProcessUserInput(userinput);
            }

            // Clear the input field after sending
            textBox1.Text = "";
        }

        /// <summary>
        /// Processes the initial greeting - handles name extraction and first response
        /// </summary>
        private void ProcessInitialGreeting(string userinput)
        {
            // Check if user is providing their name in the first message
            if (userinput.Contains("my name is "))
            {
                ExtractAndSetUserName(userinput);
                nameAsked = true;
                ShowProcessingIndicator();
                string matchedKey = "my name is ";
                QueueChatbotResponse(matchedKey);
            }
            else
            {
                // User typed something but didn't provide name
                // Ask for the name after processing their message
                nameAsked = true;
                ShowProcessingIndicator();
                
                // First, process their message
                string matchedKey = FindKeywordInSentence(userinput);
                
                if (_chatbotbrain.ContainsKey(userinput))
                {
                    QueueChatbotResponse(userinput);
                }
                else if (matchedKey != null)
                {
                    QueueChatbotResponse(matchedKey);
                }
                else
                {
                    pendingResponse = "Sorry, I don't understand. By the way, what is your name?";
                    processingTimer.Start();
                }
            }
        }

        /// <summary>
        /// Processes regular user input after initial greeting - matches keywords and generates response
        /// </summary>
        private void ProcessUserInput(string userinput)
        {
            // Check if user is providing their name
            if (userinput.Contains("my name is "))
            {
                ExtractAndSetUserName(userinput);
                string matchedKey = "my name is ";
                QueueChatbotResponse(matchedKey);
            }
            // First, try exact match
            else if (_chatbotbrain.ContainsKey(userinput)) 
            {
                QueueChatbotResponse(userinput);
            }
            else
            {
                // If no exact match, search for keywords within the user input
                string matchedKey = FindKeywordInSentence(userinput);
                
                if (matchedKey != null)
                {
                    QueueChatbotResponse(matchedKey);
                }
                else
                {
                    // No matching keyword found - provide default response
                    if (string.IsNullOrEmpty(userName))
                    {
                        pendingResponse = "Sorry, I don't understand. By the way, what is your name?";
                    }
                    else
                    {
                        pendingResponse = $"Sorry, I don't understand, {userName}.";
                    }
                    processingTimer.Start();
                }
            }
        }

        /// <summary>
        /// Queues a chatbot response and starts the processing timer
        /// </summary>
        private void QueueChatbotResponse(string key)
        {
            string[] responses = _chatbotbrain[key];
            if (responses.Length > 0)
            {
                // Select a random response from the available options
                int randomIndex = random.Next(responses.Length);
                string randomResponse = responses[randomIndex];
                
                // Replace {name} placeholder with actual user name or default
                pendingResponse = randomResponse.Replace("{name}", string.IsNullOrEmpty(userName) ? "friend" : userName);
            }
            else
            {
                pendingResponse = "Sorry, I don't understand.";
            }
            
            // Start timer to display response after delay
            processingTimer.Start();
        }

        /// <summary>
        /// Shows the "Chatbot is typing..." indicator while processing
        /// </summary>
        private void ShowProcessingIndicator()
        {
            if (string.IsNullOrEmpty(conversationHistory.Count > 0 ? conversationHistory.Last().Message : ""))
            {
                // Add processing indicator to the conversation
                conversationHistory.Add(("Chatbot", "Chatbot is typing..."));
                UpdateDisplayedMessages();
            }
        }

        /// <summary>
        /// Removes the "Chatbot is typing..." indicator before displaying actual response
        /// </summary>
        private void RemoveProcessingIndicator()
        {
            // Remove the "Chatbot is typing..." message
            if (conversationHistory.Count > 0 && conversationHistory.Last().Message == "Chatbot is typing...")
            {
                conversationHistory.RemoveAt(conversationHistory.Count - 1);
            }
        }

        /// <summary>
        /// Extracts the user's name from a message containing "my name is"
        /// </summary>
        private void ExtractAndSetUserName(string userInput)
        {
            const string namePrefix = "my name is ";
            int startIndex = userInput.IndexOf(namePrefix) + namePrefix.Length;
            userName = userInput.Substring(startIndex).Trim();
        }

        /// <summary>
        /// Searches for a keyword within the user's sentence
        /// </summary>
        private string FindKeywordInSentence(string userInput)
        {
            // Iterate through all keywords in the chatbot brain
            foreach (var key in _chatbotbrain.Keys)
            {
                if (userInput.Contains(key))
                {
                    return key;
                }
            }
            return null;
        }

        /// <summary>
        /// Displays a random response for a given keyword (unused but kept for compatibility)
        /// </summary>
        private void DisplayRandomResponse(string key)
        {
            string[] responses = _chatbotbrain[key];
            if (responses.Length > 0)
            {
                int randomIndex = random.Next(responses.Length);
                string randomResponse = responses[randomIndex];
                
                // Replace {name} placeholder with actual user name or default
                string finalResponse = randomResponse.Replace("{name}", string.IsNullOrEmpty(userName) ? "friend" : userName);
                AddMessageToHistory("Chatbot", finalResponse);
            }
            else
            {
                AddMessageToHistory("Chatbot", "Sorry, I don't understand.");
            }
        }

        /// <summary>
        /// Adds a message to the conversation history and manages the message limit
        /// </summary>
        private void AddMessageToHistory(string sender, string message)
        {
            conversationHistory.Add((sender, message));

            // Keep only the last MaxVisibleInteractions interactions (2 pairs of messages)
            if (conversationHistory.Count > MaxVisibleInteractions * 2)
            {
                conversationHistory.RemoveRange(0, conversationHistory.Count - (MaxVisibleInteractions * 2));
            }

            UpdateDisplayedMessages();
        }

        /// <summary>
        /// Updates the display with current conversation history with proper alignment
        /// </summary>
        private void UpdateDisplayedMessages()
        {
            string displayText = string.Empty;

            foreach (var (sender, message) in conversationHistory)
            {
                if (sender == "You")
                {
                    // Right-align user message
                    displayText += AlignRight(message) + Environment.NewLine;
                }
                else
                {
                    // Left-align chatbot message
                    displayText += AlignLeft(message) + Environment.NewLine;
                }
            }

            label2.Text = displayText;
        }

        /// <summary>
        /// Aligns text to the right by adding leading spaces
        /// </summary>
        private string AlignRight(string message)
        {
            // Add spaces to simulate right alignment
            int labelWidth = label2.Width / 8; // Approximate character width
            int messageLength = message.Length;
            int spacesNeeded = Math.Max(0, labelWidth - messageLength);
            return new string(' ', spacesNeeded) + message;
        }

        /// <summary>
        /// Aligns text to the left (default behavior)
        /// </summary>
        private string AlignLeft(string message)
        {
            // Left-aligned text is already left-aligned
            return message;
        }
            

        /// <summary>
        /// Form load event - initializes greeting audio and logo on startup
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            playgreeting();
            displayLogo();
            // Start the timer for the greeting audio
            greetingAudioTimer.Start();
        }

        /// <summary>
        /// Displays the greeting message asking for the user's name
        /// </summary>
        private void AskForUserName()
        {
            // Display greeting asking for the user's name
            AddMessageToHistory("Chatbot", "Hello! Welcome to our chatbot. Before we start, what is your name?");
        }

        /// <summary>
        /// Plays the greeting audio file
        /// </summary>
        private void playgreeting()
        {
            SoundPlayer player = new SoundPlayer("voice/Recording.Wav");
            player.Play();
        }

        /// <summary>
        /// Displays the logo image on the form
        /// </summary>
        private void displayLogo()
        {
            pictureBox1.Image = Image.FromFile("images/LOGO EL THE .png");

        }

        /// <summary>
        /// Handles the clear button click event - resets the conversation
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // Clear input and conversation display
            textBox1.Text = "";
            conversationHistory.Clear();
            label2.Text = "";
            
            // Stop all active timers
            processingTimer.Stop();
            greetingAudioTimer.Stop();
            askNameTimer.Stop();
            
            // Reset variables
            pendingResponse = string.Empty;
            userName = string.Empty;
            nameAsked = false;
            audioFinished = false;
            
            // Restart the greeting audio and timer
            playgreeting();
            greetingAudioTimer.Start();

        }

        /// <summary>
        /// Handles the textBox text changed event (currently unused)
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // You can leave this empty or add logic as needed
        }

        /// <summary>
        /// Handles the label click event (currently unused)
        /// </summary>
        private void label2_Click(object sender, EventArgs e)
        {

        }

       
    }
}

