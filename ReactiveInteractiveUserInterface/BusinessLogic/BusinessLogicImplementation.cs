//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System.Diagnostics;
using TP.ConcurrentProgramming.Data;
using UnderneathLayerAPI = TP.ConcurrentProgramming.Data.DataAbstractAPI;

namespace TP.ConcurrentProgramming.BusinessLogic
{
  internal class BusinessLogicImplementation : BusinessLogicAbstractAPI
  {
    #region ctor

    public BusinessLogicImplementation() : this(null)
    { }

    internal BusinessLogicImplementation(UnderneathLayerAPI? underneathLayer)
    {
      layerBellow = underneathLayer == null ? UnderneathLayerAPI.GetDataLayer() : underneathLayer;
    }

    #endregion ctor

    #region BusinessLogicAbstractAPI

    public override void Dispose()
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
      layerBellow.Dispose();
      Disposed = true;
    }

    private readonly object _collisionLock = new object();
        public override void Start(int numberOfBalls, Action<IPosition, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));

            layerBellow.Start(numberOfBalls, (startingPosition, databall) =>
            {
                databall.NewPositionNotification += CheckCollisions;
                upperLayerHandler(new Position(startingPosition.x, startingPosition.y), new Ball(databall));
            });
        }

        private void CheckCollisions(object? sender, IVector currentPos)
        {
            if (sender is not TP.ConcurrentProgramming.Data.IBall currentBall) return;

            double boardSize = 400;

            lock (_collisionLock)
            {
                if (currentBall.Position.x - currentBall.Radius <= 0 || currentBall.Position.x + currentBall.Radius >= boardSize)
                {
                    currentBall.Velocity = new LogicVector(-currentBall.Velocity.x, currentBall.Velocity.y);
                }
                if (currentBall.Position.y - currentBall.Radius <= 0 || currentBall.Position.y + currentBall.Radius >= boardSize)
                {
                    currentBall.Velocity = new LogicVector(currentBall.Velocity.x, -currentBall.Velocity.y);
                }

                var allBalls = layerBellow.GetBalls().ToList();
                foreach (var otherBall in allBalls)
                {
                    if (otherBall == currentBall) continue;

                    double dx = currentBall.Position.x - otherBall.Position.x;
                    double dy = currentBall.Position.y - otherBall.Position.y;
                    double distance = Math.Sqrt(dx * dx + dy * dy);

                    if (distance <= (currentBall.Radius + otherBall.Radius))
                    {
                        double vxDiff = currentBall.Velocity.x - otherBall.Velocity.x;
                        double vyDiff = currentBall.Velocity.y - otherBall.Velocity.y;
                        double dotProduct = vxDiff * dx + vyDiff * dy;

                        if (dotProduct < 0)
                        {
                            ResolveCollision(currentBall, otherBall, dx, dy, distance);
                        }
                    }
                }
            }
        }

        private void ResolveCollision(TP.ConcurrentProgramming.Data.IBall b1, TP.ConcurrentProgramming.Data.IBall b2, double dx, double dy, double distance)
        {
            double nx = dx / distance;
            double ny = dy / distance;

            double tx = -ny;
            double ty = nx;

            double dpNorm1 = b1.Velocity.x * nx + b1.Velocity.y * ny;
            double dpNorm2 = b2.Velocity.x * nx + b2.Velocity.y * ny;
            double dpTan1 = b1.Velocity.x * tx + b1.Velocity.y * ty;
            double dpTan2 = b2.Velocity.x * tx + b2.Velocity.y * ty;

            double m1 = (dpNorm1 * (b1.Mass - b2.Mass) + 2.0 * b2.Mass * dpNorm2) / (b1.Mass + b2.Mass);
            double m2 = (dpNorm2 * (b2.Mass - b1.Mass) + 2.0 * b1.Mass * dpNorm1) / (b1.Mass + b2.Mass);

            b1.Velocity = new LogicVector(tx * dpTan1 + nx * m1, ty * dpTan1 + ny * m1);
            b2.Velocity = new LogicVector(tx * dpTan2 + nx * m2, ty * dpTan2 + ny * m2);
        }

        private class LogicVector : IVector
        {
            public double x { get; init; }
            public double y { get; init; }
            public LogicVector(double x, double y) { this.x = x; this.y = y; }
        }

        #endregion BusinessLogicAbstractAPI

        #region private

        private bool Disposed = false;

    private readonly UnderneathLayerAPI layerBellow;

    #endregion private

    #region TestingInfrastructure

    [Conditional("DEBUG")]
    internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
    {
      returnInstanceDisposed(Disposed);
    }

    #endregion TestingInfrastructure
  }
}