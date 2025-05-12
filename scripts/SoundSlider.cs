using Godot;

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
