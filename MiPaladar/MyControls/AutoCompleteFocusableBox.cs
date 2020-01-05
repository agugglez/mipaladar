using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;

namespace MiPaladar.MyControls
{
    public class AutoCompleteFocusableBox : AutoCompleteBox
    {
        public new void Focus()
        {
            var textbox = Template.FindName("Text", this) as TextBox;
            if (textbox != null) textbox.Focus();
        }
    }
}
