using Audrey;
using System;
using System.Collections.Generic;

namespace GameJam.NUI.Widgets
{
    public class TableWidget : Widget
    {
        private List<RowWidget> _rows = new List<RowWidget>();

        public TableWidget(Engine engine) : base(engine)
        {
        }

        protected override void Initialize(Entity entity)
        {
        }

        protected override void OnComputeProperties(Entity entity)
        {
            foreach(RowWidget row in _rows)
            {
                row.ComputeProperties();
            }
        }

        public void Add(RowWidget row)
        {
            _rows.Add(row);
            row.Parent = this;

            row.Width = new RelativeValue<float>(this, "inner-width", 1.0f);

            // Height and margin-bottom/margin-top are controlled
            // depending on the rows before it.
            row.Height = new ComputedValue<float>(() =>
            {
                float height = Height.Value; // Height of table
                //for(int i = 0; i < _rows.IndexOf(row); i++) // Loop through all rows before this one
                //{
                //    height -= _rows[i].Height.Value;
                //}
                return height * (row.Units.Value / (float)CVars.Get<int>("ui_table_units"));
            });
            row.MarginTop = new ComputedValue<float>(() =>
            {
                switch (row.VAlign.Value)
                {
                    case VerticalAlignment.Top:
                        float marginTop = 0.0f; // Start aligned at the top
                        for (int i = 0; i < _rows.IndexOf(row); i++) // Loop through all rows before this one
                        {
                            if (_rows[i].VAlign.Value == VerticalAlignment.Top)
                            {
                                marginTop += _rows[i].Height.Value;
                            }
                        }
                        return marginTop;
                    case VerticalAlignment.Center:
                        throw new Exception("Rows cannot be Centered vertically.");
                    case VerticalAlignment.Bottom:
                    default:
                        return 0.0f;
                }
            });
            row.MarginBottom = new ComputedValue<float>(() =>
            {
                switch (row.VAlign.Value)
                {
                    case VerticalAlignment.Bottom:
                        float marginBottom = 0.0f; // Start aligned at the top
                        for (int i = 0; i < _rows.IndexOf(row); i++) // Loop through all rows before this one
                        {
                            if (_rows[i].VAlign.Value == VerticalAlignment.Bottom)
                            {
                                marginBottom += _rows[i].Height.Value;
                            }
                        }
                        return marginBottom;
                    case VerticalAlignment.Center:
                        throw new Exception("Rows cannot be Centered vertically.");
                    case VerticalAlignment.Top:
                    default:
                        return 0.0f;
                }
            });
        }

        public void Remove(RowWidget row)
        {
            _rows.Remove(row);
        }

        public class RowWidget : Widget
        {
            private List<ColumnWidget> _columns = new List<ColumnWidget>();

            public WidgetProperty<int> Units
            {
                get
                {
                    return Properties.GetProperty<int>("units");
                }
                set
                {
                    Properties.SetProperty("units", value);
                }
            }

            public RowWidget(Engine engine) : base(engine)
            {
            }

            protected override void Initialize(Entity entity)
            {
                Properties.SetProperty("units", new FixedValue<int>(CVars.Get<int>("ui_table_units")));

                VAlign = new FixedValue<VerticalAlignment>(VerticalAlignment.Top);
                HAlign = new FixedValue<HorizontalAlignment>(HorizontalAlignment.Center);
            }

            protected override void OnComputeProperties(Entity entity)
            {
                foreach(ColumnWidget column in _columns)
                {
                    column.ComputeProperties();
                }
            }

            public void Add(ColumnWidget column)
            {
                _columns.Add(column);
                column.Parent = this;

                column.Height = new RelativeValue<float>(this, "inner-height", 1.0f);

                // Height and margin-bottom/margin-top are controlled
                // depending on the rows before it.
                column.Width = new ComputedValue<float>(() =>
                {
                    float width = Width.Value;
                    return width * (column.Units.Value / (float)CVars.Get<int>("ui_table_units"));
                });
                column.MarginLeft = new ComputedValue<float>(() =>
                {
                    switch (column.HAlign.Value)
                    {
                        case HorizontalAlignment.Left:
                            float marginTop = 0.0f; // Start aligned at the top
                            for (int i = 0; i <_columns.IndexOf(column); i++) // Loop through all rows before this one
                            {
                                if (_columns[i].HAlign.Value == HorizontalAlignment.Left)
                                {
                                    marginTop += _columns[i].Width.Value;
                                }
                            }
                            return marginTop;
                        case HorizontalAlignment.Center:
                            throw new Exception("Columns cannot be centered horizontally.");
                        case HorizontalAlignment.Right:
                        default:
                            return 0.0f;
                    }
                });
                column.MarginRight = new ComputedValue<float>(() =>
                {
                    switch (column.HAlign.Value)
                    {
                        case HorizontalAlignment.Right:
                            float marginBottom = 0.0f; // Start aligned at the top
                            for (int i = 0; i < _columns.IndexOf(column); i++) // Loop through all rows before this one
                            {
                                if (_columns[i].HAlign.Value == HorizontalAlignment.Right)
                                {
                                    marginBottom += _columns[i].Width.Value;
                                }
                            }
                            return marginBottom;
                        case HorizontalAlignment.Center:
                            throw new Exception("Rows cannot be Centered vertically.");
                        case HorizontalAlignment.Left:
                        default:
                            return 0.0f;
                    }
                });
            }

            public void Remove(ColumnWidget row)
            {
                _columns.Remove(row);
            }

            public class ColumnWidget : ContainerWidget
            {
                public WidgetProperty<int> Units
                {
                    get
                    {
                        return Properties.GetProperty<int>("units");
                    }
                    set
                    {
                        Properties.SetProperty("units", value);
                    }
                }

                public ColumnWidget(Engine engine) : base(engine)
                {
                }

                protected override void Initialize(Entity entity)
                {
                    base.Initialize(entity);

                    Properties.SetProperty("units", new FixedValue<int>(CVars.Get<int>("ui_table_units")));

                    VAlign = new FixedValue<VerticalAlignment>(VerticalAlignment.Center);
                    HAlign = new FixedValue<HorizontalAlignment>(HorizontalAlignment.Left);
                }
            }
        }
    }
}
