using System;
using System.IO;
using Microsoft.DirectX.AudioVideoPlayback;

// USES  --> Managed DirectX 9.0c <--

namespace MMI.EBuLa.Tools
{
	public class DxSound : SoundInterface
	{
		public void PlaySound()
		{
			try
			{
				Audio.FromFile(@".\Sounds\quittung.wav").Play();
			}
			catch(Exception){}
		}

		public void PlayErrorSound()
		{
			try
			{
				Audio.FromFile(@".\Sounds\fehler.wav").Play();
			}
			catch(Exception){}
		}

		public void PlayMalfunctionSiemensSound()
		{
			try
			{
				Audio a = Audio.FromFile(@".\Sounds\stoerung_siemens.wav");
				a.Play();
				System.Threading.Thread.Sleep(1200);
			}
			catch(Exception){}
		}

		public void PlayMalfunctionBombardierSound()
		{
			try
			{	
				Audio a = Audio.FromFile(@".\Sounds\stoerung_bombardier.WAV");
				a.Play();
				System.Threading.Thread.Sleep(1200);
			}
			catch(Exception){}
		}

		/*
		 * CODE BELOW FOR DIRECT-SOUND 9
		 * 
		private Microsoft.DirectX.DirectSound.SecondaryBuffer ApplicationBuffer = null;
		private Microsoft.DirectX.DirectSound.Device ApplicationDevice = null;

		public DxSound()
		{
            ApplicationDevice = new Microsoft.DirectX.DirectSound.Device();
            ApplicationDevice.SetCooperativeLevel(new System.Windows.Forms.Control(), Microsoft.DirectX.DirectSound.CooperativeLevel.Priority);
		}

        private bool LoadSoundFile(string name)
        {
            try
            {
                ApplicationBuffer = new Microsoft.DirectX.DirectSound.SecondaryBuffer(name, ApplicationDevice);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        public void PlaySound()
        {
            LoadSoundFile(STANDARD_WAVE);

            if(null != ApplicationBuffer)
            {
                ApplicationBuffer.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
            }

            ApplicationBuffer = null;
        }

        public void PlayErrorSound()
        {
            LoadSoundFile(ERROR_WAVE);

            if(null != ApplicationBuffer)
            {
                ApplicationBuffer.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
            }

            ApplicationBuffer = null;
        }
		*/
	}
}
