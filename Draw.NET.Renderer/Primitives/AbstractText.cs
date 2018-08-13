using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.Primitives
{
    public abstract class AbstractText : AbstractPrimitive, IText
    {

        protected string familyName = "Times New Roman";
        public string FamilyName
        {
            get
            {
                return familyName;
            }
            set
            {
                if (familyName != value)
                {
                    familyName = value;
                    OnPropertyChanged(nameof(FamilyName));
                }
            }
        }




        protected float fontSize = 10;

        public float FontSize
        {
            get
            {
                return fontSize;
            }

            set
            {
                if (value != fontSize)
                {
                    fontSize = value;
                    OnPropertyChanged(nameof(FontSize));
                }
            }
        }




        protected SizeF stringSize = new SizeF();

        public SizeF StringSize
        {
            get
            {
                return stringSize;
            }

            set
            {
                if (stringSize != value)
                {
                    stringSize = value;
                    OnPropertyChanged(nameof(StringSize));
                }
            }
        }



        protected FontStyle fontStyle = FontStyle.Regular;
        public FontStyle FontStyle
        {
            get
            {
                return fontStyle;
            }

            set
            {
                if (fontStyle != value)
                {
                    fontStyle = value;
                    OnPropertyChanged(nameof(FontStyle));
                }
            }
        }


        protected PointF location = new PointF();

        public PointF Location
        {
            get
            {
                return location;
            }

            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        protected LocationType locationType = LocationType.Absolute;

        public LocationType LocationType
        {
            get
            {
                return locationType;
            }

            set
            {
                if (locationType != value)
                {
                    locationType = value;
                    OnPropertyChanged(nameof(LocationType));
                }
            }
        }

        protected Color textColor = Color.Transparent;

        public Color TextColor
        {
            get
            {
                return textColor;
            }

            set
            {
                if (textColor != value)
                {
                    textColor = value;
                    OnPropertyChanged(nameof(TextColor));
                }
            }
        }


        protected string content;

        public string Content
        {
            get { return content; }
            set
            {
                if (content != value)
                {
                    content = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }

        protected StringAlignment alignment = StringAlignment.Center;
        public StringAlignment Alignment
        {
            get
            {
                return alignment;
            }
            set
            {
                alignment = value;
                OnPropertyChanged(nameof(Alignment));
            }
        }



        protected StringAlignment lineAlignment = StringAlignment.Center;
        public StringAlignment LineAlignment
        {
            get
            {
                return lineAlignment;
            }

            set
            {
                lineAlignment = value;
                OnPropertyChanged(nameof(LineAlignment));
            }
        }

        protected StringTrimming trimming = StringTrimming.Word;
        public StringTrimming Trimming
        {
            get
            {
                return trimming;
            }
            set
            {
                trimming = value;
                OnPropertyChanged(nameof(Trimming));
            }
        }

        public abstract Font Font { get; }

        public override RectangleF GetBounds()
        {
            return new RectangleF(location, stringSize);
        }

    }
}
