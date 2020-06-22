namespace GameJam.UI
{
    public interface ISelectableWidget
    {
        string aboveID
        {
            get;
            set;
        }
        string leftID
        {
            get;
            set;
        }
        string rightID
        {
            get;
            set;
        }
        string belowID
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
