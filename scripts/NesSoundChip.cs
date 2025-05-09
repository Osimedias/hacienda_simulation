using System;
using Godot;

[GlobalClass]
public partial class NesSoundChip : AudioStreamPlayer
{
    [Export]
    public Curve frequencyCurve;
    [Export]
    public Curve volumeCurve;
    private AudioStreamGeneratorPlayback playback;

    private int sampleRate = 44100;
    private int phase = 0;
    private float frequency = 440;

    float timeElapsed = 0;
    public override void _Ready()
    {
        base._Ready();
        var stream = new AudioStreamGenerator();
        stream.BufferLength = 0.1f;
        stream.MixRate = sampleRate;

        Stream = stream;
        Play();
        playback = GetStreamPlayback() as AudioStreamGeneratorPlayback;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        timeElapsed += (float)delta;
        float volumeFactor = volumeCurve.Sample(timeElapsed % 1.0f);
        float adjustedVolume = volumeFactor * 0.5f;

        if (playback == null) return;

        float modulatedFrequency = frequencyCurve.Sample(timeElapsed % 1.0f);
        frequency = modulatedFrequency * 10000;

        var samples = new Vector2[sampleRate / 60];
        for (int i = 0; i < samples.Length; i++)
        {
            phase += (int)frequency / sampleRate;
            if (phase > 1) phase -= 1;
            float squareWave = (phase < 0.5f ? 1.0f : -1.0f) * adjustedVolume;
            float triangleWave = GenerateTriangleWave(phase * adjustedVolume);
            float noiseWave = GenerateNoiseWave() * adjustedVolume;
            float finalSample = (squareWave * 0.6f) + (triangleWave * 0.3f) + (noiseWave * 0.1f);
            samples[i] = new Vector2(finalSample, finalSample);
        }
        playback.PushBuffer(samples);
    }

    float GenerateTriangleWave(float phase)
    {
        return Mathf.Abs(phase - 0.5f) * 4.0f - 1.0f;
    }

    float GenerateNoiseWave()
    {
        Random random = new Random();
        return (float)(random.NextDouble() * 2 - 1);
    }
}
