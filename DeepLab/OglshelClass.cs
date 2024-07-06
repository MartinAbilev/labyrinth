using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

/*
 put in references OpenTK, GLCONTrol and stuff  add dlls to your project
 
 */ 
 

namespace OglShel
{
    class OglshelObject
    {
        

        public OglshelObject(DeepLab lab,string title)
        {
            initGL(lab,title);
            

        }


        void initGL(DeepLab lab,string title)
        {
            using (GlWindow glwin = new GlWindow(lab ,1024, 768))
            {
                glwin.Title = title;
                glwin.Run(30.0, 0.0);
            }




        }
    }


    #region GLWINDOW // main gl code
    public class GlWindow : GameWindow
    {
        DeepLab Lab;
        double Time;
        int texture;

        int wheel;

        float deltaX=0, deltaY=0;

        float x=0;float y=0;float z=0;
            
            Point pos;

            bool leftbutton = false;
            bool rightbutton = false;

        public GlWindow(DeepLab lab ,uint width, uint height)
            : base((int)width, (int)height)
        {
            Lab = lab;
            texture=LoadTexture(@"Asets\cell.png");


        }

        protected override void OnLoad(EventArgs e)//  inits go here
        {

            GL.ClearColor(0f,0f,0.5f,1.0f);
            GL.ShadeModel(ShadingModel.Smooth);
            //GL.ClearDepth(1.0f);
            //GL.DepthFunc(DepthFunction.Lequal);
            //GL.Enable(EnableCap.DepthTest);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            Mouse.Move += new EventHandler<MouseMoveEventArgs>(Mouse_Move);

            Mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>(Mouse_WheelChanged);

            Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonDown);
            Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonUp);

           

        }

        protected override void OnResize(EventArgs e)// calded on resize window
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            OpenTK.Matrix4 perspective = OpenTK.Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 640);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);




            
        }

        protected override void OnUpdateFrame(FrameEventArgs e)// per frame stuff
        {
            base.OnUpdateFrame(e);
            Time += e.Time;
        }

        protected override void OnUnload(EventArgs e)// on shut down
        {
            base.OnUnload(e);

            //TextureManager.Singleton.UnloadAll();
        }

        void SetPerspectiveView()
        {

            float speed = 0.1f;
            if (rightbutton)
            {
                x -= deltaX * speed; y += deltaY * speed;
            }
            
            z = 10f+(wheel*3);

            deltaX = 0; deltaY = 0;

            Matrix4 modelview = Matrix4.LookAt(new Vector3(0.0f + x, 0.0f + y, 3.0f + z), new Vector3(0.0f + x, 0.0f + y, -3.0f + z), Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
              
            
        }

        void SetOrtoView()
        {
            Matrix4 projection = Matrix4.CreateOrthographic(Width , Height , 1.0f, -1.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
              
        }

        void SetScreenView()
        {
            //GL.LoadIdentity();
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0.0f, Width, Height, 0.0f, -1.0f, 1.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
             
        }

        void DrawSprite3d()
        {
         



            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0);

            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            

            GL.TexCoord2(1, 0);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            
            GL.TexCoord2(1, 1);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            
            GL.TexCoord2(0, 1);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.End();  




        }




        void DrawSprite2d()
        {




            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);
            
            GL.TexCoord2(0, 1);
            GL.Vertex2(-100.0f, -100.0f);
            
            GL.TexCoord2(0, 0);
            GL.Vertex2(-100.0f, 100.0f);

            GL.TexCoord2(1, 0);
            GL.Vertex2(100.0f, 100.0f);

            GL.TexCoord2(1, 1);
            GL.Vertex2(100.0f, -100.0f);
            
            
            GL.End();




        }

        private void DrawCube()
        {
            //GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            
            
            GL.Begin(BeginMode.Quads);

            GL.Color3(Color.Silver);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);

            GL.Color3(Color.Honeydew);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            GL.Color3(Color.Moccasin);

            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);

            GL.Color3(Color.IndianRed);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);

            GL.Color3(Color.PaleVioletRed);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);

            GL.Color3(Color.ForestGreen);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            GL.End();
        }

        void DrawTxt()
        {
            

        }

        static int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp;
            try
            {
                bmp = new Bitmap(filename);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("\n no such file " + filename);
                return 0;
            }



            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return id;
        }

        void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    leftbutton = true;
                    break;

                case MouseButton.Middle:

                    break;
                case MouseButton.Right:
                    rightbutton = true;

                    break;
            }


        }

        void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    leftbutton = false;
                    Lab = new DeepLab();
                    break;

                case MouseButton.Middle:

                    break;
                case MouseButton.Right:
                    rightbutton = false;

                    break;
            }


        }



        void Mouse_WheelChanged(object sender, MouseWheelEventArgs e)
        {
            wheel = e.Value;

            Console.WriteLine(wheel);

        }

        void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            pos = e.Position;

            //Console.WriteLine(pos);

            deltaX = e.XDelta;
            deltaY = e.YDelta;

            //Console.WriteLine("\n deltaX {0} deltaY {0}", deltaX, deltaY);
        }

        protected override void OnRenderFrame(FrameEventArgs e)// and finaly rendering
        {

            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);// clear all screen

            
            
            GL.PushMatrix();
            SetPerspectiveView();
            for (int i = 0; i < Lab.Lab.Length; i++)
            {
                GL.PushMatrix();
                GL.Translate(new Vector3(Lab.Lab[i].X*2+.1f, Lab.Lab[i].Y*2+.1f , -120.0f));
                
                if (i == 0) GL.Color3(1.0f, 0f, 0f);
                else GL.Color3(0f, 0f,1.0f- (1.0f/Lab.depth  )   * Lab.Lab[i].depth );

                DrawSprite3d();
                GL.PopMatrix();
            }
            GL.PopMatrix();
            
            
            GL.LoadIdentity();// i dont know why here it only works

            


            /*
            GL.PushMatrix();
            SetOrtoView();
            for (int i = 0; i < Lab.Lab.Length; i++)
            {
                GL.PushMatrix();
                GL.Translate(new Vector3(Lab.Lab[i].X*200, Lab.Lab[i].Y*200,0.0f) );
                DrawSprite2d();
                GL.PopMatrix();
            }
            GL.PopMatrix();
            */


            GL.PushMatrix();
            SetScreenView();
            DrawSprite2d();
            GL.PopMatrix();
            

            SwapBuffers();
        }

    }
    #endregion

    

}
