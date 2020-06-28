using System;
using System.Numerics;

namespace Ptojekt2_Yermak
{
    class SubsidiaryClass
    {
        public Matrix4x4 ProjectionMatrix(float fNear, float fFar, float fFov, float fAspectRatio)   // rzutowanie perspektywiczne 
        {
            Matrix4x4 matrix = new Matrix4x4(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);

            float fFovRad = 1.0f / (float)Math.Tan(fFov * 0.5f / 180.0f * 3.14159f);

            matrix.M11 = fAspectRatio * fFovRad;
            matrix.M22 = fFovRad;
            matrix.M33 = fFar / (fFar - fNear);
            matrix.M43 = (-fFar * fNear) / (fFar - fNear);
            matrix.M34 = 1.0f;
            matrix.M44 = 0.0f;

            return matrix;
        }

        public Matrix4x4 RotZ(float theta) //Rotacja po Z
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

        public Matrix4x4 RotX(float theta)  //Rotacja po X
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

        public Matrix4x4 RotY(float angle) // //Rotacja po Y
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

        public Vector4 VecCrossProduct(Vector4 vector1, Vector4 vector2) //Iloczyn wektorowy 
        {
            Vector4 vectorCross = new Vector4();
            vectorCross.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            vectorCross.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            vectorCross.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;

            return vectorCross;
        }

        public Vector4 VectorNorm(Vector4 vector) // Normalizacja wektora 
        {
            float length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z); // tworzę wektor długośći
            vector.X /= length;
            vector.Y /= length;
            vector.Z /= length;

            return new Vector4(vector.X, vector.Y, vector.Z, 1);
        }

        public float VectorDot(Vector4 v1, Vector4 v2) // Iloczyn skalarny 
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public Matrix4x4 PointAtMatrix(Vector4 pos, Vector4 target, Vector4 up)
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

        public Matrix4x4 MatrixInverse(Matrix4x4 matrix) // Sprzeczność macierzy
        {
            Matrix4x4 mat = new Matrix4x4();
            mat.M11 = matrix.M11;
            mat.M12 = matrix.M21;
            mat.M13 = matrix.M31;
            mat.M14 = 0.0f;
            mat.M21 = matrix.M12;
            mat.M22 = matrix.M22;
            mat.M23 = matrix.M32;
            mat.M24 = 0.0f;
            mat.M31 = matrix.M13;
            mat.M32 = matrix.M23;
            mat.M33 = matrix.M33;
            mat.M34 = 0.0f;
            mat.M41 = -(matrix.M41 * mat.M11 + matrix.M42 * mat.M21 + matrix.M43 * mat.M31);
            mat.M42 = -(matrix.M41 * mat.M12 + matrix.M42 * mat.M22 + matrix.M43 * mat.M32);
            mat.M43 = -(matrix.M41 * mat.M13 + matrix.M42 * mat.M23 + matrix.M43 * mat.M33);
            mat.M44 = 1.0f;
            return mat;
        }

        public Vector4 VectorADD(Vector4 v1, Vector4 v2)  // Dodawanie wektorów 
        {
            Vector4 vector = new Vector4();
            vector.X = v1.X + v2.X;
            vector.Y = v1.Y + v2.Y;
            vector.Z = v1.Z + v2.Z;
            return vector;
        }

        public Vector4 VectorSUB(Vector4 v1, Vector4 v2)  // Odejmowanie wektorów 
        {
            Vector4 vector = new Vector4();
            vector.X = v1.X - v2.X;
            vector.Y = v1.Y - v2.Y;
            vector.Z = v1.Z - v2.Z;
            return vector;
        }

        public Vector4 VectorsMult(Vector4 v1, Matrix4x4 matrix) // Mnożenie wektora i macierzy
        {
            Vector4 v2 = new Vector4();
            v2.X = (v1.X * matrix.M11) + (v1.Y * matrix.M21) + (v1.Z * matrix.M31) + matrix.M41;
            v2.Y = (v1.X * matrix.M12) + (v1.Y * matrix.M22) + (v1.Z * matrix.M32) + matrix.M42;
            v2.Z = (v1.X * matrix.M13) + (v1.Y * matrix.M23) + (v1.Z * matrix.M33) + matrix.M43;
            v2.W = (v1.X * matrix.M14) + (v1.Y * matrix.M24) + (v1.Z * matrix.M34) + matrix.M44;
            return v2;
        }

