namespace TestSkia.ViewModels;

public class MainWindowVM : BaseVM
{
    private bool _useSkia = true;

    public bool UseSkia
    {
        get { return _useSkia; }
        set
        {
            if (value != _useSkia)
            {
                _useSkia = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UseOpenGl));
            }
        }
    }

    public bool UseOpenGl
    {
        get { return !_useSkia; }
        set { ; }
    }
}