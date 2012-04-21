using System.Windows.Forms;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;
using Ch0nkEngine.Cameras;
using Ch0nkEngine.Cameras.Components;
using System;

namespace Ch0nkEngine
{
    public partial class MainForm : Form
    {
        GameTime gameTime;

        public MainForm()
        {
            InitializeComponent();
            new Master(this);
            Master.I.Load();
        }

        internal void Render()
        {
            Master.I.Update(gameTime);
            Master.I.Render(gameTime);

            gameTime.Update();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            gameTime = new GameTime();
            gameTime.Start();
        }



        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
