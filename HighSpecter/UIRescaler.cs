using System.Drawing;
using System.Windows.Forms;

namespace HighSpecter
{
    class UIRescaler
    {
        //UI scaling Code
        private readonly double _defaultContainerWidth;
        private readonly double _defaultContainerHeight; 


        private Control _defaultParameters;

        private Control _dynamicParameters;
         
        private double Xscale;
        private double Yscale; 

        public UIRescaler(Control control)
        {

            _defaultContainerWidth = control.Parent.Width;
            _defaultContainerHeight = control.Parent.Height;
            _defaultParameters = new Control("", 0, 0, control.Width, control.Height)
            {
                Padding =
                    new Padding(control.Padding.Left, control.Padding.Top, control.Padding.Right, control.Padding.Bottom),
                Margin =
                    new Padding(control.Margin.Left, control.Margin.Top, control.Margin.Right, control.Margin.Bottom),
                Font = 
                    new Font(control.Font.FontFamily, control.Font.SizeInPoints, control.Font.Style),
                Location = new Point(control.Location.X, control.Location.Y)
            };
           
            _dynamicParameters = new Control("", 0, 0, control.Width, control.Height)
            {
                Padding =
                    new Padding(control.Padding.Left, control.Padding.Top, control.Padding.Right, control.Padding.Bottom),
                Margin =
                    new Padding(control.Margin.Left, control.Margin.Top, control.Margin.Right, control.Margin.Bottom),
                Font = new Font(control.Font.FontFamily, control.Font.SizeInPoints, control.Font.Style),
                Location = new Point(control.Location.X, control.Location.Y)
            };


            Xscale = 1;
         }

        public void Scale(Control control)
        {
            Xscale = control.Parent.Width/_defaultContainerWidth;
            Yscale = control.Parent.Height / _defaultContainerHeight;
           // SizeF scale = new SizeF((float)Xscale, (float)Xscale);
            //control.Scale(scale);

            _dynamicParameters.Location = new Point((int)(_defaultParameters.Location.X * Xscale),
                (int)(_defaultParameters.Location.Y * Yscale));

            _dynamicParameters.Padding = new Padding((int)(_defaultParameters.Padding.Left * Xscale),
                (int)(_defaultParameters.Padding.Top * Yscale), (int)(_defaultParameters.Padding.Right * Xscale),
                (int)(_defaultParameters.Padding.Bottom * Yscale));

            _dynamicParameters.Margin = new Padding((int)(_defaultParameters.Margin.Left * Xscale),
                (int)(_defaultParameters.Margin.Top * Yscale), (int)(_defaultParameters.Margin.Right * Xscale),
                (int)(_defaultParameters.Margin.Bottom * Yscale));

            //Need to dispose of old font 
            var temp = _dynamicParameters.Font;
            var smallest = (Yscale > Xscale) ? Xscale : Yscale;
            _dynamicParameters.Font = new Font(_defaultParameters.Font.FontFamily, (float)(_defaultParameters.Font.Size * smallest), _defaultParameters.Font.Style);
            temp.Dispose();
            
            _dynamicParameters.Width = (int)(_defaultParameters.Width * Xscale);
            _dynamicParameters.Height = (int)(_defaultParameters.Height * Yscale);
            
          
        }

        public Padding get_Padding()
        {
            return _dynamicParameters.Padding;
        }

        public Point get_Location()
        {
            return _dynamicParameters.Location;
        }

        public Padding get_Margin()
        {
            return _dynamicParameters.Margin;
        }

        public Font get_Font()
        {
            return _dynamicParameters.Font;
        }

        public Size get_Size()
        {
            return _dynamicParameters.Size;
        }
    }
}
