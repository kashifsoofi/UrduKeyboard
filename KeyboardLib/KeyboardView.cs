using System;
using System.Collections.Generic;

using CoreGraphics;
using ObjCRuntime;
using UIKit;

namespace KeyboardLib
{
    public class KeyboardView : UIView
    {
        private UIInputViewController _uiInputViewController;

        public KeyboardView(UIInputViewController uiInputViewController)
            : base()
        {
            _uiInputViewController = uiInputViewController;
        }

        public void CreateKeyboardView(Keyboard keyboard)
        {
            var rowViews = new List<UIView>();
            foreach (var row in keyboard.Rows)
            {
                var rowView = CreateRowView(row);
                AddSubview(rowView);
                rowView.TranslatesAutoresizingMaskIntoConstraints = false;
                rowViews.Add(rowView);
            }

            AddRowConstraints(rowViews, this);
        }

        private UIView CreateRowView(Row row)
        {
            var buttons = new List<UIButton>();
            var rowView = new UIView(new CGRect(0, 0, 320, 40));
            foreach (var key in row.Keys)
            {
                var button = CreateButton(key);
                buttons.Add(button);
                rowView.AddSubview(button);
            }

            AddButtonConstraints(buttons, rowView);
            return rowView;
        }

        private UIButton CreateButton(Key key)
        {
            var button = new UIButton(UIButtonType.System);
            button.Frame = new CGRect(0, 0, 20, 20);
            button.SetTitle(key.Text, UIControlState.Normal);
            button.SizeToFit();
            button.TitleLabel.Font = UIFont.SystemFontOfSize(15);
            button.TranslatesAutoresizingMaskIntoConstraints = false;
            button.BackgroundColor = UIColor.FromWhiteAlpha(1.0f, 1.0f);
            button.SetTitleColor(UIColor.DarkGray, UIControlState.Normal);
            //button.Key = key;

            button.TouchUpInside += OnTouchUpInside;
            //button.AddTarget(_uiInputViewController, new Selector("OnTouchUpInside"), UIControlEvent.TouchUpInside);

            return button;
        }

        public void OnTouchUpInside(object sender, EventArgs e)
        {
            var button = sender as UIButton;
            var text = button.Title(UIControlState.Normal);
            _uiInputViewController.TextDocumentProxy.InsertText(text);
        }

        private void AddRowConstraints(List<UIView> rowViews, UIView container)
        {
            for (int i = 0; i < rowViews.Count; i++)
            {
                var rowView = rowViews[i];
                var leftConstraint = NSLayoutConstraint.Create(rowView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, container, NSLayoutAttribute.Left, 1.0f, 1.0f);
                var rightConstraint = NSLayoutConstraint.Create(rowView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, container, NSLayoutAttribute.Right, 1.0f, -1.0f);

                NSLayoutConstraint topConstraint;
                if (i == 0)
                {
                    topConstraint = NSLayoutConstraint.Create(rowView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, container, NSLayoutAttribute.Top, 1.0f, 0.0f);
                }
                else {
                    var prevRow = rowViews[i - 1];
                    topConstraint = NSLayoutConstraint.Create(rowView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, prevRow, NSLayoutAttribute.Bottom, 1.0f, 0.0f);

                    var firstRow = rowViews[0];
                    var heightConstraint = NSLayoutConstraint.Create(firstRow, NSLayoutAttribute.Height, NSLayoutRelation.Equal, rowView, NSLayoutAttribute.Height, 1.0f, 0.0f);
                    container.AddConstraint(heightConstraint);
                }

                NSLayoutConstraint bottomConstraint;
                if (i == rowViews.Count - 1)
                {
                    bottomConstraint = NSLayoutConstraint.Create(rowView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, container, NSLayoutAttribute.Bottom, 1.0f, 0.0f);
                }
                else {
                    var nextRow = rowViews[i + 1];
                    bottomConstraint = NSLayoutConstraint.Create(rowView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, nextRow, NSLayoutAttribute.Top, 1.0f, 0.0f);
                }

                container.AddConstraints(new[] {
                    leftConstraint,
                    rightConstraint,
                    topConstraint,
                    bottomConstraint
                });
            }
        }

        private void AddButtonConstraints(List<UIButton> buttons, UIView container)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];
                var topConstraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Top, NSLayoutRelation.Equal, container, NSLayoutAttribute.Top, 1.0f, 1.0f);
                var bottomContraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, container, NSLayoutAttribute.Bottom, 1.0f, -1.0f);
                NSLayoutConstraint rightContraint;
                if (i == buttons.Count - 1)
                {
                    rightContraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Right, NSLayoutRelation.Equal, container, NSLayoutAttribute.Right, 1.0f, -1.0f);
                }
                else {
                    var nextButton = buttons[i + 1];
                    rightContraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Right, NSLayoutRelation.Equal, nextButton, NSLayoutAttribute.Left, 1.0f, -1.0f);
                }

                NSLayoutConstraint leftConstraint;
                if (i == 0)
                {
                    leftConstraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Left, NSLayoutRelation.Equal, container, NSLayoutAttribute.Left, 1.0f, 1.0f);
                }
                else {
                    var prevButton = buttons[i - 1];
                    leftConstraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Left, NSLayoutRelation.Equal, prevButton, NSLayoutAttribute.Right, 1.0f, 1.0f);

                    var firstButton = buttons[0];
                    //var widthConstraint = NSLayoutConstraint.Create (button, NSLayoutAttribute.Width, NSLayoutRelation.Equal, firstButton, NSLayoutAttribute.Width, button.Key.Width ?? 1.0f, 0.0f);
                    var widthConstraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Width, NSLayoutRelation.Equal, firstButton, NSLayoutAttribute.Width, 1.0f, 0.0f);

                    container.AddConstraint(widthConstraint);
                }
                container.AddConstraints(new[] {
                    topConstraint,
                    bottomContraint,
                    rightContraint,
                    leftConstraint
                });
            }
        }
    }
}

