//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.DataTest
{
    [TestClass]
    public class BallUnitTest
    {
        [TestMethod]
        public async Task BallMovementTest()
        {
            Vector initialPosition = new Vector(10.0, 10.0);
            Vector initialVelocity = new Vector(5.0, -2.0);
            Ball ball = new Ball(initialPosition, initialVelocity, 10.0, 5.0, "Red");

            bool eventFired = false;

            ball.NewPositionNotification += (sender, position) =>
            {
                eventFired = true;
            };

            await Task.Delay(50);

            ball.Dispose();

            Assert.IsTrue(eventFired);
        }
    }
}