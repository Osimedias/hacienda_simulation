using Godot;

/*
    file: SoundSlider.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:21 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Change the Volume in the AudioBusLayout
*/

namespace Trinketos.HaciendaSimulator
{
    public partial class SoundSlider : HSlider
    {
        [Export(PropertyHint.Enum, "Master,Music,Effect,Interface")]
        int SoundChannel = 0;



        public override void _Ready()
        {

            base._Ready();
            Value = AudioServer.GetBusVolumeLinear(SoundChannel);

        }
    }
}
