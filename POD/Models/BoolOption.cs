namespace POD.Models
{
    public class BoolOption : BaseModel
    {
        // Constructors
        public BoolOption(string name, bool isMarked = false)
        {
            Name = name;
            IsMarked = isMarked;
        }

        // Databound Properties
        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetAndNotify(ref _Name, value);
        }

        private bool _IsMarked;
        public bool IsMarked
        {
            get => _IsMarked;
            set => SetAndNotify(ref _IsMarked, value);
        }

    }
}
