using System;

using System.Threading;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        VezirProblem problem = new VezirProblem();
        VezirProblemThreadli problem2 = new VezirProblemThreadli();
        Console.Write("Çözümlenmesini istediğiniz n sayısını giriniz : ");
        byte n = Convert.ToByte(Console.ReadLine());
        problem.VezirCozumle(n);
        problem2.VezirCozumle(n);
        Console.WriteLine($"Problemi çözümlemek için geçen süre : {problem.gecenSure().ElapsedMilliseconds} ms");
        Console.WriteLine($"Thread ile Problemi çözümlemek için geçen süre : {problem2.gecenSure().ElapsedMilliseconds} ms");
        Console.ReadLine();
    }
    class VezirProblem
    {
        private Stopwatch sw = new Stopwatch();
        private int[] vezirKonumlari;
        private int cozumSayisi;

        public void VezirCozumle(int n)
        {
            sw.Start();
            vezirKonumlari = new int[n];
            cozumSayisi = 0;
            VezirYerlestir(0, n);
            Console.WriteLine($"Toplam çözüm sayısı: {cozumSayisi}");
        }
        private void VezirYerlestir(int satir, int n)
        {
            if (satir == n)
            {
                cozumSayisi++;
                CozumuYazdir(n);
            }
            for (int sutun = 0; sutun < n; sutun++)
            {
                if (GuvendeMi(satir, sutun))
                {
                    vezirKonumlari[satir] = sutun;
                    VezirYerlestir(satir + 1, n);
                }
            }
        }
        private bool GuvendeMi(int satir, int sutun)
        {
            for (int i = 0; i < satir; i++)
            {
                if (vezirKonumlari[i] == sutun ||
                    vezirKonumlari[i] - i == sutun - satir ||
                    vezirKonumlari[i] + i == sutun + satir)
                    return false;
            }
            return true;
        }
        private void CozumuYazdir(int n)
        {
            //Console.WriteLine($"Çözüm {cozumSayisi}:");
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < n; j++)
            //    {
            //        if (vezirKonumlari[i] == j)
            //        {
            //            Console.Write("V ");
            //        }
            //        else
            //        {
            //            Console.Write(". ");
            //        }
            //    }
            //    Console.WriteLine();
            //}
            //Console.WriteLine();
        }
        public Stopwatch gecenSure()
        {
            return sw;
        }
    }
    class VezirProblemThreadli
    {
        private Stopwatch sw = new Stopwatch();
        private int[] vezirKonumlari;
        private int cozumSayisi;
        private object lockObject = new object();

        public void VezirCozumle(int n)
        {
            sw.Start();
            vezirKonumlari = new int[n];
            cozumSayisi = 0;
            Thread[] threads = new Thread[n];

            for (int i = 0; i < n; i++)
            {
                int satir = i;
                threads[i] = new Thread(() => VezirYerlestir(satir, n));
                threads[i].Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
        private void VezirYerlestir(int satir, int n)
        {
            for (int sutun = 0; sutun < n; sutun++)
            {
                if (GuvendeMi(satir, sutun))
                {
                    vezirKonumlari[satir] = sutun;
                    if (satir == n - 1)
                    {
                        lock (lockObject)
                        {
                            cozumSayisi++;
                        }
                    }
                    else
                    {
                        VezirYerlestir(satir + 1, n);
                    }
                }
            }
        }
        private bool GuvendeMi(int satir, int sutun)
        {
            for (int i = 0; i < satir; i++)
            {
                if (vezirKonumlari[i] == sutun ||
                    vezirKonumlari[i] - i == sutun - satir ||
                    vezirKonumlari[i] + i == sutun + satir)
                {
                    return false;
                }
            }
            return true;
        }
        public Stopwatch gecenSure()
        {
            return sw;
        }
    }
}