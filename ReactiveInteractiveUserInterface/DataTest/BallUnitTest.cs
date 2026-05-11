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
        public void MoveTestMethod()
        {
            Vector initialPosition = new Vector(10.0, 10.0);
            Vector initialVelocity = new Vector(5.0, -2.0);
            Ball ball = new Ball(initialPosition, initialVelocity);

            IVector currentPosition = new Vector(0.0, 0.0);
            int numberOfCallBackCalled = 0;

            ball.NewPositionNotification += (sender, position) =>
            {
                Assert.IsNotNull(sender);
                currentPosition = position;
                numberOfCallBackCalled++;
            };

            ball.Move();

            Assert.AreEqual(1, numberOfCallBackCalled);
            Assert.AreEqual(15.0, currentPosition.x);
            Assert.AreEqual(8.0, currentPosition.y);
        }
    }
}