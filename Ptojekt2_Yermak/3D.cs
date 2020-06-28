using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;
using System.Drawing.Drawing2D;
using System.Windows.Input;


namespace Ptojekt2_Yermak
{
    public class Trojkat : IComparable<Trojkat>    // tworzenie wektorów dla trójkąta 
    {
        public Vector4[] listTrojkat;
        internal float fl;
        public Trojkat(Vector4 a, Vector4 b, Vector4 c)
        {
            listTrojkat = new Vector4[3];
            listTrojkat[0] = a;
            listTrojkat[1] = b;
            listTrojkat[2] = c;
        }

        public int CompareTo(Trojkat other)  // dla algorytmu malarskiego, porównujemy Z-ety 
        {
            float trojkat1 = (listTrojkat[0].Z + listTrojkat[1].Z + listTrojkat[2].Z) / 3.0f;
            float trojkat2 = (other.listTrojkat[0].Z + other.listTrojkat[1].Z + other.listTrojkat[2].Z) / 3.0f;

            return trojkat2.CompareTo(trojkat1);

            throw new NotImplementedException();
        }
    }
    class _3D
    {
        private void MatrixIncrease(Matrix4x4 projection, Trojkat a, Trojkat b)  // zapisuję się do b projekcja a 
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

        Bitmap bitmap;
        PictureBox pictureBox;
        List<Trojkat> szescian;
        Matrix4x4 matrixPr;
        SubsidiaryClass matrixProj = new SubsidiaryClass();

        public _3D(PictureBox image)    // tworzenie figur
        {
            // ładowanie obrazu z Blender  
            this.pictureBox = image;
            myCamera = new Vector4(0, 0, 0, 1);

            Figure figure = new Figure();
            figure.TakeVectors();

            szescian = figure.myFigures;

            // Macierz projekcji 
            float fNear = 0.1f;
            float fFar = 1000.0f;
            float fFov = 90.0f;
            float fAspectRatio =  (float)image.Height / (float)image.Width;
            matrixPr = matrixProj.ProjectionMatrix(fNear, fFar, fFov, fAspectRatio);
        }

        private void TrPr(Trojkat trojkat)  // dla skalowania  
        {
            for (int i = 0; i < 3; i++)
            {
                trojkat.listTrojkat[i].X += 2.0f;
                trojkat.listTrojkat[i].Y += 2.0f;

                trojkat.listTrojkat[i].X *= 0.3f * (float)pictureBox.Width;
                trojkat.listTrojkat[i].Y *= 0.3f * (float)pictureBox.Height;
            }
        }

        private void TrTr(Trojkat trojkat, Trojkat proj) // dla skalowania
        {
            for (int i = 0; i < 3; i++)
            {
                trojkat.listTrojkat[i].Z = proj.listTrojkat[i].Z + 8.0f;
            }
        }

        Vector4 myCamera;
        Vector4 myDirection;
        float theta;
        Matrix4x4 rotZ, rotX;   // dla rotacji po X oraz Z
        SubsidiaryClass subsidiaryClass = new SubsidiaryClass();

        float YawAngle;

        public void Uzytkownik(TimeSpan time)
        {
            
            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            double eTime = time.TotalMilliseconds / 1000;

            theta += 1.0f * (float)eTime; // dla obracania sceny 

            // strzałki do obracania sceny
            if ((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) > 0)   
            {
                myCamera.Y += 0.01f + (float)eTime;
            }
            if ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) > 0)
            {
                myCamera.Y -= 0.01f + (float)eTime;
            }

