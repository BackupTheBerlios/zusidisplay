namespace MMI.EBuLa.Tools
{
	public interface INetwork
	{
		void Connect();
		void ChangeState(int type, byte[] val);
		void SetConnected(bool connected);
	}

	public class Stuff
	{
		public static void SetFocusToZusi()
		{
			SystemTools.System s = new SystemTools.System();
			long ZusiHandler = s.GetZusiHwnd();
			if (ZusiHandler != 0)
			{
				SystemTools.System.SetForegroundWindow(ZusiHandler);
			}
		}
	}
}