using Godot;
using System;


namespace Game.Scripts.Overworld;

public partial class AudioPlayer : Node
{
	[Export] private AudioStream TitleTheme;
	[Export] private AudioStream OverworldTheme;
	private AudioStreamPlayer musicPlayer;

	public override void _Ready()
	{
		musicPlayer = new AudioStreamPlayer();
		AddChild(musicPlayer);
	}

	public void PlayMusic(AudioStream music, float volume = 0.0f)
	{
		if (musicPlayer.Stream == null)
		{
			musicPlayer.Stream = music;
			musicPlayer.VolumeDb = volume;
			musicPlayer.Play();
		}
		else
		{
			//Transition
		}
	}

	public void PlayTitleTheme()
	{
		PlayMusic(TitleTheme);
	}
	
	public void PlayOverWorldTheme()
	{
		PlayMusic(OverworldTheme);
	}
	

}
