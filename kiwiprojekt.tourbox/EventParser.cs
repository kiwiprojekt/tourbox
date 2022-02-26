namespace kiwiprojekt.tourbox
{
    internal static class EventParser
    {
        private static TourBoxEvent Click(byte code, params TourBoxKey?[] keys)
        {
            var pressed = (code >> 7 & 1) == 0;
            var action = pressed ? ActionType.Click : ActionType.ClickReleased;
            return new TourBoxEvent(code, action, keys);
        }
        private static TourBoxEvent DoubleClick(byte code, params TourBoxKey?[] keys)
        {
            var pressed = (code >> 7 & 1) == 0;
            var action = pressed ? ActionType.DoubleClick : ActionType.DoubleClickReleased;
            return new TourBoxEvent(code, action, keys);
        }

        private static TourBoxEvent Axis(byte code, int key, TourBoxKey? modifier = null)
        {
            return key switch
            {
                0 => Click(code, TourBoxKey.Up, modifier),
                1 => Click(code, TourBoxKey.Down, modifier),
                2 => Click(code, TourBoxKey.Left, modifier),
                3 => Click(code, TourBoxKey.Right, modifier),
                _ => throw new NotSupportedException(),
            };
        }

        private static TourBoxEvent? AxisScroll(byte code, int key, TourBoxKey scroll, ActionType type, TourBoxKey? modifier = null)
        {
            var actionFinished = (code >> 7 & 1) == 1;
            if (actionFinished) return null;

            return key switch
            {
                0 => new TourBoxEvent(code, type, scroll, TourBoxKey.Up, modifier),
                1 => new TourBoxEvent(code, type, scroll, TourBoxKey.Down, modifier),
                2 => new TourBoxEvent(code, type, scroll, TourBoxKey.Left, modifier),
                3 => new TourBoxEvent(code, type, scroll, TourBoxKey.Right, modifier),
                _ => throw new NotSupportedException(),
            };
        }

        private static TourBoxEvent? Scroll(byte code, ActionType type, TourBoxKey key, int modifier = 0)
        {

            var actionFinished = (code >> 7 & 1) == 1;
            if (actionFinished) return null;

            return modifier switch
            {
                0 => new TourBoxEvent(code, type, key),
                1 => new TourBoxEvent(code, type, key, TourBoxKey.Tall),
                2 => new TourBoxEvent(code, type, key, TourBoxKey.Short),
                3 => new TourBoxEvent(code, type, key, TourBoxKey.Top),
                4 => new TourBoxEvent(code, type, key, TourBoxKey.Side),
                _ => throw new NotSupportedException(),
            };
        }

        internal static TourBoxEvent? Parse(byte code)
        {
            var action = code & ~(1 << 7);

            return action switch
            {
                0 => Click(code, TourBoxKey.Tall),
                1 => Click(code, TourBoxKey.Side),
                2 => Click(code, TourBoxKey.Top),
                3 => Click(code, TourBoxKey.Short),

                4 => Scroll(code, ActionType.Decreased, TourBoxKey.Knob),
                >= 5 and <= 8 => Scroll(code, ActionType.Increased, TourBoxKey.Knob, action - 4),

                9 => Scroll(code, ActionType.Decreased, TourBoxKey.Scroll),
                10 => Click(code, TourBoxKey.Scroll),
                >= 11 and <= 14 => Scroll(code, ActionType.Decreased, TourBoxKey.Scroll, action - 11),

                15 => Scroll(code, ActionType.Increased, TourBoxKey.Dial),

                >= 16 and <= 19 => Axis(code, action - 16),
                >= 20 and <= 23 => Axis(code, action - 20, TourBoxKey.Side),

                24 => DoubleClick(code, TourBoxKey.Tall),
                25 => Click(code, TourBoxKey.Tall, TourBoxKey.Top),
                26 => Click(code, TourBoxKey.Tall, TourBoxKey.Short),
                27 => Click(code, TourBoxKey.Tall, TourBoxKey.Side),
                28 => DoubleClick(code, TourBoxKey.Short),
                29 => DoubleClick(code, TourBoxKey.Short, TourBoxKey.Top),
                30 => DoubleClick(code, TourBoxKey.Short, TourBoxKey.Side),
                31 => DoubleClick(code, TourBoxKey.Top),
                32 => Click(code, TourBoxKey.Side, TourBoxKey.Top),
                33 => DoubleClick(code, TourBoxKey.Side),
                34 => Click(code, TourBoxKey.C1),
                35 => Click(code, TourBoxKey.C2),
                36 => Click(code, TourBoxKey.C1, TourBoxKey.Tall),
                37 => Click(code, TourBoxKey.C2, TourBoxKey.Tall),

                >= 38 and <= 41 => AxisScroll(code, action - 38, TourBoxKey.Scroll, ActionType.Increased),
                >= 43 and <= 46 => Axis(code, action - 43, TourBoxKey.Top),
                >= 47 and <= 50 => AxisScroll(code, action - 47, TourBoxKey.Scroll, ActionType.Increased, TourBoxKey.Top),
                >= 51 and <= 54 => AxisScroll(code, action - 51, TourBoxKey.Scroll, ActionType.Increased, TourBoxKey.Side),

                57 => Click(code, TourBoxKey.C1, TourBoxKey.Short),
                58 => Click(code, TourBoxKey.C2, TourBoxKey.Short),

                42 => Click(code, TourBoxKey.Tour),

                68 => Scroll(code, ActionType.Increased, TourBoxKey.Knob),
                >= 69 and <= 72 => Scroll(code, ActionType.Decreased, TourBoxKey.Knob, action - 68),

                73 => Scroll(code, ActionType.Increased, TourBoxKey.Scroll),
                >= 75 and <= 78 => Scroll(code, ActionType.Increased, TourBoxKey.Scroll, action - 75),

                79 => Scroll(code, ActionType.Decreased, TourBoxKey.Dial),

                >= 102 and <= 105 => AxisScroll(code, action - 102, TourBoxKey.Scroll, ActionType.Decreased),
                >= 111 and <= 114 => AxisScroll(code, action - 111, TourBoxKey.Scroll, ActionType.Decreased, TourBoxKey.Top),
                >= 115 and <= 119 => AxisScroll(code, action - 115, TourBoxKey.Scroll, ActionType.Decreased, TourBoxKey.Side),

                _ => new TourBoxEvent(code, ActionType.Unknown)
            };
        }
    }
}