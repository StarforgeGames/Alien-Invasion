using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D10.Device;

namespace Graphics
{

    public class Renderer : IDisposable
    {
        private Control renderFrame;

        public Device device;
        private SwapChain swapChain;
        private Thread renderThread;
        public bool IsRendering { get; private set; }
        private RenderTargetView[] views;

        private const uint CommandsPerFrame = 3;
        private const int BackBufferCount = 1;
        private const Format BackBufferFormat = Format.R8G8B8A8_UNorm;

        public readonly CommandQueue commandQueue = new CommandQueue();
        private Form RenderForm;
        private Extractor extractor;

        public Renderer(Control control, Extractor extractor)
        {

            renderThread = new Thread(renderLoop);
            renderThread.Name = "Renderer";
            renderFrame = control;

            var currentDescription = GetDefaultSwapChainDescription(control);

            InitDeviceAndSwapChain(currentDescription);
            SetDefaultViewport();
            SetDefaultRenderingBuffer();
            control.BackColor = Color.Empty;
            

            renderFrame.Resize += new EventHandler(RenderFrame_Resize);

        }

        void RenderFrame_Resize(object sender, EventArgs e)
        {
            commandQueue.Add(
                () => {
                    device.ClearState();
                    foreach (var view in views)
                    {
                        view.Dispose();
                    }

                    swapChain.ResizeBuffers(BackBufferCount, renderFrame.ClientSize.Width >> 1 << 1,
                    renderFrame.ClientSize.Height >> 1 << 1, BackBufferFormat, SwapChainFlags.None);

                    SetDefaultRenderingBuffer();
                    SetDefaultViewport();
                });
        }

        private void SetDefaultViewport()
        {
            device.Rasterizer.SetViewports(
                new Viewport(
                    0,
                    0,
                    renderFrame.ClientSize.Width,
                    renderFrame.ClientSize.Height,
                    0.0f,
                    1.0f));
        }

        private void SetDefaultRenderingBuffer()
        {
            using (Texture2D backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0))
            {
                views = new RenderTargetView[1];
                views[0] = new RenderTargetView(device, backBuffer);

                device.OutputMerger.SetTargets(views);

            }
        }

        private void InitDeviceAndSwapChain(SwapChainDescription currentDescription)
        {
            DeviceCreationFlags flags = DeviceCreationFlags.None;
#if DEBUG
            flags |= DeviceCreationFlags.Debug;
#endif
            flags |= DeviceCreationFlags.SingleThreaded;

            DriverType type;

#if SOFTWARE
            type = DriverType.Warp;
#else
            type = DriverType.Hardware;
#endif


            Device.CreateWithSwapChain(null, type, flags, currentDescription, out device, out swapChain);

            // Stops Alt + Enter from causing fullscreen skrewiness.
            using (Factory factory = swapChain.GetParent<Factory>())
            {
                factory.SetWindowAssociation(renderFrame.Handle, WindowAssociationFlags.IgnoreAltEnter);
            }
        }

        private SwapChainDescription GetDefaultSwapChainDescription(Control control)
        {
            var swapDesc = new SwapChainDescription()
            {
                ModeDescription = new ModeDescription()
                {
                    Width = control.ClientSize.Width,
                    Height = control.ClientSize.Height,
                    RefreshRate = new Rational(60, 1),
                    Format = BackBufferFormat
                },
                SampleDescription = new SampleDescription()
                {
                    Count = 1,
                    Quality = 0
                },
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = control.Handle,
                IsWindowed = true,
                SwapEffect = SwapEffect.Discard
            };

            return swapDesc;
        }

        public void Start()
        {
            IsRendering = true;
            renderThread.Start();
        }

        public void Stop()
        {
            IsRendering = false;
        }

        private void renderLoop()
        {
            Color4 color = new Color4(Color.DarkBlue);
            Random rand = new Random(2434545);

            
            while (IsRendering || !commandQueue.Empty)
            {
                try
                {
                    if (!commandQueue.Empty)
                    {
                        commandQueue.Execute(CommandsPerFrame);
                        color = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
                    }

                    device.ClearRenderTargetView(
                        views[0],
                        color);

                    swapChain.Present(0, PresentFlags.None);
                }
                catch (SlimDX.DXGI.DXGIException)
                {
                   // IsRendering = false;
                }
            }
            
        }

        #region IDisposable Members

        public void Dispose()
        {
            Stop();
            renderThread.Join();

            foreach (var view in views)
            {
                view.Dispose();
            }

            device.Dispose();
            swapChain.Dispose();
        }

        #endregion
    }

}
