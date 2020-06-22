using System;
using System.Numerics;

namespace Ptojekt2_Yermak
{
    class SubsidiaryClass
    {
        public Matrix4x4 ProjectionMatrix(float fNear, float fFar, float fFov, float fAspectRatio)
        {
            Matrix4x4 matrix = new Matrix4x4(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);

            //float fNear = 0.1f;
            //float fFar = 1000.0f;
            //float fFov = 90.0f;
            //float fAspectRatio = (float)image.Height / (float)image.Width;
            float fFovRad = 1.0f / (float)Math.Tan(fFov * 0.5f / 180.0f * 3.14159f);

            matrix.M11 = fAspectRatio * fFovRad;
            matrix.M22 = fFovRad;
            matrix.M33 = fFar / (fFar - fNear);
            matrix.M43 = (-fFar * fNear) / (fFar - fNear);
            matrix.M34 = 1.0f;
            matrix.M44 = 0.0f;

            return matrix;
        }

        public Matrix4x4 RotZ(float theta)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.M11 = (float)Math.Cos(theta);
            matrix.M12 = (float)Math.Sin(theta);
            matrix.M21 = -(float)Math.Sin(theta);
            matrix.M22 = (float)Math.Cos(theta);
            matrix.M33 = 1;
            matrix.M44 = 1;

            return matrix;
        }

        public Matrix4x4 RotX(float theta)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.M11 = 1;
            matrix.M22 = (float)Math.Cos(theta * 0.5f);
            matrix.M23 = (float)Math.Sin(theta * 0.5f);
            matrix.M32 = -(float)Math.Sin(theta * 0.5f);
            matrix.M33 = (float)Math.Cos(theta * 0.5f);
            matrix.M44 = 1;

            return matrix;
        }

        public Matrix4x4 RotY(float angle) // change 
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.M11 = (float)Math.Cos(angle);
            matrix.M13 = (float)Math.Sin(angle);
            matrix.M31 = -(float)Math.Sin(angle);
            matrix.M22 = 1.0f;
            matrix.M33 = (float)Math.Cos(angle); ;
            matrix.M44 = 1.0f;
            return matrix;
        }

        public Vector4 VecCrossProduct(Vector4 vector1, Vector4 vector2)
        {
            Vector4 vectorCross = new Vector4();
            vectorCross.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            vectorCross.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            vectorCross.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;

            return vectorCross;
        }

        public Vector4 VectorNorm(Vector4 vector)
        {
            float length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z); // tworzę wektor długośći
            vector.X /= length;
            vector.Y /= length;
            vector.Z /= length;
            return new Vector4(vector.X, vector.Y, vector.Z, 1);
        }

        public float VectorDot(Vector4 v1, Vector4 v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public Matrix4x4 PointAt(Vector4 pos, Vector4 target, Vector4 up)
        {
            Vector4 newForward = target - pos;
            newForward = VectorNorm(newForward);

            Vector4 a = newForward * VectorDot(up, newForward);
            Vector4 newUp = up - a;
            newUp = VectorNorm(newUp);

            Vector4 newRight = VecCrossProduct(newUp, newForward);

            Matrix4x4 matrix;

            matrix.M11 = newRight.X;
            matrix.M12 = newRight.Y;
            matrix.M13 = newRight.Y;

            matrix.M11 = newRight.X;
            matrix.M12 = newRight.Y;
            matrix.M13 = newRight.Z;
            matrix.M14 = 0.0f;
            matrix.M21 = newUp.X;
            matrix.M22 = newUp.Y;
            matrix.M23 = newUp.Z;
            matrix.M24 = 0.0f;
            matrix.M31 = newForward.X;
            matrix.M32 = newForward.Y;
            matrix.M33 = newForward.Z;
            matrix.M34 = 0.0f;
            matrix.M41 = pos.X;
            matrix.M42 = pos.Y;
            matrix.M43 = pos.Z;
            matrix.M44 = 1.0f;
            return matrix;
        }

        public Matrix4x4 MatrixQuickInverse(Matrix4x4 m)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.M11 = m.M11;
            matrix.M12 = m.M21;
            matrix.M13 = m.M31;
            matrix.M14 = 0.0f;
            matrix.M21 = m.M12;
            matrix.M22 = m.M22;
            matrix.M23 = m.M32;
            matrix.M24 = 0.0f;
            matrix.M31 = m.M13;
            matrix.M32 = m.M23;
            matrix.M33 = m.M33;
            matrix.M34 = 0.0f;
            matrix.M41 = -(m.M41 * matrix.M11 + m.M42 * matrix.M21 + m.M43 * matrix.M31);
            matrix.M42 = -(m.M41 * matrix.M12 + m.M42 * matrix.M22 + m.M43 * matrix.M32);
            matrix.M43 = -(m.M41 * matrix.M13 + m.M42 * matrix.M23 + m.M43 * matrix.M33);
            matrix.M44 = 1.0f;
            return matrix;
        }

        public Vector4 VectorADD(Vector4 v1, Vector4 v2)
        {
            Vector4 vector = new Vector4();
            vector.X = v1.X + v2.X;
            vector.Y = v1.Y + v2.Y;
            vector.Z = v1.Z + v2.Z;
            return vector;
        }

        public Vector4 VectorSUB(Vector4 v1, Vector4 v2)
        {
            Vector4 vector = new Vector4();
            vector.X = v1.X - v2.X;
            vector.Y = v1.Y - v2.Y;
            vector.Z = v1.Z - v2.Z;
            return vector;
        }

        public Vector4 VectorsMult(Vector4 v1, Matrix4x4 matrix) 
        {
            Vector4 v2 = new Vector4();
            v2.X = (v1.X * matrix.M11) + (v1.Y * matrix.M21) + (v1.Z * matrix.M31) + matrix.M41;
            v2.Y = (v1.X * matrix.M12) + (v1.Y * matrix.M22) + (v1.Z * matrix.M32) + matrix.M42;
            v2.Z = (v1.X * matrix.M13) + (v1.Y * matrix.M23) + (v1.Z * matrix.M33) + matrix.M43;
            v2.W = (v1.X * matrix.M14) + (v1.Y * matrix.M24) + (v1.Z * matrix.M34) + matrix.M44;

            return v2;

        }
    }
}
