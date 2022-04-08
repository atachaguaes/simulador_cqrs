using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Repositories
{
    public class CreditoRepository : ICreditoRepository
    {
        private readonly IConfiguration _configuration;

        public CreditoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int ActualizarCredito(Credito credito)
        {
            credito.Activo = 1;
            foreach (var item in credito.Detalles)
            {
                item.Activo = 1;
                item.IdCredito = credito.IdCredito;
            }
            string jsonCredito = JsonConvert.SerializeObject(credito);

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("Coneccion")))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "CRE_ActualizarCredito_Sp";
                sqlCommand.Parameters.AddWithValue("json", jsonCredito);
                sqlCommand.Parameters.AddWithValue("DNIAct", credito.DNI);
                sqlCommand.Parameters.AddWithValue("IdCreditoAct", credito.IdCredito);
                sqlCommand.Parameters.Add("Existe", SqlDbType.Int).Direction = ParameterDirection.Output;

                sqlCommand.Connection = cn;
                cn.Open();

                sqlCommand.ExecuteNonQuery();
                int existe = Convert.ToInt32(sqlCommand.Parameters["Existe"].Value);

                return 1;
            }
        }

        public int AgregarCredito(Credito credito)
        {
            credito.Activo = 1;
            foreach (var item in credito.Detalles)
            {
                item.Activo = 1;
            }
            string jsonCredito = JsonConvert.SerializeObject(credito);

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("Coneccion")))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "CRE_AgregarCredito_Sp";
                sqlCommand.Parameters.AddWithValue("json", jsonCredito);
                sqlCommand.Parameters.AddWithValue("DNI", credito.DNI);
                sqlCommand.Parameters.Add("IdCredito", SqlDbType.Int).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("Existe", SqlDbType.Int).Direction = ParameterDirection.Output;

                sqlCommand.Connection = cn;
                cn.Open();

                sqlCommand.ExecuteNonQuery();
                int idCredito = Convert.ToInt32(sqlCommand.Parameters["IdCredito"].Value);
                int existe = Convert.ToInt32(sqlCommand.Parameters["Existe"].Value);
                
                return idCredito;
            }
        }
        public bool EliminarCredito(int idCredito)
        {
            throw new NotImplementedException();
        }

        public List<Credito> ObtenerCreditos()
        {
            List<Credito> creditos = new List<Credito>();

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("Coneccion")))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "CRE_ObtenerCreditos_Sp";
                sqlCommand.Connection = cn;
                cn.Open();
                //IdCredito, DNI, Monto, TasaAnual, DiaPago, FechaDesembolso, FechaPrimeraCuota, NumeroCuotas, TasaDiaria, TasaMensual, Activo
                using (SqlDataReader dr = sqlCommand.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Credito credito = new Credito();

                        credito.IdCredito = Convert.ToInt32(dr["IdCredito"]);
                        credito.DNI = dr["DNI"].ToString();
                        credito.Monto = Convert.ToDouble(dr["Monto"]);
                        credito.TasaAnual = Convert.ToDouble(dr["TasaAnual"]);
                        credito.DiaPago = Convert.ToInt32(dr["DiaPago"]);
                        credito.FechaDesembolso = Convert.ToDateTime(dr["FechaDesembolso"]);
                        credito.FechaPrimeraCuota = Convert.ToDateTime(dr["FechaPrimeraCuota"]);
                        credito.Cuotas = Convert.ToInt32(dr["NumeroCuotas"]);
                        credito.TasaDiaria = Convert.ToDouble(dr["TasaDiaria"]);
                        credito.TasaMensual = Convert.ToDouble(dr["TasaMensual"]);
                        credito.Activo = Convert.ToInt32(dr["Activo"]);
                        credito.Detalles = ObtenerDetallesPorIdCredito(credito.IdCredito);

                        creditos.Add(credito);
                    }
                }
            }
            return creditos;
        }

        public Credito ObtenerCreditoPorIdCredito(int idCredito)
        {
            List<Credito> creditos = new List<Credito>();
            //CDDetalle cdDetalle = new CDDetalle();

            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("Coneccion")))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "CRE_ObtenerCreditoPorIdCredito_Sp";
                sqlCommand.Parameters.AddWithValue("IdCredito", idCredito);
                sqlCommand.Connection = cn;
                cn.Open();
                //IdCredito, DNI, Monto, TasaAnual, DiaPago, FechaDesembolso, FechaPrimeraCuota, NumeroCuotas, TasaDiaria, TasaMensual, Activo
                using (SqlDataReader dr = sqlCommand.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Credito credito = new Credito();

                        credito.IdCredito = Convert.ToInt32(dr["IdCredito"]);
                        credito.DNI = dr["DNI"].ToString();
                        credito.Monto = Convert.ToDouble(dr["Monto"]);
                        credito.TasaAnual = Convert.ToDouble(dr["TasaAnual"]);
                        credito.DiaPago = Convert.ToInt32(dr["DiaPago"]);
                        credito.FechaDesembolso = Convert.ToDateTime(dr["FechaDesembolso"]);
                        credito.FechaPrimeraCuota = Convert.ToDateTime(dr["FechaPrimeraCuota"]);
                        credito.Cuotas = Convert.ToInt32(dr["NumeroCuotas"]);
                        credito.TasaDiaria = Convert.ToDouble(dr["TasaDiaria"]);
                        credito.TasaMensual = Convert.ToDouble(dr["TasaMensual"]);
                        credito.Activo = Convert.ToInt32(dr["Activo"]);
                        credito.Detalles = ObtenerDetallesPorIdCredito(credito.IdCredito);

                        creditos.Add(credito);
                    }
                }
            }
            return creditos[0];
        }

        public List<Detalle> ObtenerDetallesPorIdCredito(int idCredito)
        {
            List<Detalle> detalles = new List<Detalle>();
            using (SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("Coneccion")))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "CRE_ObtenerDetallesPorIdCredito_Sp";
                sqlCommand.Parameters.AddWithValue("IdCredito", idCredito);
                sqlCommand.Connection = cn;
                cn.Open();
                //IdDetalle, IdCredito, Item, FechaCuota, DiasCouta, SaldoCapital, Capital, Interes, Cuota, Activo
                using (SqlDataReader dr = sqlCommand.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        detalles.Add(
                            new Detalle
                            {
                                IdDetalle = Convert.ToInt32(dr["IdDetalle"]),
                                IdCredito = Convert.ToInt32(dr["IdCredito"]),
                                Item = Convert.ToInt32(dr["Item"]),
                                FechaCuota = Convert.ToDateTime(dr["FechaCuota"]),
                                DiasCouta = Convert.ToInt32(dr["DiasCouta"]),
                                SaldoCapital = Convert.ToDouble(dr["SaldoCapital"]),
                                Capital = Convert.ToDouble(dr["Capital"]),
                                Interes = Convert.ToDouble(dr["Interes"]),
                                Cuota = Convert.ToDouble(dr["Cuota"]),
                                Activo = Convert.ToInt32(dr["Activo"]),
                            });
                    }
                }
            }
            return detalles;
        }

        //public Credito GenerarCredito(Credito credito)
        //{
        //    Credito _credito = new Credito();

        //    Credito _creditoGenerado = _credito.GenerarCredito(credito);

        //    return _creditoGenerado;
        //}
    }
}
