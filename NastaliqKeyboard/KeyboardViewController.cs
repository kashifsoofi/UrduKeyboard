using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using KeyboardLib;

using ObjCRuntime;
using Foundation;
using UIKit;

namespace NastaliqKeyboard
{
    public partial class KeyboardViewController : UIInputViewController
    {
        KeyButton nextKeyboardButton;

        protected KeyboardViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void UpdateViewConstraints()
        {
            base.UpdateViewConstraints();

            // Add custom view sizing constraints here
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform custom UI setup here
            var keyboard = LoadKeyboard("nastaliq_layout");
            var keyboardView = new KeyboardView(this);
            keyboardView.CreateKeyboardView(keyboard);
            keyboardView.TranslatesAutoresizingMaskIntoConstraints = false;
            keyboardView.BackgroundColor = UIColor.Orange;

            View.Add(keyboardView);
            var widthConstraint = NSLayoutConstraint.Create(keyboardView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, View, NSLayoutAttribute.Width, 1.0f, 0.0f);
            var heightConstraint = NSLayoutConstraint.Create(keyboardView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, View, NSLayoutAttribute.Height, 1.0f, 0.0f);
            var leftConstraint = NSLayoutConstraint.Create(keyboardView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, View, NSLayoutAttribute.Left, 1.0f, 0.0f);
            var bottomConstraint = NSLayoutConstraint.Create(keyboardView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1.0f, 0.0f);
            View.AddConstraints(new[] { widthConstraint, heightConstraint, leftConstraint, bottomConstraint });

            nextKeyboardButton = new KeyButton(UIButtonType.System);

            nextKeyboardButton.SetTitle("Next Keyboard", UIControlState.Normal);
            nextKeyboardButton.SizeToFit();
            nextKeyboardButton.TranslatesAutoresizingMaskIntoConstraints = false;

            nextKeyboardButton.AddTarget(this, new Selector("advanceToNextInputMode"), UIControlEvent.TouchUpInside);

            keyboardView.AddSubview(nextKeyboardButton);

            var nextKeyboardButtonLeftSideConstraint = NSLayoutConstraint.Create(nextKeyboardButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, keyboardView, NSLayoutAttribute.Left, 1.0f, 0.0f);
            var nextKeyboardButtonBottomConstraint = NSLayoutConstraint.Create(nextKeyboardButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, keyboardView, NSLayoutAttribute.Bottom, 1.0f, 0.0f);
            keyboardView.AddConstraints(new[] { nextKeyboardButtonLeftSideConstraint, nextKeyboardButtonBottomConstraint });
        }

        public void OnTouchUpInside(object sender, EventArgs e)
        {
            //var button = sender as KeyButton;
            //var text = button.Title(UIControlState.Normal);
            //_uiInputViewController.TextDocumentProxy.InsertText(text);
            TextDocumentProxy.InsertText("?");
        }

        public override void TextWillChange(IUITextInput textInput)
        {
            // The app is about to change the document's contents. Perform any preparation here.
        }

        public override void TextDidChange(IUITextInput textInput)
        {
            // The app has just changed the document's contents, the document context has been updated.
            UIColor textColor = null;

            if (TextDocumentProxy.KeyboardAppearance == UIKeyboardAppearance.Dark)
            {
                textColor = UIColor.White;
            }
            else {
                textColor = UIColor.Black;
            }

            nextKeyboardButton.SetTitleColor(textColor, UIControlState.Normal);
        }

        private Keyboard LoadKeyboard(string name)
        {
            //var keyboard = new Keyboard
            //{
            //    Rows = new List<Row>
            //    {
            //        new Row
            //        {
            //            Keys = new List<Key>
            //            {
            //                new Key { Text = "q" }
            //            }
            //        },
            //        new Row
            //        {
            //            Keys = new List<Key>
            //            {
            //                new Key { Text = "a" }
            //            }
            //        }
            //    }
            //};

            var path = NSBundle.MainBundle.PathForResource(name, "json");
            using (var streamReader = new StreamReader(path))
            {
                var json = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<Keyboard>(json,
                                                               new JsonSerializerSettings
                                                               {
                                                                   Converters = { new NativeTypeConverter() }
                                                               });
            }
        }
    }
}

