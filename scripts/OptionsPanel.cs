using Godot;

/*
    file: OptionsPanel.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:07 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Used to track all controls of the OptionMenu also save the settings.
*/


namespace Trinketos.HaciendaSimulator {
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

        void OnSavePressed()
        {
            ConfigFile configFile = new ConfigFile();
            configFile.SetValue("Screen","Size",ScreenSize.Selected);
            configFile.SetValue("Effects","Shader",ShaderQuality.Selected);
            configFile.SetValue("Effects","Particles",ParticleQuality.Selected);
            configFile.SetValue("Effects","Shadows",ShadowQuality.Selected);
            configFile.SetValue("Effects","ScreenSpaceAmbientOcclusion",SAOQuality.Selected);
            configFile.SetValue("Models","Model",ModelQuality.Selected);
            configFile.SetValue("Models","Texture",ModelTextureQuality.Selected);
            configFile.SetValue("Terrain","Texture",TerrainTextureQuality.Selected);
            configFile.SetValue("Terrain","DetailObjects",TerrainDetails.ButtonPressed);
            configFile.SetValue("Sound","Master",MasterVolume.Value);
            configFile.SetValue("Sound","Music",MusicVolume.Value);
            configFile.SetValue("Sound","Effects",EffectVolume.Value);
            configFile.SetValue("Sound","Interface",InterfaceVolume.Value);

            configFile.Save(SavePath);
        }
        void OnCancelPressed()
        {
        }
        void OnBackPressed()
        {
            GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
        }
    }
}