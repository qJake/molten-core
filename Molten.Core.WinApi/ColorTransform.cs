using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Molten.Core.WinApi
{
    /// <summary>
    /// Defines a color transformation option from start to end.
    /// </summary>
    internal sealed class ColorTransform
    {
        public int currentStep;
        private Color goal;
        private Color start;
        public int steps;

        public ColorTransform(Color start, Color end, int steps)
        {
            this.start = start;
            this.goal = end;
            this.steps = steps;
        }

        public Color GetColor()
        {
            int red = this.start.R + ((this.currentStep * (this.goal.R - this.start.R)) / this.steps);
            int green = this.start.G + ((this.currentStep * (this.goal.G - this.start.G)) / this.steps);
            int blue = this.start.B + ((this.currentStep * (this.goal.B - this.start.B)) / this.steps);
            int alpha = this.start.A + ((this.currentStep * (this.goal.A - this.start.A)) / this.steps);
            return Color.FromArgb(alpha, red, green, blue);
        }

        public bool Transform()
        {
            if (this.currentStep < this.steps)
            {
                this.currentStep++;
                return true;
            }
            return false;
        }
    }
}
