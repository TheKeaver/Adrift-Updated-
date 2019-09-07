﻿using System.Collections.Generic;
using GameJam.UINew;
using Microsoft.Xna.Framework.Graphics;
using UI.Content.Pipeline;

namespace GameJam.States
{
    public class UIPlaygroundGameState : GameState
    {
        SpriteBatch _spriteBatch;

        Root _root;

        public UIPlaygroundGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        public override void Initialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/test"));
        }


        public override void Show()
        {
            _root.RegisterListeners();
        }

        public override void Hide()
        {
            _root.UnregisterListeners();
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Dispose()
        {
            
        }
    }
}
