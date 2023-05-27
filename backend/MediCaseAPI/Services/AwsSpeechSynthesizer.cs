using Amazon.Polly;
using Amazon.Polly.Model;
using Microsoft.Extensions.Options;
using MediCaseAPI.Exceptions;
using MediCaseAPI.Options;
using MediCaseAPI.Services.Interfaces;

namespace MediCaseAPI.Services;

public class AwsSpeechSynthesizer : ISpeechSynthesizer
{
    private readonly IAmazonPolly _polly;
    private readonly SpeechOptions _options;

    public AwsSpeechSynthesizer(IAmazonPolly polly, IOptions<SpeechOptions> options)
    {
        _polly = polly;
        _options = options.Value;
    }

    public async Task<Stream> SynthesizeAsync(string text, string languageCode)
    {
        if (!_options.Voices.TryGetValue(languageCode, out var voice))
            throw new BadRequestException($"Speech is not available for language '{languageCode}'.");
        if (string.IsNullOrWhiteSpace(text))
            throw new BadRequestException("There is no text to read.");
        if (text.Length > _options.MaxCharacters)
            throw new BadRequestException($"Text for speech can have at most {_options.MaxCharacters} characters.");

        var response = await _polly.SynthesizeSpeechAsync(new SynthesizeSpeechRequest
        {
            Engine = Engine.FindValue(_options.Engine),
            OutputFormat = OutputFormat.Mp3,
            TextType = TextType.Text,
            Text = text,
            VoiceId = VoiceId.FindValue(voice)
        });

        // The response stream is a network stream, so it is copied before the response is disposed.
        var audio = new MemoryStream();
        await using (response.AudioStream)
        {
            await response.AudioStream.CopyToAsync(audio);
        }
        audio.Position = 0;
        return audio;
    }
}
