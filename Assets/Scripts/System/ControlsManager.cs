namespace MonsterFlow.System
{
    public static class ControlsManager
    {
        private static Controls _inputs;
        public static Controls Inputs => _inputs ?? (_inputs = new Controls());
    }
}