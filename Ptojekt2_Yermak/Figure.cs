using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using System.Globalization;


namespace Ptojekt2_Yermak
{
    class Figure
    {
        public List<Trojkat> myFigures = new List<Trojkat>();
        List<Vector3> myVectors = new List<Vector3>();
        List<int[]> indexesTrojkat = new List<int[]>();
        public string sciezkaDoObj = @"C:\Users\Alena\Desktop\GitHub\Ptojekt2_Yermak\MyFiguresFromBlender.obj";

        public void TakeVectors()
        {
            string[] linesObj = File.ReadAllLines(sciezkaDoObj);

            foreach (var item in linesObj)
            {
                if (item[0] == 'v' && item[1] == ' ')
                {
                    string[] top = item.ToString().Split(' ');

                    Vector3 a = new Vector3(float.Parse(top[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(top[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(top[3], CultureInfo.InvariantCulture.NumberFormat));
                    myVectors.Add(a);
                }

                if (item[0] == 'f' && item[1] == ' ')
                {
                    string[] pointTrojat = item.Split(' ');

                    int trojkat1, trojkat2, trojkat3;

                    trojkat1 = int.Parse(pointTrojat[1]) - 1;
                    trojkat2 = int.Parse(pointTrojat[2]) - 1;
                    trojkat3 = int.Parse(pointTrojat[3]) - 1;

                    int[] idTrojkat = { trojkat1, trojkat2, trojkat3 };

                    indexesTrojkat.Add(idTrojkat);
                }

            }
            foreach (var oneIndeks in indexesTrojkat)
            {
                int ind1 = oneIndeks[0];
                int ind2 = oneIndeks[1];
                int ind3 = oneIndeks[2];

                Trojkat trojkat = new Trojkat(new Vector4(myVectors[ind1], 0f), new Vector4(myVectors[ind2], 0f), new Vector4(myVectors[ind3], 0f));
                myFigures.Add(trojkat);
            }
        }
    }
}
