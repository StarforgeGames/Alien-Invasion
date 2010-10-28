using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D10.Device;
using ResourceManagement.Resources;
using Graphics.Resources;
using System.Collections.Generic;
using ResourceManagement;
using System.Runtime.InteropServices;

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

        private IEvent evt = new BasicEvent();

        public readonly CommandQueue commandQueue = new CommandQueue();
        private Extractor extractor;
        private bool HandleCommands;
        private bool stateChanged;

        public Renderer(Control control, Extractor extractor)
        {
            this.extractor = extractor;
            renderThread = new Thread(renderLoop);
            renderThread.Name = "Renderer";
            renderFrame = control;


            var currentDescription = GetDefaultSwapChainDescription(control);

            InitDeviceAndSwapChain(currentDescription);
            SetDefaultViewport();
            SetDefaultRenderingBuffer();
            control.BackColor = Color.Empty;


            BlendStateDescription statedescr = new BlendStateDescription();

            
            statedescr.SetBlendEnable(0, true);
  //          statedescr.SetBlendEnable(1, true);
            statedescr.SetWriteMask(0, SlimDX.Direct3D10.ColorWriteMaskFlags.All);
//            statedescr.SetWriteMask(1, SlimDX.Direct3D10.ColorWriteMaskFlags.All);

            statedescr.BlendOperation = BlendOperation.Add;
            statedescr.DestinationBlend = SlimDX.Direct3D10.BlendOption.InverseSourceAlpha;
            statedescr.SourceBlend = SlimDX.Direct3D10.BlendOption.SourceAlpha;

            statedescr.AlphaBlendOperation = BlendOperation.Add;
            statedescr.DestinationAlphaBlend = SlimDX.Direct3D10.BlendOption.Zero;
            statedescr.SourceAlphaBlend = SlimDX.Direct3D10.BlendOption.Zero;
            
            BlendState newstate = BlendState.FromDescription(device, statedescr);

            device.OutputMerger.BlendState = newstate;


            renderFrame.Resize += new EventHandler(RenderFrame_Resize);


            InitDebugFont();
            QueryPerformanceFrequency(out tickFrequency);

        }

        SlimDX.Direct3D10.Font debugFont;

        private void InitDebugFont()
        {
            FontDescription fontDesc = new FontDescription()
            {
                Height = 16,
                Width = 0,
                Weight = FontWeight.Bold,
                MipLevels = 1,
                IsItalic = false,
                PitchAndFamily = FontPitchAndFamily.Default | FontPitchAndFamily.DontCare,
                FaceName = "Arial"
            };

            debugFont = new SlimDX.Direct3D10.Font(device, fontDesc);
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

        public void StartHandleCommands()
        {
            evt = new BasicEvent();
            HandleCommands = true;
            
            stateChanged = true;
            
        }

        public void StartRender()
        {
            evt = new BasicEvent();
            IsRendering = true;
            StartHandleCommands();
            stateChanged = true;
            renderThread.Start();
            
        }

        public void StopRender()
        {
            evt = new BasicEvent();
            IsRendering = false;
            stateChanged = true;
        }

        public void StopHandleCommands()
        {
            evt = new BasicEvent();
            HandleCommands = false;
            stateChanged = true;
        }

        /// <summary>
        /// Use Windows API functions as they have a higher resolution which is preferable for Direct3D Applications.
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/aa964692%28VS.80%29.aspx"/>
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        long lastTick;
        long tickFrequency;

        private void renderLoop()
        {
            Color4 color = new Color4(Color.Black);
            Random rand = new Random(2434545);

            
            
            while (IsRendering || HandleCommands)
            {
                try
                {
                    if (HandleCommands)
                    {
                        if (!commandQueue.Empty)
                        {
                            commandQueue.Execute(CommandsPerFrame);
                            //color = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
                        }
                    }

                    if (IsRendering)
                    {
                        device.ClearRenderTargetView(
                            views[0],
                            color);
                        
                        RenderObjects objs = extractor.Scene;

                        extractor.ExtractNext = true;
                        foreach (var obj in objs.Objs)
                        {
                            using (MeshResource mesh = (MeshResource)obj.Key.Acquire())
                            {
                                device.InputAssembler.SetPrimitiveTopology(mesh.primitiveTopology);
                                device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(mesh.buffer, mesh.size / 4, 0));

                                foreach (var matRes in obj.Value)
                                {
                                    using (MaterialResource material = (MaterialResource)matRes.Key.Acquire())
                                    {
                                        var positions = from RenderObject mat in matRes.Value
                                                        select mat.position;
                                        Vector2[] posArray = positions.ToArray();
                                        Effect effect = material.effect.effect;

                                        effect.GetVariableByName("tex2D").AsResource().SetResource(material.texture.texture);



                                        EffectTechnique tech = effect.GetTechniqueByName("Full");
                                        for (int i = 0; i < tech.Description.PassCount; ++i)
                                        {
                                            EffectPass pass = tech.GetPassByIndex(i);
                                            InputLayout layout = new InputLayout(device, pass.Description.Signature, mesh.inputLayout);
                                            device.InputAssembler.SetInputLayout(layout);
                                            effect.GetVariableByName("bounds").AsVector().Set(matRes.Value.First().bounds);



                                            for (int j = 0; j < posArray.Length; ++j)
                                            {
                                                effect.GetVariableByName("posi").AsVector().Set(posArray[j]);
                                                pass.Apply();
                                                device.Draw(4, 0);
                                            }


                                        }
                                    }
                                }
                            }
                        }
                        long tick;
                        QueryPerformanceCounter(out tick);

                        long diff = tick - lastTick;
                        lastTick = tick;
                        if (diff < 1)
                        {
                            diff = 1;
                        }


                        debugFont.Draw(null, "fps: " + (tickFrequency / diff).ToString(), new Rectangle(10, 10, 100, 20), FontDrawFlags.Left, new Color4(255.0f, 1.0f, 1.0f, 255.0f));
                        swapChain.Present(0, PresentFlags.None);
                        
                    }
                   // Thread.Sleep(1000);
                }
                catch (SlimDX.DXGI.DXGIException)
                {
                   // IsRendering = false;
                }

                if (stateChanged)
                {
                    stateChanged = false;
                    evt.Finish();
                }
            }

            if (stateChanged)
            {
                stateChanged = false;
                evt.Finish();
            }
            
        }

        public void WaitForCompletion()
        {
            renderThread.Join();
        }

        public void WaitForStateChange()
        {
            evt.Wait();
        }


        #region IDisposable Members

        public void Dispose()
        {
            StopRender();
            WaitForCompletion();

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
