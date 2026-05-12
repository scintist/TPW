//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using TP.ConcurrentProgramming.BusinessLogic;

namespace TP.ConcurrentProgramming.Presentation.Model.Test
{
  [TestClass]
  public class ModelBallUnitTest
  {
    [TestMethod]
        public void ConstructorTestMethod()
        {
            BusinessLogicIBallFixture fixture = new BusinessLogicIBallFixture();

            ModelBall ball = new ModelBall(0.0, 0.0, fixture.Radius,fixture);

            Assert.AreEqual<double>(-15.0, ball.Top);
            Assert.AreEqual<double>(-15.0, ball.Left);
        }

        [TestMethod]
        public void PositionChangeNotificationTestMethod()
        {
            int notificationCounter = 0;
            BusinessLogicIBallFixture fixture = new BusinessLogicIBallFixture();

            ModelBall ball = new ModelBall(0.0, 0.0, fixture.Radius,fixture);

            ball.PropertyChanged += (sender, args) => notificationCounter++;
            Assert.AreEqual(0, notificationCounter);

            ball.SetLeft(1.0);
            Assert.AreEqual<int>(1, notificationCounter);
            Assert.AreEqual<double>(-14.0, ball.Left);
            Assert.AreEqual<double>(-15.0, ball.Top);

            ball.SettTop(1.0);
            Assert.AreEqual(2, notificationCounter);
            Assert.AreEqual<double>(-14.0, ball.Left);
            Assert.AreEqual<double>(-14.0, ball.Top);
        }

        #region testing instrumentation

        private class BusinessLogicIBallFixture : BusinessLogic.IBall
    {
      public event EventHandler<IPosition>? NewPositionNotification;
            public double Radius { get; } = 15.0;
            public string Color { get; } = "White";
            public void Dispose()
      {
        throw new NotImplementedException();
      }
    }

    #endregion testing instrumentation
  }
}