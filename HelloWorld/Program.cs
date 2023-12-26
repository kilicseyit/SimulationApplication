using System;
using System.Collections.Generic;

public class IlOylari
{
    public List<int> PartilerinOylari { get; set; }
    public List<int> PartilerinAsilOylari { get; set; }
    public List<double> PartilerinOyOranlari { get; set; }
    public List<int> PartilerinMVSayisi { get; set; }

    public IlOylari()
    {
        PartilerinAsilOylari = new List<int>();
        PartilerinOylari = new List<int>();
        PartilerinOyOranlari = new List<double>();
        PartilerinMVSayisi = new List<int>();
    }

    public void HesaplaOyOranlari()
    {
        int toplamOy = 0;
        foreach (var oy in PartilerinOylari)
        {
            toplamOy += oy;
        }

        for (int pIndex = 0; pIndex < PartilerinOylari.Count; pIndex++)
        {
            double oyOrani = (double)PartilerinOylari[pIndex] / toplamOy * 100;
            PartilerinOyOranlari.Add(oyOrani);
        }
    }

    public void HesaplaMVSayilari(int mvK)
    {
        int kalanMV = mvK;

        for (int pIndex = 0; pIndex < PartilerinMVSayisi.Count; pIndex++)
        {
            PartilerinMVSayisi[pIndex] = 0;
        }

        while (kalanMV > 0)
        {
            int maxOyIndex = 0;
            for (int pIndex = 1; pIndex < PartilerinOylari.Count; pIndex++)
            {
                if (PartilerinOylari[pIndex] > PartilerinOylari[maxOyIndex])
                    maxOyIndex = pIndex;
            }

            if (kalanMV > 0)
            {
                PartilerinMVSayisi[maxOyIndex]++;
                SecimSimulasyonu.partiToplamMV[maxOyIndex]++;
                PartilerinOylari[maxOyIndex] /= 2;
                kalanMV--;
            }
            else
            {
                break;
            }
        }
    }
}

public class SecimSimulasyonu
{
    const int ilSayisi = 3;
    const int partiSayisi = 8;

    static int toplamMVSayisi;
    static int toplamOySayisi;

    static int[] partiToplamOy;
    public static int[] partiToplamMV;

    public static void Main(string[] args)
    {
        toplamMVSayisi = 0;
        toplamOySayisi = 0;

        List<IlOylari> iller = new List<IlOylari>();
        partiToplamOy = new int[partiSayisi];
        partiToplamMV = new int[partiSayisi];

        for (int i = 0; i < ilSayisi; i++)
        {
            Console.WriteLine("Il Plaka Kodu: " + (i + 1));
            Console.WriteLine("Milletvekili kontenjanı: ");
            int mvK = Convert.ToInt32(Console.ReadLine());
            toplamMVSayisi += mvK;

            IlOylari ilOylari = new IlOylari();
            //burda initialization ediyoruz ki içinde yanlış bilgi  kalmasın
            ilOylari.PartilerinMVSayisi.AddRange(new int[partiSayisi]);

            for (int pIndex = 0; pIndex < partiSayisi; pIndex++)
            {
                Console.WriteLine("Parti: " + (pIndex + 1) + " için oy sayısı giriniz: ");
                int oySayisi = Convert.ToInt32(Console.ReadLine());
                ilOylari.PartilerinOylari.Add(oySayisi);
                ilOylari.PartilerinAsilOylari.Add(oySayisi);
                toplamOySayisi += oySayisi;
                partiToplamOy[pIndex] += oySayisi;
            }

            iller.Add(ilOylari);

            ilOylari.HesaplaOyOranlari();
            ilOylari.HesaplaMVSayilari(mvK);
        }

        for (int i = 0; i < ilSayisi; i++)
        {
            Console.WriteLine("il " + (i + 1));
            for (int pIndex = 0; pIndex < partiSayisi; pIndex++)
            {
                Console.WriteLine("parti " + (pIndex + 1) + "nin oy sayısı: " + iller[i].PartilerinAsilOylari[pIndex]);
                Console.WriteLine("parti " + (pIndex + 1) + "nin oy orani: " + iller[i].PartilerinOyOranlari[pIndex].ToString("0.00") + "%");
                Console.WriteLine("parti " + (pIndex + 1) + "nin milletvekili sayisi: " + iller[i].PartilerinMVSayisi[pIndex]);
            }
            Console.WriteLine();
        }

        Console.WriteLine("Turkiye Geneli");
        for (int pIndex = 0; pIndex < partiSayisi; pIndex++)
        {
            Console.WriteLine("parti " + (pIndex + 1) + "nin oy sayisi: " + partiToplamOy[pIndex]);
            Console.WriteLine("parti " + (pIndex + 1) + "nin milletvekili sayisi: " + partiToplamMV[pIndex]);
        }
        Console.WriteLine("Milletvekili Kontenjani: " + toplamMVSayisi);
        Console.WriteLine("Gecerli Oy Sayisi: " + toplamOySayisi);

        // İktidar ve ana muhalefet partisi tespiti yapıyoruz
        int enBuyukMVIndex = 0;
        int ikinciBuyukMVIndex = -1;

        for (int pIndex = 1; pIndex < partiSayisi; pIndex++)
        {
            if (partiToplamMV[pIndex] > partiToplamMV[enBuyukMVIndex])
            {
                ikinciBuyukMVIndex = enBuyukMVIndex;
                enBuyukMVIndex = pIndex;
            }
            else if (ikinciBuyukMVIndex == -1 || partiToplamMV[pIndex] > partiToplamMV[ikinciBuyukMVIndex])
            {
                ikinciBuyukMVIndex = pIndex;
            }
        }

        Console.WriteLine("İktidar Partisi: Parti " + (enBuyukMVIndex + 1));
        Console.WriteLine("Ana Muhalefet Partisi: Parti " + (ikinciBuyukMVIndex + 1));

        // Partilerin kaç ilde birinci olduğunun tespiti
        for (int pIndex = 0; pIndex < partiSayisi; pIndex++)
        {
            int birinciPartiIlSayisi = 0;
            for (int i = 0; i < ilSayisi; i++)
            {
                if (iller[i].PartilerinAsilOylari[pIndex] == iller[i].PartilerinAsilOylari.Max())
                {
                    birinciPartiIlSayisi++;
                }
            }
            Console.WriteLine("Parti " + (pIndex + 1) + " birinci parti oldu: " + birinciPartiIlSayisi + " ilde");
        }

        // Partilerin ülke genelindeki milletvekili oranları ve oy oranları
        Console.WriteLine("\nÜlke Genelinde Parti Bilgileri");
        for (int pIndex = 0; pIndex < partiSayisi; pIndex++)
        {
            Console.WriteLine("Parti " + (pIndex + 1) + "nin oy orani: " + ((double)partiToplamOy[pIndex] / toplamOySayisi * 100).ToString("0.00") + "%");
            Console.WriteLine("Parti " + (pIndex + 1) + "nin milletvekili orani: " + ((double)partiToplamMV[pIndex] / toplamMVSayisi * 100).ToString("0.00") + "%");
        }
    }
}