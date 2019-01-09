namespace coler.Model.ColorGen
{
    public class ColorGenBase
    {
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double XParameter { get; set; }
        public double YParameter { get; set; }
        public int[] ColorParameters { get; set; }

        public void UpdateDimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void UpdateParameters(double xParameter, double yParameter)
        {
            XParameter = xParameter;
            YParameter = yParameter;
        }
    }
}
