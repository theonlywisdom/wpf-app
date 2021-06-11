namespace FriendOrganizer.Model
{
    public class LookupItem
    {
        public string DisplayMember { get; set; }

        public int Id { get; set; }
    }

    public class NullLookupItem : LookupItem
    {
        public new int? Id { get { return null; } }
    }
}
