using System;
using System.Collections.Generic;

namespace KeyboardLib
{
    public class Keyboard
    {
        public List<Row> Rows { get; set; }
    }

    public class Row
    {
        public List<Key> Keys { get; set; }
    }

    public class Key
    {
        public string Text { get; set; }
        public nfloat? Width { get; set; }
    }
}

