﻿using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ch0nkEngine.Cameras;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Buffer = SlimDX.Direct3D11.Buffer;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;

namespace Ch0nkEngine
{
    static class Program
    {
        [StructLayout(LayoutKind.Explicit)]
        struct ConstantBuffer
        {
            [FieldOffset(0)]
            public Color4 Color;
        }

        static void Main()
        {
            Device device;
            SwapChain swapChain;
            ShaderSignature inputSignature;
            VertexShader vertexShader;
            PixelShader pixelShader;
            GeometryShader geometryShader;
            Camera targetCamera;

            var form = new RenderForm("Ch0nkEngine");
            var description = new SwapChainDescription
            {
                BufferCount = 2,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = form.Handle,
                IsWindowed = true,
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, description, out device, out swapChain);

            // create a view of our render target, which is the backbuffer of the swap chain we just created
            RenderTargetView renderTarget;
            using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
                renderTarget = new RenderTargetView(device, resource);

            // setting a viewport is required if you want to actually see anything
            var context = device.ImmediateContext;
            var viewport = new Viewport(0, 0, form.ClientSize.Width, form.ClientSize.Height);
            context.OutputMerger.SetTargets(renderTarget);
            context.Rasterizer.SetViewports(viewport);
            

            targetCamera = new TargetCamera(viewport);
            targetCamera.Position = new Vector3(1,1,1);
            targetCamera.Target = new Vector3(1, 1, -1);

            // load and compile the vertex shader
            using (var bytecode = ShaderBytecode.CompileFromFile("triangle.fx", "VShader", "vs_4_0", ShaderFlags.None, EffectFlags.None))
            {
                inputSignature = ShaderSignature.GetInputSignature(bytecode);
                vertexShader = new VertexShader(device, bytecode);
            }

            // load and compile the pixel shader
            using (var bytecode = ShaderBytecode.CompileFromFile("triangle.fx", "PShader", "ps_4_0", ShaderFlags.None, EffectFlags.None))
                pixelShader = new PixelShader(device, bytecode);

            using (var bytecode = ShaderBytecode.CompileFromFile("triangle.fx", "Triangulat0r", "gs_4_0", ShaderFlags.None, EffectFlags.None))
                geometryShader = new GeometryShader(device, bytecode);

            // create test vertex data, making sure to rewind the stream afterward
            var vertices = new DataStream(12 * 3, true, true);
            vertices.Write(new Vector3(-0.5f, -0.5f, 0.5f));
            vertices.Position = 0;

            // create the vertex layout and buffer
            var elements = new[] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0) };
            var layout = new InputLayout(device, inputSignature, elements);
            var vertexBuffer = new Buffer(device, vertices, 12, ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

            // configure the Input Assembler portion of the pipeline with the vertex data
            context.InputAssembler.InputLayout = layout;
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, 12, 0));
            
            // set the shaders
            context.VertexShader.Set(vertexShader);
            context.GeometryShader.Set(geometryShader);
            context.PixelShader.Set(pixelShader);

            // prevent DXGI handling of alt+enter, which doesn't work properly with Winforms
            using (var factory = swapChain.GetParent<Factory>())
                factory.SetWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAltEnter);

            // handle alt+enter ourselves
            form.KeyDown += (o, e) =>
            {
                if (e.Alt && e.KeyCode == Keys.Enter)
                    swapChain.IsFullScreen = !swapChain.IsFullScreen;
            };

            // handle form size changes
            form.UserResized += (o, e) =>
            {
                renderTarget.Dispose();

                swapChain.ResizeBuffers(2, 0, 0, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);
                using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
                    renderTarget = new RenderTargetView(device, resource);

                context.OutputMerger.SetTargets(renderTarget);
            };
            var buffer = new ConstantBuffer();


            context.VertexShader.Se

            MessagePump.Run(form, () =>
            {
                // clear the render target to a soothing blue
                context.ClearRenderTargetView(renderTarget, new Color4(0.5f, 0.5f, 1.0f));

                // draw the triangle
                context.Draw(3, 0);
                swapChain.Present(0, PresentFlags.None);
            });

            // clean up all resources
            // anything we missed will show up in the debug output
            vertices.Close();
            vertexBuffer.Dispose();
            layout.Dispose();
            inputSignature.Dispose();
            vertexShader.Dispose();
            pixelShader.Dispose();
            renderTarget.Dispose();
            swapChain.Dispose();
            device.Dispose();
        }
    }
}
