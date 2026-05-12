//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data
{
    internal class Ball : IBall
    {
        private bool _isMoving = true;

        internal Ball(Vector initialPosition, Vector initialVelocity, double mass, double radius)
        {
            Position = initialPosition;
            Velocity = initialVelocity;
            Mass = mass;
            Radius = radius;

            Task.Run(MoveLoop);
        }

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity { get; set; }
        public IVector Position { get; private set; }
        public double Mass { get; }
        public double Radius { get; }

        private async Task MoveLoop()
        {
            while (_isMoving)
            {
                Position = new Vector(Position.x + Velocity.x, Position.y + Velocity.y);
                NewPositionNotification?.Invoke(this, Position);
                await Task.Delay(16);
            }
        }

        public void Dispose()
        {
            _isMoving = false;
        }
    }
}