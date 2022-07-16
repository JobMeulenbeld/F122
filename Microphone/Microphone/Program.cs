using System;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace Speech
{
    class Program
    {
        const string Engineer_name = "Lara";
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        static void Main(string[] args)
        {

            Program p = new Program();
            // Create an in-process speech recognizer for the en-US locale.  
            using (
            SpeechRecognitionEngine recognizer =
              new SpeechRecognitionEngine(
                new System.Globalization.CultureInfo("en-US")))
            {
                p.setup();
                
                var c = p.get_choices();
                var gb = new GrammarBuilder(c);
                var g = new Grammar(gb);


                // Create and load a dictation grammar.  
                recognizer.LoadGrammar(g);

                // Add a handler for the speech recognized event.  
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(p.recognizer_SpeechRecognized);

                // Configure input to the speech recognizer.  
                recognizer.SetInputToDefaultAudioDevice();

                // Start asynchronous, continuous speech recognition.  
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                // Keep the console window open.  
                while (true)
                {
                    Console.ReadLine();
                }
            }
        }

        // Handle the SpeechRecognized event.  
        public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine(e.Result.Text);
            switch(e.Result.Text)
            {
                case Engineer_name + " gap in front":
                    synthesizer.Speak("Gap in front is 1.4 seconds, you are gaining 2 tenths a lap");
                    break;
                case Engineer_name + " driver in front":
                    synthesizer.Speak("Hamilton just ahead, Lets put some pressure on him");
                    break;
                case Engineer_name + " race position":
                    synthesizer.Speak("You are currently in 'p' 4, lets push for a podium finish!");
                    break;
                case Engineer_name + " tyre update":
                    synthesizer.Speak("Front left tyre is overheating, please manage the tires");
                    break;
                case Engineer_name + " check engine":
                    synthesizer.Speak("The engine is looking worn, expect less power");
                    break;
                case Engineer_name + " gap behind":
                    synthesizer.Speak("Gap behind is 1.2 seconds and closing in!");
                    break;
                case Engineer_name + " damage report":
                    synthesizer.Speak("The frontwing is damaged but pace looks alright!");
                    break;
                case Engineer_name + " fastest lap":
                    synthesizer.Speak("You currently hold the fastest lap with a 1 minute 15.6!");
                    break;
                case Engineer_name + " pitstop strategy":
                    synthesizer.Speak("Pitwindow open in 2 laps time, you will be on the softs");
                    break;
                case Engineer_name + " box this lap":
                    synthesizer.Speak("Copy that. We will receive you at the end of this lap");
                    break;
                case Engineer_name + " box box":
                    synthesizer.Speak("Allright we'll be ready, watch the speed limit in the pitlane!");
                    break;
                case Engineer_name + " gap":
                    synthesizer.Speak("gap in front 2.1, gap behind 4.3");
                    break;
                default: 
                    Console.WriteLine("Unhandled input");
                    break;
            }
            
        }
        public void setup()
        {
            synthesizer.SetOutputToDefaultAudioDevice();
            
            synthesizer.SelectVoice("Microsoft Hazel Desktop");
            synthesizer.Rate = 2;
            
        }

        public Choices get_choices()
        {
            var choice = new Choices();

            choice.Add(Engineer_name + " gap in front");
            choice.Add(Engineer_name + " driver in front");
            choice.Add(Engineer_name + " race position");
            choice.Add(Engineer_name + " tyre update");
            choice.Add(Engineer_name + " check engine");
            choice.Add(Engineer_name + " gap behind");
            choice.Add(Engineer_name + " pitstop strategy");
            choice.Add(Engineer_name + " fastest lap");
            choice.Add(Engineer_name + " damage report");
            choice.Add(Engineer_name + " pitstop strategy");
            choice.Add(Engineer_name + " box this lap");
            choice.Add(Engineer_name + " box box");
            choice.Add(Engineer_name + " gap");

            return choice;
        }

    }
}