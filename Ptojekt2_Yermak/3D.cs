using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Windows.Forms;

namespace Ptojekt2_Yermak
{
    class _3D
    {
        DrawLine drawLine = new DrawLine();
        Bitmap bitmap;
        PictureBox pictureBox;

        Matrix4x4 rotZ, rotX;

        Vector3 myCamera;

        private void MatrixIncrease(Matrix4x4 projection, Trojkat a, Trojkat b)
        {
            for (int i = 0; i < 3; i++)
            {
                b.listTrojkat[i].X = a.listTrojkat[i].X * projection.M11 + a.listTrojkat[i].Y * projection.M21 + a.listTrojkat[i].Z * projection.M31 + projection.M41;
                b.listTrojkat[i].Y = a.listTrojkat[i].X * projection.M12 + a.listTrojkat[i].Y * projection.M22 + a.listTrojkat[i].Z * projection.M32 + projection.M42;
                b.listTrojkat[i].Z = a.listTrojkat[i].X * projection.M13 + a.listTrojkat[i].Y * projection.M23 + a.listTrojkat[i].Z * projection.M33 + projection.M43;

                float width = a.listTrojkat[i].X * projection.M14 + a.listTrojkat[i].Y * projection.M24 + a.listTrojkat[i].Z * projection.M34 + projection.M44;

                if (width != 0.0f)
                {
                    b.listTrojkat[i].X /= width;
                    b.listTrojkat[i].Y /= width;
                    b.listTrojkat[i].Z /= width;
                }
            }
        }

        protected class Trojkat
        {
            public Vector3[] listTrojkat;
            public Trojkat(Vector3 a, Vector3 b, Vector3 c)
            {
                listTrojkat = new Vector3[3];
                listTrojkat[0] = a;
                listTrojkat[1] = b;
                listTrojkat[2] = c;
            }
        }

        List<Trojkat> szescian;
        Matrix4x4 matrixPr;
        public _3D(PictureBox image)
        {
            this.pictureBox = image;
            myCamera = new Vector3(0, 0, 0);
            szescian = new List<Trojkat>();

            szescian.Add(new Trojkat(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0)));
            szescian.Add(new Trojkat(new Vector3(0, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0)));

            szescian.Add(new Trojkat(new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1)));
            szescian.Add(new Trojkat(new Vector3(1, 0, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1)));

            szescian.Add(new Trojkat(new Vector3(1, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 1, 1)));
            szescian.Add(new Trojkat(new Vector3(1, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1)));

            szescian.Add(new Trojkat(new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0)));
            szescian.Add(new Trojkat(new Vector3(0, 0, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0)));

            szescian.Add(new Trojkat(new Vector3(0f, 1f, 0f), new Vector3(0, 1, 1), new Vector3(1, 1, 1)));
            szescian.Add(new Trojkat(new Vector3(0f, 1f, 0f), new Vector3(1, 1, 1), new Vector3(1, 1, 0)));

            szescian.Add(new Trojkat(new Vector3(1f, 0f, 1f), new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, 0f)));
            szescian.Add(new Trojkat(new Vector3(1f, 0f, 1f), new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f)));

            // Projection 
            float fNear = 0.1f;
            float fFar = 1000.0f;
            float fFov = 90.0f;
            float fAspectRatio =  (float)image.Height / (float)image.Width;
            float fFovRad = 1.0f / (float) Math.Tan(fFov * 0.5f / 180.0f * 3.14159f);

            matrixPr.M11 = fAspectRatio * fFovRad;
            matrixPr.M22 = fFovRad;
            matrixPr.M33 = fFar / (fFar - fNear);
            matrixPr.M43 = (-fFar - fNear) / (fFar - fNear);
            matrixPr.M34 = 1.0f;
            matrixPr.M44 = 0.0f;
                      
        }

        private void TrPr(Trojkat trojkat)
        {
            for (int i = 0; i < 3; i++)
            {
                trojkat.listTrojkat[i].X += 1.0f;
                trojkat.listTrojkat[i].Y += 1.0f;

                trojkat.listTrojkat[i].X *= 0.3f * (float)pictureBox.Width;
                trojkat.listTrojkat[i].Y *= 0.3f * (float)pictureBox.Height;
            }
        }

        private void TrTr(Trojkat trojkat, Trojkat proj)
        {
            for (int i = 0; i < 3; i++)
            {
                trojkat.listTrojkat[i].Z += proj.listTrojkat[i].Z + 4.0f;
            }
        }
        float theta;
        public void Uzytkownik(TimeSpan time)
        {
            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);

            double eTime = time.TotalMilliseconds / 1000;
           
           
            theta += 1.0f * (float)eTime;

            // rotZ
            rotZ.M11 = (float)Math.Cos(theta);
            rotZ.M12 = (float)Math.Sin(theta);
            rotZ.M21 = -(float)Math.Sin(theta);
            rotZ.M22 = (float)Math.Cos(theta);
            rotZ.M33 = 1;
            rotZ.M44 = 1;

            // rotX
            rotX.M11 = 1;
            rotX.M22 = (float)Math.Cos(theta * 0.5f);
            rotX.M23 = (float)Math.Sin(theta * 0.5f);
            rotX.M32 = -(float)Math.Sin(theta * 0.5f);
            rotX.M33 = (float)Math.Cos(theta * 0.5f);
            rotX.M44 = 1;

            foreach (Trojkat item in szescian)
            {
                Trojkat trojkatPr, trojkatTr, trojkatRotZ, trojkatRotZX;

                trojkatRotZ = new Trojkat(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
                trojkatRotZX = new Trojkat(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
                MatrixIncrease(rotZ, item, trojkatRotZ);
                MatrixIncrease(rotX, trojkatRotZ, trojkatRotZX);


                trojkatPr = new Trojkat(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));

                trojkatTr  = trojkatRotZX;
                TrTr(trojkatTr, trojkatRotZX);

                MatrixIncrease(matrixPr, trojkatTr, trojkatPr);

                TrPr(trojkatPr);

                drawLine.AlgorytmPrzyrostowy(bitmap, (int)trojkatPr.listTrojkat[0].X, (int)trojkatPr.listTrojkat[0].Y, (int)trojkatPr.listTrojkat[1].X, (int)trojkatPr.listTrojkat[1].Y);
                drawLine.AlgorytmPrzyrostowy(bitmap, (int)trojkatPr.listTrojkat[1].X, (int)trojkatPr.listTrojkat[1].Y, (int)trojkatPr.listTrojkat[2].X, (int)trojkatPr.listTrojkat[2].Y);
                drawLine.AlgorytmPrzyrostowy(bitmap, (int)trojkatPr.listTrojkat[2].X, (int)trojkatPr.listTrojkat[2].Y, (int)trojkatPr.listTrojkat[0].X, (int)trojkatPr.listTrojkat[0].Y);


                pictureBox.Image = bitmap;
                
            }
        }
    }
}
