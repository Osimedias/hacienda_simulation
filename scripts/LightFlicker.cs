using Godot;

/*
    file: LightFlicker.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 12:59 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    This scripts is used to flick a OmniLight useful for candles or some shit like that.
*/

namespace Trinketos.HaciendaSimulator
{
    public partial class LightFlicker : OmniLight3D
    {
        private FastNoiseLite _noise = new FastNoiseLite();
        private float _energy;

        private const float _MAX_ENERGY = 1000000;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            GD.Randomize();
            _noise.Frequency = GD.Randf();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            _energy += 0.5f;
            if (_energy > _MAX_ENERGY) _energy = 0f;

            LightEnergy = _noise.GetNoise1D((_energy + 1) / 4f) + 0.5f;
        }
    }
}
