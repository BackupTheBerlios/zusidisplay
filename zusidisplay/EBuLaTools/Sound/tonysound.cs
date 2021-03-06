/***********************************************************************
 * TONYSOUND - WRITTEN BY TONY STEGALL 2004                            *
 * THIS WILL ALLOW YOU TO PLAY SOUND IN ANY OF YOUR C# APPLICATION     *
 * in order to use this class you must include the following in        *
 * code. using tonysound;                                              *
 * you must also make a string like this                               *
 * const string FILE_NAME = "sound.wav";                               * 
 * Sound.Play(FILENAME,PlaySoundFlags.FLAGNAME);  * 
 * null can also be used if you don't want to use any of the methods   *
 * however you MUST SPECIFY THE RESOURCE TYPE                          *  
 * for example PlaySoundFlags.SND_FILENAME                             *                                     *
 * *********************************************************************/
using System;
using System.Runtime.InteropServices;

namespace MMI.EBuLa.Tools
{
	public class APISound : SoundInterface
	{
		public void PlaySound() 
		{
			try
			{
				PlaySound(@".\Sounds\quittung.wav", IntPtr.Zero, PlaySoundFlags.SND_ASYNC);
			}
			catch(Exception) {}
		}

        public void PlayErrorSound() 
        {
			try
			{
				PlaySound(@".\Sounds\fehler.wav", IntPtr.Zero, PlaySoundFlags.SND_ASYNC & PlaySoundFlags.SND_NOSTOP);
			}
			catch(Exception) {}
        }

		public void PlayMalfunctionSiemensSound()
		{
			try
			{
				PlaySound(@".\Sounds\stoerung_siemens.wav", IntPtr.Zero, PlaySoundFlags.SND_ASYNC & PlaySoundFlags.SND_NOSTOP);
			}
			catch(Exception) {}
		}

		public void PlayMalfunctionBombardierSound()
		{
			try
			{
				PlaySound(@".\Sounds\stoerung_bombardier.wav", IntPtr.Zero, PlaySoundFlags.SND_ASYNC & PlaySoundFlags.SND_NOSTOP);
			}
			catch(Exception) {}
		}

		public void PlayWBGesperrt()
		{
			try
			{
				PlaySound(@".\Sounds\wb_gesperrt.wav", IntPtr.Zero, PlaySoundFlags.SND_ASYNC & PlaySoundFlags.SND_NOSTOP);
			}
			catch(Exception) {}
		}

		public void PlayWBFreigabe()
		{
			try
			{
				PlaySound(@".\Sounds\wb_freigabe.wav", IntPtr.Zero, PlaySoundFlags.SND_ASYNC & PlaySoundFlags.SND_NOSTOP);
			}
			catch(Exception) {}
		}

		public void PlaySoundFromFile(string filename)
		{
			try
			{
				if (System.IO.Path.GetExtension(filename).ToLower() == "wav")
					PlaySound(filename, IntPtr.Zero, PlaySoundFlags.SND_ASYNC & PlaySoundFlags.SND_NOSTOP);
			}
			catch(Exception) {}
		}

		[DllImport("winmm.dll")] //inports the winmm.dll used for sound
		private static extern bool PlaySound( string szSound, IntPtr hMod, PlaySoundFlags flags );
	}

	[Flags] //enumeration treated as a bit field or set of flags
	public enum PlaySoundFlags: int 
	{

		SND_SYNC = 0x0000, /* play synchronously (default) */
		SND_ASYNC = 0x0001, /* play asynchronously */
		SND_NODEFAULT = 0x0002, /* silence (!default) if sound notfound */
		SND_LOOP = 0x0008, /* loop the sound until nextsndPlaySound */
		SND_NOSTOP = 0x0010, /* don't stop any currently playingsound */
		SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
		SND_FILENAME = 0x00020000, /* name is file name */
		SND_RESOURCE = 0x00040004 /* name is resource name or atom */ 
	}
 
    public class NullSound : SoundInterface
    {
        public void PlaySound() {}
        public void PlayErrorSound() {}
		public void PlayMalfunctionSiemensSound() {}
		public void PlayMalfunctionBombardierSound() {}
		public void PlayWBFreigabe() {}
		public void PlayWBGesperrt() {}
		public void PlaySoundFromFile(string filename) {}
    }
}