        public Vector4 VectorFloatMult(Vector4 v1, float x) // Mnożenie wektora i liczby  
        {
            Vector4 v = new Vector4();
            v.X = v1.X * x;
            v.Y = v1.Y * x;
            v.Z = v1.Z * x;
            return v;
        }
        public Vector4 V_IntersectPlane(Vector4 p_p, Vector4 p_n, Vector4 lineStart, Vector4 lineEnd)
        {
            p_n = VectorNorm(p_n);
            float p_d = -VectorDot(p_n, p_p);
            float ad = VectorDot(lineStart, p_n);
            float bd = VectorDot(lineEnd, p_n);
            float t = (-p_d - ad) / (bd - ad);
            Vector4 lineatStarttoEnd = VectorSUB(lineEnd, lineStart);
            Vector4 lineToInter = VectorFloatMult(lineatStarttoEnd, t);
            return VectorADD(lineStart, lineToInter);
        }

        public int TClipAgainstPlane(Vector4 plane_p, Vector4 plane_n, Trojkat in_tri, ref Trojkat out_tri1, ref Trojkat out_tri2)
        {
            plane_n = VectorNorm(plane_n);

            Vector4[] insidePoints = new Vector4[3];
            Vector4[] outsidePoints = new Vector4[3];
            int nInsidePointCount = 0;
            int noutsidePointsCount = 0;

            float d0 = Distans(in_tri.listTrojkat[0], plane_n, plane_p);
            float d1 = Distans(in_tri.listTrojkat[1], plane_n, plane_p);
            float d2 = Distans(in_tri.listTrojkat[2], plane_n, plane_p);

            if (d0 >= 0) { insidePoints[nInsidePointCount++] = in_tri.listTrojkat[0]; }
            else { outsidePoints[noutsidePointsCount++] = in_tri.listTrojkat[0]; }
            if (d1 >= 0) { insidePoints[nInsidePointCount++] = in_tri.listTrojkat[1]; }
            else { outsidePoints[noutsidePointsCount++] = in_tri.listTrojkat[1]; }
            if (d2 >= 0) { insidePoints[nInsidePointCount++] = in_tri.listTrojkat[2]; }
            else { outsidePoints[noutsidePointsCount++] = in_tri.listTrojkat[2]; }

            if (nInsidePointCount == 0)
            {
                return 0;
            }

            if (nInsidePointCount == 3)
            {
                out_tri1 = in_tri;
                return 1;
            }

            if (nInsidePointCount == 1 && noutsidePointsCount == 2)
            {

                out_tri1.listTrojkat[0] = insidePoints[0];
                out_tri1.listTrojkat[1] = V_IntersectPlane(plane_p, plane_n, insidePoints[0], outsidePoints[0]);
                out_tri1.listTrojkat[2] = V_IntersectPlane(plane_p, plane_n, insidePoints[0], outsidePoints[1]);
                return 1;
            }

            if (nInsidePointCount == 2 && noutsidePointsCount == 1)
            {
                out_tri1.listTrojkat[0] = insidePoints[0];
                out_tri1.listTrojkat[1] = insidePoints[1];
                out_tri1.listTrojkat[2] = V_IntersectPlane(plane_p, plane_n, insidePoints[0], outsidePoints[0]);


                out_tri2.listTrojkat[0] = insidePoints[1];
                out_tri2.listTrojkat[1] = out_tri1.listTrojkat[2];
                out_tri2.listTrojkat[2] = V_IntersectPlane(plane_p, plane_n, insidePoints[1], outsidePoints[0]);

                return 2;
            }
            return 0;
        }

        public float Distans(Vector4 p, Vector4 plane_n, Vector4 plane_p)
        {
            return plane_n.X * p.X + plane_n.Y * p.Y + plane_n.Z * p.Z - VectorDot(plane_n, plane_p);
        }
    }
}
