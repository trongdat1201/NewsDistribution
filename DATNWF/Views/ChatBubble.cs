using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace DATNWF.Views
{
    public enum BubbleType { User, AI }

    public partial class ChatBubble : UserControl
    {
        private readonly BubbleType _bubbleType;
        private readonly Label _lblMessage;
        private readonly Label _lblTime;
        private readonly Guna2Panel _pnlBubble;

        private const int MaxBubbleWidth = 240; 
        private const int SidePadding = 12;
        private const int VerticalGap = 10;
        private const int TextPaddingX = 14;
        private const int TextPaddingY = 12;

        public ChatBubble(string message, DateTime time, BubbleType bubbleType)
        {
            _bubbleType = bubbleType;

            this.SuspendLayout();

            var isUser = _bubbleType == BubbleType.User;
            var bubbleColor = isUser ? Color.FromArgb(220, 60, 30) : Color.FromArgb(240, 242, 245);
            var textColor = isUser ? Color.White : Color.FromArgb(30, 30, 30);
            var timeColor = isUser ? Color.FromArgb(255, 220, 210) : Color.FromArgb(140, 140, 140);

            // UserControl container
            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.MinimumSize = new Size(0, 30);
            this.Padding = new Padding(0);
            this.Margin = new Padding(0, 0, 0, VerticalGap);
            this.BackColor = Color.White;

            // Outer bubble panel
            _pnlBubble = new Guna2Panel
            {
                AutoSize = false,
                BackColor = Color.Transparent,
                FillColor = bubbleColor,
                BorderRadius = 18,
                Padding = new Padding(TextPaddingX, TextPaddingY, TextPaddingX, TextPaddingY)
            };

            // Attempt to apply asymmetric border radii for a modern chat look
            try 
            {
                if (isUser) 
                {
                    _pnlBubble.CustomizableEdges.BottomRight = false;
                } 
                else 
                {
                    _pnlBubble.CustomizableEdges.BottomLeft = false;
                }
            } 
            catch { /* Ignore if CustomizableEdges is not supported in this Guna version */ }

            _lblMessage = new Label
            {
                Text = "",
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = textColor,
                TextAlign = ContentAlignment.TopLeft,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            _lblTime = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Regular),
                ForeColor = timeColor,
                AutoSize = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            _pnlBubble.Controls.Add(_lblMessage);
            _pnlBubble.Controls.Add(_lblTime);

            this.Controls.Add(_pnlBubble);
            this.ResumeLayout(false);

            SetMessage(message, time);
        }

        public void SetMessage(string message, DateTime time)
        {
            if (message == null) message = string.Empty;
            _lblMessage.Text = message;
            _lblTime.Text = time.ToString("HH:mm");
            LayoutBubble();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutBubble();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            LayoutBubble();
        }

        private void LayoutBubble()
        {
            if (_pnlBubble == null || _lblMessage == null || _lblTime == null)
                return;

            this.SuspendLayout();
            _pnlBubble.SuspendLayout();

            int containerWidth = this.ClientSize.Width;
            if (containerWidth <= 0) containerWidth = this.Width;
            if (containerWidth <= 0) return;

            // Maximum width for the bubble
            int maxBubble = Math.Min(MaxBubbleWidth, containerWidth - SidePadding * 2);
            if (maxBubble < 100) maxBubble = 100;

            // Max inner width for text
            int innerMaxWidth = maxBubble - TextPaddingX * 2;
            if (innerMaxWidth < 20) innerMaxWidth = 20;

            // Restrict label maximum width for automatic word wrapping
            _lblMessage.MaximumSize = new Size(innerMaxWidth, 0);

            // Get text preferred size
            Size textSize = _lblMessage.GetPreferredSize(new Size(innerMaxWidth, 0));
            Size timeSize = _lblTime.GetPreferredSize(Size.Empty);

            // Calculate bubble dimensions
            int bubbleWidth = Math.Min(maxBubble, Math.Max(textSize.Width, timeSize.Width) + TextPaddingX * 2);
            int bubbleHeight = TextPaddingY * 2 + textSize.Height + timeSize.Height + 2; 

            _pnlBubble.Size = new Size(bubbleWidth, bubbleHeight);

            // Position elements inside the bubble
            _lblMessage.Location = new Point(TextPaddingX, TextPaddingY);
            
            // Align time label based on bubble type
            int timeX = _bubbleType == BubbleType.User ? bubbleWidth - timeSize.Width - TextPaddingX : TextPaddingX;
            _lblTime.Location = new Point(timeX, TextPaddingY + textSize.Height + 2);

            // Position bubble inside the UserControl
            int x = _bubbleType == BubbleType.User
                ? containerWidth - bubbleWidth - SidePadding
                : SidePadding;
            
            _pnlBubble.Location = new Point(Math.Max(0, x), 0);

            // Set height of UserControl row
            this.MinimumSize = new Size(0, bubbleHeight + VerticalGap);
            this.Height = bubbleHeight + VerticalGap;

            _pnlBubble.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}