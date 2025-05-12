using Godot;

namespace Trinketos.HaciendaSimulator
{
	public partial class SoundManager : Node
	{
		[Export]
		public AudioStreamPlayer MusicPlayer;
		[Export]
		public AudioStreamPlayer EffectsPlayer;

		public void PlayMusic(AudioStream track)
		{
			MusicPlayer.Stream = track;
			MusicPlayer.Play();
		}
		public void PlayMusic(AudioStreamPlaylist track)
		{
			MusicPlayer.Stream = track;
			MusicPlayer.Play();
		}

		// Used for Interfaces only
		public void PlayEffect(AudioStream effect)
		{
			EffectsPlayer.Stream = effect;
			EffectsPlayer.Play();
		}

		public void StopMusic()
		{
			MusicPlayer.Stop();
		}

		public void SetBusVolume(string busName,float volume)
		{
			int busIdx = AudioServer.GetBusIndex(busName);
			AudioServer.SetBusVolumeDb(busIdx, Mathf.LinearToDb(volume));
		}

		public float GetBusVolume(string busName)
		{
			int busIdx = AudioServer.GetBusIndex(busName);
			return Mathf.DbToLinear(AudioServer.GetBusVolumeDb(busIdx));
		}
	}
}
