namespace MMI.EBuLa.Tools
{
    /// <summary>
    /// Interface for Soundoutput
    /// </summary>
    public interface SoundInterface
    {
        /// <summary>
        /// Play standard sound beep
        /// </summary>
        void PlaySound();

        /// <summary>
        /// Play error sound beep
        /// </summary>
        void PlayErrorSound();

		/// <summary>
		/// Play malfunction sound beep (siemens loco)
		/// </summary>
		void PlayMalfunctionSiemensSound();
	
		/// <summary>
		/// Play malfunction sound beep (bombardier loco)
		/// </summary>
		void PlayMalfunctionBombardierSound();

		/// <summary>
		/// Play "WB gesperrt"
		/// </summary>
		void PlayWBGesperrt();

		/// <summary>
		/// Play "WB Freigabe"
		/// </summary>
		void PlayWBFreigabe();

		/// <summary>
		/// Play given file 
		/// </summary>
		void PlaySoundFromFile(string filename);
	}
}