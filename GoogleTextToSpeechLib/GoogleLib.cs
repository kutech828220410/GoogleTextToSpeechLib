using Google.Cloud.TextToSpeech.V1;
using Google.Apis.Auth.OAuth2;
using System;
using System.IO;
using System.Text;

namespace TextToSpeechLib
{
    public class GoogleLib
    {
        private TextToSpeechClient _client;
        private SynthesisInput _synthesisInput;
        private VoiceSelectionParams _voiceParams;
        private AudioConfig _audioConfig;

        public GoogleLib(string credentialsPath = null)
        {
            if (string.IsNullOrEmpty(credentialsPath) || !File.Exists(credentialsPath))
            {
                throw new FileNotFoundException("Invalid credentials file path.");
            }

            // 使用 GoogleCredential.FromFile 明確載入憑證
            var credential = GoogleCredential.FromFile(credentialsPath);
            var clientBuilder = new TextToSpeechClientBuilder
            {
                Credential = credential
            };

            _client = clientBuilder.Build();
            _synthesisInput = new SynthesisInput();
            _voiceParams = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Neutral
            };
            _audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3,
                SpeakingRate = 1.0
            };
        }

        public void SetLanguage(string languageCode)
        {
            _voiceParams.LanguageCode = languageCode;
        }

        public void SetVoice(SsmlVoiceGender gender)
        {
            _voiceParams.SsmlGender = gender;
        }

        public void SetSpeakingRate(double rate)
        {
            _audioConfig.SpeakingRate = rate;
        }

        public void SetText(string text)
        {
            _synthesisInput.Text = text;
        }

        public byte[] SynthesizeToBytes()
        {
            var response = _client.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = _synthesisInput,
                Voice = _voiceParams,
                AudioConfig = _audioConfig
            });
            return response.AudioContent.ToByteArray();
        }

        public void SynthesizeToFile(string filePath, string format = "mp3")
        {
            var response = _client.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = _synthesisInput,
                Voice = _voiceParams,
                AudioConfig = _audioConfig
            });

            string extension = format.ToLower();
            if (extension != "mp3" && extension != "wav")
            {
                throw new ArgumentException("Unsupported format. Only 'mp3' and 'wav' are supported.");
            }

            File.WriteAllBytes(filePath, response.AudioContent.ToByteArray());
        }

        public string SynthesizeToBase64()
        {
            var response = _client.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = _synthesisInput,
                Voice = _voiceParams,
                AudioConfig = _audioConfig
            });
            return Convert.ToBase64String(response.AudioContent.ToByteArray());
        }

        public MemoryStream SynthesizeToStream()
        {
            var response = _client.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = _synthesisInput,
                Voice = _voiceParams,
                AudioConfig = _audioConfig
            });
            return new MemoryStream(response.AudioContent.ToByteArray());
        }
    }
}
