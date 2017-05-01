using System;
using NetCoreTUI.Controls;
using NetCoreTUI.Enums;
using NetCoreTUI.Screens;

namespace DemoApp
{
    internal static class Labels
    {
        internal static void SetupLabelwindow(Window window)
        {
            LabelPage(window);
            SingleBorderLabelPage(window);
            DoubleBorderLabelPage(window);

            ShadowLabelPage(window);
            ShadowSingleBorderLabelPage(window);
            ShadowDoubleBorderLabelPage(window);
        }

        private static void DoubleBorderLabelPage(Window window)
        {
            var page = new Page("Double Border Labels");

            var control1 = new Label("This is a left aligned label (full width, double border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = page.Width;
            control1.BorderStyle = BorderStyle.Double;

            var control2 = new Label("This is a centered label (full width, double border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = page.Width;
            control2.TextAlign = TextAlign.Center;
            control2.BorderStyle = BorderStyle.Double;

            var control3 = new Label("This is a right aligned label (full width, double border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.Width = page.Width;
            control3.TextAlign = TextAlign.Right;
            control3.BorderStyle = BorderStyle.Double;

            page.Controls.Add(control1);
            page.Controls.Add(control2);
            page.Controls.Add(control3);

            page.Footer.Text = page.Name + ". Press any key.";

            window.Add(page);

            page.AfterPaint += (s, e) =>
            {
                Console.ReadKey(true);
            };
        }

        private static void LabelPage(Window window)
        {
            var page = new Page("Basic Labels");

            var control1 = new Label("This is a left aligned label (full width, no border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = page.Width;

            var control2 = new Label("This is a centered label (full width, no border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = page.Width;
            control2.TextAlign = TextAlign.Center;

            var control3 = new Label("This is a right aligned label (full width, no border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.Width = page.Width;
            control3.TextAlign = TextAlign.Right;

            page.Controls.Add(control1);
            page.Controls.Add(control2);
            page.Controls.Add(control3);

            page.Footer.Text = page.Name + ". Press any key.";

            window.Add(page);

            page.AfterPaint += (s, e) =>
            {
                Console.ReadKey(true);
            };
        }

        private static void ShadowDoubleBorderLabelPage(Window window)
        {
            var page = new Page("Double Border Labels");

            var control1 = new Label("This is a left aligned label (shadow, double border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = page.Width - 2;
            control1.BorderStyle = BorderStyle.Double;

            var control2 = new Label("This is a centered label (shadow, double border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height + 2;
            control2.Width = page.Width - 1;
            control2.TextAlign = TextAlign.Center;
            control2.BorderStyle = BorderStyle.Double;

            var control3 = new Label("This is a right aligned label (shadow, full width, double border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height + 2;
            control3.Width = page.Width;
            control3.TextAlign = TextAlign.Right;
            control3.BorderStyle = BorderStyle.Double;

            control1.HasShadow = true;
            control2.HasShadow = true;
            control3.HasShadow = true;

            page.Controls.Add(control1);
            page.Controls.Add(control2);
            page.Controls.Add(control3);

            page.Footer.Text = page.Name + ". Press any key.";

            window.Add(page);

            page.AfterPaint += (s, e) =>
            {
                Console.ReadKey(true);
            };
        }

        private static void ShadowLabelPage(Window window)
        {
            var page = new Page("Basic Labels");

            var control1 = new Label("This is a left aligned label (shadow, no border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = page.Width - 2;

            var control2 = new Label("This is a centered label (shadow, no border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height + 2;
            control2.Width = page.Width - 1;
            control2.TextAlign = TextAlign.Center;

            var control3 = new Label("This is a right aligned label (shadow, full width, no border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height + 2;
            control3.Width = page.Width;
            control3.TextAlign = TextAlign.Right;

            control1.HasShadow = true;
            control2.HasShadow = true;
            control3.HasShadow = true;

            page.Controls.Add(control1);
            page.Controls.Add(control2);
            page.Controls.Add(control3);

            page.Footer.Text = page.Name + ". Press any key.";

            window.Add(page);

            page.AfterPaint += (s, e) =>
            {
                Console.ReadKey(true);
            };
        }

        private static void ShadowSingleBorderLabelPage(Window window)
        {
            var page = new Page("Single Border Labels");

            var control1 = new Label("This is a left aligned label (shadow, single border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = page.Width - 2;
            control1.BorderStyle = BorderStyle.Single;

            var control2 = new Label("This is a centered label (shadow, single border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height + 2;
            control2.Width = page.Width - 1;
            control2.TextAlign = TextAlign.Center;
            control2.BorderStyle = BorderStyle.Single;

            var control3 = new Label("This is a right aligned label (shadow, full width, single border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height + 2;
            control3.Width = page.Width;
            control3.TextAlign = TextAlign.Right;
            control3.BorderStyle = BorderStyle.Single;

            control1.HasShadow = true;
            control2.HasShadow = true;
            control3.HasShadow = true;

            page.Controls.Add(control1);
            page.Controls.Add(control2);
            page.Controls.Add(control3);

            page.Footer.Text = page.Name + ". Press any key.";

            window.Add(page);

            page.AfterPaint += (s, e) =>
            {
                Console.ReadKey(true);
            };
        }

        private static void SingleBorderLabelPage(Window window)
        {
            var page = new Page("Single Border Labels");

            var control1 = new Label("This is a left aligned label (full width, single border).");

            control1.Left = 0;
            control1.Top = 0;
            control1.Width = page.Width;
            control1.BorderStyle = BorderStyle.Single;

            var control2 = new Label("This is a centered label (full width, single border).");

            control2.Left = 0;
            control2.Top = control1.Top + control1.Height;
            control2.Width = page.Width;
            control2.TextAlign = TextAlign.Center;
            control2.BorderStyle = BorderStyle.Single;

            var control3 = new Label("This is a right aligned label (full width, single border).");

            control3.Left = 0;
            control3.Top = control2.Top + control2.Height;
            control3.Width = page.Width;
            control3.TextAlign = TextAlign.Right;
            control3.BorderStyle = BorderStyle.Single;

            page.Controls.Add(control1);
            page.Controls.Add(control2);
            page.Controls.Add(control3);

            page.Footer.Text = page.Name + ". Press any key.";

            window.Add(page);

            page.AfterPaint += (s, e) =>
            {
                Console.ReadKey(true);
            };
        }
    }
}