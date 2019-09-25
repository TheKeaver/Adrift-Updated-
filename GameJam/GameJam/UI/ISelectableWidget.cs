namespace GameJam.UI
{
    public interface ISelectableWidget
    {
        ISelectableWidget above
        {
            get;
            set;
        }
        ISelectableWidget left
        {
            get;
            set;
        }
        ISelectableWidget right
        {
            get;
            set;
        }
        ISelectableWidget below
        {
            get;
            set;
        }
        bool isSelected
        {
            get;
            set;
        }
    }
}
