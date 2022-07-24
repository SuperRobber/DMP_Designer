using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Numerics;
using Janus;

public class Shapes_2 : IGenerator {
    public static Random _rand;

    public void Generate(int seed) {
        _rand = new Random(seed);
        int count=9;

        Data.lines.Clear();
        Data.dots.Clear();

        int curveindex = count;

        for (int lineIndex=0;lineIndex<count;lineIndex++) {
            int dotIndex=0;

            List<Vector2> lineDots = new List<Vector2>();
            Vector2 center = new Vector2((float)(lineIndex%3) * 500f - 500f,(float)(lineIndex/3) * 500f - 500f);
            int dotStart = dotIndex;
            for(float r=_rand.NextSingle()*20f;r<360f;r=r+(5f + _rand.NextSingle()*60f)) {
                Quaternion rot = Quaternion.CreateFromAxisAngle(Vector3.UnitZ,r * (MathF.PI/180f));

                Data.dots.Insert(dotIndex, new Dot());
                Data.dots[dotIndex].layer=0;
                Data.dots[dotIndex].size=5.0f;
                Data.dots[dotIndex].color=Veldrid.RgbaFloat.LightGrey;
                Data.dots[dotIndex].position = Vector2.Transform(new Vector2(0,_rand.NextSingle()*160f+100f),rot) + center;
                lineDots.Add(Data.dots[dotIndex].position);
                dotIndex++;
            }
            lineDots.Add(Data.dots[dotStart].position);

            //add one line connecting all dots
            Line l = new Line();
            l.type=lineType.Straight;
            l.lineData = lineDots.ToArray();
            // Data.lines.Add(l);

            //add multiple quadriv bezier lines

            for (int c=0;c<lineDots.Count;c++) {
                int index0 = c;
                int index1 = (c+1)%(lineDots.Count-1);
                int index2 = (c+2)%(lineDots.Count-1);
                Vector2 A = (lineDots[index0] + lineDots[index1])/2.0f; 
                Vector2 B = (lineDots[index1]); 
                Vector2 C = (lineDots[index1] + lineDots[index2])/2.0f; 

                Line ql = new Line();
                ql.type=lineType.QuadraticBezier;
                ql.lineData = new Vector2[] {A,B,C};
                Data.lines.Add(ql);
            }




        }
    }
}