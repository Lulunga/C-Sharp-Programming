using System;
using System.Collections.Generic;
using System.Linq;

namespace Inheritance.Geometry.Virtual
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract bool ContainsPoint(Vector3 point);
        public abstract RectangularCuboid GetBoundingBox();
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var vector = point - Position;
            var length2 = vector.GetLength2();
            return length2 <= Radius * Radius;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            double length = Radius * 2;
            return new RectangularCuboid(Position, length, length, length);
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var halfWidth = SizeX / 2;
            var halfHeight = SizeY / 2;
            var halfDepth = SizeZ / 2;

            var relativePoint = point - Position;
            return Math.Abs(relativePoint.X) <= halfWidth &&
                   Math.Abs(relativePoint.Y) <= halfHeight &&
                   Math.Abs(relativePoint.Z) <= halfDepth;
        }

        public override RectangularCuboid GetBoundingBox() => this;
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            var relativePoint = point - Position;
            var xyDistance = Math.Sqrt(relativePoint.X * relativePoint.X + relativePoint.Y * relativePoint.Y);
            return xyDistance <= Radius && Math.Abs(relativePoint.Z) <= SizeZ / 2;
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var length = Radius * 2;
            return new RectangularCuboid(Position, length, length, SizeZ);
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override bool ContainsPoint(Vector3 point)
        {
            return Parts.Any(component => component.ContainsPoint(point));
        }

        public override RectangularCuboid GetBoundingBox()
        {
            var (minX, minY, minZ) = (double.MaxValue, double.MaxValue, double.MaxValue);
            var (maxX, maxY, maxZ) = (double.MinValue, double.MinValue, double.MinValue);

            foreach (var component in Parts)
            {
                var boundingBox = component.GetBoundingBox();
                minX = Math.Min(minX, boundingBox.Position.X - boundingBox.SizeX / 2);
                minY = Math.Min(minY, boundingBox.Position.Y - boundingBox.SizeY / 2);
                minZ = Math.Min(minZ, boundingBox.Position.Z - boundingBox.SizeZ / 2);
                maxX = Math.Max(maxX, boundingBox.Position.X + boundingBox.SizeX / 2);
                maxY = Math.Max(maxY, boundingBox.Position.Y + boundingBox.SizeY / 2);
                maxZ = Math.Max(maxZ, boundingBox.Position.Z + boundingBox.SizeZ / 2);
            }

            var (width, height, depth) = (maxX - minX, maxY - minY, maxZ - minZ);

            return new RectangularCuboid(new Vector3((minX + maxX) / 2, (minY + maxY) / 2,
                (minZ + maxZ) / 2), width, height, depth);
        }
    }
}