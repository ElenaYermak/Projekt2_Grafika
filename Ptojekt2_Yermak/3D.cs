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
        Matrix4x4 matrix;
        Bitmap bitmap;
        PictureBox pictureBox;

        Vector3 myCamera;

        private void MatrixMnożenia(Matrix4x4 projection, Vector3 a, Vector3 b)
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

            szescian.Add(new Trojkat(new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1)));
            szescian.Add(new Trojkat(new Vector3(0, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 1, 0)));

            szescian.Add(new Trojkat(new Vector3(1, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 0)));
            szescian.Add(new Trojkat(new Vector3(1, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 0)));
        }

        public void Uzytkownik(TimeSpan time)
        {
            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);

            foreach (Trojkat item in szescian)
            {

            }
        }
    }
}
