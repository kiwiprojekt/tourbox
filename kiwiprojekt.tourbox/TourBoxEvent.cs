namespace kiwiprojekt.tourbox
{
    public class TourBoxEvent
    {
        internal TourBoxEvent(byte code, ActionType action, params TourBoxKey?[] keys)
        {
            Keys = keys.Where(k => k.HasValue).Select(k => k.Value).ToArray();
            Action = action;
            Code = code;
        }

        public TourBoxKey[] Keys { get; }
        public ActionType Action { get; }
        public byte Code { get; }

        public bool Is(ActionType action, params TourBoxKey[] keys)
        {
            return

                (action == Action)
                && Keys.Length == keys.Length
                && Keys.All(k => keys.Contains(k));
        }

        public override string ToString() => $"{Code} - {string.Join("+", Keys.Select(k => k.ToString()))} {Action}";
    }
}