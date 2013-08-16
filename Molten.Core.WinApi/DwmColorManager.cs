using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Molten.Core.WinApi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DwmColorParams
    {
        public uint ColorizationColor;
        public uint ColorizationAfterglow;
        public uint ColorizationColorBalance;
        public uint ColorizationAfterglowBalance;
        public uint ColorizationBlurBalance;
        public uint ColorizationGlassReflectionIntensity;
        public uint ColorizationOpaqueBlend;
    };

    /// <summary>
    /// Contains utility methods for interacting with the Desktop Window Manager color options.
    /// </summary>
    public static class DwmColorManager
    {
        private const int FRAMES_PER_SECOND = 30;
        private const float DURATION = 3;

        [DllImport("dwmapi.dll", EntryPoint = "#131")]
        private static extern int DwmpSetColorizationParameters(ref DwmColorParams dcpParams, bool alwaysTrue);
        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        private static extern int DwmpGetColorizationParameters(out DwmColorParams dcpParams);

        /// <summary>
        /// Sets the Windows DWM color.
        /// </summary>
        /// <param name="newColor">The color to set.</param>
        public static void SetColor(Color newColor)
        {
            DwmColorParams p = new DwmColorParams();
            DwmpGetColorizationParameters(out p);
            p.ColorizationColor = (uint)newColor.ToArgb();
            p.ColorizationAfterglow = p.ColorizationColor;
            DwmpSetColorizationParameters(ref p, true);
        }

        /// <summary>
        /// Retrieves the current DWM color.
        /// </summary>
        /// <returns>The current DWM color setting.</returns>
        public static Color GetColor()
        {
            DwmColorParams p = new DwmColorParams();
            DwmpGetColorizationParameters(out p);

            return Color.FromArgb((int)p.ColorizationColor);
        }

#if ENABLE_45_ASYNC

        /// <summary>
        /// Asynchronously fades from the current DWM color to <paramref name="newColor" />.
        /// </summary>
        /// <param name="newColor">The new color to fade to.</param>
        public static async Task BeginChangeColor(Color newColor, float duration = DURATION)
        {
            await Task.Run(() =>
            {
                int ticks = (int)(FRAMES_PER_SECOND * DURATION); // Round to the nearest whole step

                DwmColorParams p = new DwmColorParams();
                DwmpGetColorizationParameters(out p);

                Color startColor = Color.FromArgb((int)p.ColorizationColor);

                if (newColor == startColor)
                {
                    return;
                }

                ColorTransform ct = new ColorTransform(startColor, newColor, ticks);

                while (ct.Transform())
                {
                    if (p.ColorizationColor != (uint)ct.GetColor().ToArgb())
                    {
                        p.ColorizationColor = (uint)ct.GetColor().ToArgb();
                        p.ColorizationAfterglow = p.ColorizationColor;
                        DwmpSetColorizationParameters(ref p, true);
                        Thread.Sleep((int)((1.00 / FRAMES_PER_SECOND) * 1000)); // Sleep for one "tick" (based on FPS).
                    }
                }
            });
        }

#endif

#if !ENABLE_45_ASYNC
        
        /// <summary>
        /// Asynchronously fades from the current DWM color to <paramref name="newColor" />.
        /// </summary>
        /// <param name="newColor">The new color to fade to.</param>
        public static void BeginChangeColor(Color newColor, Action onCompleted, float duration = DURATION)
        {
            new Task(() =>
            {
                int ticks = (int)(FRAMES_PER_SECOND * DURATION); // Round to the nearest whole step

                DwmColorParams p = new DwmColorParams();
                DwmpGetColorizationParameters(out p);

                Color startColor = Color.FromArgb((int)p.ColorizationColor);

                if (newColor == startColor)
                {
                    return;
                }

                ColorTransform ct = new ColorTransform(startColor, newColor, ticks);

                while (ct.Transform())
                {
                    if (p.ColorizationColor != (uint)ct.GetColor().ToArgb())
                    {
                        p.ColorizationColor = (uint)ct.GetColor().ToArgb();
                        p.ColorizationAfterglow = p.ColorizationColor;
                        DwmpSetColorizationParameters(ref p, true);
                        Thread.Sleep((int)((1.00 / FRAMES_PER_SECOND) * 1000)); // Sleep for one "tick" (based on FPS).
                    }
                }

                onCompleted();
            }).Start();
        }

#endif
    }
}
