using Godot;


namespace Trinketos.HaciendaSimulator
{
    public partial class OptionsPanel : Control
    {
        [Export]
        public string SavePath;

        [Export]
        OptionButton ScreenSize;
        [Export]
        OptionButton ShaderQuality;
        [Export]
        OptionButton ParticleQuality;
        [Export]
        OptionButton ShadowQuality;
        [Export]
        OptionButton SAOQuality;
        [Export]
        OptionButton ModelQuality;
        [Export]
        OptionButton ModelTextureQuality;
        [Export]
        OptionButton TerrainTextureQuality;
        [Export]
        CheckButton TerrainDetails;
        [Export]
        Slider MasterVolume;
        [Export]
        Slider MusicVolume;
        [Export]
        Slider EffectVolume;
        [Export]
        Slider InterfaceVolume;

        SceneTransition st;

        public override void _Ready()
        {
            base._Ready();
            st = GetNode<SceneTransition>("/root/SceneTransition");
        }

        void OnSavePressed()
        {
            ConfigFile configFile = new ConfigFile();
            configFile.SetValue("Screen", "Size", ScreenSize.Selected);
            configFile.SetValue("Effects", "Shader", ShaderQuality.Selected);
            configFile.SetValue("Effects", "Particles", ParticleQuality.Selected);
            configFile.SetValue("Effects", "Shadows", ShadowQuality.Selected);
            configFile.SetValue("Effects", "ScreenSpaceAmbientOcclusion", SAOQuality.Selected);
            configFile.SetValue("Models", "Model", ModelQuality.Selected);
            configFile.SetValue("Models", "Texture", ModelTextureQuality.Selected);
            configFile.SetValue("Terrain", "Texture", TerrainTextureQuality.Selected);
            configFile.SetValue("Terrain", "DetailObjects", TerrainDetails.ButtonPressed);
            configFile.SetValue("Sound", "Master", MasterVolume.Value);
            configFile.SetValue("Sound", "Music", MusicVolume.Value);
            configFile.SetValue("Sound", "Effects", EffectVolume.Value);
            configFile.SetValue("Sound", "Interface", InterfaceVolume.Value);

            configFile.Save(SavePath);
        }
        void OnCancelPressed()
        {
        }
        void OnBackPressed()
        {
            st.GoToScene("res://scenes/main_menu.tscn");
        }
    }
}
