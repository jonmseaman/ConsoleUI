using System;
using System.ComponentModel;
using System.Threading;

namespace ConsoleUI
{
    public class ProgressBar : Control
    {
        public ConsoleColor BlockColor = ConsoleColor.White;
        private int marqueeEnd;
        private int marqueeStart;
        private int maximum;
        private int minimum;
        private ProgressBarStyle progressBarStyle;
        private Timer timer;

        private int value;

        public ProgressBar()
        {
            Minimum = 0;
            Maximum = 100;
            Value = 0;
            Height = 1;
            var t = TimeSpan.FromMilliseconds(100);
            timer = new Timer(Timer_Elapsed, new AutoResetEvent(false), t, t);
        }

        public int Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                SetProperty(ref maximum, value);
            }
        }

        public int Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                SetProperty(ref minimum, value);
            }
        }

        public ProgressBarStyle ProgressBarStyle
        {
            get
            {
                return progressBarStyle;
            }
            set
            {
                SetProperty(ref progressBarStyle, value);
            }
        }

        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                SetProperty(ref this.value, value);
            }
        }

        private double Percent
        {
            get
            {
                if (Value == 0)
                    return 0;

                if (Maximum == 0)
                    return 0;

                var range = Maximum - Minimum;

                if (range == 0)
                    return 0;

                return ((double)Value / (double)range);
            }
        }

        public void Increment(int value)
        {
            Value += value;

            if (Value > Maximum)
                Value = Maximum;

            if (Value < Minimum)
                Value = Minimum;
        }

        protected override void DrawControl()
        {
            if (!ShouldDraw)
                return;

            if (ProgressBarStyle == ProgressBarStyle.Blocks)
                DrawBlocks();
        }

        protected override void HandlePropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value" || e.PropertyName == "Maximum" || e.PropertyName == "Minimum")
            {
                DrawControl();
                Paint();
            }

            if (e.PropertyName == "ProgressBarStyle")
            {
                if (ProgressBarStyle == ProgressBarStyle.Marquee)
                    StartTimer();
                else
                    StopTimer();
            }

            base.HandlePropertyChanged(e);
        }

        private void DrawBlocks()
        {
            if (!ShouldDraw)
                return;

            var position = (int)(ClientWidth * Percent);

            for (int i = 0; i < ClientWidth; i++)
            {
                Owner.Buffer.Write((short)ClientLeft + i, (short)ClientTop, i <= position ? (byte)219 : (byte)32, BlockColor, BackgroundColor);
            }
        }

        private void DrawMarquee()
        {
            if (!ShouldDraw)
                return;

            if (!Owner.Visible)
                return;

            if (!Visible)
                return;

            StopTimer();

            marqueeEnd += 5;

            if (marqueeEnd > 100)
                marqueeEnd = 100;

            if (marqueeStart < (marqueeEnd - 20) || marqueeEnd == 100)
                marqueeStart += 5;

            if (marqueeStart > 100)
            {
                marqueeStart = 0;
                marqueeEnd = 0;
            }

            var position1 = (int)(ClientWidth * ((double)marqueeStart / 100));
            var position2 = (int)(ClientWidth * ((double)marqueeEnd / 100));

            for (int i = 0; i < ClientWidth; i++)
            {
                Owner.Buffer.Write((short)ClientLeft + i, (short)ClientTop, (i >= position1 & i <= position2) ? (byte)219 : (byte)32, BlockColor, BackgroundColor);
            }

            Paint();

            StartTimer();
        }

        private void Timer_Elapsed(object stateInfo)
        {
            DrawMarquee();
        }

        private void StartTimer()
        {
            var t = TimeSpan.FromMilliseconds(100);
            timer.Change(t, t);
        }

        private void StopTimer()
        {
            timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }
    }
}