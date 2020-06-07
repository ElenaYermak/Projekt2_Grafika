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
        
        Bitmap bitmap;
        PictureBox pictureBox;

        Vector3 myCamera;

        private void MatrixIncrease(Matrix4x4 projection, Vector3 a, Vector3 b)
        {
            b.X = a.X * projection.M11 + a.Y * projection.M21 + a.Z * projection.M31 + projection.M41;
            b.Y = a.X * projection.M12 + a.Y * projection.M22 + a.Z * projection.M32 + projection.M42;
            b.Z = a.X * projection.M13 + a.Y * projection.M23 + a.Z * projection.M33 + projection.M43;

            float width = a.X * projection.M14 + a.Y * projection.M24 + a.Z * projection.M34 + projection.M44;

            if (width != 0.0f)
            {
                b.X /= width;
                b.Y /= width;
                b.Z /= width;
            }
        }

        protected class Trojkat
        {
            List<Vector3> listTrojkat;
            public Trojkat(Vector3 a, Vector3 b, Vector3 c)
            {
                listTrojkat = new List<Vector3>();
                listTrojkat.Add(a);
                listTrojkat.Add(b);
                listTrojkat.Add(c);
            }
        }

        List<Trojkat> szescian;
        Matrix4x4 matrixPr;
        public _3D(PictureBox image)
        {
            this.pictureBox = image;
            myCamera = new Vector3(0, 0, 0);
            szescian = new List<Trojkat>();

            szescian.Add(new Trojkat(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(1f, 1f, 0f)));
            szescian.Add(new Trojkat(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 0f), new Vector3(1f, 0f, 0f)));

            szescian.Add(new Trojkat(new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 0f), new Vector3(1f, 1f, 1f)));
            szescian.Add(new Trojkat(new Vector3(1f, 0f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 0f, 1f)));

            szescian.Add(new Trojkat(new Vector3(1f, 0f, 1f), new Vector3(1f, 1f, 1f), new Vector3(0f, 1f, 1f)));
            szescian.Add(new Trojkat(new Vector3(1f, 0f, 1f), new Vector3(0f, 1f, 1f), new Vector3(0f, 0f, 1f)));

            szescian.Add(new Trojkat(new Vector3(0f, 0f, 1f), new Vector3(0f, 1f, 1f), new Vector3(0f, 1f, 0f)));
            szescian.Add(new Trojkat(new Vector3(0f, 0f, 1f), new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 0f)));

            szescian.Add(new Trojkat(new Vector3(0f, 1f, 0f), new Vector3(0f, 1f, 1f), new Vector3(1f, 1f, 1f)));
            szescian.Add(new Trojkat(new Vector3(0f, 1f, 0f), new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 0f)));

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

        public void Uzytkownik(TimeSpan time)
        {
            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);

            foreach (Trojkat item in szescian)
            {
                Trojkat trojkatPr;
                MatrixIncrease(matrixPr, item[0], trojkatPr[0]);
                MatrixIncrease(matrixPr, item[1], trojkatPr[1]);
                MatrixIncrease(matrixPr, item[2], trojkatPr[2]);
                

            }
        }
    }
}
