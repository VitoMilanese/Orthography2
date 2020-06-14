using System;

namespace Orthography.Shared
{
	public class CheckBox
    {
        private bool m_checked;
        public bool Checked
        {
            get => m_checked;
            set
            {
                if (!Enabled) return;
                m_checked = value;
                OnChanged?.Invoke(this, value);
            }
        }

        public bool Enabled { get; set; } = true;

        public EventHandler<bool> OnChanged { get; set; }
    }
}
