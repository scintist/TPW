//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2023, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//  by introducing yourself and telling us what you do with this community.
//_____________________________________________________________________________________________________________________________________

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TP.ConcurrentProgramming.BusinessLogic;
using LogicIBall = TP.ConcurrentProgramming.BusinessLogic.IBall;

namespace TP.ConcurrentProgramming.Presentation.Model
{
  internal class ModelBall : IBall, INotifyPropertyChanged
    {
        public ModelBall(double top, double left, double radius, string color, LogicIBall underneathBall)
        {
            _radius = radius;
            Diameter = radius * 2;
            Color = color;

            TopBackingField = top - _radius;
            LeftBackingField = left - _radius;

            underneathBall.NewPositionNotification += NewPositionNotification;
        }

        #region IBall

        public double Top
        {
            get { return TopBackingField; }
            private set
            {
                if (TopBackingField == value)
                    return;
                TopBackingField = value;
                RaisePropertyChanged();
            }
        }

        public double Left
        {
            get { return LeftBackingField; }
            private set
            {
                if (LeftBackingField == value)
                    return;
                LeftBackingField = value;
                RaisePropertyChanged();
            }
        }

        public double Diameter { get; }

        public string Color { get; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged

        #endregion IBall

        #region private

        private double TopBackingField;
        private double LeftBackingField;
        private readonly double _radius;

        private void NewPositionNotification(object sender, IPosition e)
        {
            Top = e.y - _radius;
            Left = e.x - _radius;
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion private

    #region testing instrumentation

    [Conditional("DEBUG")]
    internal void SetLeft(double x)
    { Left = x - _radius; }

    [Conditional("DEBUG")]
    internal void SettTop(double x)
    { Top = x - _radius; }
    #endregion testing instrumentation
  }
}