            if ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) > 0)
            {
                myCamera.X += 0.01f + (float)eTime;
            }
            if ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) > 0)
            {
                myCamera.X -= 0.01f + (float)eTime;
            }

            Vector4 vectorForaward = myDirection * (8.0f * (float)eTime);

            // obracanie ze względu kamery 
            if ((Keyboard.GetKeyStates(Key.W) & KeyStates.Down) > 0)
            {
                myCamera = subsidiaryClass.VectorADD(myCamera, vectorForaward);
            }
            if ((Keyboard.GetKeyStates(Key.S) & KeyStates.Down) > 0)
            {
                myCamera = subsidiaryClass.VectorSUB(myCamera, vectorForaward);
            }

            if ((Keyboard.GetKeyStates(Key.A) & KeyStates.Down) > 0)
            {
                YawAngle += 0.05f + (float)eTime;
            }
            if ((Keyboard.GetKeyStates(Key.D) & KeyStates.Down) > 0)
            {
                YawAngle -= 0.05f + (float)eTime;
            }


            rotZ = subsidiaryClass.RotZ(theta);
            rotX = subsidiaryClass.RotX(theta);

            // camera
            Vector4 vectorUp = new Vector4( 0, 1, 0, 1 );
            Vector4 target = new Vector4(0, 0, 1, 1);
            Matrix4x4 cameraRotY = subsidiaryClass.RotY(YawAngle);
            myDirection = subsidiaryClass.VectorsMult(target, cameraRotY);
            target = subsidiaryClass.VectorADD(myCamera, myDirection);
            Matrix4x4 matrixCamera = subsidiaryClass.PointAtMatrix(myCamera, target, vectorUp);

            //make view matrix from camera
            Matrix4x4 matrixView = subsidiaryClass.MatrixInverse(matrixCamera);

            List<Trojkat> fillTrokat = new List<Trojkat>();

            // rysujemy trójkąty za pomocą macierzy projekcji 
            foreach (Trojkat item in szescian)
            {
                Trojkat trojkatPr, trojkatTr, trojkatRotZ, trojkatRotZX, trojkatViewed;

                trojkatViewed = new Trojkat(new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f));
                trojkatRotZ = new Trojkat(new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f));
                trojkatRotZX = new Trojkat(new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f));
                MatrixIncrease(rotZ, item, trojkatRotZ);
                MatrixIncrease(rotX, trojkatRotZ, trojkatRotZX);

                trojkatPr = new Trojkat(new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f));

                // zumowanie figury
                trojkatTr  = trojkatRotZX;
                TrTr(trojkatTr, trojkatRotZX);

                Vector4 vectorNorm, v1, v2;
                v1 = new Vector4(0, 0, 0, 0);
                v2 = new Vector4(0, 0, 0, 0);

                v1.X = trojkatTr.listTrojkat[1].X - trojkatTr.listTrojkat[0].X;
                v1.Y = trojkatTr.listTrojkat[1].Y - trojkatTr.listTrojkat[0].Y;
                v1.Z = trojkatTr.listTrojkat[1].Z - trojkatTr.listTrojkat[0].Z;

                v2.X = trojkatTr.listTrojkat[2].X - trojkatTr.listTrojkat[1].X;
                v2.Y = trojkatTr.listTrojkat[2].Y - trojkatTr.listTrojkat[1].Y;
                v2.Z = trojkatTr.listTrojkat[2].Z - trojkatTr.listTrojkat[1].Z;

                vectorNorm = subsidiaryClass.VecCrossProduct(v1, v2);

                // normalizacja wektorów
                vectorNorm = subsidiaryClass.VectorNorm(vectorNorm);

                if(vectorNorm.X * (trojkatTr.listTrojkat[0].X - myCamera.X) + 
                   vectorNorm.Y * (trojkatTr.listTrojkat[0].Y - myCamera.Y) +
                   vectorNorm.Z * (trojkatTr.listTrojkat[0].Z - myCamera.Z) < 0.0f)   // bo widzimy tylko ujemne z-ety
                {
                    Vector4 light = new Vector4(0.0f, -1.0f, -1.0f, 1.0f); // ustawienie kierunku światła
                    light = subsidiaryClass.VectorNorm(light);

                    float fl = vectorNorm.X * light.X + vectorNorm.Y * light.Y + vectorNorm.Z * light.Z;

                    MatrixIncrease(matrixView, trojkatTr, trojkatViewed); 

                    int ClippedT = 0;
                    Trojkat[] cliped = new Trojkat[2] { new Trojkat(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 1)), new Trojkat(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 1), new Vector4(0, 0, 0, 1)) }; ;
                    Vector4 p_p = new Vector4(0.0f, 0.0f, 0.1f, 1.0f);
                    Vector4 p_n = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                    ClippedT = subsidiaryClass.TClipAgainstPlane(p_p, p_n, trojkatViewed, ref cliped[0], ref cliped[1]);
                    for (int i = 0; i < ClippedT; i++)
                    {
                        MatrixIncrease(matrixPr, cliped[i], trojkatPr);
                        TrPr(trojkatPr);
                        trojkatPr.fl = fl;
                        fillTrokat.Add(trojkatPr);
                    }
                }
            }

            fillTrokat.Sort();

            foreach (var item in fillTrokat)
            {
                FillUpTrokat(item, bitmap);
            }

            pictureBox.Image = bitmap;
        }

        // Zafarbowanie i cienowanie figur (algorytm malarski)
        public void FillUpTrokat(Trojkat trojkat, Bitmap bitmap)
        {
            float fl = trojkat.fl;
            float R = fl * 255;
            float G = fl * 255;
            float B = fl * 255;

            if (R < 0)
            {
                R = 0;
            }
            if (G < 0)
            {
                G = 0;
            }
            if (B < 0)
            {
                B = 0;
            }

            SolidBrush myBrush = new SolidBrush(Color.FromArgb((int)R, (int)G, (int)B));

            Point pointFirst = new Point((int)trojkat.listTrojkat[0].X, (int)trojkat.listTrojkat[0].Y);
            Point pointSecond = new Point((int)trojkat.listTrojkat[1].X, (int)trojkat.listTrojkat[1].Y);
            Point pointThird = new Point((int)trojkat.listTrojkat[2].X, (int)trojkat.listTrojkat[2].Y);

            Point[] points = { pointFirst, pointSecond, pointThird };

            using (var figure = Graphics.FromImage(bitmap))
            {
                figure.FillPolygon(myBrush, points);
            }
        }
    }
}
