using System;
using System.Numerics;
using System.Text;
using RaylibTest.MainAssembly;
using RaylibTest.Rendering;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.SPIRV;
using Veldrid.StartupUtilities;

namespace GettingStarted
{
    class Program
    {
        static readonly ShaderHelper Shaderlib = new ShaderHelper();
        static GraphicsDevice _graphicsDevice;
        static CommandList _commandList;
        static DeviceBuffer _vertexBuffer;
        static DeviceBuffer _indexBuffer;
        static DeviceBuffer myUniformBlock;
        static Shader[] _shaders;
        static Pipeline _pipeline;

        static void Main(string[] args)
        {
            Game GameClass = new Game();
            GameClass.Startup();
        }

        static void RunRenderer()
        {
            WindowCreateInfo windowCI = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = 960,
                WindowHeight = 540,
                WindowTitle = "Veldrid Tutorial"
            };
            Sdl2Window window = VeldridStartup.CreateWindow(ref windowCI);

            _graphicsDevice = VeldridStartup.CreateGraphicsDevice(window);

            CreateResources();

            while (window.Exists)
            {
                window.PumpEvents();

                if (window.Exists) Draw();
            }

            DisposeResources();
        }

        static void CreateResources()
        {
            ResourceFactory factory = _graphicsDevice.ResourceFactory;

            VertexPositionColor[] quadVertices =
            {
                new VertexPositionColor(new Vector2(-.5f, .5f), RgbaFloat.Red),
                new VertexPositionColor(new Vector2(.5f, .5f), RgbaFloat.Green),
                new VertexPositionColor(new Vector2(-.5f, -.5f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector2(.5f, -.5f), RgbaFloat.Yellow)
            };

            BufferDescription vbDescription =
                new BufferDescription(4 * VertexPositionColor.SizeInBytes, BufferUsage.VertexBuffer);
            _vertexBuffer = factory.CreateBuffer(vbDescription);
            _graphicsDevice.UpdateBuffer(_vertexBuffer, 0, quadVertices);

            ushort[] quadIndices = {0, 1, 2, 3};
            BufferDescription ibDescription = new BufferDescription(4 * sizeof(ushort), BufferUsage.IndexBuffer);
            _indexBuffer = factory.CreateBuffer(ibDescription);
            _graphicsDevice.UpdateBuffer(_indexBuffer, 0, quadIndices);

            VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate,
                    VertexElementFormat.Float2),
                new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate,
                    VertexElementFormat.Float4));

            ShaderDescription vertexShaderDesc = new ShaderDescription(ShaderStages.Vertex,
                Encoding.UTF8.GetBytes(Shaderlib.GetShaderText("VertexShader.vp")), "main");
            ShaderDescription fragmentShaderDesc = new ShaderDescription(ShaderStages.Fragment,
                Encoding.UTF8.GetBytes(Shaderlib.GetShaderText("FragmentShader.fp")), "main");

            _shaders = factory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);

            // Create pipeline
            GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription();
            pipelineDescription.BlendState = BlendStateDescription.SingleOverrideBlend;
            pipelineDescription.DepthStencilState =
                new DepthStencilStateDescription(true, true, ComparisonKind.LessEqual);

            pipelineDescription.RasterizerState = new RasterizerStateDescription(FaceCullMode.Back,
                PolygonFillMode.Solid, FrontFace.Clockwise, true, false);

            pipelineDescription.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            pipelineDescription.ResourceLayouts = Array.Empty<ResourceLayout>();
            pipelineDescription.ShaderSet = new ShaderSetDescription(new[] {vertexLayout}, _shaders);
            pipelineDescription.Outputs = _graphicsDevice.SwapchainFramebuffer.OutputDescription;

            _pipeline = factory.CreateGraphicsPipeline(pipelineDescription);

            _commandList = factory.CreateCommandList();
        }

        static void Draw()
        {
            // Begin() must be called before commands can be issued.
            _commandList.Begin();

            // We want to render directly to the output window.
            _commandList.SetFramebuffer(_graphicsDevice.SwapchainFramebuffer);
            _commandList.ClearColorTarget(0, RgbaFloat.Black);

            // Set all relevant state to draw our quad.
            _commandList.SetVertexBuffer(0, _vertexBuffer);
            _commandList.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
            _commandList.SetPipeline(_pipeline);
            // Issue a Draw command for a single instance with 4 indices.
            _commandList.DrawIndexed(
                4,
                1,
                0,
                0,
                0);

            // End() must be called before commands can be submitted for execution.
            _commandList.End();
            _graphicsDevice.SubmitCommands(_commandList);

            // Once commands have been submitted, the rendered image can be presented to the application window.
            _graphicsDevice.SwapBuffers();
        }

        static void DisposeResources()
        {
            _pipeline.Dispose();
            foreach (Shader shader in _shaders) shader.Dispose();
            _commandList.Dispose();
            _vertexBuffer.Dispose();
            _indexBuffer.Dispose();
            _graphicsDevice.Dispose();
        }
    }

    struct VertexPositionColor
    {
        public const uint SizeInBytes = 24;
        public Vector2 Position;
        public RgbaFloat Color;

        public VertexPositionColor(Vector2 position, RgbaFloat color)
        {
            Position = position;
            Color = color;
        }
    }
}