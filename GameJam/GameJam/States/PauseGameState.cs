﻿using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Events;
using GameJam.Directors;
using GameJam.Events;
using GameJam.Events.Audio;
using GameJam.Events.UI.Pause;
using GameJam.Graphics.Text;
using GameJam.Processes.Animations;
using GameJam.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.States
{
    public class PauseGameState : CommonGameState, IEventListener
    {
        private SpriteBatch _spriteBatch;
        private FieldFontRenderer _fieldFontRenderer;
        private Root _root;
        private readonly Process _gameProcess;

        public PauseGameState(GameManager gameManager, SharedGameState sharedState, Process gameProcess) : base(gameManager, sharedState)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
            _fieldFontRenderer = new FieldFontRenderer(Content, GameManager.GraphicsDevice);
            _gameProcess = gameProcess;
        }

        protected override void OnInitialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_pause_menu"));

            _root.AutoControlModeSwitching = true;

            ProcessManager.Attach(new PauseDirector(null, Content, ProcessManager));

            EventManager.Instance.QueueEvent(new PauseAllSoundsEvent());

            base.OnInitialize();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnFixedUpdate(float dt)
        {
            base.OnFixedUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _root.Render(_spriteBatch, _fieldFontRenderer);

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnTogglePause()
        {
            base.OnTogglePause();
        }

        protected override void OnKill()
        {
            _root.AutoControlModeSwitching = false;

            EventManager.Instance.QueueEvent(new ResumeAllSoundsEvent());
            base.OnKill();
        }

        protected override void RegisterListeners()
        {
            _root.RegisterListeners();

            EventManager.Instance.RegisterListener<TogglePauseGameEvent>(this);
            EventManager.Instance.RegisterListener<ExitToLobbyEvent>(this);

            base.RegisterListeners();
        }

        protected override void UnregisterListeners()
        {
            _root.UnregisterListeners();

            EventManager.Instance.UnregisterListener(this);

            base.UnregisterListeners();
        }

        public bool Handle(IEvent evt)
        {
            if (evt is TogglePauseGameEvent)
            {
                HandleUnpause();

                return true;
            }
            if (evt is ExitToLobbyEvent)
            {
                HandleReturnToLobby();
            }

            return false;
        }

        private void HandleUnpause()
        {
            GameManager.ProcessManager.TogglePauseAll();
            Kill();
        }

        private void HandleReturnToLobby()
        {
            GameManager.ProcessManager.TogglePauseAll();
            _gameProcess.Kill();
            SharedState.ProcessManager.Attach(new CameraPositionZoomResetProcess(SharedState.Camera, CVars.Get<float>("game_over_camera_reset_duration"), Vector2.Zero, 1, Easings.Functions.CubicEaseOut));
            ChangeState(new UILobbyGameState(GameManager, SharedState));
        }
    }
}
