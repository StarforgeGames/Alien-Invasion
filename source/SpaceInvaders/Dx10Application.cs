using System;
using System.Drawing;
using System.Globalization;
using SlimDX;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using SlimDX.Windows;
using Device = SlimDX.Direct3D10.Device;
using Font = SlimDX.Direct3D10.Font;

namespace SpaceInvaders
{
    /// <summary>
    /// Abstract base class that implements the basic Diret3D 10 initialization and provides extensions hooks.
    /// </summary>
    public abstract class Dx10Application : IDisposable
    {
        public RenderForm RenderForm { get; set; }
        public Device Device { get; set; }
        public SwapChain SwapChain { get; set; }
        public RenderTargetView RenderTargetView { get; set; }
        public Texture2D BackBuffer { get; set; }

        public Font DebugFont { get; set; }
        public GameTimer Timer { get; set; }
        public string FrameStats { get; set; }

        public string GameTitle { get; set; }
        public string GameDirectory { get; set; }

        public bool ShowFps { get; set; }

        private int frameCount;
        private float baseTime;

        public Dx10Application()
        {
            GameTitle = "Dx10 Application";
            ShowFps = true;
        }

        /// <summary>
        /// Initialize the application including Direct3D 10.
        /// </summary>
        public virtual void Initialize()
        {
            RenderForm = new RenderForm(GameTitle);
            initializeDirect3D();

            FontDescription fontDesc = new FontDescription() {
                Height = 16,
                Width = 0,
                Weight = FontWeight.Bold,
                MipLevels = 1,
                IsItalic = false,
                PitchAndFamily = FontPitchAndFamily.Default | FontPitchAndFamily.DontCare,
                FaceName = "Arial"
            };

            DebugFont = new Font(Device, fontDesc);
            Timer = new GameTimer();
            FrameStats = String.Empty;
        }

        /// <summary>
        /// Initializes Direct3D 10.
        /// </summary>
        private void initializeDirect3D()
        {
            var swapDesc = new SwapChainDescription() {
                ModeDescription = new ModeDescription() {
                    Width = RenderForm.ClientSize.Width,
                    Height = RenderForm.ClientSize.Height,
                    RefreshRate = new Rational(60, 1),
                    Format = Format.R8G8B8A8_UNorm

                },
                SampleDescription = new SampleDescription() {
                    Count = 1,
                    Quality = 0
                },
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = RenderForm.Handle,
                IsWindowed = true,
                SwapEffect = SwapEffect.Discard
            };

            DeviceCreationFlags flags = DeviceCreationFlags.None;
#if DEBUG
            flags |= DeviceCreationFlags.Debug;
#endif
            Device dev;
            SwapChain sc;
            Device.CreateWithSwapChain(null, DriverType.Hardware, flags, swapDesc, out dev, out sc);
            Device = dev;
            this.SwapChain = sc;

            // Stops Alt + Enter from causing fullscreen skrewiness.
            Factory factory = SwapChain.GetParent<Factory>();
            factory.SetWindowAssociation(RenderForm.Handle, WindowAssociationFlags.IgnoreAll);

            Texture2D backBuffer = Texture2D.FromSwapChain<Texture2D>(SwapChain, 0);
            RenderTargetView = new RenderTargetView(Device, backBuffer);

            Device.OutputMerger.SetTargets(RenderTargetView);
            Device.Rasterizer.SetViewports(
                new Viewport(0, 0, RenderForm.ClientSize.Width, RenderForm.ClientSize.Height, 0.0f, 1.0f));
        }

        /// <summary>
        /// Updates the scene every tick.
        /// </summary>
        /// <param name="deltaTime">Time that passed since last update.</param>
        public virtual void Update(float deltaTime)
        {
            if (ShowFps) {
                frameCount++;

                // Compute averages over one second period
                if ((Timer.Time - baseTime) >= 1.0f) {
                    float fps = (float)frameCount; // fps = frameCount / 1
                    float milliSecondsPerFrame = 1000.0f / fps;

                    FrameStats = String.Format("FPS: {0}\nMPF: {1} ms", fps.ToString("#"),
                        milliSecondsPerFrame.ToString("0.000", CultureInfo.InvariantCulture));

                    // Reset for next average.
                    frameCount = 0;
                    baseTime += 1.0f;
                }
            }
        }

        /// <summary>
        /// Draw the scene.
        /// </summary>
        public virtual void Render()
        {
            Device.ClearRenderTargetView(RenderTargetView, Color.Black);

            if (ShowFps) {
                Rectangle r = new Rectangle(5, 5, 0, 0);
                DebugFont.Draw(null, FrameStats, r, FontDrawFlags.NoClip, new Color4(Color.Yellow));
            }

            SwapChain.Present(0, PresentFlags.None);
        }

        /// <summary>
        /// Run main loop.
        /// </summary>
        public void Run()
        {
            Timer.Reset();

            MessagePump.Run(RenderForm, () => {
                Timer.Tick();
                Update(Timer.DeltaTime);
                Render();
            });
        }

        /// <summary>
        /// Dispose of all allocated resources.
        /// </summary>
        public void Dispose()
        {
            DebugFont.Dispose();
            RenderTargetView.Dispose();
            BackBuffer.Dispose();
            Device.Dispose();
            SwapChain.Dispose();
            RenderForm.Dispose();
        }
    }
}
