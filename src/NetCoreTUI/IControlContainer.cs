using NetCoreTUI.Buffers;

namespace NetCoreTUI
{
    public interface IControlContainer
    {
        ConsoleBuffer Buffer { get; }
        bool Visible { get; }

        void Paint();

        void SuspendLayout();
        void ResumeLayout();
    }
}