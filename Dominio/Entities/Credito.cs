using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Credito
    {
        public int IdCredito { get; set; }
        public string? DNI { get; set; }
        public double Monto { get; set; }
        public double TasaAnual { get; set; }
        public int DiaPago { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public DateTime FechaPrimeraCuota { get; set; }
        public int Cuotas { get; set; }
        public double TasaDiaria { get; set; }
        public double TasaMensual { get; set; }
        public int Activo { get; set; } 
        public List<Detalle> Detalles { get; set; }
        public Credito()
        {
            this.FechaDesembolso = DateTime.Parse(DateTime.Now.ToShortDateString());//DateTime.Parse("17/03/2022");
            this.FechaPrimeraCuota = this.FechaDesembolso.AddMonths(1).AddDays(-1);
            this.Detalles = new List<Detalle>();
            this.Activo = 1;
        }

        public Credito GenerarCredito(Credito credito)
        {
            double num1 = 0.0027777777777778;
            double num2 = 0.0833333333333333;
            
            //POTENCIA(1+B7,1/360)-1
            credito.TasaDiaria = (Math.Pow(1 + credito.TasaAnual / 100, num1) - 1);
            //POTENCIA(1+B7,1/12)-1
            credito.TasaMensual = (Math.Pow(1 + credito.TasaAnual / 100, num2) - 1);

            double montoCuota = Math.Round(credito.Monto * (credito.TasaMensual * (Math.Pow(1 + credito.TasaMensual, credito.Cuotas))) / (Math.Pow(1 + credito.TasaMensual, credito.Cuotas) - 1), 2);

            List<Detalle> detalles = new List<Detalle>();

            //IdDetalle, IdCredito, Numero, FechaCuota, DiasCouta, SaldoCapital, Capital, Interes, Couta
            for (int i = 1; i <= credito.Cuotas; i++)
            {
                Detalle detalle = new Detalle();

                //detalle.Activo = true;
                detalle.Item = i;
                detalle.Cuota = montoCuota;
                if (i == 1)
                {
                    detalle.FechaCuota = DateTime.Parse(credito.DiaPago.ToString() + "/" + (DateTime.Now.Month + 1).ToString() + "/" + DateTime.Now.Year.ToString());
                    detalle.SaldoCapital = credito.Monto;
                    detalle.DiasCouta = Convert.ToInt32((detalle.FechaCuota - credito.FechaDesembolso).TotalDays);
                }
                else
                {
                    detalle.FechaCuota = (detalles[i - 2].FechaCuota).AddMonths(1);
                    detalle.DiasCouta = Convert.ToInt32((detalle.FechaCuota - detalles[i - 2].FechaCuota).TotalDays);
                    detalle.SaldoCapital = Math.Round(detalles[i - 2].SaldoCapital - detalles[i - 2].Capital, 2);
                }

                detalle.Interes = Math.Round(detalle.SaldoCapital * (Math.Pow(1 + credito.TasaDiaria, detalle.DiasCouta) - 1), 2);
                detalle.Capital = Math.Round(detalle.Cuota - detalle.Interes, 2);

                if (i == credito.Cuotas)
                {

                    detalle.SaldoCapital = Math.Round(detalles[i - 2].SaldoCapital - detalles[i - 2].Capital, 2);
                    detalle.Capital = Math.Round(detalle.SaldoCapital, 2);
                    detalle.Cuota = Math.Round(detalle.Capital + detalle.Interes, 2);
                }
                detalles.Add(detalle);

            }

            credito.Detalles = detalles;
            return credito; 
        }
    }
}
