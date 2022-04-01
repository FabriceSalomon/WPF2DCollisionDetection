﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DCollisionLibrary.Interfaces
{
    public interface ICollisionManager
    {
        bool IsCollision(IGeometry collisionElement, IGeometry collidable);
    }
}
