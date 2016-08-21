using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleUI;
using Xunit;

namespace ConsoleUI.Tests.Controls
{
    
    public class TestTextbox
    {
        [Fact]
        public void TestCreateTextBox()
        {
            var screen = new Screen(20, 20, "Jon");
            var tbox = new TextBox();
            screen.Controls.Add(tbox);
            screen.Controls.Add(tbox);
            tbox.Text = "Hello";
        }
    }
}
