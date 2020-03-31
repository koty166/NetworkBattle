using System;
using System.Collections.Generic;
using Events;
using System.Drawing;
using ObjectClasses;


namespace Network_Battle
{
    sealed internal class PersonDrawTickImage : ObjectForDraw
    {
        int _Stadia = 0, _AnimLenght , Pointer = 0;
       internal Person _Person;
        Image Bit;
        Image[] Anims;

        public PersonDrawTickImage()
        { }

        internal PersonDrawTickImage(Person _p, int _AnimL, ref Image[] Ims, Image Bitmap , int _Pointer = 0) : base(Bitmap)
        {
            _AnimLenght = _AnimL;
            Anims = Ims;
            _Person = _p;
            Bit = Bitmap;
            Pointer = _Pointer;
        }

        internal void InvokeEventForAddToDrawList (EventsClass.AddToDrawList AddEvent) =>
            AddEvent?.Invoke(_Person, -1, ref Anims, Bit, Pointer);

        override internal void Draw(bool IsNeedToDraw )
        {
            _Person.X += _Person.XSpeed;
            _Person.Y += _Person.YSpeed;
            
            if (_Stadia == _AnimLenght)
            {
                IsNeedToDestroy = true;
                return;
            }

            g.DrawImage(Anims[Pointer + _Stadia], _Person.X, _Person.Y);
            if (_AnimLenght == -1)
                return;

            _Stadia++;
            if (!IsNeedToDraw) _Stadia--;
        }

       public bool Equals(Person _p) => _Person.ID == _p.ID;
    }
}
