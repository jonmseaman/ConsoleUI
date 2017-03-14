using System;
using System.ComponentModel;
using System.Threading;
using NetCoreTUI.Enums;

namespace NetCoreTUI.Controls
{
    public class ProgressBar : Control
    {
        public ConsoleColor BlockColor = ConsoleColor.White;
        private int _marqueeEnd;
        private int _marqueeStart;
        private int _maximum;
        private int _minimum;
        private ProgressBarStyle _progressBarStyle;
        private Timer _timer;
        private const char FullBlock = (char) 0x2588;

        private int _value;

        public ProgressBar()
        {
            Minimum = 0;
            Maximum = 100;
            Value = 0;
            Height = 1;
            var t = TimeSpan.FromMilliseconds(100);
            _timer = new Timer(Timer_Elapsed, new AutoResetEvent(false), t, t);
        }

        public int Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                SetProperty(ref _maximum, value);
            }
        }

        public int Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                SetProperty(ref _minimum, value);
            }
        }

        public ProgressBarStyle ProgressBarStyle
        {
            get
            {
                return _progressBarStyle;
            }
            set
            {
                SetProperty(ref _progressBarStyle, value);
            }
        }

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                SetProperty(ref _value, value);
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

                return Value / (double)range;
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
                Owner.Buffer.Write((short)ClientLeft + i, (short)ClientTop, i <= position ? FullBlock : ' ', BlockColor, BackgroundColor);
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

            _marqueeEnd += 5;

            if (_marqueeEnd > 100)
                _marqueeEnd = 100;

            if (_marqueeStart < (_marqueeEnd - 20) || _marqueeEnd == 100)
                _marqueeStart += 5;

            if (_marqueeStart > 100)
            {
                _marqueeStart = 0;
                _marqueeEnd = 0;
            }

            var position1 = (int)(ClientWidth * ((double)_marqueeStart / 100));
            var position2 = (int)(ClientWidth * ((double)_marqueeEnd / 100));

            for (int i = 0; i < ClientWidth; i++)
            {
                Owner.Buffer.Write((short)ClientLeft + i, (short)ClientTop, (i >= position1 & i <= position2) ? FullBlock : ' ', BlockColor, BackgroundColor);
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
            _timer.Change(t, t);
        }

        private void StopTimer()
        {
            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }
    }
}