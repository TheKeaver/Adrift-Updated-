﻿using System;
using System.Collections;
using Events;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.UI.Widgets
{
    public class DropDownPanel : Widget, IParentWidget, ISelectableWidget
    {
        private NinePatchRegion2D _releasedNinePatch;
        private NinePatchRegion2D _hoverNinePatch;
        private NinePatchRegion2D _pressedNinePatch;
        public NinePatchRegion2D ReleasedNinePatch
        {
            get
            {
                return _releasedNinePatch;
            }
            set
            {
                _releasedNinePatch = value;

                ComputeProperties();
            }
        }
        public NinePatchRegion2D HoverNinePatch
        {
            get
            {
                return _hoverNinePatch;
            }
            set
            {
                _hoverNinePatch = value;

                ComputeProperties();
            }
        }
        public NinePatchRegion2D PressedNinePatch
        {
            get
            {
                return _pressedNinePatch;
            }
            set
            {
                _pressedNinePatch = value;

                ComputeProperties();
            }
        }

        public Button SubPanel
        {
            get;
            private set;
        }
        public Panel ContentsPanel
        {
            get;
            private set;
        }

        private WeakReference<ISelectableWidget> _previousSelected = new WeakReference<ISelectableWidget>(null);
        private bool _showDropDown = false;
        public bool ShowDropDown
        {
            get
            {
                return _showDropDown;
            }
            set
            {
                _showDropDown = value;
                if (_showDropDown)
                {
                    TryChangeSelectionToDropDown();
                } else
                {
                    TryChangeSelectionFromDropDown();
                }
            }
        }
        public string aboveID { get => ((ISelectableWidget)SubPanel).aboveID; set => ((ISelectableWidget)SubPanel).aboveID = value; }
        public string leftID { get => ((ISelectableWidget)SubPanel).leftID; set => ((ISelectableWidget)SubPanel).leftID = value; }
        public string rightID { get => ((ISelectableWidget)SubPanel).rightID; set => ((ISelectableWidget)SubPanel).rightID = value; }
        public string belowID { get => ((ISelectableWidget)SubPanel).belowID; set => ((ISelectableWidget)SubPanel).belowID = value; }
        public bool isSelected { get => ((ISelectableWidget)SubPanel).isSelected; set => ((ISelectableWidget)SubPanel).isSelected = value; }

        public DropDownPanel(NinePatchRegion2D releasedNinePatch,
            NinePatchRegion2D hoverNinePatch,
            NinePatchRegion2D pressedNinePatch,
            HorizontalAlignment hAlign,
            AbstractValue horizontal,
            VerticalAlignment vAlign,
            AbstractValue vertical,
            AbstractValue width,
            AbstractValue height,
            AbstractValue contentsWidth,
            AbstractValue contentsHeight)
            :base(hAlign, horizontal, vAlign, vertical, width, height)
        {
            SubPanel = new Button(releasedNinePatch,
                hoverNinePatch,
                pressedNinePatch,
                HorizontalAlignment.Center, new FixedValue(0),
                VerticalAlignment.Center, new FixedValue(0),
                new RelativeValue(1, () => { return Width; }),
                new RelativeValue(1, () => { return Height; }));
            SubPanel.Parent = this;
            SubPanel.Action = () =>
            {
                ShowDropDown = !ShowDropDown;
            };

            ContentsPanel = new Panel(HorizontalAlignment.Left, new FixedValue(0),
                VerticalAlignment.Top, new RelativeValue(1, () =>
                {
                    return Height;
                }),
                contentsWidth, contentsHeight);
            ContentsPanel.Parent = this;

            ComputeProperties();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Hidden)
            {
                ShowDropDown = false;
            }
            if(!Hidden)
            {
                SubPanel.ForceShowAsPressed = ShowDropDown;
                SubPanel.Draw(spriteBatch);
                if (ShowDropDown)
                {
                    Root.RequestDeferredDraw(ContentsPanel);
                }
            }
        }

        public override bool Handle(IEvent evt)
        {
            if (ShowDropDown)
            {
                GamePadButtonDownEvent gamePadButtonDownEvent = evt as GamePadButtonDownEvent;
                if(gamePadButtonDownEvent != null)
                {
                    if (gamePadButtonDownEvent._pressedButton == Microsoft.Xna.Framework.Input.Buttons.B)
                    {
                        ShowDropDown = false;
                        return true;
                    }
                }

                if (ContentsPanel.Handle(evt))
                {
                    return true;
                }

                MouseButtonEvent mouseButtonEvent = evt as MouseButtonEvent;
                if (mouseButtonEvent != null)
                {
                    // Close if clicked outside
                    if (mouseButtonEvent.LeftButtonState == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        // Subpanel (button) handles closing already; exclude
                        bool insideSubPanel = mouseButtonEvent.CurrentPosition.X > SubPanel.TopLeft.X
                            && mouseButtonEvent.CurrentPosition.X < SubPanel.BottomRight.X
                            && mouseButtonEvent.CurrentPosition.Y > SubPanel.TopLeft.Y
                            && mouseButtonEvent.CurrentPosition.Y < SubPanel.BottomRight.Y;
                        bool insideContentsPanel = mouseButtonEvent.CurrentPosition.X > ContentsPanel.TopLeft.X
                            && mouseButtonEvent.CurrentPosition.X < ContentsPanel.BottomRight.X
                            && mouseButtonEvent.CurrentPosition.Y > ContentsPanel.TopLeft.Y
                            && mouseButtonEvent.CurrentPosition.Y < ContentsPanel.BottomRight.Y;
                        if (!(insideContentsPanel || insideSubPanel))
                        {
                            ShowDropDown = false;
                        }
                    }
                }
            }

            if (SubPanel.Handle(evt))
            {
                return true;
            }

            return false;
        }

        public void Add(Widget widget)
        {
            SubPanel.Add(widget);
            ComputeProperties();
        }

        public void AddContent(Widget widget)
        {
            ContentsPanel.Add(widget);
            ComputeProperties();
        }

        public void Remove(Widget widget)
        {
            SubPanel.Remove(widget);
            ComputeProperties();
        }

        public void RemoveContent(Widget widget)
        {
            ContentsPanel.Add(widget);
            ComputeProperties();
        }

        protected override void OnComputeProperties()
        {
            SubPanel.ComputeProperties();
            ContentsPanel.ComputeProperties();

            base.OnComputeProperties();
        }

        private void TryChangeSelectionToDropDown()
        {
            ISelectableWidget selectedWidget = Root.FindSelectedWidget();
            _previousSelected = new WeakReference<ISelectableWidget>(selectedWidget);
            
            foreach(Widget widget in ContentsPanel)
            {
                ISelectableWidget selectableWidget = widget as ISelectableWidget;
                if(selectableWidget != null)
                {
                    selectableWidget.isSelected = true;
                    selectedWidget.isSelected = false;
                    break;
                }
            }
        }

        private void TryChangeSelectionFromDropDown()
        {
            foreach (Widget widget in ContentsPanel)
            {
                ISelectableWidget selectableWidget = widget as ISelectableWidget;
                if (selectableWidget != null)
                {
                    selectableWidget.isSelected = false;
                }
            }
            ISelectableWidget previousSelectedWidget;
            _previousSelected.TryGetTarget(out previousSelectedWidget);
            if(previousSelectedWidget == null || previousSelectedWidget == this)
            {
                isSelected = true;
                return;
            }
            previousSelectedWidget.isSelected = true;
        }

        public IEnumerator GetEnumerator()
        {
            return ((IParentWidget)SubPanel).GetEnumerator();
        }

        public ISelectableWidget FindSelectedWidget()
        {
            if(isSelected)
            {
                return this;
            }
            return ContentsPanel.FindSelectedWidget();
        }
    }
}
