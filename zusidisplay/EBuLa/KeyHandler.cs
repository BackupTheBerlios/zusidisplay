using System;
using System.Windows.Forms;

namespace MMI.EBuLa
{
    public interface KeyHandlerInterface
    {
        void B_E_Click(object sender, System.EventArgs e);
        void B_C_Click(object sender, System.EventArgs e);
    }

    public class KeyHandler
    {
        KeyHandlerInterface m_form = null;

        public KeyHandler(KeyHandlerInterface f)
        {
            m_form = f;
        }

        public void Button_E_Clicked(object sender, EventArgs e)
        {
            if (m_form != null) 
            {
                m_form.B_E_Click(sender,e);
            }
        }

        public void Button_C_Clicked(object sender, EventArgs e)
        {
            if (m_form != null) m_form.B_C_Click(sender,e);
        }

        public KeyHandlerInterface Form
        {
            set{m_form = value;}
            get{return m_form;}
        }
    }
}
