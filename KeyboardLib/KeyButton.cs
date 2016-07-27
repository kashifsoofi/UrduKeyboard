using System;
using Foundation;

using UIKit;

namespace KeyboardLib
{
    [Register("KeyButton")]
    public class KeyButton : UIButton
    {
        public Key Key { get; set; }
    }
}

