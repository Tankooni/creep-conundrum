#region File Description
//-----------------------------------------------------------------------------
// Game1.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SplitScreen;

namespace SplitScreenWindows
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model Ring;
        float RingRotation = 0f;
        Vector3 RingPosition = Vector3.Zero;

        SampleArcBallCamera[] Camera = new SampleArcBallCamera[4];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferMultiSampling = true;
        }


        protected override void Initialize()
        {
            for (int i = 0; i < 4; i++)
            {
                Camera[i] = new SampleArcBallCamera(SampleArcBallCameraMode.Free);
                Camera[i].Target = new Vector3(0, 0, 0);
                Camera[i].Distance = 50f;
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        Viewport defaultViewport;
        Viewport[] viewports = new Viewport[4];

        Matrix projectionMatrix;
        Matrix halfprojectionMatrix;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            defaultViewport = GraphicsDevice.Viewport;

            for (int i = 0; i < 4; i++)
            {
                viewports[i] = defaultViewport;
                viewports[i].Width = viewports[i].Width / 2;
                viewports[i].Height = viewports[i].Height / 2;
            }

            viewports[1].X = viewports[0].Width;
            viewports[2].Y = viewports[0].Height;
            viewports[3].X = viewports[0].Width;
            viewports[3].Y = viewports[0].Height;


            //            rightViewport.X = leftViewport.Width + 1;

            Ring = Content.Load<Model>("redtorus");

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 4.0f / 3.0f, 1.0f, 10000f);
            halfprojectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 2.0f / 1.5f, 1.0f, 10000f);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

#if WINDOWS
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
#endif
            GamePadState PlayerOne = GamePad.GetState(PlayerIndex.One);

            Camera[0].OrbitUp(PlayerOne.ThumbSticks.Left.Y / 4);
            Camera[0].OrbitRight(PlayerOne.ThumbSticks.Left.X / 4);

            Camera[1].OrbitUp(PlayerOne.ThumbSticks.Right.Y / 4);
            Camera[1].OrbitRight(PlayerOne.ThumbSticks.Right.X / 4);

            Camera[2].OrbitUp(PlayerOne.Triggers.Left / 4);
            Camera[2].OrbitRight(PlayerOne.Triggers.Right / 4);

            //Camera[3].OrbitUp(PlayerOne.ThumbSticks.Right.Y / 4);
            //Camera[3].OrbitRight(PlayerOne.ThumbSticks.Right.X / 4);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Viewport = defaultViewport;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            for (int i = 0; i < 4; i++)
            {
                GraphicsDevice.Viewport = viewports[i];
                DrawScene(gameTime, Camera[i].ViewMatrix, halfprojectionMatrix);
            }

            base.Draw(gameTime);

        }

        protected void DrawScene(GameTime gameTime, Matrix view,
            Matrix projection)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in Ring.Meshes)
            {
                //This is where the mesh orientation is set, 
                //as well as our camera and projection
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.Identity *
                        Matrix.CreateRotationY(RingRotation) *
                        Matrix.CreateTranslation(RingPosition);
                    effect.View = view;
                    effect.Projection = projection;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }

    }
}
